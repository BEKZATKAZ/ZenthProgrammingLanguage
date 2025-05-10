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
using Zenth.Core.Shared.Utilities;
using System.Collections.Immutable;
using Zenth.Core.Common.Expressions.Interfaces;

namespace Zenth.Core.Common.Values.ValueObjects;

public readonly record struct StringValue(
    string Value,
    ImmutableDictionary<EOperator, IValueCalculator> Operators) : IValue, IValueElementRead
{
    public override string ToString() => Value;

    public IValue Get(IValue key)
    {
        return CreateNew(Value[Convert.ToInt32(Parser.ToNumber(key))].ToString());
    }

    public static StringValue CreateNew(string value)
    {
        return new StringValue(value, SupportedOperators.String);
    }

    public static StringValue CreateNew(ReadOnlySpan<char> value)
    {
        return new StringValue(value.ToString(), SupportedOperators.String);
    }
}
