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

using Zenth.Core.Common.Codes;
using Zenth.Core.Common.Interfaces.Repositories;

namespace Zenth.Infrastructure.Repositories.Files;

public sealed class SourceCodeRepository : FileRepository<UncompiledCode>, IStorageRepositoryMany<UncompiledCode>
{
    public override void Save(string key, UncompiledCode value)
    {
        try
        {
            IEnumerable<string> lines = value.Code.Select(x => x.Origin);
            File.WriteAllLines(key, lines);
        }
        catch (IOException)
        {
            Console.WriteLine($"Failed to save source code to path `{key}`");
        }
    }

    public IEnumerable<UncompiledCode> LoadAll(string key)
    {
        foreach (string filePath in Directory.EnumerateFiles(key, "*.zth", SearchOption.AllDirectories))
        {
            yield return OnLoaded(File.ReadAllLines(filePath)) with { FileName = Path.GetFileName(filePath) };
        }
    }

    protected override UncompiledCode OnLoaded(string[] content)
    {
        IEnumerable<LineSource> lines = content.Select((x, i) => LineSource.CreateNew(x, i + 1));
        return new UncompiledCode("", [.. lines]);
    }
}
