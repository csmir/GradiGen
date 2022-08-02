using GradiGen.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen
{
    /// <summary>
    ///     Represents a comparer for system colors in the <see cref="KnownColor"/> range.
    /// </summary>
    public class ColorComparer : IComparer<Color>
    {
        private readonly int _repetitions;

        /// <summary>
        ///     Creates a new instance of the default <see cref="Color"/> comparer.
        /// </summary>
        /// <param name="maxRepetitions"></param>
        public ColorComparer(int maxRepetitions = 8)
        {
            _repetitions = maxRepetitions;
        }

        private const double _rTarget = .241;
        private const double _gTarget = .691;
        private const double _bTarget = .068;

        /// <summary>
        ///     Compares 2 <see cref="Color"/>'s.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(Color x, Color y)
        {
            var xResult = CalculateIntegrity(x);
            var xValue = (xResult.Item1 + xResult.Item2 + xResult.Item3 + xResult.Item4);

            var yResult = CalculateIntegrity(y);
            var yValue = (yResult.Item1 + yResult.Item2 + yResult.Item3 + yResult.Item4);

            if (xValue > yValue)
                return 1;
            if (xValue < yValue)
                return -1;
            return 0;
        }

        // Calculates the accountable values for this color.
        private (double, double, double, double) CalculateIntegrity(Color color)
        {
            var (r, g, b) = ((float)color.R, (float)color.G, (float)color.B);
            var lum = Math.Sqrt(_rTarget * r + _gTarget * g + _bTarget * b);

            var (h, s, v) = color.ToHSV();

            h *= _repetitions;
            lum *= _repetitions;
            v *= _repetitions;

            if (h % 2 is 1)
            {
                v = _repetitions - v;
                lum = _repetitions - lum;
            }

            return (h, lum, v, s);
        }
    }
}
