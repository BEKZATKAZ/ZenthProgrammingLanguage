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
using Zenth.Core.Common.Values.ValueObjects;
using Zenth.Core.Execution.Interfaces.Contexts;
using Zenth.Core.Execution.Interfaces.Databases;
using System.Collections.Immutable;
using Zenth.Core.Common.Codes;

namespace Zenth.Core.Common.Functions;

public sealed record UserFunction(
    string Name, in ImmutableArray<FunctionArgument> Arguments,
    CompiledCode Body, EAccessModifier AccessModifier) : FunctionBase(Name, AccessModifier, Arguments.Length)
{
    public override IValue Execute(Guid ownerId, IExecutionContext context, IValue[] arguments)
    {
        IBlockExecutionContext functionContext = context.Factory
            .CreateFunctionContext(ownerId, Body.CompilationContext, context);

        IVariableDatabase variables = functionContext.DeclaredVariables;

        for (int i = 0; i < Arguments.Length; i++)
        {
            FunctionArgument argument = Arguments[i];

            if (argument.RequiredType is not null && argument.RequiredType != arguments[i].GetType())
            {
                throw new InvalidCastException(
                    $"The type of the argument value `{arguments[i].GetType().Name}` " +
                    $"does not match the required data type `{argument.RequiredType.GetType()}`");
            }

            variables.Get(argument.Name).Write(arguments[i], context, ownerId, ownerId);
        }

        Body.Execute(functionContext);

        return functionContext.ReturnedValue ?? new NullValue();
    }
}
