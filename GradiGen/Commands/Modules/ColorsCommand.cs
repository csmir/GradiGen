using GradiGen.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace GradiGen.Commands.Modules
{
    [Command("colors", "Displays the full list of available console colors.")]
    public class ColorsCommand : ICommand
    {
        public async Task ExecuteAsync(CommandContext context)
        {
            await Task.CompletedTask;

            var names = Enum.GetNames<KnownColor>();

            List<System.Drawing.Color> colors = new();

            foreach (var name in names)
            {
                if (string.IsNullOrEmpty(name))
                    continue;

                colors.Add(System.Drawing.Color.FromName(name));
            }

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

            //var (r, g, b) = ((float)color.R, (float)color.G, (float)color.B);
            //float total = r + g + b;
            //r /= total;
            //g /= total;
            //b /= total;
            //var result = (total / 765) * 4 + r * 3 + g * 2 + b;

            //(R*65025+2) + (G*255+1) + B (old)

            colors = colors.OrderByDescending(x => x, new ColorComparer()).ToList();

            int maxEntriesPerColumn = (colors.Count / 5);
            for (int i = 1; i < maxEntriesPerColumn - 1; i++)
            {
                var color1 = colors[i];
                var color2 = colors[i + maxEntriesPerColumn];
                var color3 = colors[i + (maxEntriesPerColumn * 2)];
                var color4 = colors[i + (maxEntriesPerColumn * 3)];
                var color5 = colors[i + (maxEntriesPerColumn * 4)];

                table.AddRow(
                    new Markup($"{color1.Name}", new Style(color1.ToSpectreColor())),
                    new Markup($"{color2.Name}", new Style(color2.ToSpectreColor())),
                    new Markup($"{color3.Name}", new Style(color3.ToSpectreColor())),
                    new Markup($"{color4.Name}", new Style(color4.ToSpectreColor())),
                    new Markup($"{color5.Name}", new Style(color5.ToSpectreColor())));
            }

            AnsiConsole.Write(table);
        }
    }
}
