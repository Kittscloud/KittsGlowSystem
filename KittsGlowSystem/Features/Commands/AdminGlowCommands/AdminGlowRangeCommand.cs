using CommandSystem;
using KittsGlowSystem.Features.Database;
using KittsGlowSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsGlowSystem.Features.Commands.AdminGlowCommands;

internal sealed class AdminGlowRangeCommand : ICommand
{
    public string Command => "range";
    public string[] Aliases => ["r"];
    public string Description => "Change range";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 2)
        {
            response = $"Usage: aglow range <player> <newrange>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player player))
        {
            response = "Player not found";
            return false;
        }

        if (!int.TryParse(arguments.At(1), out int newRange) || newRange < 1)
        {
            response = "Range must be a number greater than 0";
            return false;
        }

        GlowData data = player.GetGlowData();

        data.Range = newRange;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Glow range is now {data.Range}.";

        Log.Debug("GlowToggleCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}