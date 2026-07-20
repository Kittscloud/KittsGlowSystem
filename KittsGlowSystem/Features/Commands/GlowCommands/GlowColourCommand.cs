using CommandSystem;
using KittsGlowSystem.Features.Database;
using KittsGlowSystem.Features.Models;
using KittsGlowSystem.Features.Types;
using LabApi.Features.Wrappers;
using System;
using System.Linq;

namespace KittsGlowSystem.Features.Commands.GlowCommands;

internal sealed class GlowColourCommand : ICommand
{
    public string Command => "colour";
    public string[] Aliases => ["color", "c"];
    public string Description => "Set badge colour";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 1)
        {
            response = $"Usage: badge colour <colour>\nAvailable Colours: {string.Join(", ", ColourTypes.AllValid.Keys.Select(c => c.ToString().ToLower()))}";
            return false;
        }

        if (!ColourTypes.TryGet(arguments.At(0), out ColourInfo colour))
        {
            response = $"Unknown colour.\nAvailable colours: {string.Join(", ", ColourTypes.AllValid.Keys.Select(c => c.ToString().ToLower()))}";
            return false;
        }

        Player.Get(sender).GetGlowData().GlowColour = colour.EnumColour;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Glow colour set to {colour.EnumColour}.";

        Log.Debug("GlowColourCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}
