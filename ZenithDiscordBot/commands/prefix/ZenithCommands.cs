using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using ZenithBot;
using ZenithDiscordBot.other;

namespace ZenithDiscordBot.commands.prefix
{
    public class ZenithCommands : BaseCommandModule
    {
        [Command("supersecretembedtest1")]
        [RequireGuild]
        public async Task embed1Command(CommandContext ctx)
        {
            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithTitle("First embed method")
                .WithDescription($"This command was executed by {ctx.User.Mention}")
                .WithFooter($"This command was executed by {ctx.User.Username}")
                .WithColor(DiscordColor.Aquamarine));

            await ctx.Channel.SendMessageAsync(message);
        }

        [Command("supersecretembedtest2")]
        [RequireGuild]
        public async Task embed2Command(CommandContext ctx)
        {
            var message = new DiscordEmbedBuilder
            {
                Title = "Second embed method",
                Description = $"This command was executed by {ctx.User.Mention}",
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = $"This command was executed by {ctx.User.Username}" },
                Color = DiscordColor.Aquamarine
            };

            await ctx.Channel.SendMessageAsync(embed: message);
        }

        [Command("supersecretdmtest")]
        [RequireGuild]
        public async Task dmCommand(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = $"Hi {ctx.User.Username}.",
                Description = "BOTTOM TEXT",
                Color = DiscordColor.DarkButNotBlack
            };

            var user = ctx.User as DiscordMember;
            await user.SendMessageAsync(embed: embed);
        }

        [Command("supersecretnwordtest")]
        [RequireGuild]
        public async Task nwordCommand(CommandContext ctx)
        {
            var interactivity = Program.Client.GetInteractivity();

            var messageToRetrieve = await interactivity.WaitForMessageAsync(message => message.Content == "nigga" || message.Content == "Nigga");

            if (messageToRetrieve.Result.Content == "nigga" || messageToRetrieve.Result.Content == "Nigga")
            {
                var userWhoSaidHello = messageToRetrieve.Result.Author.Username;
                await ctx.Channel.SendMessageAsync($"{userWhoSaidHello} said the forbidden word!");
            }
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
            else if (userCard.SelectedNumber < botCard.SelectedNumber)
            {
                var botWinsEmbed = new DiscordEmbedBuilder
                {
                    Title = $"The bot wins!",
                    Color = DiscordColor.Red
                };

                await ctx.Channel.SendMessageAsync(embed: botWinsEmbed);
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