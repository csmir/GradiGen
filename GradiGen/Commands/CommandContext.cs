using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.Commands
{
    public class CommandContext
    {
        /// <summary>
        ///     The name of the command.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The raw input of the command.
        /// </summary>
        public string RawInput { get; }

        /// <summary>
        ///     The command parameters.
        /// </summary>
        public List<string> Parameters { get; }

        private CommandContext(string name, string rawInput, List<string> parameters)
        {
            Name = name;
            RawInput = rawInput;
            Parameters = parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="color"></param>
        public void Respond(object input, Color? color = null)
        {
            SetOutputColor(color);
            AnsiConsole.WriteLine($"{input.ToString()}");
            ResetOutputColor();
        }

        /// <summary>
        ///     Resets the command output color.
        /// </summary>
        public void ResetOutputColor()
            => AnsiConsole.Foreground = Color.White;

        /// <summary>
        ///     Sets the command output color.
        /// </summary>
        /// <param name="color"></param>
        public void SetOutputColor(Color? color)
        {
            if (color != null)
                AnsiConsole.Foreground = color.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static CommandContext Parse(string input)
        {
            var range = input.Split(' ');

            string commandName = string.Empty;

            List<string> commandParams = new();
            List<string> partialParam = new();

            foreach (var entry in range)
            {
                if (commandName == string.Empty)
                {
                    commandName = entry;
                    continue;
                }

                if (partialParam.Any())
                {
                    if (entry.EndsWith('"'))
                    {
                        partialParam.Add(entry.Replace("\"", ""));
                        commandParams.Add(string.Join(" ", partialParam));
                        partialParam.Clear();
                        continue;
                    }
                    partialParam.Add(entry);
                    continue;
                }

                // start checking the command.
                if (entry.StartsWith('"'))
                {
                    if (entry.EndsWith('"'))
                        commandParams.Add(entry.Replace("\"", ""));
                    else
                        partialParam.Add(entry.Replace("\"", ""));
                    continue;
                }

                commandParams.Add(entry);
            }

            return new(commandName, input, commandParams);
        }
    }
}
