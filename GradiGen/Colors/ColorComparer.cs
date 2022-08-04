namespace GradiGen.Colors
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
            if (x.Integrity > y.Integrity)
                return 1;
            if (x.Integrity < y.Integrity)
                return -1;
            return 0;
        }
    }
}
