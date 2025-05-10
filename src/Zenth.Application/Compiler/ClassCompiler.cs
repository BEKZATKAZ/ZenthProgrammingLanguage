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

using Zenth.Core.Common;
using Zenth.Core.Common.Codes;
using Zenth.Core.Common.Lines.ValueObjects.Definitions;
using Zenth.Core.Common.Members;
using Zenth.Core.Compiler.Exceptions;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Shared.Extensions;
using Zenth.Core.Shared.Kernel;

namespace Zenth.Application.Compiler;

public sealed class ClassCompiler
{
    public Line Compile(in LineSource line, ICompilationContext context)
    {
        LineSource fromName = line[Keywords.CLASS.Length..].Trim();

        ReadOnlySpan<char> name = fromName.Span.TakeWhile(char.IsLetter);

        LineSource afterName = fromName[name.Length..].Trim();

        if (afterName.StartsWith(':'))
        {
            LineSource fromBaseClass = afterName[1..].Trim();
            return CompileDerived(line, fromBaseClass, name, context);
        }

        return Line.Create<ClassDefinition>(name.ToString(), null);
    }

    private static Line CompileDerived(
        in LineSource line,
        in LineSource fromBaseClass,
        ReadOnlySpan<char> name,
        ICompilationContext context)
    {
        context.ImportedLibraries.TryGetMember(fromBaseClass.Span, out MemberBase? member);

        if (member is null)
        {
            throw new InvalidMemberException(line, fromBaseClass.ToString(), "Invalid base class");
        }

        return Line.Create<ClassDefinition>(name.ToString(), member);
    }
}
