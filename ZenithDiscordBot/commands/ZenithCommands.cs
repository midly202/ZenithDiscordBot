using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ZenithDiscordBot.other;

namespace ZenithDiscordBot.commands
{
    public class ZenithCommands : BaseCommandModule
    {
        [Command("whoami")]
        public async Task whoamiCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"Account ping: {ctx.User.Mention}");
            await ctx.Channel.SendMessageAsync($"Account username: {ctx.User.Username}");
            await ctx.Channel.SendMessageAsync($"Account ID: {ctx.User.Id}");
            await ctx.Channel.SendMessageAsync($"Account creation date: {ctx.User.CreationTimestamp}");
            await ctx.Channel.SendMessageAsync($"Account PFP: {ctx.User.AvatarUrl}");
        }

        [Command("embed1")]
        public async Task embed1Command(CommandContext ctx)
        {
            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithTitle("First embed method")
                .WithDescription($"This command was executed by {ctx.User.Mention}")
                .WithColor(DiscordColor.Aquamarine));

            await ctx.Channel.SendMessageAsync(message);
        }

        [Command("embed2")]
        public async Task embed2Command(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "Second embed method",
                Description = $"This command was executed by {ctx.User.Mention}",
                Color = DiscordColor.Aquamarine
            };

            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("insult")]
        public async Task insultCommand(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = $"Yo {ctx.User.Username}, you're a bitch",
                Description = $"This command was executed by {ctx.User.Mention}",
                Color = DiscordColor.Red
            };

            var user = ctx.User as DiscordMember;
            await user.SendMessageAsync(embed: embed);
        }


        [Command("cardgame")]
        public async Task cardgameCommand(CommandContext ctx)
        {
            var userCard = new CardSystem();
            var botCard = new CardSystem();

            var userCardEmbed = new DiscordEmbedBuilder
            {
                Title = $"You drew a {userCard.SelectedCard}",
            };
            await ctx.Channel.SendMessageAsync(embed: userCardEmbed);

            var botCardEmbed = new DiscordEmbedBuilder
            {
                Title = $"Bot drew a {botCard.SelectedCard}",
            };
            await ctx.Channel.SendMessageAsync(embed: botCardEmbed);

            if (userCard.SelectedNumber > botCard.SelectedNumber)
            {
                var userWinsEmbed = new DiscordEmbedBuilder
                {
                    Title = $"{ctx.User.Username} wins!",
                    Color = DiscordColor.Aquamarine
                };

                await ctx.Channel.SendMessageAsync(embed: userWinsEmbed);
            }
            else if(userCard.SelectedNumber < botCard.SelectedNumber)
            {
                var botWinsEmbed = new DiscordEmbedBuilder
                {
                    Title = $"The bot wins!",
                    Color = DiscordColor.Red
                };

                await ctx.Channel.SendMessageAsync(embed:  botWinsEmbed);
            }
            else
            {
                var tieEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Tie!",
                    Color = DiscordColor.CornflowerBlue
                };

                await ctx.Channel.SendMessageAsync(embed: tieEmbed);
            }
        }
    }
}