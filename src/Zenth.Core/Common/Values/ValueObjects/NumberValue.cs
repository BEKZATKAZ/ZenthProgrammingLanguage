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
using Zenth.Core.Common.Values.Interfaces;
using System.Collections.Immutable;
using System.Globalization;
using Zenth.Core.Common.Expressions.Interfaces;

namespace Zenth.Core.Common.Values.ValueObjects;

public readonly record struct NumberValue(
    float Value,
    ImmutableDictionary<EOperator, IValueCalculator> Operators) :
    IValue, IConvertableToBool, IConvertableToNumber, IValueInvertable
{
    public readonly IValue Invert() => CreateNew(-Value);
    public readonly bool ToBoolean() => Value > 0;
    public readonly float ToNumber() => Value;
    public override readonly string ToString() => Value.ToString(CultureInfo.InvariantCulture);

    public static NumberValue CreateNew(float value)
    {
        return new NumberValue(value, SupportedOperators.Number);
    }

    public static NumberValue CreateNew(ReadOnlySpan<char> value)
    {
        return new NumberValue(float.Parse(value, CultureInfo.InvariantCulture), SupportedOperators.Number);
    }
}
