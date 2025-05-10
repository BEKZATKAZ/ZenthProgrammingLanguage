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

using Zenth.Core.Common.Functions;
using Zenth.Core.Compiler.Interfaces.Databases;
using Zenth.Core.Execution.Interfaces.Databases;
using Zenth.Core.Shared.Extensions;
using Zenth.Infrastructure.Execution.Databases;

namespace Zenth.Infrastructure.Compiler.Databases;

public sealed record CompilationFunctionDatabase(
    ICompilationFunctionDatabase? BaseDatabase) : ICompilationFunctionDatabase
{
    public int Count => _functions.Count;

    private readonly Dictionary<string, FunctionOverloads> _functions = [];

    public void Declare(FunctionBase function)
    {
        FunctionOverloads overloads = _functions.GetOrAdd(function.Name, () => new FunctionOverloads());
        overloads.AddOverload(function);
    }

    public FunctionBase Get(ReadOnlySpan<char> name, int argumentCount)
    {
        bool hasFunction = _functions
            .GetAlternateLookup<ReadOnlySpan<char>>()
            .FasterTryGetValue(name, out FunctionOverloads? overloads);

        return hasFunction && overloads!.TryGetOverload(argumentCount, out FunctionBase? function)
            ? function : BaseDatabase?.Get(name, argumentCount)
            ?? throw new InvalidDataException($"No function overload `{name}` with {argumentCount} arguments");
    }

    public bool Has(ReadOnlySpan<char> name, int argumentCount)
    {
        bool hasFunction = _functions
            .GetAlternateLookup<ReadOnlySpan<char>>()
            .FasterTryGetValue(name, out FunctionOverloads? overloads);

        return hasFunction
            && overloads!.TryGetOverload(argumentCount, out _)
            || (BaseDatabase?.Has(name, argumentCount) ?? false);
    }

    public IFunctionDatabase ToExecution()
    {
        return new FunctionDatabase(_functions, BaseDatabase?.ToExecution());
    }
}
