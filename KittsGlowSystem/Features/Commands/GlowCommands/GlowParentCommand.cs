using CommandSystem;
using KittsGlowSystem.Features.Database;
using KittsGlowSystem.Features.Models;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;

namespace KittsGlowSystem.Features.Commands.GlowCommands;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal sealed class GlowParentCommand : ParentCommand
{
    public override string Command => "glow";
    public override string[] Aliases => ["g"];
    public override string Description => "Manage your glow";

    public GlowParentCommand()
    {
        LoadGeneratedCommands();
    }

    public override void LoadGeneratedCommands()
    {
        RegisterCommand(new GlowToggleCommand());
        RegisterCommand(new GlowColourCommand());
        RegisterCommand(new GlowToggleShadowCommand());
    }

    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.HasPermissions(KittsGlowSystem.Config.PlayerGlowPermission))
        {
            response = "<color=red>You do not have permission to use this command.</color>";
            return false;
        }

        Player player = Player.Get(sender);
        GlowData data = player.GetGlowData();

        response =
            "<b><color=yellow>Glow Settings</color></b>\n\n" +
            $"Enabled: {(data.GlowEnabled ? "<color=green>Yes</color>" : "<color=red>No</color>")}\n" +
            $"Shadow: {(data.ShadowEnabled ? "<color=green>Yes</color>" : "<color=red>No</color>")}\n" +
            $"Colour: {data.GlowColour}\n\n" +
            "<b>Commands</b>\n" +
            "glow toggle\n" +
            "glow toggleshadow\n" +
            "glow colour <colour>";

        Log.Debug("GlowParentCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}