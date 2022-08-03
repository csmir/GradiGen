using GradiGen.Formatting;
using System.Drawing;

namespace GradiGen.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class GradientExtensions
    {
        /// <summary>
        ///     Generates a gradient from the input values.
        /// </summary>
        /// <param name="initial"></param>
        /// <param name="final"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        public static IEnumerable<Color> GenerateGradient(this Color initial, Color final, int steps)
        {
            var (rMin, gMin, bMin) = (initial.R, initial.G, initial.B);
            var (rMax, gMax, bMax) = (final.R, final.G, final.B);

            for (int i = 0; i < steps; i++)
            {
                var rAverage = rMin + (rMax - rMin) * i / steps;
                var gAverage = gMin + (gMax - gMin) * i / steps;
                var bAverage = bMin + (bMax - bMin) * i / steps;

                yield return Color.FromArgb(rAverage, gAverage, bAverage);
            }
        }

        /// <summary>
        ///     Formats the input gradient alongside the input text.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="textToFormat"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IEnumerable<IFormatted> FormatGradient(this IEnumerable<Color> input, string textToFormat, FormatType type)
        {
            var array = input.ToArray();

            if (string.IsNullOrEmpty(textToFormat))
                for (int i = 0; i < array.Length; i++)
                    switch (type)
                    {
                        case FormatType.None:
                            yield return new NotFormatted(array[i], "N/A");
                            break;
                        case FormatType.Terraria:
                            yield return new TerrariaFormatted(array[i], "N/A", true);
                            break;
                        case FormatType.ScrapMechanic:
                            yield return new ScrapMechanicFormatted(array[i], "N/A", true);
                            break;
                        case FormatType.Tgg:
                            yield return new TggFormatted(array[i], "N/A", true);
                            break;
                    }

            else
            {
                float stepsPerColor = textToFormat.Length / (float)array.Length;

                int minIndex = 0;
                for (int i = 1; i <= array.Length; i++)
                {
                    int maxIndex = (int)(stepsPerColor * i);
                    var characters = textToFormat[minIndex..maxIndex];
                    minIndex = maxIndex;

                    switch (type)
                    {
                        case FormatType.Terraria:
                            yield return new TerrariaFormatted(array[i - 1], characters, false);
                            break;
                        case FormatType.ScrapMechanic:
                            yield return new ScrapMechanicFormatted(array[i - 1], characters, false);
                            break;
                        case FormatType.Tgg:
                            yield return new TggFormatted(array[i - 1], characters, false);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
            }
        }
    }
}
