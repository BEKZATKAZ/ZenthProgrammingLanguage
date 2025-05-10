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
using Zenth.Core.Common;
using Zenth.Core.Common.Codes;
using Zenth.Core.Common.Expressions.Interfaces;
using Zenth.Core.Common.Lines.ValueObjects.Definitions;
using Zenth.Core.Compiler.Exceptions;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Shared.Extensions;
using Zenth.Core.Shared.Kernel;

namespace Zenth.Application.Compiler;

public sealed class VariableCompiler(ExpressionCompiler expressionCompiler)
{
    public Line Compile(in LineSource line, ICompilationContext context)
    {
        LineSource fromName = line[Keywords.VARIABLE.Length..].Trim();

        ReadOnlySpan<char> name = fromName.Span.TakeWhile(x => char.IsLetter(x) || char.IsDigit(x) || x is '_');

        if (context.DeclaredVariables.Has(name))
            throw new DuplicateException(line, name.ToString());

        if (fromName.Length == name.Length)
            return Line.Create<VariableDeclaration>(name.ToString(), null);

        LineSource afterName = fromName[name.Length..].Trim();

        if (afterName[0] != '=')
            throw new SyntaxException(line, $"Cannot assign value to variable `{name}`");

        LineSource valueExpression = afterName[1..].Trim();
        IExpression expressionValue = expressionCompiler.Compile(valueExpression, context);

        return Line.Create<VariableDeclaration>(name.ToString(), expressionValue);
    }
}
