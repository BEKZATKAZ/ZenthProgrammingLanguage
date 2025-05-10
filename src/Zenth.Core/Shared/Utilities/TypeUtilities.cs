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

using Zenth.Core.Common.Values.ValueObjects;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Shared.Kernel;

namespace Zenth.Core.Shared.Utilities;

public static class TypeUtilities
{
    public static Type? GetValueTypeFromName(ReadOnlySpan<char> name, ICompilationContext context)
    {
        if (name.SequenceEqual(Keywords.AUTO_TYPE)) return null;
        if (name.SequenceEqual(Keywords.ARRAY_TYPE)) return typeof(ArrayValue);
        if (name.SequenceEqual(Keywords.OBJECT_TYPE)) return typeof(ObjectValue);
        if (name.SequenceEqual(Keywords.BOOLEAN_TYPE)) return typeof(BooleanValue);
        if (name.SequenceEqual(Keywords.NUMBER_TYPE)) return typeof(NumberValue);
        if (name.SequenceEqual(Keywords.STRING_TYPE)) return typeof(StringValue);

        if (context.ImportedLibraries.TryGetMember(name, out _)) return typeof(ObjectValue);
        throw new NotImplementedException($"Type `{name}` does not exist");
    }
}
