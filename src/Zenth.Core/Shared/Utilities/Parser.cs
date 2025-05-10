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

using Zenth.Core.Common.Values.Interfaces;

namespace Zenth.Core.Shared.Utilities;

public static class Parser
{
    public static bool ToBoolean(IValue value) => value switch
    {
        IConvertableToBool convertable => convertable.ToBoolean(),
        _ => throw new InvalidOperationException($"Cannot parse `{value}` to boolean")
    };

    public static float ToNumber(IValue value) => value switch
    {
        IConvertableToNumber convertable => convertable.ToNumber(),
        _ => throw new InvalidOperationException($"Cannot parse `{value}` to number")
    };

    public static IValue Invert(IValue value) => value switch
    {
        IValueInvertable invertable => invertable.Invert(),
        _ => throw new InvalidOperationException($"Value `{value}` cannot be inverted")
    };
}
