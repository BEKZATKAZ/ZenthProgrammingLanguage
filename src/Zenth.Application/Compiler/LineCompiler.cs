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

using Zenth.Application.Compiler.Expressions;
using Zenth.Application.Compiler.MemberPaths;
using Zenth.Core.Common;
using Zenth.Core.Common.Codes;
using Zenth.Core.Common.Expressions.Interfaces;
using Zenth.Core.Common.Lines.ValueObjects;
using Zenth.Core.Common.Lines.ValueObjects.MemberAccessing;
using Zenth.Core.Compiler.Interfaces;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Compiler.Readers;
using static Zenth.Core.Common.Line;
using static Zenth.Core.Shared.Kernel.Keywords;

namespace Zenth.Application.Compiler;

public sealed class LineCompiler(
    ExpressionCompiler expressionCompiler,
    VariableCompiler variableCompiler,
    FunctionCompiler functionCompiler,
    ClassCompiler classCompiler,
    MemberPathCompiler memberPathCompiler)
{
    private readonly SkipReader _keywordReader = new(new ReaderFull('(', ')'), ' ');

    public Line Compile(in LineSource line, ICompilationContext context)
    {
        Line? result = CompileLine(line, context);
        return result ?? Create<MemberPathLine>(memberPathCompiler.Compile(line, context));
    }

    private Line? CompileLine(LineSource line, ICompilationContext context)
    {
        IExpression Expression(ReadOnlySpan<char> keyword)
        {
            return expressionCompiler.Compile(_keywordReader, keyword, line, context);
        }

        if (line.StartsWith(ELSE_IF)) return Create<ElseIfStatement>(Expression(ELSE_IF));
        if (line.StartsWith(IF)) return Create<IfStatement>(Expression(IF));
        if (line.StartsWith(ELSE)) return Create<ElseIfStatement>([null]);

        if (line.StartsWith(WHILE_LOOP)) return Create<WhileLoop>(Expression(WHILE_LOOP));
        if (line.StartsWith(RETURN)) return Create<FunctionReturn>(Expression(RETURN));
        if (line.StartsWith(IMPORT)) return Create<LibraryImporter>(line.Origin[IMPORT.Length..].Trim());
        if (line.StartsWith(DELETE)) return Create<ObjectDelete>(Expression(DELETE));

        if (line.StartsWith(VARIABLE)) return variableCompiler.Compile(line, context);
        if (line.StartsWith(FUNCTION)) return functionCompiler.Compile(line, context);
        if (line.StartsWith(CLASS)) return classCompiler.Compile(line, context);

        return null;
    }
}
