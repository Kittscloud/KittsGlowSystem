using KittsGlowSystem.Features;
using KittsGlowSystem.Features.Database;
using LabApi.Features;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
using System;
#if MONGODB
using KittsGlowSystem.Features.Events;
using LabApi.Events.CustomHandlers;
#endif

namespace KittsGlowSystem;

public class KittsGlowSystem : Plugin
{
    public static Plugin Instance { get; private set; }

    public override string Name { get; } = "KittsGlowSystem";
    public override string Author { get; } = "Kittscloud";
    public override string Description { get; } = "";
    public override LoadPriority Priority { get; } = LoadPriority.Medium;
    public override Version Version { get; } = new Version(0, 1, 1);
    public override Version RequiredApiVersion { get; } = new Version(LabApiProperties.CompiledVersion);

    public static Config Config { get; set; }
    private bool _errorLoadingConfig = false;

#if MONGODB
    private GlowEvents _glowEvents;
#endif

    public override void Enable()
    {
        Instance = this;

        if (_errorLoadingConfig)
            Log.Error("Invalid config file, check config file or generate a new one.");

#if MONGODB
        DatabaseMongo.Init();

        _glowEvents = new();

        CustomHandlersManager.RegisterEventsHandler(_glowEvents);
#else
        DatabaseJson.Init();
#endif

        PlayerGlowManager.Instance.Start();

        Log.Send($"Successfully Enabled {Name}@{Version}", colour: ConsoleColor.Green);
    }

    public override void Disable()
    {
        this.SaveConfig(Config, "config.yml");

#if MONGODB
        CustomHandlersManager.UnregisterEventsHandler(_glowEvents);

        _glowEvents = null;

        DatabaseMongo.Stop();
#else
        DatabaseJson.Stop();
#endif

        PlayerGlowManager.Instance.Stop();

        Instance = null;

        Log.Send($"Successfully Disabled {Name}@{Version}", colour: ConsoleColor.Green);
    }

    public override void LoadConfigs()
    {
        _errorLoadingConfig = !this.TryLoadConfig("config.yml", out Config config);
        Config = config ?? new Config();

        base.LoadConfigs();
    }
}