using System.Diagnostics;

namespace GradiGen
{
    /// <summary>
    ///     Represents a static class referencing the windows clipboard.
    /// </summary>
    public static class Clipboard
    {
        /// <summary>
        ///     Sets the windows PC clipboard text as the provided input.
        /// </summary>
        /// <param name="input"></param>
        public static void SetText(string input)
        {
            Process clipboardExecutable = new()
            {
                StartInfo = new ProcessStartInfo // Creates the process
                {
                    RedirectStandardInput = true,
                    FileName = @"clip",
                }
            };
            clipboardExecutable.Start();

            clipboardExecutable.StandardInput.Write(input);
            clipboardExecutable.StandardInput.Close();

            return;
        }
    }
}
