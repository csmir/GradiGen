using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GradiGen.Colors;
using GradiGen.Extensions;
using Spectre.Console;

namespace GradiGen.Commands.Modules
{
    [Command("color-info", "Gets detailed information about a color.")]
    [Parameter("color", "The color to be informed about.")]
    [Aliases("c", "cinfo", "c-info", "color")]
    public class ColorCommand : CommandBase<CommandContext>
    {
        public override async Task ExecuteAsync()
        {
            await Task.CompletedTask;

            if (Context.Parameters.Count is not 1)
                AnsiConsole.Markup("[red]Please specify a color as parameter.[/]");

            else
            {
                if (!IntegrityColor.TryParse(Context.Parameters[0], out var result))
                    AnsiConsole.Markup("[red]The specified color is not available to evaluate.[/]");

                else
                {
                    var spectreColor = result.Color.ToSpectreColor();

                    var table = new Table()
                        .Title($"Information about the color {result.Color.Name}")
                        .RoundedBorder()
                        .BorderColor(spectreColor)
                        .AddColumn("Core")
                        .HideHeaders();

                    var coreTable = new Table()
                        .Expand()
                        .RoundedBorder()
                        .BorderColor(spectreColor)
                        .AddColumn("Raw Values", c => c.PadLeft(2))
                        .AddColumn("Breakdown", c => c.PadRight(1));

                    var spectrumTable = new Table()
                        .Title("Related colors")
                        .Expand()
                        .RoundedBorder()
                        .BorderColor(spectreColor)
                        .AddColumn("Name")
                        .AddColumn("Hex Value")
                        .AddColumn("RGB Value")
                        .AddColumn("Integrity");

                    var sortedColors = Spectrum.GetSortedSpectrum();

                    var range = sortedColors.GetWrappedRange(result, 5).ToArray();
                    
                    foreach (var item in range)
                    {
                        var style = new Style(item.Color.ToSpectreColor());

                        spectrumTable.AddRow(
                            new Markup($"{item.Color.Name}", style),
                            new Markup($"{item.ToString(ColorType.Hex)}", style),
                            new Markup($"{item.ToString(ColorType.RGB)}", style),
                            new Markup($"{item.Integrity}", style));
                    }

                    coreTable.AddRow(
                        result.RenderCodes(),
                        spectreColor.RenderBreakdown());

                    table.AddRow(
                        coreTable);
                    table.AddRow(
                        spectrumTable);

                    AnsiConsole.Write(table);
                }
            }
        }
    }
}
