using GradiGen.Colors;
using GradiGen.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using GradiGen.Extensions;

namespace GradiGen.Application.Commands
{
    [Command("sort", "Sorts a range of self-defined colors.")]
    [Aliases("s")]
    [Parameter("path", "A predefined file path with a range of colors.", false)]
    public class SortCommand : CommandBase<CommandContext>
    {
        public override async Task ExecuteAsync()
        {
            List<IntegrityColor> colors = new();
            bool fromFile = Context.Parameters.Count is 1;

            if (fromFile)
            {
                if (!File.Exists(Context.Parameters[0]))
                {
                    AnsiConsole.MarkupLine("[red]Failed to find file from the defined path.[/]");
                    return;
                }

                var fileInfo = new FileInfo(Context.Parameters[0]);

                if (fileInfo.Extension is not ".txt")
                {
                    AnsiConsole.MarkupLine("[red]Please define a .txt file.[/]");
                    return;
                }

                var lines = await File.ReadAllLinesAsync(fileInfo.FullName);

                foreach (var line in lines)
                {
                    if (IntegrityColor.TryParse(line, out var color))
                        colors.Add(color);
                }

                if (!colors.Any())
                {
                    AnsiConsole.MarkupLine("[red]Unable to parse provided colors.[/]");
                    return;
                }

                AnsiConsole.MarkupLine($"[grey]Added {colors.Count} colors from provided file.[/]");
            }
            else
            {
                bool ready = false;
                while (!ready)
                {
                    var value = AnsiConsole.Prompt(new TextPrompt<string>("[grey]Please define a color to sort. (Leave empty to continue)[/]")
                        .AllowEmpty());

                    if (string.IsNullOrEmpty(value))
                    {
                        if (colors.Count < 2)
                            AnsiConsole.MarkupLine("[red]Please define at least 2 colors to sort.[/]");

                        ready = true;
                        continue;
                    }

                    if (!IntegrityColor.TryParse(value, out var color))
                    {
                        AnsiConsole.MarkupLine("[red]Failed to parse input. Please retry with a valid value.[/]");
                        continue;
                    }
                    colors.Add(color);
                }
            }

            var sorted = colors.OrderByDescending(x => x, new ColorComparer());

            var table = new Table()
                .Title("Sorted all provided colors:")
                .AddColumn("chart")
                .BorderColor(Color.Grey)
                .RoundedBorder();

            var barChart = new BarChart()
                .LeftAlignLabel()
                .ShowValues(false);

            for (int i = 0; i < colors.Count; i++)
            {
                barChart.AddItem(
                    label: $"{colors[i].ToString(ColorType.RGB)} ({colors[i].ToString(ColorType.Hex)})", 
                    value: 100, 
                    color: colors[i].Color.ToSpectreColor());
            }

            table.AddRow(barChart);

            AnsiConsole.Write(barChart);

            if (AnsiConsole.Confirm("[grey]Do you want to save the reorganized colors?[/]"))
            {
                var type = AnsiConsole.Prompt(new SelectionPrompt<ColorType>()
                    .Title("[grey]What format do you want to save the colors in?[/]")
                    .AddChoices(Enum.GetValues<ColorType>().Where(x => x is not ColorType.Name)));

                string fileName = string.Empty;
                if (fromFile)
                    fileName = Context.Parameters[0];
                else
                    fileName = AnsiConsole.Prompt(new TextPrompt<string>("[grey]What file do you want to save the colors to?[/]"));

                await File.WriteAllLinesAsync(fileName, colors.Select(x => x.ToString(type)));

                AnsiConsole.MarkupLine("[grey]Wrote colors to file succesfully.[/]");
            }
            else
                AnsiConsole.MarkupLine("[grey]Skipped saving sorted colors.[/]");
        }
    }
}
