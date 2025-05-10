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

using Microsoft.Extensions.DependencyInjection;
using Zenth.Application.Compiler;
using Zenth.Core.Common.Interfaces.Repositories;
using Zenth.Core.Common;
using Zenth.Core.Compiler.Interfaces.Contexts;
using Zenth.Core.Compiler.Interfaces.Factories;
using Zenth.Core.Shared.Kernel;
using Zenth.Infrastructure.Execution;
using Zenth.Infrastructure.Execution.Databases;
using Zenth.Infrastructure.Modifiers;
using Zenth.Infrastructure.Repositories.Files;
using Zenth.Library.Export;

namespace Zenth.Api.Installation;

internal sealed class CompilerInstaller
{
    private static T Service<T>(IServiceProvider services) where T : notnull
    {
        return services.GetRequiredService<T>();
    }

    internal static Compiler CreateCompiler(IServiceProvider services)
    {
        ICompilationContextFactory contextFactory = Service<ICompilationContextFactory>(services);
        IExecutionContextFactory executionContextFactory = Service<IExecutionContextFactory>(services);

        LineModifiers lineModifiers = new LineModifiers
        {
            Override = new OneTimeModifier(Keywords.OVERRIDE),

            Static = new OneTimeModifier(Keywords.STATIC),

            Access = new VariableAccessModifier(
                new AccessModifier(
                    new OneTimeModifier(Keywords.PUBLIC),
                    new OneTimeModifier(Keywords.PRIVATE),
                    new OneTimeModifier(Keywords.PROTECTED)),
                new AccessModifier(
                    new OneTimeModifier(Keywords.PUBLIC),
                    new OneTimeModifier(Keywords.PRIVATE),
                    new OneTimeModifier(Keywords.PROTECTED))),
        };

        contextFactory.Initialize(lineModifiers, Service<ILibraryRepository>(services));
        Exporter.ExportAll(Service<AllLibraryDatabase>(services));

        Heap heap = new Heap(executionContextFactory);
        executionContextFactory.Initialize(heap);

        IMemberFactory memberFactory = Service<IMemberFactory>(services);
        ICompilationContext rootContext = contextFactory.CreateRootContext(heap, memberFactory);
        ProgramCompiler compiler = Service<ProgramCompiler>(services);

        return new Compiler(rootContext, heap, compiler, new SourceCodeRepository());
    }
}
