using System.Drawing;

namespace GradiGen.Formatting
{
    /// <inheritdoc/>
    public readonly struct TggFormatted : IFormatted
    {
        public Color Color { get; }

        public string Value { get; }

        public string RawValue { get; }

        /// <summary>
        ///     The Terraria color format, presented as: [c/FFFFFF:]
        /// </summary>
        public static string Format
            => "<color:hex={0:X2}{1:X2}{2:X2}>{3}<color:pop>";

        public TggFormatted(Color color, string rawValue, bool isNoneValue)
        {
            Color = color;
            RawValue = rawValue;
            Value = string.Format(Format, color.R, color.G, color.B, isNoneValue ? " " : rawValue);
        }

        public string GetMarkupCompatibleValue()
        {
            return Value;
        }
    }
}
