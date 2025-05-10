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

using System.Collections.Immutable;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Zenth.Core.Common.Enums;
using Zenth.Core.Common.Expressions;
using Zenth.Core.Common.Expressions.Tokens;
using Zenth.Core.Common.Expressions.Interfaces;
using Zenth.Core.Common.Values.Interfaces;
using Zenth.Core.Compiler.Exceptions;
using Zenth.Core.Compiler.Interfaces;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Compiler.ValueObjects;
using Zenth.Core.Common.Codes;

namespace Zenth.Application.Compiler.Expressions;

public sealed class ExpressionCompiler(OperatorCompiler operatorCompiler, ExpressionTokenCompiler tokenCompiler)
{
    public void Initialize() => tokenCompiler.SetExpressionCompiler(this);

    public IExpression Compile(
        ILineReader reader, ReadOnlySpan<char> keyword, in LineSource line, ICompilationContext context)
    {
        string syntax = reader.Read(keyword.Length, out _, line).ToString();
        LineSource expression = line with { Origin = syntax };
        return Compile(expression, context);
    }

    public IExpression Compile(in LineSource line, ICompilationContext context)
    {
        if (line.Length == 0) throw new SyntaxException(line, "No expression");

        var (tokens, operators) = CompileTokensAndOperators(line, context);

        if (tokens.Length == 1 && operators.Length == 0) return new OneValueExpression(tokens[0]);

        if (tokens.Any(x => x is not ExpressionConstantToken))
        {
            return new CalculativeExpression(tokens, operators);
        }

        IValue value = new CalculativeExpression(tokens, operators).Evaluate(null!);
        IExpressionToken valueToken = new ExpressionConstantToken(value);

        return new OneValueExpression(valueToken);
    }

    private (ImmutableArray<IExpressionToken>, ImmutableArray<EOperator>) CompileTokensAndOperators(
        LineSource line, ICompilationContext context)
    {
        var (tokens, operators) = (CreateArrayBuilder<IExpressionToken>(), CreateArrayBuilder<EOperator>());
        var (seemsLikeOperatorFrom, i) = (-1, 0);

        OperatorCompilerOutput lastOperatorOutput = default;
        bool isLastCompilationToken = false;

        Queue<Range> operatorBuffer = new Queue<Range>(4);

        void TryCompileOperators()
        {
            while (operatorBuffer.TryDequeue(out Range operatorRange))
            {
                ReadOnlySpan<char> operatorSpan = line[operatorRange].Trim().Span;

                lastOperatorOutput = operatorCompiler.CompileOperator(operatorSpan, new OperatorCompilerInput(
                    isLastCompilationToken, line,
                    isLastCompilationToken ? tokens[^1] : null, operators.Add));

                isLastCompilationToken = false;
            }
        }

        ReadOnlySpan<char> span = line.Span;
        ref char reference = ref MemoryMarshal.GetReference(span);

        for (i = 0; i < span.Length; i++)
        {
            char current = Unsafe.Add(ref reference, i);

            if (operatorCompiler.SeemsLikeOperator(current))
            {
                if (seemsLikeOperatorFrom == -1) seemsLikeOperatorFrom = i;
                continue;
            }
            else if (seemsLikeOperatorFrom != -1)
            {
                operatorBuffer.Enqueue(seemsLikeOperatorFrom..i);
                seemsLikeOperatorFrom = -1;
            }

            if (current is ' ' or ')' or ']') continue;
            if (current is '{' or '}' or ',') break;

            TryCompileOperators();

            IExpressionToken token = tokenCompiler.Compile(line, ref i, context);
            
            if (lastOperatorOutput.InvertNextToken)
            {
                if (token is IExpressionInvertable invertable) invertable.Invert();
                else throw new SyntaxException(line, "This expression token cannot be inverted");
            }

            tokens.Add(token);
            lastOperatorOutput = default;
            isLastCompilationToken = true;
        }

        TryCompileOperators();

        return (tokens.ToImmutable(), operators.ToImmutable());
    }

    private static ImmutableArray<T>.Builder CreateArrayBuilder<T>() => ImmutableArray.CreateBuilder<T>();
}
