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
using Zenth.Core.Common.Codes;

namespace Zenth.Core.Compiler.Readers;

public sealed class BodyReader
{
    public List<LineSource> Read(ref int index, List<LineSource> lines)
    {
        while (index < lines.Count && lines[index].StartsWith('{') == false)
        {
            index++;
        }

        int count = 1;
        int startIndex = index++;

        ReadOnlySpan<LineSource> linesSpan = CollectionsMarshal.AsSpan(lines);
        ref LineSource reference = ref MemoryMarshal.GetReference(linesSpan);

        while (index < lines.Count)
        {
            var line = Unsafe.Add(ref reference, index);

            if (line.StartsWith('{')) count++;
            else if (line.StartsWith('}') && --count == 0) break;

            index++;
        }

        if (count > 0) throw new Exception("A block must define its body");

        return lines.GetRange(startIndex + 1, index - startIndex - 1);
    }
}
