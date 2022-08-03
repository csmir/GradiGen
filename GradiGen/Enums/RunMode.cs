using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.Enums
{
    /// <summary>
    ///     Sets the application startup settings.
    /// </summary>
    public enum RunMode
    {
        /// <summary>
        ///     Run the application in production.
        /// </summary>
        Production,

        /// <summary>
        ///     Run the application in debug.
        /// </summary>
        Debug,
    }
}
