using GradiGen;
using GradiGen.Commands;
using GradiGen.Enums;
using Spectre.Console;
using GradiGen.App;

Lifetime.Start();

var manager = new CommandService();

await manager.RegisterCommandsAsync(typeof(Program).Assembly);

while (Lifetime.IsRunning)
{
    var command = AnsiConsole.Ask<string>("[grey]Command: ([/][orange1]'help'[/] [grey]for more info)[/]");

    if (string.IsNullOrEmpty(command))
        continue;

    var ctx = CommandContext.Create(command);

    var result = await manager.ExecuteCommandAsync(ctx);

    if (!result.IsSuccess)
    {
        if (result is SearchResult search)
            AnsiConsole.MarkupLine($"[red]Invalid command! {search.Message}[/].");

        else if (result.Exception is not null)
        {
            AnsiConsole.WriteException(result.Exception);
            if (Debugger.IsDebugEnabled)
                Lifetime.Stop();
        }
    }
}