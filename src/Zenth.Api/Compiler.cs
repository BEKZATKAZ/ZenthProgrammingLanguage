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

using Zenth.Application.Compiler;
using Zenth.Core.Common.Codes;
using Zenth.Core.Common.Interfaces.Repositories;
using Zenth.Core.Common.Members;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Execution.Interfaces.Databases;

namespace Zenth.Api;

internal sealed class Compiler
{
    internal Compiler(
        ICompilationContext context,
        IHeap heap,
        ProgramCompiler compiler,
        IStorageRepositoryMany<UncompiledCode> repository)
    {
        _context = context;
        _heap = heap;
        _compiler = compiler;
        _repository = repository;
    }

    internal string? ProjectPath { get; set; }

    private readonly ICompilationContext _context;
    private readonly IHeap _heap;
    private readonly ProgramCompiler _compiler;
    private readonly IStorageRepositoryMany<UncompiledCode> _repository;

    internal void Compile()
    {
        if (ProjectPath is null) throw new NullReferenceException("No project to compile");

        foreach (UncompiledCode script in _repository.LoadAll(ProjectPath))
        {
            _compiler.Compile(script, _context);
        }

        _context.ImportedLibraries.ImportLibrary(string.Empty);
    }

    internal void Launch()
    {
        if (!_context.ImportedLibraries.TryGetMember("Program", out MemberBase? program) || program is null)
        {
            throw new NullReferenceException("Uncompiled program class");
        }

        Console.Clear();
        Console.CursorVisible = true;
        Console.ForegroundColor = ConsoleColor.White;

        _heap.CreateObject(program);
    }
}
