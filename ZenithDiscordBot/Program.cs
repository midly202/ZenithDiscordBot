using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using ZenithDiscordBot.commands.prefix;
using ZenithDiscordBot.commands.slash;

/* 
 * TO-D0:
 * 
 * Discord + Github webhook thing
 * 
 * !help command that will only show up to the user that executed the command (ephemeral message)
 * ^ maybe not possible but /help is possible
 * 
 * /help command that will only show up to the user that executed the command (ephemeral message)
 * ^ done, need to edit button event handler to be the same
 * 
 * /blackjack command to play blackjack (ephemeral message)
 * 
 * /ticket commmand to make a ticket to communicate with the admins on
*/

namespace ZenithBot
{
    public sealed class Program
    {
        public static DiscordClient Client { get; private set; }
        public static CommandsNextExtension Commands { get; private set; }
        static async Task Main(string[] args)
        {
            var jsonReader = new ZenithDiscordBot.config.JSONReader();
            await jsonReader.ReadJSON();

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All, 
                Token = jsonReader.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            Client = new DiscordClient(discordConfig);

            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            Client.Ready += Client_Ready;
            Client.ComponentInteractionCreated += ComponentInteractionHandler;

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { jsonReader.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false,
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.CommandErrored += CommandHandler;

            var slashCommandsConfiguration = Client.UseSlashCommands();

            Commands.RegisterCommands<ZenithCommands>();
            slashCommandsConfiguration.RegisterCommands<ZenithSlash>();
            slashCommandsConfiguration.RegisterCommands<CalculatorSlash>();

            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task ComponentInteractionHandler(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {
            switch (e.Interaction.Data.CustomId)
            {
                case "prefixButton":

                    await e.Interaction.DeferAsync();
                    var prefixButtonEmbed = new DiscordEmbedBuilder
                    {
                        Color = DiscordColor.DarkButNotBlack,
                        Title = "Prefix Commands",
                        Description = "!help -> Shows a list of all the bot's commands. \n\n" +
                                      "!whoami -> Returns information about your account. \n\n" +
                                      "!cardgame -> Draws a random card for both the user and the bot. Whoever's card is worth more, wins. \n\n" +
                                      "!poll -> Creates a poll with 4 options, results are gathered after 30 seconds. Syntax: !poll Option1 Option2 Option3 Option4 Poll Title"
                    };
                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().AddEmbed(prefixButtonEmbed));
                    break;

                case "slashButton":

                    await e.Interaction.DeferAsync();
                    var slashButtonEmbed = new DiscordEmbedBuilder
                    {
                        Color = DiscordColor.DarkButNotBlack,
                        Title = "Slash Commands",
                        Description = "/stalk -> Returns information about specified user's account. \n\n" +
                                      "/calculator -> Group of commands that allow the user to perform simple arithmatic operations. "
                    };
                    await e.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder().AddEmbed(slashButtonEmbed));
                    break;

            }
        }

        private static async Task CommandHandler(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            if (e.Exception is ChecksFailedException exception)
            {
                string timeLeft = string.Empty;

                foreach (var check in exception.FailedChecks)
                {
                    var coolDown = (CooldownAttribute)check;
                    timeLeft = coolDown.GetRemainingCooldown(e.Context).ToString(@"hh\:mm\:ss");
                }

                var coolDownMessage = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Title = "Command on cooldown!",
                    Description = $"Time left: {timeLeft}"
                };

                await e.Context.Channel.SendMessageAsync(embed: coolDownMessage);
            }
        }

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}