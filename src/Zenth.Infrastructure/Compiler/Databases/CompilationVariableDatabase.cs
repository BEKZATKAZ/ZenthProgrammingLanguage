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

using Zenth.Core.Common.Entities.Variables;
using Zenth.Core.Compiler.Interfaces.Databases;
using Zenth.Core.Execution.Interfaces.Databases;
using Zenth.Core.Shared.Extensions;
using Zenth.Infrastructure.Execution.Databases;

namespace Zenth.Infrastructure.Compiler.Databases;

public record CompilationVariableDatabase(
    ICompilationVariableDatabase? BaseDatabase) : ICompilationVariableDatabase
{
    public int Count => Variables.Count;

    protected readonly Dictionary<string, VariableBase> Variables = [];

    public virtual IVariableDatabase ToExecution(IVariableDatabase? existingBaseDatabase = null)
    {
        Dictionary<string, VariableBase> variables = Variables.ToDictionary(k => k.Key, x => x.Value.Copy());
        IVariableDatabase? baseDatabase = existingBaseDatabase ?? BaseDatabase?.ToExecution();
        return new VariableDatabase(variables, baseDatabase);
    }

    public virtual void Declare(VariableBase variable)
    {
        if (Variables.FasterContainsKey(variable.Name))
            throw new ArgumentException($"Already has variable `{variable.Name}`");

        Variables.Add(variable.Name, variable);
    }

    public bool Has(ReadOnlySpan<char> variableName) =>
        Variables.GetAlternateLookup<ReadOnlySpan<char>>().FasterContainsKey(variableName) ||
        (BaseDatabase?.Has(variableName) ?? false);

    public VariableBase Get(ReadOnlySpan<char> variableName) =>
        Variables.GetAlternateLookup<ReadOnlySpan<char>>()
            .FasterTryGetValue(variableName, out VariableBase? variable)
            ? variable : BaseDatabase?.Get(variableName)
            ?? throw new InvalidDataException($"Variable `{variableName}` does not exist");
}
