using System.Drawing;

namespace GradiGen.Formatting
{
    /// <summary>
    ///     Represents a formatted gradient entry.
    /// </summary>
    public interface IFormatted
    {
        /// <summary>
        ///     The original color for this formatted entry.
        /// </summary>
        public Color Color { get; }

        /// <summary>
        ///     The value of this formatted entry.
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     The raw value of this formatted entry.
        /// </summary>
        public string RawValue { get; }

        /// <summary>
        ///     Returns the <see cref="Value"/> property with characters like '[' and ']' removed for markup compatability.
        /// </summary>
        /// <returns></returns>
        public string GetMarkupCompatibleValue();
    }
}
