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

using Zenth.Core.Common.Enums;
using Zenth.Core.Common.Expressions.Tokens;
using Zenth.Core.Common.Lines.Interfaces;
using Zenth.Core.Common.Lines.ValueObjects.MemberAccessing.Variables;
using Zenth.Core.Compiler.Exceptions;
using Zenth.Core.Compiler.ValueObjects;
using Zenth.Core.Shared.Extensions;

namespace Zenth.Application.Compiler.Expressions;

public sealed class OperatorCompiler
{
    private readonly Dictionary<string, EOperator> _operators = new()
    {
        { "+", EOperator.Addition },
        { "-", EOperator.Subtraction },
        { "*", EOperator.Multiplication },
        { "/", EOperator.Division },
        { "%", EOperator.Modulus },

        { "&&", EOperator.And },
        { "||", EOperator.Or },
        { "!", EOperator.Not },
        { "==", EOperator.Equal },
        { "!=", EOperator.NotEqual },
        { ">", EOperator.Greater },
        { "<", EOperator.Less },
        { ">=", EOperator.GreaterEqual },
        { "<=", EOperator.LessEqual },

        { "=", EOperator.Definition },
        { "++", EOperator.Addition },
        { "--", EOperator.Subtraction }
    };

    public bool SeemsLikeOperator(char operatorCharacter)
    {
        return _operators.Any(x => x.Key.StartsWith(operatorCharacter));
    }

    public OperatorCompilerOutput CompileOperator(
        ReadOnlySpan<char> operatorSpan,
        in OperatorCompilerInput input)
    {
        if (TryCompileOperator(operatorSpan, out EOperator operatorResult) == false)
        {
            throw new SyntaxException(input.Line, "No operator found");
        }

        if (operatorSpan.SequenceEqual("++") == false && operatorSpan.SequenceEqual("--") == false ||
            input.LastToken is not MemberExpressionToken memberPath)
        {
            if (input.IsLastCompilationToken) input.OnCompiled(operatorResult);
            return new OperatorCompilerOutput(input.IsLastCompilationToken == false);
        }

        IMemberAccess last = memberPath.Path.GetLast();

        if (last is not VariableAccess variableAccess)
        {
            throw new SyntaxException(input.Line, "Increment may be performed on variables only");
        }

        IMemberAccess operation = operatorSpan.SequenceEqual("++")
            ? new VariableIncrement(variableAccess.VariableName, false)
            : new VariableDecrement(variableAccess.VariableName, false);

        memberPath.Path.SetLast(operation);
        return new OperatorCompilerOutput(false);
    }

    private bool TryCompileOperator(ReadOnlySpan<char> operatorString, out EOperator result)
    {
        _operators.GetAlternateLookup<ReadOnlySpan<char>>().FasterTryGetValue(operatorString, out result);
        return result is not EOperator.None;
    }
}
