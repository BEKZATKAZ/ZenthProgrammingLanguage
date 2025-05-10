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

namespace Zenth.Core.Shared.Extensions;

public static class RangeExtensions
{
    public static Range Merge(this Range current, Range other, int length)
    {
        int start = current.Start.IsFromEnd
            ? length - current.Start.Value
            : current.Start.Value;

        int end = current.End.IsFromEnd
            ? length - current.End.Value
            : current.End.Value;

        int additionalStart = other.Start.IsFromEnd
            ? end - other.Start.Value
            : start + other.Start.Value;

        int additionalEnd = other.End.IsFromEnd
            ? end - other.End.Value
            : start + other.End.Value;

        return new Range(additionalStart, additionalEnd);
    }
}
