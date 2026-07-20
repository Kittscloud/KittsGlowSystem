using CommandSystem;
using KittsGlowSystem.Features.Database;
using KittsGlowSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsGlowSystem.Features.Commands.AdminGlowCommands;

internal sealed class AdminGlowIntensityCommand : ICommand
{
    public string Command => "intensity";
    public string[] Aliases => ["i"];
    public string Description => "Change intensity";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 2)
        {
            response = $"Usage: aglow intensity <player> <newintensity>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player player))
        {
            response = "Player not found";
            return false;
        }

        if (!int.TryParse(arguments.At(1), out int newIntensity) || newIntensity < 1)
        {
            response = "Range must be a number greater than 1";
            return false;
        }

        GlowData data = player.GetGlowData();

        data.Intensity = newIntensity;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Glow intensity is now {data.Intensity}.";

        Log.Debug("GlowToggleCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}