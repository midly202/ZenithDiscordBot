using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace ZenithDiscordBot.commands.slash
{
    public class ZenithSlash : ApplicationCommandModule
    {
        [SlashCommand("help", "Returns information about the bot and how it works.")]
        public async Task helpSlashCommand(InteractionContext ctx)
        {
            var prefixButton = new DiscordButtonComponent(ButtonStyle.Primary, "prefixButton", "Prefix Commands");
            var slashButton = new DiscordButtonComponent(ButtonStyle.Success, "slashButton", "Slash Commands");

            var message = new DiscordInteractionResponseBuilder()
                    .WithContent($"Please select what you need help with: \n" +
                                 $"If you want to report a bug, send a DM to <@!339773443333554176>.") // maybe change later?
                    .AddComponents(prefixButton)
                    .AddComponents(slashButton)
                    .AsEphemeral(true);

            await ctx.CreateResponseAsync(message);
        }

        [SlashCommand("stalk", "Returns information about mentioned user.")]
        public async Task stalkSlashCommand(InteractionContext ctx, [Option("user", "username")] DiscordUser user)
        {
            var button = new DiscordButtonComponent(ButtonStyle.Primary, "button1", "Button");

            await ctx.DeferAsync();
            //var member = (DiscordMember)user;
            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Violet,
                Title = "Account Information",
                Description = $"Account ping: {user.Mention} \n" +
                              $"Account username: {user.Username} \n" +
                              $"Account ID: {user.Id} \n" +
                              $"Account creation date: {user.CreationTimestamp} \n" +
                              $"Account PFP: {user.AvatarUrl} \n",
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = $"This command was executed by {ctx.User.Username}" },
                
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
    }
}
