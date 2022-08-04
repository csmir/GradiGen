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

            int maxEntriesPerColumn = (int)Math.Ceiling(colors.Count / 5f);
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
            if (colors.Count > targetIndex + 1)
            {
                var color1 = colors[targetIndex];
                return new Markup($"{color1.Color.Name}", new Style(color1.Color.ToSpectreColor()));
            }
            else
                return new Markup($" ");
        }
    }
}
