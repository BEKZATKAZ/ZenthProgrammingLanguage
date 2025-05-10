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
using Zenth.Core.Common.Entities.Variables;
using Zenth.Core.Common.Functions;
using Zenth.Core.Common.Lines.Interfaces;
using Zenth.Core.Common.Lines.ValueObjects.Definitions;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Infrastructure.Compiler.Databases;

namespace Zenth.Infrastructure.Compiler.Contexts;

internal sealed record FunctionCompilationContext(ICompilationContext BaseContext) :
    BlockCompilationContext(BaseContext, new CompilationBlockVariableDatabase(BaseContext.DeclaredVariables)),
    ICompilationContextBuild, ICompilationContextBodyReady
{
    public ICompilationContext OnBuilt(ILine line)
    {
        if (line is not FunctionCreation functionLine)
            throw new InvalidCastException("Invalid function compilation");

        UserFunction function = (UserFunction)DeclaredFunctions
            .Get(functionLine.Name, functionLine.Arguments.Length);

        foreach (FunctionArgument argument in function.Arguments)
        {
            VariableBase variable = new UserVariable(
                Guid.NewGuid(), argument.Name, Modifiers.Access.ReadAccess, Modifiers.Access.WriteAccess);

            DeclaredVariables.Declare(variable);
        }

        return this;
    }

    public void OnReady(ILine line, CompiledCode body)
    {
        if (line is not FunctionCreation functionLine)
            throw new InvalidCastException("Invalid function compilation");

        UserFunction function = (UserFunction)DeclaredFunctions
            .Get(functionLine.Name, functionLine.Arguments.Length);

        function.Body.Reset(body);
    }
}
