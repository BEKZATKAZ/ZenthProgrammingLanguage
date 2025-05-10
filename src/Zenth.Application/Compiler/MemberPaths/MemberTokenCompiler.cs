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
using Zenth.Core.Common.Lines.ValueObjects.MemberAccessing;
using Zenth.Core.Compiler.Exceptions;
using Zenth.Core.Shared.Extensions;
using Zenth.Core.Shared.Kernel;

namespace Zenth.Application.Compiler.MemberPaths;

public sealed class MemberTokenCompiler(
    MemberVariableCompiler variables,
    MemberCollectionCompiler collections,
    MemberFunctionCompiler functions,
    MemberNewObjectCompiler objectCreation)
{
    public IMemberAccess Compile(MemberTokenCompilationData data, ReadOnlySpan<char> token)
    {
        if (token.Contains('[')) return collections.Compile(data, token, Compile);

        if (data.FromTokenLine[token.Length..].Trim().StartsWith('('))
        {
            if (data.Index == 0 && token.StartsWith($"{Keywords.NEW} "))
                return objectCreation.Compile(data, token);

            return functions.Compile(data, token);
        }

        IMemberAccess? result = data.Index == 0 ? CompileFirstToken(data, token) : null;
        return result ?? variables.Compile(data, token.TakeWhile(x => x is not ')'));
    }

    private IMemberAccess? CompileFirstToken(MemberTokenCompilationData data, ReadOnlySpan<char> token)
    {
        if (token.SequenceEqual(Keywords.SELF))
            return new ThisMember();

        if (token.SequenceEqual(Keywords.BASE))
            return new BaseMember();

        if (token.StartsWith($"{Keywords.NEW} "))
            return objectCreation.Compile(data, token);

        if (data.Context.DeclaredVariables.Has(token) == false)
            throw new InvalidVariableException(data.Line, token.ToString());

        return null;
    }
}
