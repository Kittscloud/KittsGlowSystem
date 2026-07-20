using CommandSystem;
using KittsGlowSystem.Features.Database;
using KittsGlowSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsGlowSystem.Features.Commands.AdminGlowCommands;

internal sealed class AdminGlowToggleShadowCommand : ICommand
{
    public string Command => "toggleshadow";
    public string[] Aliases => ["ts", "tos", "tgs"];
    public string Description => "Toggle shadow";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 1)
        {
            response = $"Usage: aglow toggleshadow <player>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player player))
        {
            response = "Player not found";
            return false;
        }

        GlowData data = player.GetGlowData();

        data.ShadowEnabled = !data.ShadowEnabled;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Glow Shadow {(data.ShadowEnabled ? "enabled" : "disabled")}.";

        Log.Debug("GlowToggleCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}