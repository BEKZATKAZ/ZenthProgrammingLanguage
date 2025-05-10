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

using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Zenth.Core.Common.Codes;
using Zenth.Core.Common.Expressions.Interfaces;
using Zenth.Core.Common.Expressions.Tokens;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Compiler.Readers;

namespace Zenth.Application.Compiler.Expressions;

public sealed class CollectionCompiler(ExpressionTokenCompiler tokenCompiler)
{
    private readonly ElementsReader _elementsReader = new();

    public void Initialize() => tokenCompiler.SetCollectionCompiler(this);

    public IExpressionToken Compile(in LineSource line, ICompilationContext context)
    {
        Span<char> elements = stackalloc char[line.Length];
        Span<Range> elementRanges = stackalloc Range[line.Length];
        int elementCount = _elementsReader.Read(line, elements, elementRanges);

        return new ArrayExpressionToken(CompileElements(line, elements, elementRanges, elementCount, context));
    }

    private ImmutableArray<IExpressionToken> CompileElements(LineSource line,
        ReadOnlySpan<char> elements,
        ReadOnlySpan<Range> elementRanges,
        int elementCount,
        ICompilationContext context)
    {
        ImmutableArray<IExpressionToken>.Builder builder = ImmutableArray.CreateBuilder<IExpressionToken>();
        ref Range rangeReference = ref MemoryMarshal.GetReference(elementRanges);

        for (int i = 0; i < elementCount; i++)
        {
            ReadOnlySpan<char> element = elements[Unsafe.Add(ref rangeReference, i)];
            builder.Add(Compile(line, element.ToString(), context));
        }

        return builder.ToImmutable();
    }

    private IExpressionToken Compile(in LineSource line, string element, ICompilationContext context)
    {
        LineSource newLine = LineSource.CreateNew(element, line.Number);
        int index = 0;
        return tokenCompiler.Compile(newLine, ref index, context);
    }
}
