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

[PackageMember("Math")]
public sealed class MathClass
{
    [MemberVariable("PI")]
    public static NumberValue PI => NumberValue.CreateNew(MathF.PI);

    [MemberFunction("abs")]
    public static NumberValue Abs(NumberValue x)
    {
        return NumberValue.CreateNew(x.Value < 0 ? -x.Value : x.Value);
    }

    [MemberFunction("sign")]
    public static NumberValue Sign(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Sign(x.Value));
    }

    [MemberFunction("floor")]
    public static NumberValue Floor(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Floor(x.Value));
    }

    [MemberFunction("round")]
    public static NumberValue Round(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Round(x.Value));
    }

    [MemberFunction("ceil")]
    public static NumberValue Ceil(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Ceiling(x.Value));
    }

    [MemberFunction("min")]
    public static NumberValue Min(NumberValue x, NumberValue y)
    {
        return NumberValue.CreateNew(x.Value > y.Value ? y.Value : x.Value);
    }

    [MemberFunction("max")]
    public static NumberValue Max(NumberValue x, NumberValue y)
    {
        return NumberValue.CreateNew(x.Value > y.Value ? x.Value : y.Value);
    }

    [MemberFunction("clamp")]
    public static NumberValue Clamp(NumberValue value, NumberValue min, NumberValue max)
    {
        return NumberValue.CreateNew(Math.Clamp(value.Value, min.Value, max.Value));
    }

    [MemberFunction("clamp01")]
    public static NumberValue Clamp01(NumberValue value)
    {
        return NumberValue.CreateNew(Math.Clamp(value.Value, 0, 1));
    }

    [MemberFunction("sin")]
    public static NumberValue Sin(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Sin(x.Value));
    }

    [MemberFunction("cos")]
    public static NumberValue Cos(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Cos(x.Value));
    }

    [MemberFunction("tan")]
    public static NumberValue Tan(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Tan(x.Value));
    }

    [MemberFunction("sinh")]
    public static NumberValue Sinh(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Sinh(x.Value));
    }

    [MemberFunction("cosh")]
    public static NumberValue Cosh(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Cosh(x.Value));
    }

    [MemberFunction("tanh")]
    public static NumberValue Tanh(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Tanh(x.Value));
    }

    [MemberFunction("asin")]
    public static NumberValue Asin(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Asin(x.Value));
    }

    [MemberFunction("acos")]
    public static NumberValue Acos(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Acos(x.Value));
    }

    [MemberFunction("atan")]
    public static NumberValue Atan(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Atan(x.Value));
    }

    [MemberFunction("atan2")]
    public static NumberValue Atan2(NumberValue y, NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Atan2(y.Value, x.Value));
    }

    [MemberFunction("asinh")]
    public static NumberValue Asinh(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Asinh(x.Value));
    }

    [MemberFunction("acosh")]
    public static NumberValue Acosh(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Acosh(x.Value));
    }

    [MemberFunction("atanh")]
    public static NumberValue Atanh(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Atanh(x.Value));
    }

    [MemberFunction("exp")]
    public static NumberValue Exp(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Exp(x.Value));
    }

    [MemberFunction("pow")]
    public static NumberValue Pow(NumberValue x, NumberValue y)
    {
        return NumberValue.CreateNew(MathF.Pow(x.Value, y.Value));
    }

    [MemberFunction("log")]
    public static NumberValue Log(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Log(x.Value));
    }

    [MemberFunction("log")]
    public static NumberValue Log(NumberValue x, NumberValue y)
    {
        return NumberValue.CreateNew(MathF.Log(x.Value, y.Value));
    }

    [MemberFunction("log10")]
    public static NumberValue Log10(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Log10(x.Value));
    }

    [MemberFunction("log2")]
    public static NumberValue Log2(NumberValue x)
    {
        return NumberValue.CreateNew(MathF.Log2(x.Value));
    }
}
