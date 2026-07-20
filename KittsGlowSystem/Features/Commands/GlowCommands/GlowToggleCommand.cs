using CommandSystem;
using KittsGlowSystem.Features.Database;
using KittsGlowSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsGlowSystem.Features.Commands.GlowCommands;

internal sealed class GlowToggleCommand : ICommand
{
    public string Command => "toggle";
    public string[] Aliases => ["t", "to", "tg"];
    public string Description => "Toggle glow";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        GlowData data = Player.Get(sender).GetGlowData();

        data.GlowEnabled = !data.GlowEnabled;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Glow {(data.GlowEnabled ? "enabled" : "disabled")}.";

        Log.Debug("GlowToggleCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}