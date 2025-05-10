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

using Zenth.Core.Common.Codes;
using Zenth.Core.Common.Expressions.Interfaces;
using Zenth.Core.Common.Lines.Interfaces;
using Zenth.Core.Common.Lines.ValueObjects.MemberAccessing;
using Zenth.Core.Compiler.Readers;
using Zenth.Core.Shared.Extensions;

namespace Zenth.Application.Compiler.MemberPaths;

public sealed class MemberCollectionCompiler : MemberCompilerBase
{
    public IMemberAccess Compile(
        MemberTokenCompilationData data,
        ReadOnlySpan<char> token,
        Func<MemberTokenCompilationData, ReadOnlySpan<char>, IMemberAccess> fullCompilation)
    {
        token = token.TakeWhile(x => x is not '[').Trim();

        LineSource fromName = data.FromTokenLine[token.Length..];

        ReadOnlySpan<char> thisExpression = new ReaderFull('[', ']').Read(0, out _, fromName.Trim());
        LineSource newLine = LineSource.CreateNew(token.ToString(), data.Line.Number);
        IMemberAccess collection = fullCompilation(data, token);

        LineSource expressionLine = LineSource.CreateNew(thisExpression.ToString(), data.Line.Number);
        IExpression expression = ExpressionCompiler.Compile(expressionLine, data.Context);

        if (fromName.Length > thisExpression.Length + 3)
        {
            LineSource fromExpression = fromName[(thisExpression.Length + 3)..].Trim();

            if (fromExpression.StartsWith('='))
            {
                IExpression expressionValue = ExpressionCompiler
                    .Compile(fromExpression[1..].Trim(), data.Context);

                return new MemberElementWrite(collection, expression, expressionValue);
            }
        }
        return new MemberElementRead(collection, expression);
    }
}
