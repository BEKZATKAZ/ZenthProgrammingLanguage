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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Zenth.Core.Common.Codes;
using Zenth.Core.Common.Expressions.Interfaces;
using Zenth.Core.Common.Lines.Interfaces;
using Zenth.Core.Common.Lines.ValueObjects.MemberAccessing;
using Zenth.Core.Common.Lines.ValueObjects.MemberAccessing.Functions;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Compiler.Readers;
using Zenth.Core.Shared.Extensions;

namespace Zenth.Application.Compiler.MemberPaths;

public class MemberFunctionCompiler : MemberCompilerBase
{
    private readonly ElementsReader _argumentReader = new();

    public virtual IMemberAccess Compile(MemberTokenCompilationData data, ReadOnlySpan<char> token)
    {
        var (functionName, arguments) = ReadFunctionCall(data, token);

        return data.PreviousMember is BaseMember
            ? new FunctionCallInBase(functionName, arguments)
            : new FunctionCall(functionName, arguments);
    }

    protected (string functionName, ImmutableArray<IExpression> arguments) ReadFunctionCall(
        MemberTokenCompilationData data,
        ReadOnlySpan<char> token)
    {
        ReadOnlySpan<char> functionName = token.TakeWhile(x => x is not '(');
        ReadOnlySpan<char> fromArguments = data.FromTokenLine[(functionName.Length + 1)..].Span;

        LineSource newLine = LineSource.CreateNew(fromArguments.ToString(), data.Line.Number);

        Span<char> arguments = stackalloc char[newLine.Length];
        Span<Range> argumentRanges = stackalloc Range[newLine.Length];
        int argumentCount = _argumentReader.Read(newLine, arguments, argumentRanges);

        if (argumentCount == 0) return (functionName.Trim().ToString(), []);

        ImmutableArray<IExpression> argumentsArray = CompileArguments(
            data.Line, arguments, argumentRanges, argumentCount, data.Context);

        return (functionName.Trim().ToString(), argumentsArray);
    }

    private ImmutableArray<IExpression> CompileArguments(
        in LineSource line,
        ReadOnlySpan<char> arguments,
        ReadOnlySpan<Range> argumentRanges,
        int argumentCount,
        ICompilationContext context)
    {
        ImmutableArray<IExpression>.Builder builder = ImmutableArray.CreateBuilder<IExpression>();

        ref Range rangeReference = ref MemoryMarshal.GetReference(argumentRanges);

        for (int i = 0; i < argumentCount; i++)
        {
            ReadOnlySpan<char> argument = arguments[Unsafe.Add(ref rangeReference, i)];

            builder.Add(ExpressionCompiler
                .Compile(LineSource.CreateNew(argument.ToString(), line.Number), context));
        }

        return builder.ToImmutable();
    }
}
