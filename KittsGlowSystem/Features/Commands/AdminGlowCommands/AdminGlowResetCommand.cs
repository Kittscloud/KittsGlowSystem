using CommandSystem;
using KittsGlowSystem.Features.Database;
using KittsGlowSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsGlowSystem.Features.Commands.AdminGlowCommands;

internal sealed class AdminGlowResetCommand : ICommand
{
    public string Command => "reset";
    public string[] Aliases => [];
    public string Description => "Reset glow";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 1)
        {
            response = $"Usage: aglow reset <player>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player player))
        {
            response = "Player not found";
            return false;
        }

        GlowData data = player.GetGlowData();

        data.GlowEnabled = false;
        data.GlowColour = Enums.Colour.Clear;
        data.ShadowEnabled = true;
        data.Intensity = 2f;
        data.Range = 8f;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Reset {player.DisplayName}'s glow";

        Log.Debug("AdminGlowResetCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}