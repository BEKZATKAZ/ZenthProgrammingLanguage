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
using Zenth.Core.Common;
using Zenth.Core.Common.Codes;
using Zenth.Core.Common.Functions;
using Zenth.Core.Common.Lines.ValueObjects.Definitions;
using Zenth.Core.Compiler.Exceptions;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Compiler.Readers;
using Zenth.Core.Shared.Kernel;
using Zenth.Core.Shared.Utilities;

namespace Zenth.Application.Compiler;

public sealed class FunctionCompiler
{
    private readonly IdentifierReader _identifierReader = new();
    private readonly SkipReader _argumentsReader = new(new ReaderFull('(', ')'), ' ');

    public Line Compile(LineSource line, ICompilationContext context)
    {
        LineSource fromName = line[Keywords.FUNCTION.Length..].Trim();
        int index = 0;

        ReadOnlySpan<char> functionName = _identifierReader.Read(index, out index, fromName);
        Span<Range> functionNameSplit = stackalloc Range[functionName.Length];
        Span<Range> argumentSplit = stackalloc Range[3];

        ReadOnlySpan<char> arguments = _argumentsReader.Read(0, out _, fromName[index..].Trim());
        int argumentCount = arguments.Split(functionNameSplit, ',', StringSplitOptions.RemoveEmptyEntries);
        var argumentArrayBuilder = ImmutableArray.CreateBuilder<FunctionArgument>();

        ref Range argumentRange = ref MemoryMarshal.GetReference(functionNameSplit);

        for (int i = 0; i < argumentCount; i++)
        {
            ReadOnlySpan<char> argument = arguments[Unsafe.Add(ref argumentRange, i)].Trim();
            int splitCount = argument.Split(argumentSplit, ':', StringSplitOptions.RemoveEmptyEntries);

            ReadOnlySpan<char> argumentName = default;
            Type? requiredType = null;

            if (splitCount == 1) argumentName = argument;

            else if (splitCount == 2)
            {
                argumentName = argument[argumentSplit[0]].Trim();
                requiredType = TypeUtilities.GetValueTypeFromName(argument[argumentSplit[1]].Trim(), context);
            }

            else throw new SyntaxException(line, "Arguments declared not properly");

            argumentArrayBuilder.Add(new FunctionArgument(argumentName.ToString(), requiredType));
        }

        ImmutableArray<FunctionArgument> passingArguments = argumentArrayBuilder.ToImmutable();

        if (context.DeclaredFunctions.Has(functionName, argumentCount))
        {
            if (context.Modifiers.Override.IsApplied == false)
                throw new DuplicateException(line, functionName.ToString());

            return Line.Create<FunctionCreation>(functionName.ToString(), passingArguments);
        }

        context.Modifiers.Override.ThrowIfApplied(line);
        return Line.Create<FunctionCreation>(functionName.ToString(), passingArguments);
    }
}
