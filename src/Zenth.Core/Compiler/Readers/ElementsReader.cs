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
using Zenth.Core.Shared.Extensions;

namespace Zenth.Core.Compiler.Readers;

public sealed class ElementsReader
{
    private readonly ArgumentReader _argumentReader = new();

    public int Read(in LineSource line, Span<char> destination, Span<Range> rangeDestination)
    {
        int index = 0;
        int destinationIndex = 0;
        int rangeDestinationIndex = 0;

        while (index < line.Length)
        {
            ReadOnlySpan<char> element = _argumentReader.Read(index, out index, line);

            if (element.IsNullOrWhiteSpace())
            {
                index++;
                continue;
            }

            ref char elementReference = ref MemoryMarshal.GetReference(element);
            int lastDestinationIndex = destinationIndex;

            for (int i = 0; i < element.Length; i++)
            {
                destination[destinationIndex++] = Unsafe.Add(ref elementReference, i);
            }

            rangeDestination[rangeDestinationIndex++] = new Range(lastDestinationIndex, destinationIndex);
            index++;
        }

        return rangeDestinationIndex;
    }
}
