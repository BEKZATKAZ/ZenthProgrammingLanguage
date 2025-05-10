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

namespace Zenth.Api.Sections;

internal sealed class SectionController
{
    internal SectionController(Dictionary<string, Section> sections)
    {
        _sections = sections;
        _runSectionKeys = new Stack<string>();
        _initialCursorTop = Console.CursorTop;
    }

    private int LastWindowWidth
    {
        get => _lastWindowWidth;
        set
        {
            if (_lastWindowWidth == value) return;
            _whiteSpace = new string(' ', value);
            _lastWindowWidth = value;
        }
    }

    private readonly Dictionary<string, Section> _sections;
    private readonly Stack<string> _runSectionKeys;
    private readonly int _initialCursorTop;
    private string? _currentSectionKey;
    private string? _whiteSpace;
    private int _lastWindowWidth;

    internal void Run(string key, bool applyHistory)
    {
        Section section = _sections[key];
        bool justOpened = true;
        int choiceIndex = -1;

        if (applyHistory) _runSectionKeys.Push(_currentSectionKey!);
        _currentSectionKey = key;

        Console.CursorVisible = false;

        while (choiceIndex < 0 || choiceIndex >= section.Choices.Length)
        {
            LastWindowWidth = Console.WindowWidth;
            int currentCursorTop = Console.CursorTop;

            for (int i = currentCursorTop; i >= _initialCursorTop; i--)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine(_whiteSpace);
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"** {section.Title} **\n");

            for (int i = 0; i < section.Choices.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($" * ");

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{i + 1}: ");

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(section.Choices[i].Text.Value);
            }

            if (justOpened)
            {
                Console.Beep(500, 100);
                justOpened = false;
            }

            Console.ForegroundColor = ConsoleColor.White;
            choiceIndex = (int)Console.ReadKey().Key - (int)ConsoleKey.D1;
        }

        Choice choice = section.Choices[choiceIndex];
        choice.Action.Invoke(this);
    }

    internal void Run()
    {
        Run(_sections.First().Key, false);
    }

    internal void Back()
    {
        Run(_runSectionKeys.Pop(), false);
    }
}
