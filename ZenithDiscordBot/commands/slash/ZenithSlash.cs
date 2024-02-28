using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace ZenithDiscordBot.commands.slash
{
    public class ZenithSlash : ApplicationCommandModule
    {
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
