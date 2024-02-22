using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Command("logo")]
        public async Task logoCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"High resolution discord logo: {ctx.User.DefaultAvatarUrl}!");
        }
    }
}