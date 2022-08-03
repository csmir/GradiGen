using GradiGen.Colors;

namespace GradiGen
{
    /// <summary>
    ///     Represents a comparer for system colors in the RGB spectrum.
    /// </summary>
    public class ColorComparer : IComparer<IntegrityColor>
    {
        /// <summary>
        ///     Compares 2 <see cref="IntegrityColor"/>'s.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(IntegrityColor x, IntegrityColor y)
        {
            var xValue = x.CalculateIntegrity();
            var yValue = y.CalculateIntegrity();

            if (xValue > yValue)
                return 1;
            if (xValue < yValue)
                return -1;
            return 0;
        }
    }
}
