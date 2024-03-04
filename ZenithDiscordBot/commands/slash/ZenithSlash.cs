using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using System.Text.RegularExpressions;
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
        public async Task pollSlashCommand(InteractionContext ctx, [Option("time", "Time to vote in seconds")] double time, [Option("title", "Poll title")] string title, [Option("options", "Poll options, seperate by comma (option1, option2)")] string options)
        {
            await ctx.DeferAsync();
            var interactivity = Program.Client.GetInteractivity();
            var pollTime = TimeSpan.FromSeconds(time);

            string[] optionsArray = options.Split(',').Select(option => option.Trim()).ToArray();

            var failedMessage = new DiscordEmbedBuilder
            {
                Title = "Error!",
                Description = "Poll must have 2 to 9 options.",
                Color = DiscordColor.DarkButNotBlack
            };

            var failedMessageWebhook = new DiscordWebhookBuilder()
                .AddEmbed(failedMessage);

            int optionCount = optionsArray.Length;
            if (optionCount < 2 || optionCount > 9)
            {
                await ctx.EditResponseAsync(failedMessageWebhook);                                          // Failed Response
                return;
            }

            DiscordEmoji[] emojiOptions = {
                DiscordEmoji.FromName(Program.Client, ":one:"),
                DiscordEmoji.FromName(Program.Client, ":two:"),
                DiscordEmoji.FromName(Program.Client, ":three:"),
                DiscordEmoji.FromName(Program.Client, ":four:"),
                DiscordEmoji.FromName(Program.Client, ":five:"),
                DiscordEmoji.FromName(Program.Client, ":six:"),
                DiscordEmoji.FromName(Program.Client, ":seven:"),
                DiscordEmoji.FromName(Program.Client, ":eight:"),
                DiscordEmoji.FromName(Program.Client, ":nine:")
    };

            var pollMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Title = title,
                Description = ""
            };

            for (int i = 0; i < optionCount; i++)
            {
                pollMessage.Description += $"{emojiOptions[i]} | {optionsArray[i]}\n";
            }

            var pollMessageWebhook = new DiscordWebhookBuilder()
                .AddEmbed(pollMessage);

            var sentPoll = await ctx.EditResponseAsync(pollMessageWebhook);                                 // Poll Message
            for (int i = 0; i < optionCount; i++)
            {
                await sentPoll.CreateReactionAsync(emojiOptions[i]);
            }

            var totalReactions = await interactivity.CollectReactionsAsync(sentPoll, pollTime);

            int[] voteCounts = new int[optionsArray.Length];
            for (int i = 0; i < optionCount; i++)
            {
                voteCounts[i] = 0;
            }

            foreach (var emoji in totalReactions)
            {
                for (int i = 0; i < optionCount; i++)
                {
                    if (emoji.Emoji == emojiOptions[i])
                    {
                        voteCounts[i]++;
                        break;
                    }
                }
            }

            int totalVotes = 0;
            string resultsDescription = "";

            for (int i = 0; i < optionCount; i++)
            {
                totalVotes += voteCounts[i];
                resultsDescription += $"{optionsArray[i]}: {voteCounts[i]} Votes\n";
            }

            resultsDescription += $"\nTotal votes: {totalVotes}";

            var resultEmbed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Aquamarine,
                Title = "Results of the poll",
                Description = resultsDescription
            };

            await sentPoll.DeleteAllReactionsAsync();
            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(resultEmbed));                 // Results Message
        }

    }
}
