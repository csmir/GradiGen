using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;
using Spectre.Console;
using GradiGen.Enums;

namespace GradiGen.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandService
    {
        private readonly IReadOnlyDictionary<string, ModuleInfo> _callback;

        private readonly RunMode _level;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        public CommandService(Assembly assembly, RunMode level)
        {
            _level = level;
            _callback = RegisterModules(assembly);
        }

        private IReadOnlyDictionary<string, ModuleInfo> RegisterModules(Assembly assembly)
        {
            var types = assembly.GetTypes();

            var interfaceType = typeof(ICommand);

            var callback = new Dictionary<string, ModuleInfo>();

            foreach (var type in types)
            {
                if (interfaceType.IsAssignableFrom(type) && !type.IsAbstract)
                {
                    var executionMethod = type.GetMethod("ExecuteAsync");

                    if (executionMethod is null)
                        throw new InvalidOperationException($"An unexpected error has occurred while trying to find execution method of type {type.FullName}.");

                    var primaryCtor = type.GetConstructors().First();

                    if (primaryCtor is null)
                        throw new InvalidOperationException($"An unexpected error has occurred while trying to find primary constructor of type {type.FullName}");

                    foreach (var attr in type.GetCustomAttributes(false))
                    {
                        if (attr is CommandAttribute cmd)
                        {
                            if (_level is RunMode.Debug)
                                AnsiConsole.WriteLine($"Succesfully registered {cmd.Name} with {executionMethod.Name} && {primaryCtor.Name} as members.");
                            callback.Add(cmd.Name, new ModuleInfo(primaryCtor, executionMethod));
                        }
                    }
                }
            }

            return callback;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<bool> ExecuteCommandAsync(CommandContext context, IServiceProvider? provider = null)
        {
            if (_callback.TryGetValue(context.Name, out var value))
            {
                var instance = value.Constructor.Invoke(null);

                var result = value.Method.Invoke(instance, new[] { context });

                if (result is not Task t)
                    throw new InvalidOperationException("Unexpected return type received from executing command.");

                await t;
                AnsiConsole.WriteLine();
                return true;
            }
            return false;
        }
    }
}
