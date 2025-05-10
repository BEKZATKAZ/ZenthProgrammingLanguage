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

using Zenth.Core.Common.Values.Interfaces;
using Zenth.Core.Common.Values.ValueObjects;
using Zenth.Library.Attributes;

namespace Zenth.Library.Source;

[PackageMember("Console")]
public sealed class ConsoleClass
{
    [MemberVariable("cursorLeft")]
    public static NumberValue CursorLeft
    {
        get => NumberValue.CreateNew(Console.CursorLeft);
        set => Console.CursorLeft = (int)value.Value;
    }

    [MemberVariable("cursorTop")]
    public static NumberValue CursorTop
    {
        get => NumberValue.CreateNew(Console.CursorTop);
        set => Console.CursorTop = (int)value.Value;
    }

    [MemberVariable("textColor")]
    public static NumberValue TextColor
    {
        get => NumberValue.CreateNew((int)Console.ForegroundColor);
        set => Console.ForegroundColor = (ConsoleColor)value.Value;
    }

    [MemberVariable("backgroundColor")]
    public static NumberValue BackgroundColor
    {
        get => NumberValue.CreateNew((int)Console.BackgroundColor);
        set => Console.BackgroundColor = (ConsoleColor)value.Value;
    }

    [MemberVariable("isCursorVisible")]
    public static BooleanValue IsCursorVisible
    {
        get => BooleanValue.CreateNew(Console.CursorVisible);
        set => Console.CursorVisible = value.Value;
    }

    [MemberVariable("title")]
    public static IValue Title
    {
        get => StringValue.CreateNew(Console.Title);
        set => Console.Title = value.ToString()!;
    }

    [MemberFunction("clear")]
    public static void Clear()
    {
        Console.Clear();
    }

    [MemberFunction("beep")]
    public static void Beep(NumberValue frequency, NumberValue duration)
    {
        Console.Beep((int)frequency.Value, (int)duration.Value);
    }

    [MemberFunction("readLine")]
    public static IValue ReadLine()
    {
        return StringValue.CreateNew(Console.ReadLine() ?? "");
    }

    [MemberFunction("readKey")]
    public static IValue ReadKey()
    {
        return NumberValue.CreateNew((int)Console.ReadKey().Key);
    }

    [MemberFunction("print")]
    public static void Print(IValue message)
    {
        Console.Write(message.ToString());
    }

    [MemberFunction("printLine")]
    public static void PrintLine()
    {
        Console.WriteLine();
    }

    [MemberFunction("printLine")]
    public static void PrintLine(IValue message)
    {
        Console.WriteLine(message.ToString());
    }
}
