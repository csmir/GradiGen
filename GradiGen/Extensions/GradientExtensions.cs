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
            yield return final;
        }
    }
}
