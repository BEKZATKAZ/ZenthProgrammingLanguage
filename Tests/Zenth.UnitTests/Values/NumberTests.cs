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

using NUnit.Framework;
using Zenth.Domain.Common.ValueObjects.Calculations;
using Zenth.Domain.Common.Values.Interfaces;
using Zenth.Domain.Common.Values.ValueObjects;

namespace Zenth.UnitTests.Values;

[TestFixture]
public sealed class NumberTests
{
    [TestCase(100, 1, 101)]
    [TestCase(50, -10, 40)]
    [TestCase(0.4f, 10, 10.4f)]
    [TestCase(0, 1, 1)]
    [TestCase(0, 0, 0)]
    public void Addition(float a, float b, float expected)
    {
        Calculate(new NumberAddition(), NumberValue.CreateNew(a), NumberValue.CreateNew(b),
            NumberValue.CreateNew(expected), message: "Addition failed");
    }

    [TestCase(100, 1, 99)]
    [TestCase(50, -10, 60)]
    [TestCase(10.4f, 2, 8.4f)]
    [TestCase(0, 1, -1)]
    [TestCase(0, 0, 0)]
    public void Substraction(float a, float b, float expected)
    {
        Calculate(new NumberSubstraction(), NumberValue.CreateNew(a), NumberValue.CreateNew(b),
            NumberValue.CreateNew(expected), message: "Substraction failed");
    }

    [TestCase(100, 5, 500)]
    [TestCase(1.1f, 4, 4.4f)]
    [TestCase(0, -1, 0)]
    [TestCase(1, -1, -1)]
    [TestCase(0, 0, 0)]
    public void Multiplication(float a, float b, float expected)
    {
        Calculate(new NumberMultiplication(), NumberValue.CreateNew(a), NumberValue.CreateNew(b),
            NumberValue.CreateNew(expected), message: "Multiplication failed");
    }

    private void Calculate(IValueCalculator calculator, IValue a, IValue b, IValue expected, string message)
    {
        IValue result = null!;
        Assert.DoesNotThrow(() => result = calculator.Calculate(a, b), message);
        Assert.That(result.ToString() == expected.ToString(), Is.True, message);
    }
}
