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

using Zenth.Core.Common.Lines.Interfaces;
using Zenth.Core.Common.Lines.ValueObjects.Definitions;
using Zenth.Core.Common.Members;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Compiler.Interfaces.Databases;

namespace Zenth.Infrastructure.Compiler.Contexts;

internal sealed record MemberCompilationContext(
    ICompilationContext BaseContext,
    ICompilationVariableDatabase DeclaredVariables) :
    BlockCompilationContext(BaseContext, DeclaredVariables), ICompilationContextBuild
{
    public ICompilationContext OnBuilt(ILine line)
    {
        if (line is not ClassDefinition classDefinition)
            throw new InvalidCastException("Member compilation must go after defining a class or a struct");

        string memberName = classDefinition.Name;

        BaseContext.ImportedLibraries.TryGetMember(memberName, out MemberBase? member);

        if (member is null || member is not CustomMember)
            throw new NullReferenceException($"Invalid member `{memberName}`");

        return this with { DeclaredVariables = member.Fields, DeclaredFunctions = member.Functions };
    }
}
