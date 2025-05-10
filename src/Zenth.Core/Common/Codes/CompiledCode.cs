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

using Zenth.Core.Common.Lines.Interfaces;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Execution.Interfaces.Contexts;
using Zenth.Core.Execution.Services;

namespace Zenth.Core.Common.Codes;

public sealed class CompiledCode
{
    private CompiledCode(ILineExecute[]? lines, ICompilationContext? context)
    {
        _lines = lines;
        _context = context;
    }

    private ILineExecute[]? _lines;
    private ICompilationContext? _context;

    public ICompilationContext CompilationContext =>
        _context ?? throw new InvalidDataException("Invalid compilation context");

    public static CompiledCode Empty => new(null, null);

    public void Reset(CompiledCode original)
    {
        _lines = original._lines;
        _context = original._context;
    }

    public void Execute(IBlockExecutionContext context)
    {
        if (_lines is null) return;

        using IEnumerator<ILineExecute> enumerator = _lines
            .AsEnumerable()
            .GetEnumerator();

        CodeExecuter executer = new(enumerator, context);
        executer.Execute();
    }

    public static CompiledCode CreateNew(
        IEnumerable<ILineExecute> lines,
        ICompilationContext context) => new([.. lines], context);
}
