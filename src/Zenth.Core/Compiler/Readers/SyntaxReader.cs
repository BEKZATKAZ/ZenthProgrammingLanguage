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

using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Zenth.Core.Common.Codes;
using Zenth.Core.Compiler.Interfaces;

namespace Zenth.Core.Compiler.Readers;

public abstract class SyntaxReader(bool skipFirstOne = false, bool readOneAfterStopping = false) : ILineReader
{
    private readonly bool _skipFirstOne = skipFirstOne;
    private readonly bool _readOneAfterStopping = readOneAfterStopping;

    public ReadOnlySpan<char> Read(int startIndex, out int index, LineSource line)
    {
        if (_skipFirstOne) startIndex++;

        index = startIndex;

        if (index >= line.Length) return line[index].ToString();

        ref char characterReference = ref MemoryMarshal.GetReference(line.Span);

        while (++index < line.Length)
        {
            char current = Unsafe.Add(ref characterReference, index);
            char previous = Unsafe.Add(ref characterReference, index - 1);

            ELineReaderStatus status = ReadCharacters(current, previous);

            if (status == ELineReaderStatus.InvalidSyntax)
                throw new InvalidOperationException("Wrong syntax");

            if (status == ELineReaderStatus.StopReading)
            {
                if (_readOneAfterStopping && index < line.Length - 1) index++;
                break;
            }
        }

        return line[startIndex..index].Span;
    }

    protected abstract ELineReaderStatus ReadCharacters(char current, char previous);
}
