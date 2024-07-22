using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using EmbedBot.Interactivity;

namespace EmbedBot.Commands;

internal sealed partial class EmbedCommand
{
    [SlashCommand("create", "Creates a new embed.", false)]
    public async Task CreateEmbedAsync(InteractionContext context,
        [Option("channel", "The channel in which to send the embed. Defaults to the current channel.")]
        DiscordChannel? channel = null)
    {
        channel ??= context.Channel;

        var modal = new DiscordModalBuilder(context.Client);
        modal.WithTitle("Create Embed");

        DiscordModalTextInput titleInput = modal.AddInput("Title", "The title of the embed.", maxLength: 256, isRequired: false);
        DiscordModalTextInput colorInput = modal.AddInput("Color", "e.g. #007EC6", maxLength: 7, isRequired: false);
        DiscordModalTextInput descriptionInput = modal.AddInput("Description", "The body of the embed.", isRequired: false, maxLength: 2048, inputStyle: TextInputStyle.Paragraph);

        DiscordModalResponse response = await modal.Build().RespondToAsync(context.Interaction, TimeSpan.FromMinutes(5));
        if (response == DiscordModalResponse.Timeout)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(titleInput.Value) && string.IsNullOrWhiteSpace(descriptionInput.Value))
        {
            return;
        }

        DiscordColor color = DiscordColor.CornflowerBlue;

        if (!string.IsNullOrWhiteSpace(colorInput.Value))
        {
            color = new DiscordColor(colorInput.Value);
        }

        var embed = new DiscordEmbedBuilder();
        embed.WithColor(color);
        embed.WithTitle(titleInput.Value);
        embed.WithDescription(descriptionInput.Value);

        await channel.SendMessageAsync(embed);
        await context.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"Embed created in {channel.Mention}"));
    }
}
