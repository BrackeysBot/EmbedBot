using System.Text.RegularExpressions;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using EmbedBot.Interactivity;

namespace EmbedBot.Commands;

internal sealed partial class EmbedCommand
{
    private static readonly Regex MessageLinkRegex = GetMessageLinkRegex();

    [SlashCommand("edit", "Edits an existing embed.", false)]
    public async Task EditAsync(InteractionContext context,
        [Option("message", "The link to the message to edit.")]
        string messageLink)
    {
        var response = new DiscordInteractionResponseBuilder();
        Match match = MessageLinkRegex.Match(messageLink);

        if (!match.Success)
        {
            response.WithContent("Invalid message link.");
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        ulong guildId = ulong.Parse(match.Groups[1].Value);
        ulong channelId = ulong.Parse(match.Groups[2].Value);
        ulong messageId = ulong.Parse(match.Groups[3].Value);

        if (guildId != context.Guild.Id)
        {
            response.WithContent("The message must be in this guild.");
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        DiscordChannel? channel = context.Guild.GetChannel(channelId);
        if (channel is null)
        {
            response.WithContent($"The channel {channelId} does not exist.");
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        DiscordMessage? message = await channel.GetMessageAsync(messageId);
        if (message is null)
        {
            response.WithContent($"The message {messageId} does not exist.");
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        if (message.Author.Id != context.Client.CurrentUser.Id)
        {
            response.WithContent("The message must be sent by this bot.");
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        if (message.Embeds.Count == 0)
        {
            response.WithContent("The message must contain an embed to edit.");
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
            return;
        }

        var builder = new DiscordEmbedBuilder(message.Embeds[0]);

        var modal = new DiscordModalBuilder(context.Client);
        modal.WithTitle("Edit Embed");

        DiscordModalTextInput titleInput = modal.AddInput("Title", "The title of the embed.", maxLength: 256, isRequired: false, initialValue: builder.Title);
        DiscordModalTextInput colorInput = modal.AddInput("Color", "e.g. #007EC6", maxLength: 7, isRequired: false, initialValue: builder.Color.HasValue ? builder.Color.Value.ToString() : null);
        DiscordModalTextInput descriptionInput = modal.AddInput("Description", "The body of the embed.", isRequired: true, maxLength: 2048, inputStyle: TextInputStyle.Paragraph, initialValue: builder.Description);

        DiscordModalResponse modalResponse =
            await modal.Build().RespondToAsync(context.Interaction, TimeSpan.FromMinutes(5));
        if (modalResponse == DiscordModalResponse.Timeout)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(titleInput.Value) && string.IsNullOrWhiteSpace(descriptionInput.Value))
        {
            return;
        }

        DiscordColor color = builder.Color.HasValue ? builder.Color.Value : DiscordColor.CornflowerBlue;

        if (!string.IsNullOrWhiteSpace(colorInput.Value))
        {
            color = new DiscordColor(colorInput.Value);
        }

        builder.WithColor(color);
        builder.WithTitle(titleInput.Value);
        builder.WithDescription(descriptionInput.Value);

        await message.ModifyAsync(embed: builder.Build());
        await context.FollowUpAsync(new DiscordFollowupMessageBuilder().WithContent($"Embed edited for {message.JumpLink}"));
    }

    [GeneratedRegex("^https://discord(?:canary)?\\.com/channels/(\\d+)/(\\d+)/(\\d+)$", RegexOptions.Compiled)]
    private static partial Regex GetMessageLinkRegex();
}
