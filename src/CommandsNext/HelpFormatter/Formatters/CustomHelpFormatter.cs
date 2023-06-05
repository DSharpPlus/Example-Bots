using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;

namespace DSharpPlus.ExampleBots.CommandsNext.HelpFormatter.Formatters
{
    public class CustomHelpFormatter : BaseHelpFormatter
    {
        // Optional; a map of user-friendly type names.
        private static Dictionary<Type, string> _friendlyTypeNames = new()
        {
            {typeof(DiscordGuild), "guild"}, {typeof(DiscordChannel), "channel"}, {typeof(DiscordMember), "member"},
            {typeof(DiscordUser), "user"}, {typeof(DiscordRole), "role"}, {typeof(DiscordMessage), "message"},
            {typeof(DiscordEmoji), "emoji"}, {typeof(DiscordColor), "color"},
            {typeof(short), "integer"}, {typeof(int), "integer"}, {typeof(long), "integer"},
            {typeof(ushort), "integer"}, {typeof(uint), "integer"}, {typeof(ulong), "integer"},
            {typeof(decimal), "decimal"},  {typeof(float), "decimal"}, {typeof(double), "decimal"},  
            {typeof(bool), "boolean"},  {typeof(string), "string"},
        };
        
        // This will contain the help message for the command.
        private StringBuilder _helpStringBuilder;

        // Holds the prefix used to invoke the help command.
        private string _prefix;
        
        // Whether or not the help command was invoked with arguments.
        private bool _displayCommandList;

        public CustomHelpFormatter(CommandContext ctx) : base(ctx)
        {
            _helpStringBuilder = new StringBuilder();
            _displayCommandList = ctx.RawArguments.Count is 0;
            _prefix = ctx.Prefix;
            
        }
        
        // This method is invoked first when a command is passed as an argument to the help command.
        public override BaseHelpFormatter WithCommand(Command command)
        {
            // Append the description of the command.
            // This is pulled from the [Description] attribute applied to a command method.
            _helpStringBuilder.AppendLine(Formatter.Colorize(command.QualifiedName, AnsiColor.Magenta, AnsiColor.Underline))
                .AppendLine(command.Description ?? "The description for this command was not set.");

            // Append any aliases for the command.
            // This is pulled from the [Aliases] attribute applied to a command method.
            if (command.Aliases.Count is not 0)
            {
                _helpStringBuilder.AppendLine()
                    .AppendLine(Formatter.Colorize("aliases:", AnsiColor.Red, AnsiColor.Bold))
                    .AppendJoin(", ", command.Aliases.Select(alias => Formatter.Colorize(alias, AnsiColor.Bold)))
                    .AppendLine();
            }
            
            // Append the available combinations of arguments for the command.
            _helpStringBuilder.AppendLine().AppendLine(Formatter.Colorize("usage:", AnsiColor.Red, AnsiColor.Bold));
            AppendCommandArguments(command);

            return this;
        }

        // This is called after WithCommand() if the command provided was a command group.
        // This can also be called first if no arguments were passed to the help command, in which case childCommands will contain all top level commands.
        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> childCommands)
        {
            if (_displayCommandList)
            {
                _helpStringBuilder.AppendJoin(", ", childCommands.Where(cmd => cmd.Name.ToLower() is not "help").Select(cmd => cmd.Name));
            }
            else
            {
                foreach (Command childCommand in childCommands)
                {
                    AppendCommandArguments(childCommand);
                }
            }

            return this;
        }

        // Helper method to eliminate code duplication.
        // Iterates through the overloads for the provided command and appends its arguments to the string builder.
        private void AppendCommandArguments(Command cmd)
        {
            foreach (CommandOverload overload in cmd.Overloads.OrderBy(overload => overload.Priority))
            {
                _helpStringBuilder.Append($"{_prefix}{cmd.QualifiedName} ");

                if (overload.Arguments.Count is not 0)
                {
                    foreach (CommandArgument argument in overload.Arguments)
                    {
                        if (_friendlyTypeNames.TryGetValue(argument.Type, out string typeName) is false)
                        {
                            typeName = argument.Type.Name;
                        }

                        typeName = Formatter.Colorize(typeName, AnsiColor.White, AnsiColor.BlackBackground);
                        string argName = Formatter.Colorize(argument.Name, AnsiColor.Bold);
                        char[] braces = argument.IsOptional ? new[] { '(', ')' } : new[] { '[', ']' };
                            
                        _helpStringBuilder.Append($"{braces[0]}{typeName} {argName}{braces[1]}");
                    }
                }

                _helpStringBuilder.AppendLine();
            }
        }
        
        // This method is called last; use this method to build your help message.
        public override CommandHelpMessage Build()
        {
            StringBuilder messageStringBuilder = new();

            if (_displayCommandList)
            {
                messageStringBuilder.AppendLine(Formatter.Colorize("available commands", AnsiColor.Magenta, AnsiColor.Underline));
                messageStringBuilder.AppendLine(Formatter.Colorize(_helpStringBuilder.ToString(), AnsiColor.Yellow, AnsiColor.Bold));
                messageStringBuilder.AppendLine().AppendLine(Formatter.Colorize("specify a command to see its usage", AnsiColor.Black));
            }
            else
            {
                messageStringBuilder.AppendLine(_helpStringBuilder.ToString());
                messageStringBuilder.AppendLine(Formatter.Colorize("[arg] = required • (arg) = optional", AnsiColor.Black));
            }
            
            return new CommandHelpMessage(content: Formatter.BlockCode(messageStringBuilder.ToString(), "ansi"));
        }
    }
}
