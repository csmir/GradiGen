namespace GradiGen.Commands
{
    /// <summary>
    ///     
    /// </summary>
    public interface ICommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract Task ExecuteAsync();
    }
}
