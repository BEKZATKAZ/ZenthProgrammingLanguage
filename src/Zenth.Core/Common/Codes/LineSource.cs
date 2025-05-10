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

using Zenth.Core.Shared.Extensions;

namespace Zenth.Core.Common.Codes;

public readonly record struct LineSource(string Origin, int Number, Range SliceRange)
{
    public readonly ReadOnlySpan<char> Span => Origin.AsSpan(SliceRange);

    public static LineSource Empty => new(string.Empty, 0, Range.All);
    public readonly int Length => Span.Length;
    public readonly bool IsEmpty => string.IsNullOrWhiteSpace(Origin);
    public readonly char this[int index] => Span[index];

    public readonly LineSource this[Range range] =>
        this with { SliceRange = SliceRange.Merge(range, Origin.Length) };

    public override string ToString() => Span.ToString();

    public bool StartsWith(char character) => Span.StartsWith(character);
    public bool StartsWith(string text) => Span.StartsWith(text);

    public LineSource Trim()
    {
        ReadOnlySpan<char> trimmedSpan = Span.Trim();

        int originalStart = SliceRange.Start.IsFromEnd
            ? Origin.Length - SliceRange.Start.Value
            : SliceRange.Start.Value;

        int newStart = originalStart + Span.IndexOf(trimmedSpan);
        int newEnd = newStart + trimmedSpan.Length;

        Range newRange = new(newStart, newEnd);

        return this with { SliceRange = newRange };
    }

    public static LineSource CreateNew(string origin, int number)
    {
        return new LineSource(origin, number, Range.All);
    }

    public static LineSource CreateNew(ReadOnlySpan<char> origin, int number)
    {
        return new LineSource(origin.ToString(), number, Range.All);
    }
}
