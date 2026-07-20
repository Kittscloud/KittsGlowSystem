#if MONGODB
using KittsGlowSystem.Features.Database;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace KittsGlowSystem.Features.Events;

internal sealed class GlowEvents : CustomEventsHandler
{
    public override void OnPlayerPreAuthenticated(PlayerPreAuthenticatedEventArgs ev)
    {
        DatabaseMongo.LoadGlowData(ev.UserId);
    }

    public override void OnPlayerLeft(PlayerLeftEventArgs ev)
    {
        ev.Player.UploadGlowData();
    }
}
#endif