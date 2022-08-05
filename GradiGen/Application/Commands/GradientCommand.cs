using GradiGen.Colors;
using GradiGen.Commands;
using GradiGen.Enums;
using GradiGen.Extensions;
using GradiGen.Formatting;
using Spectre.Console;
using System.Drawing;

namespace GradiGen.App.Commands
{
    [Command("gradient", "generates a gradient")]
    [Aliases("grad", "g")]
    public class GradientCommand : CommandBase<CommandContext>
    {
        public override async Task ExecuteAsync()
        {
            await Task.CompletedTask;
            var type = AnsiConsole.Prompt(
                new SelectionPrompt<ColorType>()
                    .PageSize(4)
                    .AddChoices(Enum.GetValues<ColorType>())
                    .Title("[grey]What color format do you want to use?[/]")
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]"));

            var result = type switch
            {
                ColorType.Name => NameGeneration(),
                ColorType.RGB => RGBGeneration(),
                ColorType.Hex => HexGeneration(),
                ColorType.UInt32 => UInt32Generation(),
                _ => throw new NotImplementedException()
            };

            var input = AnsiConsole.Prompt<string>(
                new TextPrompt<string>("[grey]Formatting Text (Leave empty if none):[/]")
                    .Validate(x => x.Length is > 2 or 0)
                    .ValidationErrorMessage("[red]Input text needs to be 0 or more than 2 characters long.[/]")
                    .AllowEmpty());

            int steps;
            if (!string.IsNullOrEmpty(input))
            {
                var stepsPrompt = new SelectionPrompt<int>()
                    .Title("[grey]How dense do you want your gradient to be?[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]");

                for (int i = 1; i < input.Length + 1; i++) // Fixes density allowing each char to be assigned a color
                    stepsPrompt.AddChoice(i);

                steps = AnsiConsole.Prompt<int>(stepsPrompt);
            }
            else
                steps = AnsiConsole.Prompt<int>(new TextPrompt<int>("[grey]How dense do you want your gradient to be?[/]")
                    .Validate(x => x is > 1 and < int.MaxValue) // Fixes custom density to have any desired inputs
                    .ValidationErrorMessage("[red]Please define a value between 2 and 100."));

            var gradient = result.Item1.GenerateGradient(result.Item2, steps - 1); // Fixes the output from producing an unwanted extra color

            var formatValues = Enum.GetValues<FormatType>();

            if (!string.IsNullOrEmpty(input))
                formatValues = formatValues.Where(x => x is not FormatType.None).ToArray();

            var formatType = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[grey]What format do you want to use?[/]")
                    .AddChoices(formatValues.Select(x => x.ToString()).Concat(Formatter.GetFormatNames())));

            var formatted = gradient.Format(input!, formatType);

            var table = new Table()
                .Title($"Generated gradient from {result.Item1.Name} to {result.Item2.Name}:")
                .RoundedBorder()
                .BorderColor(result.Item1.ToSpectreColor())
                .AddColumn("Raw Text")
                .AddColumn("Applied Color")
                .AddColumn($"Format ({formatType})")
                .AddColumn("Result")
                .Expand();

            foreach (var fragment in formatted)
            {
                table.AddRow(
                    new Markup($"{fragment.RawValue}"),
                    new Markup($"{fragment.Color.R}, {fragment.Color.G}, {fragment.Color.B}", new Style(fragment.Color.ToSpectreColor())),
                    new Markup($"{fragment.GetMarkupCompatibleValue()}"),
                    new Markup($"{fragment.RawValue}", new Style(fragment.Color.ToSpectreColor())));
            }

            AnsiConsole.Write(table);

            if (Lifetime.System is SystemEnvironment.Windows)
            {
                if (AnsiConsole.Confirm("[grey]Do you want to copy your format to the system clipboard?[/]"))
                {
                    Clipboard.SetText(string.Join("", formatted.Select(x => x.Value)));
                    AnsiConsole.MarkupLine("[grey]Copied gradient to clipboard.[/]");
                }
                else
                    AnsiConsole.MarkupLine("[grey]Skipped copying to clipboard.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[grey]Your system does not support copying to clipboard from this application.[/]");
                if (AnsiConsole.Confirm("[grey]Do you want to visualize the format in a message instead?"))
                {
                    AnsiConsole.WriteLine(string.Join("", formatted.Select(x => x.Value)));
                    AnsiConsole.MarkupLine("[grey]Pasted gradient in console.[/]");
                }
                else
                    AnsiConsole.MarkupLine("[grey]Skipped pasting to console.[/]");
            }
        }

