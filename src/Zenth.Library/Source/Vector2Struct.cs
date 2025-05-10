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
using Zenth.Core.Common.Values.ValueObjects;
using Zenth.Core.Shared.Utilities;
using Zenth.Library.Attributes;

namespace Zenth.Library.Source;

[PackageMember("vec2")]
public sealed class Vector2Struct
{
    public Vector2Struct() => (X, Y) = (NumberValue.CreateNew(0), NumberValue.CreateNew(0));
    public Vector2Struct(NumberValue x, NumberValue y) => (X, Y) = (x, y);

    [MemberVariable("x", typeof(NumberValue))]
    public NumberValue X { get; set; }

    [MemberVariable("y", typeof(NumberValue))]
    public NumberValue Y { get; set; }

    [MemberVariable("magnitude")]
    public NumberValue Magnitude
    {
        get
        {
            float x = Parser.ToNumber(X);
            float y = Parser.ToNumber(Y);
            return NumberValue.CreateNew(MathF.Sqrt(x * x + y * y));
        }
    }
}
