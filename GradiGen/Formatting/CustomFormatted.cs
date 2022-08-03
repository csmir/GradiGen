using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.Formatting
{
    /// <summary>
    ///     A provider for custom formatted values.
    /// </summary>
    public readonly struct CustomFormatted : IFormatted
    {
        public Color Color { get; }

        public string Value { get; }

        public string RawValue { get; }

        public string Format { get; }

        public CustomFormatted(string name, Color color, string rawValue, bool isNoneValue)
        {
            if (Formatter.FormatProvider.TryGetValue(name, out var formattableString))
            {
                Format = formattableString;
            }
            else
                Format = NotFormatted.Format;
            Value = string.Format(Format, color.R, color.G, color.B, isNoneValue ? " " : rawValue);
            RawValue = rawValue;
            Color = color;
        }

        public string GetMarkupCompatibleValue()
            => Value.Replace("[", "").Replace("]", "");
    }
}
