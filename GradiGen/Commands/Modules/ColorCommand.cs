using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GradiGen.Extensions;
using Spectre.Console;

namespace GradiGen.Commands.Modules
{
    [Command("color", "Gets detailed information about a color.")]
    public class ColorCommand : ICommand
    {
        public async Task ExecuteAsync(CommandContext context)
        {
            await Task.CompletedTask;

            if (context.Parameters.Count is not 1)
                context.Respond("Please specify a color as parameter.", Color.Red);

            else
            {
                if (!context.Parameters[0].TryParseColor(out var result))
                    context.Respond("The specified color is not available to evaluate.");

                else
                {
                    var spectreColor = result.ToSpectreColor();

                    var table = new Table()
                        .Collapse()
                        .Title($"Information about the color {result.Name}", new Style(spectreColor))
                        .RoundedBorder()
                        .BorderColor(spectreColor)
                        .AddColumn("Raw Values", c => c.PadLeft(2))
                        .AddColumn("Breakdown", c => c.PadRight(1));

                    table.AddRow(
                        spectreColor.RenderCodes(),
                        spectreColor.RenderBreakdown());

                    AnsiConsole.Write(table);
                }
            }
        }
    }
}
