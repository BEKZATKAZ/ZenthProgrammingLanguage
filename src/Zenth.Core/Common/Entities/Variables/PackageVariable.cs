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

using Zenth.Core.Common.Enums;
using Zenth.Core.Common.Values.Interfaces;
using Zenth.Core.Execution.Interfaces.Contexts;

namespace Zenth.Core.Common.Entities.Variables;

public delegate IValue ReadDelegate(object? definition);

public delegate void WriteDelegate(object? definition, IValue newValue);

public sealed class PackageVariable : VariableBase
{
    public PackageVariable(
        Guid id, string name,
        EAccessModifier readAccess, EAccessModifier writeAccess,
        ReadDelegate reader, WriteDelegate writer) : base(id, name, readAccess, writeAccess)
    {
        _reader = reader;
        _writer = writer;
    }

    private readonly ReadDelegate _reader;
    private readonly WriteDelegate _writer;

    protected override IValue OnRead(IExecutionContext context, Guid ownerId)
    {
        PackageMemoryObject memoryObject = (PackageMemoryObject)context.Heap.GetObject(ownerId);
        return _reader(memoryObject.Definition);
    }

    protected override void OnWrite(IValue newValue, IExecutionContext context, Guid ownerId)
    {
        PackageMemoryObject memoryObject = (PackageMemoryObject)context.Heap.GetObject(ownerId);
        _writer(memoryObject.Definition, newValue);
    }

    public override VariableBase Copy()
    {
        return new PackageVariable(Id, Name, ReadAccessModifier, WriteAccessModifier, _reader, _writer);
    }
}