        #region hex
        private static (System.Drawing.Color, System.Drawing.Color) HexGeneration()
        {
            static System.Drawing.Color GetColor(TextPrompt<string> prompt)
            {
                var hex = AnsiConsole.Prompt<string>(prompt).Replace("#", "");

                return System.Drawing.Color.FromArgb((int)Convert.ToUInt32(hex, 16));
            }

            var prompt = new TextPrompt<string>("[grey]Please specify an Hex color ((#)FFFFFF).[/]")
                .Validate(x =>
                {
                    var input = x.Replace("#", "");
                    if (input.Length is 6)
                    {
                        try
                        {
                            Convert.ToUInt32(input, 16);
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    return false;
                })
                .ValidationErrorMessage("[red]Please specify a valid Hex color.[/]");

            var initColor = GetColor(prompt);
            var finalColor = GetColor(prompt);

            return (initColor, finalColor);
        }
        #endregion

        #region uint
        private static (System.Drawing.Color, System.Drawing.Color) UInt32Generation()
        {
            static System.Drawing.Color GetColor(TextPrompt<uint> prompt)
            {
                var hex = AnsiConsole.Prompt<uint>(prompt);
                return System.Drawing.Color.FromArgb((int)hex);
            }

            var prompt = new TextPrompt<uint>("[grey]Please specify an UInt32 color ((#)FFFFFF).[/]")
                .ValidationErrorMessage("[red]Please specify a valid UInt32 color.[/]");

            var initColor = GetColor(prompt);
            var finalColor = GetColor(prompt);

            return (initColor, finalColor);
        }
        #endregion

        #region rgb
        private static (System.Drawing.Color, System.Drawing.Color) RGBGeneration()
        {
            static System.Drawing.Color GetColor(TextPrompt<string> prompt)
            {
                var rgb = AnsiConsole.Prompt<string>(prompt);

                var input = rgb.Split(',', StringSplitOptions.TrimEntries).Select(x => int.Parse(x)).ToArray();
                return System.Drawing.Color.FromArgb(input[0], input[1], input[2]);
            }

            var prompt = new TextPrompt<string>("[grey]Please specify a RGB color (xxx,xxx,xxx).[/]")
                .Validate(x =>
                {
                    var input = x.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    if (input.Length is 3)
                    {
                        foreach (var entry in input)
                        {
                            if (entry.Length > 3)
                                return false;

                            if (!int.TryParse(entry, out int i) && i is >= 0 and <= 255)
                                return false;
                            return true;
                        }
                    }
                    return false;
                })
                .ValidationErrorMessage("[red]Please specify a valid RGB color.[/]");

            var initColor = GetColor(prompt);
            var finalColor = GetColor(prompt);

            return (initColor, finalColor);
        }
        #endregion

        #region named
        private static (System.Drawing.Color, System.Drawing.Color) NameGeneration()
        {
            var approach = AnsiConsole.Prompt(new SelectionPrompt<GenerationApproach>()
                .Title("[grey]What generation approach do you want to use?[/]")
                .AddChoices(Enum.GetValues<GenerationApproach>()));

            System.Drawing.Color initColor;
            System.Drawing.Color finalColor;

            switch (approach)
            {
                case GenerationApproach.Randomized:

                    var allColors = Enum.GetValues<KnownColor>();
                    var random = new Random();

                    var color1 = allColors[random.Next(allColors.Length - 1)];
                    initColor = System.Drawing.Color.FromKnownColor(color1);
                    var color2 = allColors[random.Next(allColors.Length - 1)];
                    finalColor = System.Drawing.Color.FromKnownColor(color2);

                    break;

                case GenerationApproach.Picker:
                    var prompt = new SelectionPrompt<KnownColor>()
                        .PageSize(10)
                        .AddChoices(Enum.GetValues<KnownColor>())
                        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]");

                    var init = AnsiConsole.Prompt(prompt.Title("[grey]What color should the gradient start with?[/]"));
                    initColor = System.Drawing.Color.FromKnownColor(init);

                    var final = AnsiConsole.Prompt(prompt.Title("[grey]What color should the gradient end with?[/]"));
                    finalColor = System.Drawing.Color.FromKnownColor(final);

                    break;
                default:
                    throw new NotSupportedException();
            }

            return (initColor, finalColor);
        }
        #endregion
    }
}
