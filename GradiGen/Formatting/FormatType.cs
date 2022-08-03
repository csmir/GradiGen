using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.Formatting
{
    /// <summary>
    ///     Represents the format target to use when applying gradients to text.
    /// </summary>
    public enum FormatType
    {
        /// <summary>
        ///     The Terraria color format.
        /// </summary>
        Terraria,

        /// <summary>
        ///     The Scrap Mechanic color format.
        /// </summary>
        ScrapMechanic,

        /// <summary>
        ///     The TGG server color format.
        /// </summary>
        Tgg,

        /// <summary>
        ///     No formatting mode.
        /// </summary>
        None,
    }
}
