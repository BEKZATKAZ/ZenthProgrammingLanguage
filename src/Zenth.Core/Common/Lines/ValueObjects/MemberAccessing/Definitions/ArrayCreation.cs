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

using Zenth.Core.Common.Expressions.Interfaces;
using Zenth.Core.Common.Values.ValueObjects;
using Zenth.Core.Common.Values.Interfaces;
using Zenth.Core.Execution.Interfaces.Contexts;
using Zenth.Core.Shared.Extensions;
using Zenth.Core.Shared.Utilities;
using Zenth.Core.Common.Lines.Interfaces;

namespace Zenth.Core.Common.Lines.ValueObjects.MemberAccessing.Definitions;

public sealed record ArrayCreation(IExpression ElementCount) : IMemberAccess
{
    public IValue Access(IBlockExecutionContext context, Guid ownerId)
    {
        int count = (int)Parser.ToNumber(ElementCount.Evaluate(context));
        if (count < 0) throw new InvalidDataException($"Element count must be 0 or higher");
        return ArrayValue.CreateNew(LinqExtensions.Create<IValue>(count, _ => new NullValue()));
    }
}
