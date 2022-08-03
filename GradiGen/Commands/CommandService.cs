using System.Diagnostics;
using System.Reflection;

namespace GradiGen.Commands
{
    /// <summary>
    ///     Represents the handler for registered commands.
    /// </summary>
    public class CommandService
    {
        /// <summary>
        ///     The range of registered commands.
        /// </summary>
        public Dictionary<string, CommandInfo> CommandMap { get; private set; }

        /// <summary>
        ///     Represents the service container used to 
        /// </summary>
        public IServiceProvider? Services { get; }

        /// <summary>
        ///     Creates a new instance of the <see cref="CommandService"/> for the target assembly.
        /// </summary>
        /// <param name="assembly"></param>
        public CommandService(IServiceProvider services = null!)
        {
            Services = services;
            CommandMap = new();
        }

        /// <summary>
        ///     Registers all commands in the provided assembly to the <see cref="CommandMap"/> for execution.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public async Task RegisterCommandsAsync(Assembly assembly)
        {
            var types = assembly.GetTypes();

            var baseType = typeof(ICommandBase);

            foreach (var type in types)
                if (baseType.IsAssignableFrom(type) && !type.IsAbstract)
                    await RegisterInternalAsync(type);
        }

        private async Task RegisterInternalAsync(Type type)
        {
            await Task.CompletedTask;

            Debugger.Message($"Found type: {type.FullName}");

            var method = type.GetMethod("ExecuteAsync");

            if (method is null)
                throw new InvalidOperationException($"An unexpected error has occurred while trying to find execution method of type {type.FullName}.");

            var ctor = type.GetConstructors().First();

            if (ctor is null)
                throw new InvalidOperationException($"An unexpected error has occurred while trying to find primary constructor of type {type.FullName}");

            var attr = type.GetCustomAttributes(false);

            foreach (var a in attr)
            {
                if (a is not CommandAttribute cmd)
                    continue;

                var commandInfo = new CommandInfo(ctor, method, type, attr, cmd.Name, cmd.Description);

                if (attr.Where(x => x is AliasesAttribute).FirstOrDefault() is AliasesAttribute ali)
                {
                    string[]? aliases = ali.Aliases;
                    foreach (var alias in aliases)
                    {
                        Debugger.Message($"Succesfully registered {alias} for {cmd.Name} with {method.Name} & {ctor.Name} as members.");
                        CommandMap.Add(alias, commandInfo);
                    }
                }

                Debugger.Message($"Succesfully registered {cmd.Name} with {method.Name} && {ctor.Name} as members.");

                CommandMap.Add(cmd.Name, commandInfo);
            }
        }

        /// <summary>
        ///     Executes the found command with the provided context.
        /// </summary>
        /// <param name="commandContext"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<IResult> ExecuteCommandAsync<T>(T commandContext) where T : ICommandContext
        {
            if (CommandMap.TryGetValue(commandContext.Name, out var info))
            {
                var obj = info.Constructor.Invoke(null);

                Debugger.Message($"Succesfully created scope for {info.Type.FullName}.");

                if (obj is not CommandBase<T> module)
                    return ModuleResult.FromError($"Failed to interpret module type with matching type of {nameof(CommandBase<T>)}");

                try
                {
                    await ExecuteInternalAsync(commandContext, module, info);
                }
                catch (Exception ex)
                {
                    return CommandResult.FromError(ex.Message, ex);
                }

                return CommandResult.FromSuccess();
            }

            return SearchResult.FromError($"Failed to find command with name: {commandContext.Name}");
        }

        private async Task ExecuteInternalAsync<T>(T context, CommandBase<T> module, CommandInfo info) where T : ICommandContext
        {
            module.SetContext(context);
            module.SetInformation(info);

            var stopwatch = Stopwatch.StartNew();

            await module.BeforeExecuteAsync(info, context);

            await module.ExecuteAsync();

            await module.AfterExecuteAsync(info, context);

            stopwatch.Stop();
            Debugger.Message($"Succesfully executed {info.Name} in {stopwatch.ElapsedTicks} ticks.");
        }
    }
}
