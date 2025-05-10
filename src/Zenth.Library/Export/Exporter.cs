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
using Zenth.Core.Common.Entities;
using Zenth.Core.Common.Entities.Variables;
using Zenth.Core.Common.Functions;
using Zenth.Core.Common.Members;
using Zenth.Core.Shared.Extensions;
using Zenth.Infrastructure.Compiler.Databases;
using Zenth.Infrastructure.Execution.Databases;
using Zenth.Library.Attributes;

namespace Zenth.Library.Export;

public sealed class Exporter
{
    public static void ExportAll(AllLibraryDatabase database)
    {
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.GetCustomAttribute<PackageMember>() is not null))
        {
            ClassMember member = ExportMember(type);
            PackageMember attribute = type.GetCustomAttribute<PackageMember>()!;

            string path = attribute.GetFullPath("zenth");
            Directive directive = database.Libraries.GetOrAdd(path, () => new Directive([]));
            directive.TryAddMember(attribute.Name, member);
        }
    }

    private static PackageClassMember ExportMember(Type memberType)
    {
        IEnumerable<PackageFunction> functions = memberType.GetMethods()
            .Where(x => x.GetCustomAttribute<MemberFunction>() is not null)
            .Select(x => FunctionExporter.Export(x, x.GetCustomAttribute<MemberFunction>()!));

        IEnumerable<PackageVariable> variables = memberType.GetProperties()
            .Where(x => x.GetCustomAttribute<MemberVariable>() is not null)
            .Select(x => VariableExporter.Export(x, x.GetCustomAttribute<MemberVariable>()!));

        CompilationFunctionDatabase functionDatabase = new CompilationFunctionDatabase(null);
        CompilationVariableDatabase variableDatabase = new CompilationVariableDatabase(null);

        foreach (PackageFunction function in functions) functionDatabase.Declare(function);
        foreach (PackageVariable variable in variables) variableDatabase.Declare(variable);

        return new PackageClassMember(memberType, null, variableDatabase, functionDatabase);
    }
}
