using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using ZenithBot;

namespace ZenithDiscordBot.commands.slash
{
    public class ZenithSlash : ApplicationCommandModule
    {
        [SlashCommand("help", "Returns information about the bot and how it works.")]
        public async Task helpSlashCommand(InteractionContext ctx)
        {
            var prefixButton = new DiscordButtonComponent(ButtonStyle.Primary, "prefixButton", "Prefix Commands");
            var slashButton = new DiscordButtonComponent(ButtonStyle.Success, "slashButton", "Slash Commands");

            var embed = new DiscordEmbedBuilder()
                .WithTitle("Need Help?")
                .WithDescription("Please select what you need help with.")
                .AddField("Report a Bug", "If you want to report a bug, send a DM to <@!339773443333554176>. \n")
                .AddField("Support Server", "If you wish to join the support server, join here: https://discord.gg/behEHApJhQ")
                .WithColor(DiscordColor.DarkButNotBlack);

            var message = new DiscordInteractionResponseBuilder()
                .AddEmbed(embed)
                .AddComponents(prefixButton, slashButton)
                .AsEphemeral(true);

            await ctx.CreateResponseAsync(message);
        }

        [SlashCommand("stalk", "Returns information about targeted user.")]
        public async Task stalkSlashCommand(InteractionContext ctx, [Option("user", "username")] DiscordUser user)
        {
            var button = new DiscordButtonComponent(ButtonStyle.Primary, "button1", "Button");

            //var member = (DiscordMember)user;
            var embed = new DiscordEmbedBuilder
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

            var message = new DiscordInteractionResponseBuilder()
                .AddEmbed(embed)
                .AsEphemeral(true);

            await ctx.CreateResponseAsync(message);
        }
    }
}
