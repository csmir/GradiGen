using GradiGen.Commands;
using Spectre.Console;

namespace GradiGen.App.Commands
{
    [Command("help", "Displays help about all available commands.")]
    [Aliases("h")]
    public class HelpCommand : CommandBase<CommandContext>
    {
        public override async Task ExecuteAsync()
        {
            AnsiConsole.WriteLine();

            await Task.CompletedTask;

            var commands = GetAllCommands();

            var table = new Table()
                .Title("All commands")
                .AddColumn("Command", c => c.Centered())
                .AddColumn("Parameters", c => c.Centered())
                .SimpleBorder();

            foreach (var command in commands)
            {
                var parameters = command.Attributes.Where(x => x is ParameterAttribute)
                    .Select(x => x as ParameterAttribute);

                var commandTable = new Table()
                    .AddColumn("", c => c.NoWrap().RightAligned().Width(15))
                    .AddColumn("")
                    .HideHeaders()
                    .SimpleBorder();

                commandTable.AddRow(
                        new Markup($"{command.Name}:", new(Color.Orange1, decoration: Decoration.Bold | Decoration.Underline)),
                        new Markup($"{command.Description}"));

                var aliases = command.Attributes.Where(x => x is AliasesAttribute)
                    .Select(x => x as AliasesAttribute);

                if (aliases.Any())
                    foreach (var alias in aliases.First()!.Aliases.OrderByDescending(x => x.Length))
                        commandTable.AddRow(
                            new Markup($"{alias}", new(Color.Grey, decoration: Decoration.Italic)),
                            new Markup(""));

                var paramTable = new Table()
                    .AddColumn("Name", c => c.Width(10).NoWrap())
                    .AddColumn("Required?")
                    .AddColumn("Description", c => c.Width(25))
                    .SimpleBorder()
                    .Expand();

                foreach (var parameter in parameters)
                {
                    paramTable.AddRow(
                        new Markup($"{parameter!.Name}", new(Color.Orange1, decoration: Decoration.Bold | Decoration.Underline)),
                        new Markup($"{(parameter.IsRequired ? "Yes" : "No")}", new(parameter.IsRequired ? Color.Red : Color.Yellow)),
                        new Markup($"{parameter.Description}"));
                }

                if (!paramTable.Rows.Any())
                    paramTable.AddEmptyRow()
                        .HideHeaders();

                table.AddRow(
                    commandTable,
                    paramTable);
            }

            AnsiConsole.Write(table);
        }

        private List<CommandInfo> GetAllCommands()
        {
            List<CommandInfo> commands = new();
            foreach (var command in Service.CommandMap)
            {
                if (commands.Any(x => x.Name == command.Value.Name))
                    continue;
                commands.Add(command.Value);
            }
            return commands;
        }
    }
}
