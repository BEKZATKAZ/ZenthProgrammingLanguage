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
using Zenth.Core.Common.Enums;
using Zenth.Core.Common.Values.ValueObjects;
using Zenth.Core.Common.Values.Interfaces;
using Zenth.Core.Common.Expressions.Interfaces;
using Zenth.Core.Execution.Interfaces.Contexts;

namespace Zenth.Core.Common.Expressions;

public sealed record CalculativeExpression(
    in ImmutableArray<IExpressionToken> Tokens,
    in ImmutableArray<EOperator> Operators) : IExpression
{
    public IValue Evaluate(IBlockExecutionContext context)
    {
        IValue result = Tokens[0].Evaluate(context);

        for (int i = 1; i < Tokens.Length; i++)
        {
            IValue second = Tokens[i].Evaluate(context);
            result = Calculate(Operators[i - 1], result, second);
        }

        return result;
    }

    private static IValue Calculate(EOperator currentOperator, IValue first, IValue second)
    {
        var operators = (first is NullValue && second is not NullValue ? second : first).Operators;

        operators.TryGetValue(currentOperator, out IValueCalculator? calculator);

        return calculator is null
            ? throw new InvalidOperationException(
                $"{first.GetType().Name} does not support the operator {currentOperator}")
            : calculator.Calculate(first, second);
    }
}
