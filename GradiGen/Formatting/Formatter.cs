using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.Formatting
{
    /// <summary>
    ///     A static class that formats provided values into a string.
    /// </summary>
    public static class Formatter
    {
        /// <summary>
        ///     A dictionary holding the name and respective format of added format providers.
        /// </summary>
        public static Dictionary<string, string> FormatProvider { get; } = new();

        public static IEnumerable<IFormatted> Format(this IEnumerable<Color> colors, string text, string type)
        {
            if (string.IsNullOrEmpty(text))
                return FormatEmpty(colors.ToArray(), type);
            return FormatFilled(colors.ToArray(), type, text);
        }

        private static IEnumerable<IFormatted> FormatFilled(Color[] colors, string type, string text)
        {
            float stepsPerColor = text.Length / (float)colors.Length;

            int minIndex = 0;
            for (int i = 1; i <= colors.Length; i++)
            {
                int maxIndex = (int)(stepsPerColor * i);
                var characters = text[minIndex..maxIndex];
                minIndex = maxIndex;

                if (Enum.TryParse<FormatType>(type, true, out var formatType))
                {
                    switch (formatType)
                    {
                        case FormatType.Terraria:
                            yield return new TerrariaFormatted(colors[i - 1], characters, false);
                            break;
                        case FormatType.ScrapMechanic:
                            yield return new ScrapMechanicFormatted(colors[i - 1], characters, false);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
                else
                    yield return new CustomFormatted(type, colors[i - 1], characters, false);
            }
        }

        private static IEnumerable<IFormatted> FormatEmpty(Color[] colors, string type)
        {
            for (int i = 0; i < colors.Length; i++)
            {
                if (Enum.TryParse<FormatType>(type, true, out var formatType))
                {
                    switch (formatType)
                    {
                        case FormatType.None:
                            yield return new NotFormatted(colors[i], "N/A");
                            break;
                        case FormatType.Terraria:
                            yield return new TerrariaFormatted(colors[i], "N/A", true);
                            break;
                        case FormatType.ScrapMechanic:
                            yield return new ScrapMechanicFormatted(colors[i], "N/A", true);
                            break;
                    }
                }
                else
                    yield return new CustomFormatted(type, colors[i], "N/A", true);
            }
        }

        /// <summary>
        ///     Adds a format provider from the provided string.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="formattableString"></param>
        public static void AddFormatProvider(string name, string formattableString)
            => FormatProvider.TryAdd(name, formattableString);

        /// <summary>
        ///     Gets the names of all available formats.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetFormatNames()
            => FormatProvider.Keys.ToList();
    }
}
