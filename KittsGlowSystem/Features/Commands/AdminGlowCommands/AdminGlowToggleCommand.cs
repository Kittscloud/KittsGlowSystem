using CommandSystem;
using KittsGlowSystem.Features.Database;
using KittsGlowSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsGlowSystem.Features.Commands.AdminGlowCommands;

internal sealed class AdminGlowToggleCommand : ICommand
{
    public string Command => "toggle";
    public string[] Aliases => ["t", "to", "tg"];
    public string Description => "Toggle glow";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 1)
        {
            response = $"Usage: aglow toggle <player>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player player))
        {
            response = "Player not found";
            return false;
        }

        GlowData data = player.GetGlowData();

        data.GlowEnabled = !data.GlowEnabled;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Glow {(data.GlowEnabled ? "enabled" : "disabled")}.";

        Log.Debug("AdminGlowToggleCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}