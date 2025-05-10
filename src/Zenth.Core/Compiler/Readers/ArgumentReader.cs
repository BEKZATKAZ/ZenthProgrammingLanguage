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
using Zenth.Core.Compiler.Interfaces;

namespace Zenth.Core.Compiler.Readers;

public sealed class ArgumentReader : ILineReader
{
    private readonly StringValueReader _stringReader = new(true, false);
    private readonly ReaderFull _fullReader = new('(', ')');
    private readonly ReaderFull _collectionReader = new('[', ']');

    public ReadOnlySpan<char> Read(int startIndex, out int index, LineSource line)
    {
        ref char characterReference = ref MemoryMarshal.GetReference(line.Span);

        ILineReader? currentReader = null;
        int parentheses = 0;
        index = startIndex;

        while (index < line.Length)
        {
            char current = Unsafe.Add(ref characterReference, index);

            if (current is ',' || current is ')' or ']' && --parentheses <= 0)
            {
                if (currentReader == _fullReader || currentReader == _collectionReader) index++;
                break;
            }

            currentReader = current switch
            {
                '(' => _fullReader,
                '[' => _collectionReader,
                '"' => _stringReader,
                _ => null
            };

            if (currentReader is not null)
            {
                currentReader.Read(index, out index, line);
                index++;
                continue;
            }

            index++;
        }

        return line[startIndex..index].Trim().Span;
    }
}
