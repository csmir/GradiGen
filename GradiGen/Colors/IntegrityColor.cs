using GradiGen.Extensions;
using System.Drawing;

namespace GradiGen.Colors
{
    /// <summary>
    ///     Represents a color by it's integrity value.
    /// </summary>
    public readonly struct IntegrityColor
    {
        private const double _rTarget = .241;
        private const double _gTarget = .691;
        private const double _bTarget = .068;

        /// <summary>
        ///     The underlying system color for this integrity value.
        /// </summary>
        public Color Color { get; }

        /// <summary>
        ///     The red component of this color.
        /// </summary>
        public byte R { get; }

        /// <summary>
        ///     The green component of this color.
        /// </summary>
        public byte G { get; }

        /// <summary>
        ///     The blue component of this color.
        /// </summary>
        public byte B { get; }

        /// <summary>
        ///     The hue of this color.
        /// </summary>
        public double Hue { get; }

        /// <summary>
        ///     The luminisoty of this color.
        /// </summary>
        public double Luminosity { get; }

        /// <summary>
        ///     The value of this color.
        /// </summary>
        public double Value { get; }

        /// <summary>
        ///     The saturation of this color.
        /// </summary>
        public double Saturation { get; }

        /// <summary>
        ///     The represented integrity of this color.
        /// </summary>
        public double Integrity { get; }

        private IntegrityColor(Color color, double hue, double lum, double val, double sat)
        {
            R = color.R;
            G = color.G;
            B = color.B;

            Color = color;
            Hue = hue;
            Luminosity = lum;
            Value = val;
            Saturation = sat;
            Integrity = hue + lum + val + sat;
        }

        /// <summary>
        ///     Converts a system <see cref="System.Drawing.Color"/> into a <see cref="IntegrityColor"/>.
        /// </summary>
        /// <param name="color"></param>
        public static implicit operator IntegrityColor(Color color)
            => Create(color);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => ToString(ColorType.Hex);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string ToString(ColorType type)
        {
            switch (type)
            {
                case ColorType.UInt32:
                    string hexValue = $"{R:X2}{G:X2}{B:X2}";
                    return Convert.ToUInt32(hexValue, 16).ToString();
                case ColorType.Hex:
                    return $"{R:X2}{G:X2}{B:X2}";
                case ColorType.RGB:
                    return $"{R}, {G}, {B}";
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Creates a new <see cref="IntegrityColor"/> from the provided color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="repetitions"></param>
        /// <returns></returns>
        public static IntegrityColor Create(Color color, int repetitions = 8)
        {
            var (r, g, b) = ((float)color.R, (float)color.G, (float)color.B);
            var lum = Math.Sqrt(_rTarget * r + _gTarget * g + _bTarget * b);
            var (h, s, v) = color.ToHSV();

            h *= repetitions;
            lum *= repetitions;
            v *= repetitions;

            if (h % 2 is 1)
            {
                v = repetitions - v;
                lum = repetitions - lum;
            }

            return new(color, h, lum, v, s);
        }

        /// <summary>
        ///     Attempts to create an <see cref="IntegrityColor"/> from the input string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static bool TryParse(string value, out IntegrityColor color)
        {
            color = default!;
            if (string.IsNullOrEmpty(value))
                return false;

            var param = value.Split(',', StringSplitOptions.TrimEntries);
            var rawInput = value.Replace("#", "");

            if (param.Length is 3)
            {
                if (!byte.TryParse(param[0], out byte r))
                    return false;
                if (!byte.TryParse(param[1], out byte g))
                    return false;
                if (!byte.TryParse(param[2], out byte b))
                    return false;

                color = Color.FromArgb(r, g, b);
                return true;
            }
            else if (rawInput.Length is 6)
            {
                try
                {
                    var @uint = Convert.ToUInt32(rawInput, 16);
                    color = Color.FromArgb((int)@uint);
                    return true;
                }
                catch
                {
                    try
                    {
                        color = Color.FromName(value);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            else if (uint.TryParse(value, out uint integer))
            {
                try
                {
                    color = Color.FromArgb((int)integer);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    color = Color.FromName(value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///     Calculates the integrity value to sort the colors with.
        /// </summary>
        /// <returns></returns>
        public double CalculateIntegrity()
            => Hue + Luminosity + Value + Saturation;
    }
}
