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
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Compiler.Interfaces.Databases;
using Zenth.Core.Compiler.Interfaces.Factories;
using Zenth.Core.Execution.Interfaces.Contexts;
using Zenth.Core.Execution.Interfaces.Databases;
using Zenth.Infrastructure.Execution.Contexts;

namespace Zenth.Infrastructure.Execution.Factories;

public sealed class ExecutionContextFactory : IExecutionContextFactory
{
    private IHeap? _heap;

    public void Initialize(IHeap heap) => _heap = heap;

    public IRootExecutionContext CreateRootContext()
    {
        if (_heap is null) throw new NullReferenceException("Unitialized execution context factory");
        return new RootExecutionContext(this, _heap);
    }

    public IExecutionContext CreateMemberContext(
        Guid objectId,
        IRootExecutionContext baseContext)
    {
        return new MemberExecutionContext(objectId, baseContext);
    }

    public IBlockExecutionContext CreateBlockContext(
        ICompilationContext from,
        IBlockExecutionContext baseContext)
    {
        return new BlockExecutionContext(
            baseContext,
            from.ImportedLibraries,
            from.DeclaredVariables.ToExecution(baseContext.DeclaredVariables),
            from.DeclaredFunctions.ToExecution());
    }

    public IBlockExecutionContext CreateFunctionContext(
        Guid ownerId,
        ICompilationContext from,
        IExecutionContext baseContext)
    {
        MemoryObject owner = baseContext.Heap.GetObject(ownerId);
        ICompilationFunctionDatabase functions = from.DeclaredFunctions;

        if (functions.Count == 0 && functions.BaseDatabase is not null)
        {
            functions = functions.BaseDatabase;
        }

        return new FunctionExecutionContext(
            ownerId,
            baseContext,
            from.ImportedLibraries,
            from.DeclaredVariables.ToExecution(owner.Fields),
            functions.ToExecution());
    }
}
