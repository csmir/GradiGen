using GradiGen.Colors;
using GradiGen.Extensions;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradiGen.Commands.Modules
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
