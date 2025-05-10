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

using Zenth.Api;
using Zenth.Api.Configuration;
using Zenth.Api.Installation;
using Zenth.Api.Sections;
using Zenth.Api.Sections.Actions;
using Zenth.Api.Sections.Titles;

using ServiceInstaller installer = new();
installer.Build();

IServiceProvider services = installer.Provider;
LanguageConfiguration.LoadConfiguration("keywords.cfg");
Compiler program = CompilerInstaller.CreateCompiler(services);

string[] titleText =
[
    @"########\                        ##\     ##\   ##\ ",
    @"## __### |                       ## |    ## |  ## |",
    @"\_/ ### /   ######\  #######\ ########\  ## |  ## |",
    @"   ### /   ## /  ##\ ##  __##\   ##  __| ######## |",
    @"  ### /    #######  |## |  ## |  ## |    ## __ ## |",
    @" ### / ##\ ##      / ## |  ## |  ## |##\ ## || ## |",
    @"######### | ######\  ## |  ## |  \####  |## | \## |",
    @"\_________| \______| \__|  \__|   \____/ \__|  \__|"
];

MainTitle mainTitle = new(titleText, "0.1.0", 1, "https://github.com/BEKZATKAZ/ZenthProgrammingLanguage");
mainTitle.Display();

SectionController sectionController = new(new()
{
    { "MAIN", new Section("Press the following keys on your keyboard to proceed",
        new Choice(
            new DynamicText("Select project ({0})", () => program.ProjectPath is null
                ? "No project selected" : $"{program.ProjectPath} selected"),
            new NextSectionAction("PROJECT-SELECTION")),

        new Choice(new BasicText("Compile & Launch"), new BranchAction(() => program.ProjectPath is not null,
            OnTrue: new ValidatorAction(program.Compile,
                OnCompleted: null,
                OnFaulted: new NextSectionAction("COMPILE-ERROR")),
            OnFalse: new NextSectionAction("NO-PROJECT-SELECTED")))) },

    { "PROJECT-SELECTION", new Section("Select a project to launch",
        new Choice(new BasicText("Game Of Life"), new ActionChain(
            new ActionCallback(() => program.ProjectPath = "examples/GameOfLife"),
            new PreviousSectionAction())),

        new Choice(new BasicText("C# Quiz"), new ActionChain(
            new ActionCallback(() => program.ProjectPath = "examples/CSharpQuiz"),
            new PreviousSectionAction())),

        new Choice(new BasicText("Back"), new PreviousSectionAction())) },

    { "COMPILE-ERROR", new Section("Compilation error",
        new Choice(new BasicText("Back"), new PreviousSectionAction())) },

    { "NO-PROJECT-SELECTED", new Section("No project selected. Please select a project",
        new Choice(new BasicText("Back"), new PreviousSectionAction())) }
});

sectionController.Run();
Console.Clear();

try
{
    program.Launch();
}
finally
{
    Console.ReadLine();
}
