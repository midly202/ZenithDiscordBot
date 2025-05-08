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

namespace ZenithBot
{
    public sealed class Program
    {
        // Initialize as nullable and mark as initialized after creation
        public static DiscordClient Client { get; private set; } = null!;
        public static CommandsNextExtension Commands { get; private set; } = null!;

        static async Task Main(string[] args)
        {
            // KILL DUPLICATES ON STARTUP
            foreach (var proc in System.Diagnostics.Process.GetProcessesByName("ZenithDiscordBot"))
            {
                if (proc.Id != System.Diagnostics.Process.GetCurrentProcess().Id)
                    proc.Kill();
            }

            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex}");
            }
        }

        private static async Task ComponentInteractionHandler(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {
            if (e.Interaction == null) return;

            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling interaction: {ex}");
            }
        }

        private static async Task CommandHandler(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            if (e.Exception is ChecksFailedException exception)
            {
                string timeLeft = string.Empty;

                foreach (var check in exception.FailedChecks)
                {
                    if (check is CooldownAttribute coolDown)
                    {
                        timeLeft = coolDown.GetRemainingCooldown(e.Context).ToString(@"hh\:mm\:ss");
                    }
                }

                var coolDownMessage = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Title = "Command on cooldown!",
                    Description = $"Time left: {timeLeft}"
                };

                if (e.Context.Channel != null)
                {
                    await e.Context.Channel.SendMessageAsync(embed: coolDownMessage);
                }
            }
        }

        private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}