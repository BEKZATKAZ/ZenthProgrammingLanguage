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

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Zenth.Core.Common;
using Zenth.Core.Common.Codes;
using Zenth.Core.Common.Lines.Interfaces;
using Zenth.Core.Common.Lines.ValueObjects.MemberAccessing;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Compiler.Readers;
using Zenth.Core.Shared.Extensions;

namespace Zenth.Application.Compiler.MemberPaths;

public sealed class MemberPathCompiler(MemberTokenCompiler tokenCompiler)
{
    public MemberPath Compile(in LineSource line, ICompilationContext context)
    {
        ReadOnlySpan<char> trimmed = new MemberPathReader().Read(0, out _, line).TakeWhile(x => x is not '(');

        Span<Range> tokens = stackalloc Range[trimmed.Length];
        int tokenCount = trimmed.Split(tokens, '.');
        IMemberAccess[] compiledTokens = CompileTokens(line, trimmed, tokens[..tokenCount], context);

        return new MemberPath(compiledTokens);
    }

    private IMemberAccess[] CompileTokens(
        in LineSource line,
        ReadOnlySpan<char> origin,
        Span<Range> tokenRanges,
        ICompilationContext context)
    {
        IMemberAccess[] members = new IMemberAccess[tokenRanges.Length];
        LineSource fromToken = line;

        MemberTokenCompilationData compilationData = new MemberTokenCompilationData
        {
            Line = line,
            Context = context
        };

        ref Range tokenRange = ref MemoryMarshal.GetReference(tokenRanges);

        for (int i = 0; i < tokenRanges.Length; i++)
        {
            ReadOnlySpan<char> token = origin[Unsafe.Add(ref tokenRange, i)];

            compilationData = compilationData with { Index = i, FromTokenLine = fromToken };
            IMemberAccess result = tokenCompiler.Compile(compilationData, token.Trim());

            fromToken = fromToken[(token.Length + 1)..];
            compilationData = compilationData with { PreviousMember = result };
            members[i] = result;
        }

        return members;
    }
}
