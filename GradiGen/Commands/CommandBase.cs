using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.Commands
{
    /// <summary>
    ///     Represents a generic commandbase to implement commands with.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommandBase<T> : ICommandBase where T : ICommandContext
    {
        /// <summary>
        ///     Gets the command's context.
        /// </summary>
        public T Context { get; private set; } = default!;
        internal void SetContext(T context)
            => Context = context;

        /// <summary>
        ///     
        /// </summary>
        public CommandInfo CommandInfo { get; private set; } = default!;
        internal void SetInformation(CommandInfo info)
            => CommandInfo = info;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract Task ExecuteAsync();

        /// <summary>
        ///     
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Task BeforeExecuteAsync(CommandInfo info, T context)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Task AfterExecuteAsync(CommandInfo info, T context)
        {
            return Task.CompletedTask;
        }
    }
}
