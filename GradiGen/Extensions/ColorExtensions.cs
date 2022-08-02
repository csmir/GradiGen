using GradiGen.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        ///     Attempts to parse a <see cref="System.Drawing.Color"/> from the provided <see cref="string"/> and <see cref="ColorType"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Thrown for not implemented <see cref="ColorType"/> overloads.</exception>
        public static bool TryParseColor(this string? input, out Color output)
        {
            output = default!;
            if (string.IsNullOrEmpty(input))
                return false;

            var param = input.Split(',', StringSplitOptions.TrimEntries);
            var rawInput = input.Replace("#", "");

            if (param.Length is 3)
            {
                if (!byte.TryParse(param[0], out byte r))
                    return false;

                if (!byte.TryParse(param[1], out byte g))
                    return false;

                if (!byte.TryParse(param[2], out byte b))
                    return false;

                output = Color.FromArgb(r, g, b);
                return true;
            }
            else if (rawInput.Length is 6)
            {
                try
                {
                    var @uint = Convert.ToUInt32(rawInput, 16);
                    output = Color.FromArgb((int)@uint);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else if (uint.TryParse(input, out uint value))
            {
                try
                {
                    output = Color.FromArgb((int)value);
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
                    output = Color.FromName(input);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static (double, double, double) ToHSV(this Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            return (color.GetHue(), (max == 0) ? 0 : 1d - (1d * min / max), max / 255d);
        }

        public static Color FromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }
}
