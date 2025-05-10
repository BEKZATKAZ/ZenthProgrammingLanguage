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
using Zenth.Core.Common.Functions;
using Zenth.Core.Common.Members;
using Zenth.Core.Common.Values.Interfaces;
using Zenth.Core.Compiler.Interfaces.Factories;
using Zenth.Core.Execution.Interfaces.Contexts;
using Zenth.Core.Execution.Interfaces.Databases;
using Zenth.Core.Shared.Extensions;
using Zenth.Core.Shared.Kernel;

namespace Zenth.Infrastructure.Execution;

public sealed record Heap(IExecutionContextFactory ContextFactory) : IHeap
{
    private readonly Dictionary<Guid, MemoryObject> _objects = [];

    public MemoryObject CreateObject(MemberBase definition, IValue[]? arguments = null)
    {
        Guid id = Guid.NewGuid();

        MemoryObject memoryObject = definition is PackageClassMember
            ? PackageMemoryObject.CreateNew(id, definition)
            : MemoryObject.CreateNew(id, definition);

        _objects.Add(id, memoryObject);

        if (memoryObject is PackageMemoryObject packageMemoryObject)
        {
            packageMemoryObject.CreateDefinition(arguments ?? []);
            return memoryObject;
        }

        if (memoryObject.Functions.Has(Keywords.CONSTRUCT) == false)
        {
            if ((arguments?.Length ?? 0) > 0) throw new Exception();
            return memoryObject;
        }

        FunctionBase constructor = memoryObject.Functions.Get(Keywords.CONSTRUCT, arguments?.Length ?? 0);

        IRootExecutionContext root = ContextFactory.CreateRootContext();
        IExecutionContext context = ContextFactory.CreateMemberContext(id, root);
        constructor.Execute(id, context, arguments ?? []);

        return memoryObject;
    }

    public void DeleteObject(Guid reference, IExecutionContext context)
    {
        if (_objects.FasterTryGetValue(reference, out MemoryObject? memoryObject) == false)
        {
            throw new NullReferenceException("Object does not exist or is already deleted");
        }

        if (memoryObject.Functions.Has(Keywords.DECONSTRUCT))
        {
            FunctionBase deconstructor = memoryObject.Functions.Get(Keywords.DECONSTRUCT, 0);
            deconstructor.Execute(reference, context, []);
        }

        _objects.Remove(reference);
    }

    public MemoryObject GetObject(Guid reference) => _objects[reference];
}
