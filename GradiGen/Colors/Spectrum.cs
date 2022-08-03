using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.Colors
{
    /// <summary>
    /// 
    /// </summary>
    public class Spectrum
    {
        public List<IntegrityColor> Items { get; }

        private Spectrum(List<IntegrityColor> colors)
        {
            AnsiConsole.WriteLine($"[Debug] created spectrum: {colors.Count}.");
            Items = colors.OrderByDescending(x => x, new ColorComparer()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        /// <param name="wrapAround"></param>
        /// <returns></returns>
        public IEnumerable<IntegrityColor> GetWrappedRange(IntegrityColor color, int wrapAround = 4)
        {
            var match = GetClosestMatch(color.Integrity);
            var matchIndex = Items.IndexOf(match);

            while ((matchIndex + wrapAround) > Items.Count)
                matchIndex--;

            while ((matchIndex - wrapAround) < 0)
                matchIndex++;

            for (int i = matchIndex - wrapAround; i < matchIndex + wrapAround;i++)
            {
                yield return Items[i];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="integrity"></param>
        /// <returns></returns>
        public IntegrityColor GetClosestMatch(double integrity)
        {
            IntegrityColor closest = default;

            for (int i = 0; i < Items.Count; i++)
            {
                var foundIntegrity = Items[i].Integrity;

                if (foundIntegrity > integrity)
                {
                    closest = Items[i];
                }
            }

            return closest;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <returns></returns>
        public static Spectrum GetSortedSpectrum()
        {
            List<IntegrityColor> colors = new();

            var names = Enum.GetNames<KnownColor>();

            foreach (var name in names)
                colors.Add(System.Drawing.Color.FromName(name));

            AnsiConsole.WriteLine($"[Debug] Added named entries: {colors.Count}");

            return new(colors);
        }
    }
}
