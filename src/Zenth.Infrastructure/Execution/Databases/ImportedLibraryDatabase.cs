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

using Zenth.Core.Common.Entities;
using Zenth.Core.Common.Interfaces.Repositories;
using Zenth.Core.Common.Members;
using Zenth.Core.Execution.Interfaces.Databases;

namespace Zenth.Infrastructure.Execution.Databases;

public sealed record ImportedLibraryDatabase(ILibraryRepository Repository) : ILibraryDatabase
{
    private readonly List<string> _imported = [string.Empty];

    public void ImportLibrary(ReadOnlySpan<char> path)
    {
        if (Repository.TryGetLibrary(path, out _) == false)
            throw new ArgumentException($"Cannot import library `{path}`");

        _imported.Add(path.ToString());
    }

    public bool TryGetMember(ReadOnlySpan<char> memberName, out MemberBase? member)
    {
        MemberBase? reference = null;

        foreach (string imported in _imported)
        {
            bool libraryExists = Repository.TryGetLibrary(imported, out Directive? directive);
            if (libraryExists && directive!.TryGetMember(memberName, out reference)) break;
        }

        member = reference;
        return reference is not null;
    }
}
