using GradiGen.Colors;
using GradiGen.Commands;
using GradiGen.Extensions;
using Spectre.Console;

namespace GradiGen.App.Commands
{
    [Command("color-list", "Displays the full list of available console colors.")]
    [Aliases("c-list", "colors", "clist")]
    public class ColorsCommand : CommandBase<CommandContext>
    {
        public override async Task ExecuteAsync()
        {
            await Task.CompletedTask;

            var colors = Spectrum.GetSortedSpectrum().Items;

            var table = new Table()
                .Title("All available colors:")
                .Expand()
                .AddColumn("1")
                .AddColumn("2")
                .AddColumn("3")
                .AddColumn("4")
                .AddColumn("5")
                .RoundedBorder()
                .BorderColor(Spectre.Console.Color.Grey)
                .HideHeaders();

            int maxEntriesPerColumn = (colors.Count / 5);
            for (int i = 1; i < maxEntriesPerColumn - 1; i++)
            {
                var color1 = colors[i];
                var color2 = colors[i + maxEntriesPerColumn];
                var color3 = colors[i + (maxEntriesPerColumn * 2)];
                var color4 = colors[i + (maxEntriesPerColumn * 3)];
                var color5 = colors[i + (maxEntriesPerColumn * 4)];

                table.AddRow(
                    new Markup($"{color1.Color.Name}", new Style(color1.Color.ToSpectreColor())),
                    new Markup($"{color2.Color.Name}", new Style(color2.Color.ToSpectreColor())),
                    new Markup($"{color3.Color.Name}", new Style(color3.Color.ToSpectreColor())),
                    new Markup($"{color4.Color.Name}", new Style(color4.Color.ToSpectreColor())),
                    new Markup($"{color5.Color.Name}", new Style(color5.Color.ToSpectreColor())));
            }

            AnsiConsole.Write(table);
        }
    }
}
