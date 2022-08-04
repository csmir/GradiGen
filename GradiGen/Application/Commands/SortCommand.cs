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
    public class SortCommand : CommandBase<CommandContext>
    {
        public override async Task ExecuteAsync()
        {
            bool ready = false;
            List<IntegrityColor> colors = new();
            while (!ready)
            {
                var value = AnsiConsole.Prompt(new TextPrompt<string>("[grey]Please define a color to sort. (Leave empty to continue)[/]")
                    .AllowEmpty());

                if (string.IsNullOrEmpty(value))
                {
                    if (colors.Count < 5)
                        AnsiConsole.MarkupLine("[red]Please define at least 5 colors to sort.[/]");

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

            var sorted = colors.OrderByDescending(x => x, new ColorComparer());

            var table = new Table()
                .Title("Sorted all provided colors:")
                .Expand()
                .AddColumn("1")
                .AddColumn("2")
                .AddColumn("3")
                .AddColumn("4")
                .AddColumn("5")
                .RoundedBorder()
                .BorderColor(Color.Grey)
                .HideHeaders();

            int maxEntriesPerColumn = (colors.Count / 5);
            for (int i = 0; i < maxEntriesPerColumn; i++)
            {
                var markup1 = GetMarkup(colors, i);
                var markup2 = GetMarkup(colors, i + maxEntriesPerColumn);
                var markup3 = GetMarkup(colors, i + (maxEntriesPerColumn * 2));
                var markup4 = GetMarkup(colors, i + (maxEntriesPerColumn * 3));
                var markup5 = GetMarkup(colors, i + (maxEntriesPerColumn * 4));

                table.AddRow(markup1, markup2, markup3, markup4, markup5);
            }

            AnsiConsole.Write(table);
        }

        private static Markup GetMarkup(List<IntegrityColor> colors, int targetIndex)
        {
            if (colors.Count == targetIndex + 1)
            {
                var color1 = colors[targetIndex];
                return new Markup($"{color1.Color.Name}", new Style(color1.Color.ToSpectreColor()));
            }
            else
                return new Markup($" ");
        }
    }
}
