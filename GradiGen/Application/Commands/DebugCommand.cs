using GradiGen.Commands;

namespace GradiGen.App.Commands
{
    [Command("debug", "Debugging command")]
    [Aliases("d")]
    public class DebugCommand : CommandBase<CommandContext>
    {
        public override async Task ExecuteAsync()
        {
            await Task.CompletedTask;
        }
    }
}
