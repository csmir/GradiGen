using GradiGen.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.Formatting
{
    /// <inheritdoc />
    public readonly struct TerrariaFormatted : IFormatted
    {
        public Color Color { get; }

        public string Value { get; }

        public string RawValue { get; }

        /// <summary>
        ///     The Terraria color format, presented as: [c/FFFFFF:]
        /// </summary>
        public static string Format
            => "[c/{0:X2}{1:X2}{2:X2}:{3}]";

        public TerrariaFormatted(Color color, string rawValue, bool isNoneValue)
        {
            Color = color;
            RawValue = rawValue;
            Value = string.Format(Format, color.R, color.G, color.B, isNoneValue ? " " : rawValue);
        }

        public string GetMarkupCompatibleValue()
        {
            return Value.Replace("[", "").Replace("]", "");
        }
    }
}
