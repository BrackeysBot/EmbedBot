using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;

namespace EmbedBot.Commands;

/// <summary>
///     Represents a class which implements the <c>createembed</c> command.
/// </summary>
[SlashCommandGroup("embed", "Manges embeds.", false)]
[SlashRequireGuild]
internal sealed partial class EmbedCommand : ApplicationCommandModule;
