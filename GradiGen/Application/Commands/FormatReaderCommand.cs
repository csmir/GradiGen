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
    [Command("format-read", "Read formats from a file.")]
    [Aliases("fr", "formatread", "readformat", "rformat")]
    [Parameter("path", "The path to read.", true)]
    public class FormatReaderCommand : CommandBase<CommandContext>
    {
        public override async Task ExecuteAsync()
        {
            await Task.CompletedTask;

            if (Context.Parameters.Count is not 1)
            {
                AnsiConsole.MarkupLine($"[red]Please specify a path to read.[/]");
                return;
            }

            if (!File.Exists(Context.Parameters[0]))
            {
                AnsiConsole.MarkupLine($"[red]This file does not exist.[/]");
                return;
            }

            var fileInfo = new FileInfo(Context.Parameters[0]);

            if (fileInfo.Extension != ".txt")
            {
                AnsiConsole.MarkupLine($"[red]Specified an invalid file format.[/] [grey]Supported formats:[/] [orange1].txt[/]");
                return;
            }

            var txt = File.ReadAllLines(Context.Parameters[0]);

            int added = 0;

            foreach (var line in txt)
            {
                if (!line.Contains(':'))
                    continue;
                    
                var name = line[..line.IndexOf(':')];
                var format = line[(line.IndexOf(':') + 1)..];

                Formatter.AddFormatProvider(name, format);
                added++;
            }

            AnsiConsole.MarkupLine("[grey]Added[/] [orange1]{added}[/] [grey]new formats to the format provider.[/]");
        }
    }
}
