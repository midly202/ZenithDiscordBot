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
        [Command("help")]
        public async Task helpCommand(CommandContext ctx)
        {
            var prefixButton = new DiscordButtonComponent(ButtonStyle.Primary, "prefixButton", "Prefix Commands");
            var slashButton = new DiscordButtonComponent(ButtonStyle.Success, "slashButton", "Slash Commands");

            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithColor(DiscordColor.DarkButNotBlack)
                .WithTitle("Help Section")
                .WithDescription("Please press a button to view its commands"))
                .AddComponents(slashButton, prefixButton);
            

            await ctx.Channel.SendMessageAsync(message);
        }

        [Command("whoami")]
        public async Task whoamiCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"Account ping: {ctx.User.Mention}");
            await ctx.Channel.SendMessageAsync($"Account username: {ctx.User.Username}");
            await ctx.Channel.SendMessageAsync($"Account ID: {ctx.User.Id}");
            await ctx.Channel.SendMessageAsync($"Account creation date: {ctx.User.CreationTimestamp}");
            await ctx.Channel.SendMessageAsync($"Account PFP: {ctx.User.AvatarUrl}");
        }

        [Command("supersecretembed1test")]
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

        [Command("supersecretembed2test")]
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

        [Command("dm")]
        [RequireGuild]
        public async Task dmCommand(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = $"Yo {ctx.User.Username}",
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

        [Command("nword")]
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

        [Command("poll")] // rewrite as slash command
        [RequirePermissions(DSharpPlus.Permissions.Administrator)]
        [RequireGuild]
        [Cooldown(1, 30, CooldownBucketType.Channel)]
        public async Task pollCommand(CommandContext ctx, string option1, string option2, string option3, string option4, [RemainingText] string pollTitle)
        {
            var interactivity = Program.Client.GetInteractivity();
            var pollTime = TimeSpan.FromSeconds(30);

            DiscordEmoji[] emojiOptions = { DiscordEmoji.FromName(Program.Client, ":one:"),
                                            DiscordEmoji.FromName(Program.Client, ":two:"),
                                            DiscordEmoji.FromName(Program.Client, ":three:"),
                                            DiscordEmoji.FromName(Program.Client, ":four:")};

            string optionsDescription = $"{emojiOptions[0]} | {option1} \n" +
                                        $"{emojiOptions[1]} | {option2} \n" +
                                        $"{emojiOptions[2]} | {option3} \n" +
                                        $"{emojiOptions[3]} | {option4}";

            var pollMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Title = pollTitle,
                Description = optionsDescription
            };

            var sentPoll = await ctx.Channel.SendMessageAsync(embed: pollMessage);
            foreach (var emoji in emojiOptions)
            {
                await sentPoll.CreateReactionAsync(emoji);
            }

            var totalReactions = await interactivity.CollectReactionsAsync(sentPoll, pollTime);

            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;

            foreach (var emoji in totalReactions)
            {
                if (emoji.Emoji == emojiOptions[0])
                {
                    count1++;
                }
                if (emoji.Emoji == emojiOptions[1])
                {
                    count2++;
                }
                if (emoji.Emoji == emojiOptions[2])
                {
                    count3++;
                }
                if (emoji.Emoji == emojiOptions[3])
                {
                    count4++;
                }
            }

            int totalVotes = count1 + count2 + count3 + count4;
            string resultsDescription = $"{emojiOptions[0]}: {count1} Votes \n" +
                                        $"{emojiOptions[1]}: {count2} Votes \n" +
                                        $"{emojiOptions[2]}: {count3} Votes \n" +
                                        $"{emojiOptions[3]}: {count4} Votes \n \n" +
                                        $"Total votes: {totalVotes}";

            var resultEmbed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Aquamarine,
                Title = "Results of the poll",
                Description = resultsDescription
            };

            await ctx.Channel.SendMessageAsync(embed: resultEmbed);
        }
    }
}