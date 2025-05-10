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

using Zenth.Application.Compiler.MemberPaths;
using Zenth.Core.Common.Codes;
using Zenth.Core.Common.Expressions.Interfaces;
using Zenth.Core.Common.Expressions.Tokens;
using Zenth.Core.Common.Values.ValueObjects;
using Zenth.Core.Compiler.Exceptions;
using Zenth.Core.Compiler.Interfaces;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Compiler.Readers;
using Zenth.Core.Shared.Kernel;

namespace Zenth.Application.Compiler.Expressions;

public sealed class ExpressionTokenCompiler(MemberPathCompiler memberPathCompiler)
{
    private ExpressionCompiler _expressionCompiler = null!;
    private CollectionCompiler _collectionCompiler = null!;

    public void SetExpressionCompiler(ExpressionCompiler expressionCompiler)
    {
        _expressionCompiler = expressionCompiler;
    }

    public void SetCollectionCompiler(CollectionCompiler collectionCompiler)
    {
        _collectionCompiler = collectionCompiler;
    }

    public IExpressionToken Compile(LineSource line, ref int index, ICompilationContext context)
    {
        int simpleIndex = index;

        ReadOnlySpan<char> Read(ILineReader reader) => reader.Read(simpleIndex, out simpleIndex, line);

        IExpressionToken? result = line[index] switch
        {
            '(' => new SubExpressionToken(
                _expressionCompiler.Compile(
                    LineSource.CreateNew(
                        Read(new ReaderFull('(', ')')), line.Number), context)),

            '[' => _collectionCompiler.Compile(
                    LineSource.CreateNew(
                        Read(new ReaderFull('[', ']')), line.Number), context),

            '"' => new ExpressionConstantToken(
                StringValue.CreateNew(
                    Read(new StringValueReader(true, false)).ToString().Replace("\\n", "\n"))),

            _ when char.IsDigit(line[index]) =>
                new ExpressionConstantToken(
                    NumberValue.CreateNew(Read(new NumberValueReader()))),

            _ when char.IsLetter(line[index]) || line[index] is '_' =>
                ReadOther(line, Read(new MemberPathReader()), context),

            _ => null
        };

        index = simpleIndex;

        return result ?? throw new SyntaxException(line, "Could not read tokens");
    }

    private IExpressionToken ReadOther(
        in LineSource line, ReadOnlySpan<char> token, ICompilationContext context)
    {
        if (token.SequenceEqual(Keywords.TRUE))
            return new ExpressionConstantToken(BooleanValue.CreateNew(true));

        if (token.SequenceEqual(Keywords.FALSE))
            return new ExpressionConstantToken(BooleanValue.CreateNew(false));

        if (token.SequenceEqual(Keywords.NULL))
            return new ExpressionConstantToken(new NullValue());

        LineSource newLine = LineSource.CreateNew(token, line.Number);

        return new MemberExpressionToken(
            memberPathCompiler.Compile(newLine, context));
    }
}
