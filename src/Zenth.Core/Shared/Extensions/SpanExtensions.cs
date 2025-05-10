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

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zenth.Core.Shared.Extensions;

public static class SpanExtensions
{
    public static ReadOnlySpan<T> TakeWhile<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
    {
        ref T elementReference = ref MemoryMarshal.GetReference(source);

        for (int i = 0; i < source.Length; i++)
        {
            T element = Unsafe.Add(ref elementReference, i);
            if (predicate(element) == false) return source[..i];
        }

        return source;
    }

    public static bool All<T>(this ReadOnlySpan<T> source, Func<T, bool> predicate)
    {
        ref T elementReference = ref MemoryMarshal.GetReference(source);

        for (int i = 0; i < source.Length; i++)
        {
            T element = Unsafe.Add(ref elementReference, i);
            if (predicate(element) == false) return false;
        }

        return true;
    }

    public static bool IsNullOrWhiteSpace(this ReadOnlySpan<char> source)
    {
        ref char elementReference = ref MemoryMarshal.GetReference(source);

        for (int i = 0; i < source.Length; i++)
        {
            char element = Unsafe.Add(ref elementReference, i);
            if (char.IsWhiteSpace(element) == false) return false;
        }

        return true;
    }
}
