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

using Zenth.Core.Common.Members;
using Zenth.Core.Execution.Interfaces.Databases;

namespace Zenth.Core.Common.Entities;

public class MemoryObject
{
    protected MemoryObject(
        Guid id, MemberBase originalMember,
        IVariableDatabase fields, IFunctionDatabase functions)
    {
        Id = id;
        OriginalMember = originalMember;
        Fields = fields;
        Functions = functions;
    }

    public Guid Id { get; }
    public MemberBase OriginalMember { get; }
    public IVariableDatabase Fields { get; }
    public IFunctionDatabase Functions { get; }

    public static MemoryObject CreateNew(Guid id, MemberBase definition)
    {
        IVariableDatabase fields = definition.Fields.ToExecution();
        IFunctionDatabase functions = definition.Functions.ToExecution();
        return new MemoryObject(id, definition, fields, functions);
    }
}
