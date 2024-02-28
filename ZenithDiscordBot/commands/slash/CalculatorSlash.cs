using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace ZenithDiscordBot.commands.slash
{
    [SlashCommandGroup("calculator", "Perform calculations")]
    public class CalculatorSlash : ApplicationCommandModule
    {
        [SlashCommand("add", "Returns sum after addition")]
        public async Task Add(InteractionContext ctx, [Option("number1", "First number")] double number1, [Option("number2", "Second number")] double number2)
        {
            await ctx.DeferAsync();

            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.IndianRed,
                Title = $"{number1} + {number2} = {number1 + number2}",
                Description = $"This command was executed by {ctx.User.Mention}"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }

        [SlashCommand("subtract", "Returns sum after subtraction.")]
        public async Task Sub(InteractionContext ctx, [Option("number1", "First number")] double number1, [Option("number2", "Second number")] double number2)
        {
            await ctx.DeferAsync();

            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.IndianRed,
                Title = $"{number1} - {number2} = {number1 - number2}",
                Description = $"This command was executed by {ctx.User.Mention}"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }

        [SlashCommand("multiply", "Returns sum of 2 multiplied numbers.")]
        public async Task Multiply(InteractionContext ctx, [Option("number1", "First number")] double number1, [Option("number2", "Second number")] double number2)
        {
            await ctx.DeferAsync();

            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.IndianRed,
                Title = $"{number1} * {number2} = {number1 * number2}",
                Description = $"This command was executed by {ctx.User.Mention}"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }

        [SlashCommand("divide", "Returns sum after division")]
        public async Task Divide(InteractionContext ctx, [Option("number1", "First number")] double number1, [Option("number2", "Second number")] double number2)
        {
            await ctx.DeferAsync();

            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.IndianRed,
                Title = $"{number1} : {number2} = {number1 / number2}",
                Description = $"This command was executed by {ctx.User.Mention}"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }

        [SlashCommand("modulo", "Returns remainder of division")]
        public async Task Modulo(InteractionContext ctx, [Option("number1", "First number")] double number1, [Option("number2", "Second number")] double number2)
        {
            await ctx.DeferAsync();

            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.IndianRed,
                Title = $"{number1} % {number2} = {number1 % number2}",
                Description = $"This command was executed by {ctx.User.Mention}"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
    }
}
