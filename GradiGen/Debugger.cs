using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen
{
    /// <summary>
    ///     Controls the application's debugging tools.
    /// </summary>
    public static class Debugger
    {
        /// <summary>
        ///     Gets or sets if debugging is enabled.
        /// </summary>
        public static bool IsDebugEnabled { get; set; }

        /// <summary>
        ///     Writes a debug message to the controlling application.
        /// </summary>
        /// <param name="text"></param>
        public static void Message(string text)
        {
            if (IsDebugEnabled)
                AnsiConsole.MarkupLine($"<debug> [yellow]{text}[/]");
        }

        /// <summary>
        ///     Writes an exception to the controlling application.
        /// </summary>
        /// <param name="exception"></param>
        public static void Exception(Exception exception)
        {
            if (IsDebugEnabled)
                AnsiConsole.WriteException(exception);
        }
    }
}
