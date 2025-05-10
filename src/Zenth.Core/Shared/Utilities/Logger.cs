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

using System.Text;

namespace Zenth.Core.Shared.Utilities;

public static class Logger
{
    public static void WriteLine(string message)
    {
        Stack<ConsoleColor> colors = [];

        for (int i = 0; i < message.Length; i++)
        {
            if (message[i] is '<')
            {
                if (message[i + 1] is '/')
                {
                    colors.Pop();
                    i++;
                    continue;
                }

                i++;
                colors.Push(GetColor(message, ref i));
                continue;
            }

            if (message[i] is '>') continue;

            Console.ForegroundColor = colors.TryPeek(out ConsoleColor peekedColor)
                ? peekedColor : ConsoleColor.White;

            Console.Write(message[i]);
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write('\n');
    }

    private static ConsoleColor GetColor(string message, ref int i)
    {
        StringBuilder builder = new StringBuilder();

        while (i < message.Length)
        {
            if (message[i] is '>') return Enum.Parse<ConsoleColor>(builder.ToString(), true);
            builder.Append(message[i]);
            i++;
        }

        return ConsoleColor.White;
    }
}
