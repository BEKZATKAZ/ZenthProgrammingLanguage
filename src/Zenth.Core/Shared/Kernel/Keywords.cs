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

namespace Zenth.Core.Shared.Kernel;

public class Keywords
{
    public static string IF { get; private set; } = string.Empty;
    public static string ELSE_IF { get; private set; } = string.Empty;
    public static string ELSE { get; private set; } = string.Empty;

    public static string FOR_LOOP { get; private set; } = string.Empty;
    public static string WHILE_LOOP { get; private set; } = string.Empty;

    public static string VARIABLE { get; private set; } = string.Empty;
    public static string FUNCTION { get; private set; } = string.Empty;
    public static string RETURN { get; private set; } = string.Empty;

    public static string IMPORT { get; private set; } = string.Empty;
    public static string BREAK { get; private set; } = string.Empty;
    public static string CONTINUE { get; private set; } = string.Empty;

    public static string CLASS { get; private set; } = string.Empty;
    public static string STRUCT { get; private set; } = string.Empty;
    public static string INTERFACE { get; private set; } = string.Empty;

    public static string PUBLIC { get; private set; } = string.Empty;
    public static string PRIVATE { get; private set; } = string.Empty;
    public static string PROTECTED { get; private set; } = string.Empty;

    public static string STATIC { get; private set; } = string.Empty;
    public static string OVERRIDE { get; private set; } = string.Empty;

    public static string TRUE { get; private set; } = string.Empty;
    public static string FALSE { get; private set; } = string.Empty;
    public static string NULL { get; private set; } = string.Empty;

    public static string NEW { get; private set; } = string.Empty;
    public static string CONSTRUCT { get; private set; } = string.Empty;
    public static string DECONSTRUCT { get; private set; } = string.Empty;

    public static string SELF { get; private set; } = string.Empty;
    public static string BASE { get; private set; } = string.Empty;
    public static string DELETE { get; private set; } = string.Empty;

    public static string AUTO_TYPE { get; private set; } = string.Empty;
    public static string ARRAY_TYPE { get; private set; } = string.Empty;
    public static string OBJECT_TYPE { get; private set; } = string.Empty;
    public static string BOOLEAN_TYPE { get; private set; } = string.Empty;
    public static string NUMBER_TYPE { get; private set; } = string.Empty;
    public static string STRING_TYPE { get; private set; } = string.Empty;
}
