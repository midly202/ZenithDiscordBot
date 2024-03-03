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
                .AsEphemeral(true);                ;

            await ctx.CreateResponseAsync(message);
        }

        [SlashCommand("stalk", "Returns information about targeted user.")]
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

        [SlashCommand("poll", "Makes a poll and returns the results.")]
        public async Task pollSlashCommand(InteractionContext ctx, [Option("time", "Time to vote in seconds")] double time, [Option("title", "Title of the poll")] string title, [Option("option1", "First option")] string option1, [Option("option2", "Second option")] string option2, [Option("option3", "Third option")] string option3, [Option("option4", "Fourth option")] string option4)
        {
            await ctx.DeferAsync();
            var interactivity = Program.Client.GetInteractivity();
            var pollTime = TimeSpan.FromSeconds(time);

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
                Title = title,
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
            string resultsDescription = $"{option1}: {count1} Votes \n" +
                                        $"{option2}: {count2} Votes \n" +
                                        $"{option3}: {count3} Votes \n" +
                                        $"{option4}: {count4} Votes \n \n" +
                                        $"Total votes: {totalVotes}";

            var resultEmbed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Aquamarine,
                Title = "Results of the poll",
                Description = resultsDescription
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(resultEmbed));
        }

    }
}
