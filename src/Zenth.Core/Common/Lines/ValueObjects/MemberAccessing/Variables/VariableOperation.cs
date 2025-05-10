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

using Zenth.Core.Common.Entities.Variables;
using Zenth.Core.Common.Expressions.Calculations;
using Zenth.Core.Common.Expressions.Interfaces;
using Zenth.Core.Common.Values.Interfaces;
using Zenth.Core.Common.Values.ValueObjects;
using Zenth.Core.Execution.Interfaces.Contexts;

namespace Zenth.Core.Common.Lines.ValueObjects.MemberAccessing.Variables;

public abstract record VariableOperation(
    string VariableName,
    bool IsPrefix,
    IValueCalculator Calculation) : VariableAccess(VariableName)
{
    protected override IValue PerformOperation(
        VariableBase variable, IBlockExecutionContext context, Guid ownerId)
    {
        IValue oldValue = variable.Read(context, ownerId, context.ObjectId);

        if (oldValue is not NumberValue)
            throw new InvalidCastException(
                $"Variable `{VariableName}` must be a number in order to perform this operation");

        IValue newValue = Calculation.Calculate(oldValue, NumberValue.CreateNew(1));
        variable.Write(newValue, context, ownerId, context.ObjectId);

        return IsPrefix ? newValue : oldValue;
    }

    public override int GetHashCode() =>
        HashCode.Combine(VariableName.GetHashCode(), IsPrefix.GetHashCode(), Calculation.GetHashCode());
}

public sealed record VariableIncrement(
    string VariableName,
    bool IsPrefix) : VariableOperation(VariableName, IsPrefix, new NumberAddition());

public sealed record VariableDecrement(
    string VariableName,
    bool IsPrefix) : VariableOperation(VariableName, IsPrefix, new NumberSubstraction());
