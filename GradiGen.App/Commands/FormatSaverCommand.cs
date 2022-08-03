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
    [Command("format-save", "Saves all of the added custom formats to a file.")]
    [Aliases("fs", "formatsave", "saveformat", "sformat")]
    [Parameter("directory", "The file directory to save to.")]
    [Parameter("name", "The file name if the file already exists.")]
    public class FormatSaverCommand : CommandBase<CommandContext>
    {
        public override async Task ExecuteAsync()
        {
            if (Context.Parameters.Count is < 1)
            {
                AnsiConsole.MarkupLine($"[red]Please specify a directory to use.[/]");
                return;
            }

            if (Context.Parameters.Count is > 2)
            {
                AnsiConsole.MarkupLine($"[red]Failed to parse parameters.[/]");
                return;
            }

            var fileSpecified = Context.Parameters.Count is 2;

            if (!Directory.Exists(Context.Parameters[0]))
            {
                AnsiConsole.MarkupLine($"[red]This directory does not exist.[/]");
                return;
            }

            if (fileSpecified)
            {
                var path = Path.Combine(Context.Parameters[0], Context.Parameters[1] + ".txt");
                if (File.Exists(path))
                {
                    using var sw = new StreamWriter(path);

                    foreach (var format in Formatter.FormatProvider)
                    {
                        await sw.WriteLineAsync($"{format.Key}:{format.Value}");
                    }

                    AnsiConsole.MarkupLine($"[grey]Wrote[/] [orange1]{Formatter.FormatProvider.Count}[/] [grey]formats to the specified file.");
                    return;
                }
                AnsiConsole.MarkupLine($"[red]The specified file name does not exist in the specified directory.[/]");
                return;
            }
            else
            {
                var fileName = AnsiConsole.Prompt(
                    new TextPrompt<string>("[grey]What name do you want to give the file to save to?[/]")
                    .Validate(x => !string.IsNullOrEmpty(x)).ValidationErrorMessage("[red]The name cannot be null or empty![/]"));

                var path = Path.Combine(Context.Parameters[0], fileName += ".txt");

                await File.WriteAllLinesAsync(path, Formatter.FormatProvider.Select(x => $"{x.Key}:{x.Value}"));

                AnsiConsole.MarkupLine($"[grey]Wrote[/] [orange1]{Formatter.FormatProvider.Count}[/] [grey]formats to the newly created file.[/]");
                return;
            }
        }
    }
}
