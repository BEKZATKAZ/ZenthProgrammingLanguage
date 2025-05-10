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

using System.Reflection;
using Zenth.Core.Common.Entities.Variables;
using Zenth.Core.Common.Enums;
using Zenth.Core.Common.Values.Interfaces;
using Zenth.Library.Attributes;

namespace Zenth.Library.Export;

internal static class VariableExporter
{
    internal static PackageVariable Export(PropertyInfo property, MemberVariable attribute)
    {
        EAccessModifier readAccess = property.CanRead ? EAccessModifier.Public : EAccessModifier.Private;
        EAccessModifier writeAccess = property.CanWrite ? EAccessModifier.Public : EAccessModifier.Private;

        ReadDelegate reader = (definition) =>
        {
            return (IValue)property.GetValue(definition)!;
        };

        WriteDelegate writer = (definition, newValue) =>
        {
            if (attribute.EnsureType is not null && newValue.GetType() != attribute.EnsureType)
                throw new InvalidCastException();

            property.SetValue(definition, newValue);
        };

        return new PackageVariable(Guid.NewGuid(), attribute.Name, readAccess, writeAccess, reader, writer);
    }
}
