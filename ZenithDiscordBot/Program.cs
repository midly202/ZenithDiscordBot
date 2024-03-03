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
 * Requirements in slash commands (only admins, only in guilds, etc)
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
                    var embed1 = new DiscordEmbedBuilder()
                        .WithTitle("Prefix commands")
                        .AddField("!cardgame", "Draws a random card for both the user and the bot. Whoever's card is worth more, wins.")
                        .WithColor(DiscordColor.DarkButNotBlack);

                    var message1 = new DiscordInteractionResponseBuilder()
                        .AddEmbed(embed1)
                        .AsEphemeral(true);

                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, message1);
                    break;

                case "slashButton":
                    var embed2 = new DiscordEmbedBuilder()
                        .WithTitle("Slash commands")
                        .AddField("/stalk", "Returns information about targeted user's account.")
                        .AddField("/poll", "Makes a poll and returns the results.")
                        .AddField("/caclulator", "Commands to do simple arithmatic.")
                        .WithColor(DiscordColor.DarkButNotBlack);

                    var message2 = new DiscordInteractionResponseBuilder()
                        .AddEmbed(embed2)
                        .AsEphemeral(true);

                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, message2);
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