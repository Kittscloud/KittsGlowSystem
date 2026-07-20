using CommandSystem;
using KittsGlowSystem.Features.Database;
using KittsGlowSystem.Features.Models;
using KittsGlowSystem.Features.Types;
using LabApi.Features.Wrappers;
using System;
using System.Linq;

namespace KittsGlowSystem.Features.Commands.AdminGlowCommands;

internal sealed class AdminGlowColourCommand : ICommand
{
    public string Command => "colour";
    public string[] Aliases => ["color", "c"];
    public string Description => "Set glow colour";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 2)
        {
            response = $"Usage: aglow colour <player> <colour>\nAvailable Colours: {string.Join(", ", ColourTypes.AllValid.Keys.Select(c => c.ToString().ToLower()))}";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player player))
        {
            response = "Player not found";
            return false;
        }

        if (!ColourTypes.TryGet(arguments.At(1), out ColourInfo colour))
        {
            response = $"Unknown colour.\nAvailable colours: {string.Join(", ", ColourTypes.AllValid.Keys.Select(c => c.ToString().ToLower()))}";
            return false;
        }

        Player.Get(sender).GetGlowData().GlowColour = colour.EnumColour;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Badge colour set to {colour.EnumColour}.";

        Log.Debug("BadgeColourCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}
