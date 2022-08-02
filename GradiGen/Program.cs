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

var manager = new CommandService(typeof(Program).Assembly, logLevel);

bool restart = true;
while (restart)
{
    var input = AnsiConsole.Ask<string>("Command: ([red]'help'[/] for more info)");

    if (string.IsNullOrEmpty(input))
        continue;

    var command = CommandContext.Parse(input);

    if (!await manager.ExecuteCommandAsync(command))
        restart = false;
}
