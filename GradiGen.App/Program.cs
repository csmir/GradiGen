using GradiGen;
using GradiGen.Commands;
using GradiGen.Enums;
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

await manager.RegisterCommandsAsync(typeof(Program).Assembly);

bool restart = true;

while (restart)
{
    var command = AnsiConsole.Ask<string>("Command: ([red]'help'[/] for more info)");

    if (string.IsNullOrEmpty(command))
        continue;

    var ctx = CommandContext.Create(command);

    var result = await manager.ExecuteCommandAsync(ctx);

    if (!result.IsSuccess)
    {
        if (result is SearchResult search)
            AnsiConsole.MarkupLine("[red]Invalid command![/].");

        else if (result.Exception is not null)
        {
            AnsiConsole.WriteException(result.Exception);
            if (Debugger.IsDebugEnabled)
                restart = false;
        }
    }
}