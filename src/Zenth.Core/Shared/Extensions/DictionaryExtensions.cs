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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zenth.Core.Shared.Extensions;

public static class DictionaryExtensions
{
    public static bool FasterContainsKey<TKey, TValue>(
        this Dictionary<TKey, TValue> source, TKey key)
        where TKey : notnull
    {
        ref TValue valRef = ref CollectionsMarshal.GetValueRefOrNullRef(source, key);
        return Unsafe.IsNullRef(ref valRef) == false;
    }

    public static bool FasterContainsKey<TKey, TValue, TAlternateKey>(
        this Dictionary<TKey, TValue>.AlternateLookup<TAlternateKey> source,
        TAlternateKey key)
        where TKey : notnull where TAlternateKey : notnull, allows ref struct
    {
        ref TValue valRef = ref CollectionsMarshal.GetValueRefOrNullRef(source, key);
        return Unsafe.IsNullRef(ref valRef) == false;
    }

    public static bool FasterTryGetValue<TKey, TValue>(
        this Dictionary<TKey, TValue> source, TKey key, [MaybeNullWhen(false)] out TValue value)
        where TKey : notnull
    {
        ref TValue valRef = ref CollectionsMarshal.GetValueRefOrNullRef(source, key);

        if (Unsafe.IsNullRef(ref valRef))
        {
            value = default;
            return false;
        }

        value = valRef;
        return true;
    }

    public static bool FasterTryGetValue<TKey, TValue, TAlternateKey>(
        this Dictionary<TKey, TValue>.AlternateLookup<TAlternateKey> source,
        TAlternateKey key, [MaybeNullWhen(false)] out TValue value)
        where TKey : notnull where TAlternateKey : notnull, allows ref struct
    {
        ref TValue valRef = ref CollectionsMarshal.GetValueRefOrNullRef(source, key);

        if (Unsafe.IsNullRef(ref valRef))
        {
            value = default;
            return false;
        }

        value = valRef;
        return true;
    }

    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> source, TKey key, Func<TValue> value)
        where TKey : notnull
    {
        ref TValue? val = ref CollectionsMarshal.GetValueRefOrAddDefault(source, key, out bool exists);
        if (exists == false) val = value();
        return val!;
    }

    public static TValue GetOrAdd<TKey, TValue, TAlternateKey>(
        this Dictionary<TKey, TValue>.AlternateLookup<TAlternateKey> source,
        TAlternateKey key, Func<TValue> value)
        where TKey : notnull where TAlternateKey : notnull, allows ref struct
    {
        ref TValue? val = ref CollectionsMarshal.GetValueRefOrAddDefault(source, key, out bool exists);
        if (exists == false) val = value();
        return val!;
    }
}
