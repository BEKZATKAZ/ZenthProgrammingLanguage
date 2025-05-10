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

namespace Zenth.Api.Sections.Titles;

internal class MainTitle
{
    internal MainTitle(string[] title, string version, uint build, string repository)
    {
        _title = string.Join('\n', title.Select(x => $"   {x}"));
        _version = version;
        _build = build;
        _titleWidth = title[0].Length + 3;
        _repository = repository;
    }

    private readonly string _title;
    private readonly string _version;
    private readonly string _repository;
    private readonly uint _build;
    private readonly int _titleWidth;

    internal void Display()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"\n{_title}\n");
        Console.ForegroundColor = ConsoleColor.White;

        DisplayCopyright();
        DisplayParameter("Version", _version, ConsoleColor.Yellow);
        DisplayParameter("Build", _build, ConsoleColor.Yellow);
        DisplayParameter("Framework", RuntimeInformation.FrameworkDescription, ConsoleColor.Cyan);
        DisplayParameter("Repository", _repository, ConsoleColor.Red);

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
    }

    private static void DisplayParameter<T>(string name, T value, ConsoleColor color)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" * ");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{$"{name}", -11}: ");

        Console.ForegroundColor = color;
        Console.WriteLine(value);
        Console.ForegroundColor = ConsoleColor.White;
    }

    private void DisplayCopyright()
    {
        string text = "(c) 2025 BEKZATKAZ. All rights reserved.\n";
        int leftPadding = (_titleWidth - text.Length) / 2;
        Console.CursorLeft = leftPadding;
        Console.WriteLine(text);
    }
}
