using GradiGen.Commands;

namespace GradiGen.App.Commands
{
    [Command("quit", "Quits the application.")]
    [Aliases("q")]
    public class QuitCommand : CommandBase<CommandContext>
    {
        public override async Task ExecuteAsync()
        {
            await Task.CompletedTask;
            Environment.Exit(0);
        }
    }
}
