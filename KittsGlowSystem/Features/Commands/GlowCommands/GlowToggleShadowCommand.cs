using CommandSystem;
using KittsGlowSystem.Features.Database;
using KittsGlowSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsGlowSystem.Features.Commands.GlowCommands;

internal sealed class GlowToggleShadowCommand : ICommand
{
    public string Command => "toggleshadow";
    public string[] Aliases => ["ts", "tos", "tgs"];
    public string Description => "Toggle glow shadow";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        GlowData data = Player.Get(sender).GetGlowData();

        data.ShadowEnabled = !data.ShadowEnabled;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Glow Shadow {(data.ShadowEnabled ? "enabled" : "disabled")}.";

        Log.Debug("GlowToggleCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}