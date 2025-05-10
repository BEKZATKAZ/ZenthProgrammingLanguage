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
using Zenth.Core.Common.Codes;
using Zenth.Core.Common.Interfaces.Modifiers;

namespace Zenth.Infrastructure.Modifiers;

public class ModifierGroup : IModifier
{
    public ModifierGroup(in ImmutableArray<IModifierWithKeyword> modifiers)
    {
        _modifiers = modifiers;
    }

    public bool IsApplied => _modifiers.Any(x => x.IsApplied);

    private readonly ImmutableArray<IModifierWithKeyword> _modifiers;

    public bool ApplyIfStartsWith(LineSource line, out string? keyword)
    {
        string? outKeyword = null;

        IModifierWithKeyword? modifier = _modifiers
            .FirstOrDefault(x => x.ApplyIfStartsWith(line, out outKeyword));

        keyword = outKeyword;

        if (modifier is null) return false;

        keyword = modifier.Keyword;

        return true;
    }

    public void Reset()
    {
        foreach (IModifier modifier in _modifiers)
        {
            modifier.Reset();
        }
    }

    public void ThrowIfApplied(in LineSource line, string? message = null)
    {
        foreach (IModifier modifier in _modifiers)
        {
            modifier.ThrowIfApplied(line, message);
        }
    }
}
