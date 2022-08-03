using GradiGen;
using GradiGen.Commands;
using GradiGen.Enums;
using GradiGen.Extensions;
using Spectre.Console;

AnsiConsole.Write(
    new FigletText("GradiGen by Rozen")
        .Centered()
        .Color(Color.Purple));

var logLevel = AnsiConsole.Prompt(
    new SelectionPrompt<RunMode>()
        .Title("What mode are you launching the application in?")
        .AddChoices(Enum.GetValues<RunMode>()));

Debugger.IsDebugEnabled = logLevel is RunMode.Debug;

var manager = new CommandService();

bool restart = true;
while (restart)
{
    var input = AnsiConsole.Ask<string>("Command: ([red]'help'[/] for more info)");

    if (string.IsNullOrEmpty(input))
        continue;

    if (CommandContext.TryParse(input, out var context))
    {
        if (!await manager.ExecuteCommandAsync(context))
            AnsiConsole.MarkupLine("[red]Invalid command![/].");
    }
}
