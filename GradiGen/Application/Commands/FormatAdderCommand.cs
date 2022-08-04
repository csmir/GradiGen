using GradiGen.Commands;
using GradiGen.Formatting;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.App.Commands
{
    [Command("format-add", "Adds a formatter to the gradient generator")]
    [Aliases("af", "addformat", "formatadd", "aformat")]
    public class FormatAdderCommand : CommandBase<CommandContext>
    {
        public override async Task ExecuteAsync()
        {
            await Task.CompletedTask;

            var format = AnsiConsole.Prompt(new TextPrompt<string>("[grey]Provide a format to add:[/]"));

            var name = AnsiConsole.Prompt(new TextPrompt<string>("[grey]Add an identifying name:[/]"));
            Formatter.AddFormatProvider(name, format);

            AnsiConsole.MarkupLine($"[grey]Added custom formatter by key:[/] [orange1]'{name}'[/]");
        }
    }
}
