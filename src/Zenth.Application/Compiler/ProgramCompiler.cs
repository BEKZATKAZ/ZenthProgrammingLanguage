// MIT License
// 
// Copyright (c) 2025 BEKZATKAZ
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Zenth.Core.Common;
using Zenth.Core.Common.Codes;
using Zenth.Core.Common.Lines.Interfaces;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Compiler.Readers;

namespace Zenth.Application.Compiler;

public sealed class ProgramCompiler(
    LineCompiler lineCompiler,
    ModifierCompiler modifierCompiler,
    ProgramBuilder builder)
{
    private readonly BodyReader _bodyReader = new();

    public CompiledCode Compile(UncompiledCode code, ICompilationContext context)
    {
        List<LineSource> trimmedCode = [.. code.TrimAndGetNecessary()];
        return Compile(trimmedCode, context);
    }

    public CompiledCode Compile(List<LineSource> trimmedCode, ICompilationContext context)
    {
        using IEnumerator<Line> builders = CompileLines(trimmedCode, context);

        try
        {
            IEnumerable<ILineExecute> lines = builder.Build(builders, context, Compile);
            return CompiledCode.CreateNew(lines, context);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"COMPILATION ERROR: {exception}");
            throw;
        }

        throw new NotImplementedException();
    }

    private IEnumerator<Line> CompileLines(List<LineSource> code, ICompilationContext context)
    {
        for (int i = 0; i < code.Count; i++)
        {
            LineSource line = code[i];

            while (modifierCompiler.TryCompile(line, context, out string? keyword))
            {
                if (keyword is null) throw new Exception();
                line = line[keyword.Length..].Trim();
            }

            Line builder = lineCompiler.Compile(line, context);
            builder.Source = line;
            context.Modifiers.ResetAll();

            if (builder.LineType.GetInterface(nameof(ILineBlock)) == typeof(ILineBlock))
            {
                builder.BodyCode = _bodyReader.Read(ref i, code);
            }

            yield return builder;
        }
    }
}
