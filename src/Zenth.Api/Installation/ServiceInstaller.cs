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
using Microsoft.Extensions.Hosting;
using Zenth.Application.Compiler;
using Zenth.Application.Compiler.Expressions;
using Zenth.Application.Compiler.MemberPaths;
using Zenth.Core.Common.Interfaces.Repositories;
using Zenth.Core.Compiler.Interfaces.Factories;
using Zenth.Infrastructure.Compiler.Factories;
using Zenth.Infrastructure.Execution.Databases;
using Zenth.Infrastructure.Execution.Factories;
using Zenth.Infrastructure.Repositories;

namespace Zenth.Api.Installation;

internal sealed class ServiceInstaller : IDisposable
{
    private IHost? _host;
    private IServiceScope? _score;

    internal IServiceProvider Provider => _score?.ServiceProvider
        ?? throw new NullReferenceException("Host is not initialized");

    public void Dispose()
    {
        _host?.Dispose();
        _score?.Dispose();
    }

    internal void Build()
    {
        _host = CreateBuilder().Build();
        _score = _host.Services.CreateScope();

        ExpressionCompiler expressionCompiler = Provider.GetRequiredService<ExpressionCompiler>();
        expressionCompiler.Initialize();

        Provider.GetRequiredService<CollectionCompiler>().Initialize();
        Provider.GetRequiredService<MemberCollectionCompiler>().SetExpressionCompiler(expressionCompiler);
        Provider.GetRequiredService<MemberFunctionCompiler>().SetExpressionCompiler(expressionCompiler);
        Provider.GetRequiredService<MemberNewObjectCompiler>().SetExpressionCompiler(expressionCompiler);
        Provider.GetRequiredService<MemberVariableCompiler>().SetExpressionCompiler(expressionCompiler);
    }

    private static IHostBuilder CreateBuilder()
    {
        return Host.CreateDefaultBuilder().ConfigureServices(CreateServices);
    }

    private static void CreateServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddSingleton<OperatorCompiler>();
        services.AddSingleton<ExpressionCompiler>();

        services.AddSingleton<MemberCollectionCompiler>();
        services.AddSingleton<MemberVariableCompiler>();
        services.AddSingleton<MemberFunctionCompiler>();
        services.AddSingleton<MemberNewObjectCompiler>();

        services.AddSingleton<CollectionCompiler>();
        services.AddSingleton<MemberTokenCompiler>();
        services.AddSingleton<MemberPathCompiler>();
        services.AddSingleton<VariableCompiler>();
        services.AddSingleton<FunctionCompiler>();
        services.AddSingleton<ClassCompiler>();
        services.AddSingleton<ExpressionTokenCompiler>();
        services.AddSingleton<ModifierCompiler>();
        services.AddSingleton<LineCompiler>();

        services.AddSingleton<AllLibraryDatabase>();
        services.AddSingleton<ILibraryRepository, LibraryRepository>();

        services.AddSingleton<ICompilationContextFactory, CompilationContextFactory>();
        services.AddSingleton<IExecutionContextFactory, ExecutionContextFactory>();
        services.AddSingleton<IMemberFactory, MemberFactory>();

        services.AddSingleton<ProgramBuilder>();
        services.AddSingleton<ProgramCompiler>();
    }
}
