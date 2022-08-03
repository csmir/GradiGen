namespace GradiGen.Commands
{
    /// <summary>
    ///     Represents a default interface for the <see cref="CommandContext"/> class.
    /// </summary>
    public interface ICommandContext
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
    }
}
