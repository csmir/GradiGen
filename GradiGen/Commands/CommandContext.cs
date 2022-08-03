using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.Commands
{
    /// <summary>
    ///     Represents a class thats used to describe data from the command.
    /// </summary>
    public class CommandContext : ICommandContext
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
        ///     Attempts to parse the command input to a valid range of values.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool TryParse(string input, out CommandContext context)
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

            context = new(commandName, input, commandParams);
            return true;
        }
    }
}
