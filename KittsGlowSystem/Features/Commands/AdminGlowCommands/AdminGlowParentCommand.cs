using CommandSystem;
using KittsGlowSystem.Features.Database;
using KittsGlowSystem.Features.Models;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;

namespace KittsGlowSystem.Features.Commands.AdminGlowCommands;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
[CommandHandler(typeof(GameConsoleCommandHandler))]
internal sealed class AdminGlowParentCommand : ParentCommand
{
    public override string Command => "aglow";
    public override string[] Aliases => ["ag"];
    public override string Description => "Manage another player's glow";

    public AdminGlowParentCommand()
    {
        LoadGeneratedCommands();
    }

    public override void LoadGeneratedCommands()
    {
        RegisterCommand(new AdminGlowColourCommand());
        RegisterCommand(new AdminGlowIntensityCommand());
        RegisterCommand(new AdminGlowToggleCommand());
        RegisterCommand(new AdminGlowRangeCommand());
        RegisterCommand(new AdminGlowToggleShadowCommand());
        RegisterCommand(new AdminGlowResetCommand());
    }

    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.HasPermissions(KittsGlowSystem.Config.AdminGlowPermission))
        {
            response = "<color=red>You do not have permission to use this command.</color>";
            return false;
        }

        if (arguments.Count == 0)
        {
            response = "Usage: aglow <player>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player player))
        {
            response = "Player not found";
            return false;
        }

        GlowData data = player.GetGlowData();

        response =
            "<b><color=yellow>Glow Settings</color></b>\n\n" +
            $"Enabled: {(data.GlowEnabled ? "<color=green>Yes</color>" : "<color=red>No</color>")}\n" +
            $"Shadow: {(data.ShadowEnabled ? "<color=green>Yes</color>" : "<color=red>No</color>")}\n" +
            $"Colour: {data.GlowColour}\n" +
            $"Range: {data.Range}\n" +
            $"Intensity: {data.Intensity}\n\n" +
            "<b>Commands</b>\n" +
            $"aglow toggle {player.PlayerId}\n" +
            $"aglow toggleshadow {player.PlayerId}\n" +
            $"aglow colour {player.PlayerId} <colour>\n" +
            $"aglow range {player.PlayerId} <range>\n" +
            $"aglow intensity {player.PlayerId} <intensity>\n" +
            $"aglow reset {player.PlayerId}";

        Log.Debug("AdminGlowParentCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}