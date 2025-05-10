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

using Zenth.Core.Shared.Extensions;

namespace Zenth.Infrastructure.Repositories.Files;

public sealed class ConfigRepository : FileRepository<Dictionary<string, string>>
{
    public override void Save(string key, Dictionary<string, string> value)
    {
        try
        {
            IEnumerable<string> lines = value.Select(x => $"\"{x.Key}\" = \"{x.Value}\"");
            File.WriteAllLines(key, lines);
        }
        catch (IOException)
        {
            Console.WriteLine($"Failed to save config to path `{key}`");
        }
    }

    protected override Dictionary<string, string> OnLoaded(string[] content)
    {
        return Loading(content).ToDictionary();
    }

    private IEnumerable<KeyValuePair<string, string>> Loading(string[] content)
    {
        foreach (string line in content)
        {
            ReadOnlySpan<char> trimmedLine = line.AsSpan().Trim();

            if (CanSkipLine(trimmedLine)) continue;

            GetNameAndValueFromLine(trimmedLine, out ReadOnlySpan<char> name, out ReadOnlySpan<char> value);

            if (name.All(IsValidCharacterForName) && value.All(IsValidCharacterForValue))
            {
                yield return KeyValuePair.Create(name.ToString(), value.ToString());
                continue;
            }

            throw new ArgumentException("An invalid name or an invalid value in a config file");
        }
    }

    private void GetNameAndValueFromLine(
        ReadOnlySpan<char> line,
        out ReadOnlySpan<char> name,
        out ReadOnlySpan<char> value)
    {
        Span<Range> tokens = stackalloc Range[line.Length];
        int count = line.Split(tokens, '"', StringSplitOptions.RemoveEmptyEntries);

        if (count != 3) throw new InvalidOperationException("Invalid syntax in a config file");

        ReadOnlySpan<char> equals = line[tokens[1]].Trim();

        if (equals.StartsWith('=') == false)
        {
            throw new InvalidOperationException("Invalid syntax in a config file");
        }

        name = line[tokens[0]].Trim();
        value = line[tokens[2]].Trim();
    }

    private static bool CanSkipLine(ReadOnlySpan<char> line) =>
        line.IsNullOrWhiteSpace() || line.StartsWith("//") || line.StartsWith('#');

    private static bool IsValidCharacterForName(char character) =>
        char.IsLetter(character) || character is '_' or '-';

    private static bool IsValidCharacterForValue(char character) =>
        char.IsLetter(character) || character is ' ';
}
