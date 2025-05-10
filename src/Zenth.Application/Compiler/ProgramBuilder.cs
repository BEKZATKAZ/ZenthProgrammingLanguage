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
using Zenth.Core.Compiler.Interfaces.Factories;

namespace Zenth.Application.Compiler;

public sealed class ProgramBuilder(ICompilationContextFactory contextFactory)
{
    public IEnumerable<ILineExecute> Build(
        IEnumerator<Line> builders,
        ICompilationContext context,
        CompilationDelegate compilation)
    {
        while (builders.MoveNext())
        {
            Line builder = builders.Current;

            ILine line = builder.Build();

            if (line is ILineCompile compile) compile.OnCompiled(context);

            if (line is ILineBlock block)
            {
                var bodyContext = BuildContextFromBlock(builder, context);

                if (bodyContext is ICompilationContextBuild build)
                    bodyContext = build.OnBuilt(line);

                CompiledCode body = compilation(builder.BodyCode!, bodyContext);
                block.Body.Reset(body);

                if (bodyContext is ICompilationContextBodyReady ready)
                    ready.OnReady(line, block.Body);
            }

            if (line is ILineExecute execute) yield return execute;
        }
    }

    private ICompilationContext BuildContextFromBlock(Line builder, ICompilationContext baseContext)
    {
        if (builder.HasInterface<ILineBlock>() == false || builder.BodyCode is null)
            throw new NullReferenceException("Cannot build a compilation context");

        if (builder.HasInterface<ILineClassDefine>())
            return contextFactory.CreateMemberContext(baseContext);

        if (builder.HasInterface<ILineFunctionDefine>())
            return contextFactory.CreateFunctionContext(baseContext);

        return contextFactory.CreateBlockContext(baseContext);
    }
}
