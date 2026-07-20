using KittsGlowSystem.Features.Database;
using KittsGlowSystem.Features.Enums;
using KittsGlowSystem.Features.Models;
using KittsGlowSystem.Features.Types;
using LabApi.Features.Wrappers;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace KittsGlowSystem.Features;

internal sealed class PlayerGlowManager
{
    public static readonly PlayerGlowManager Instance = new();

    private readonly Dictionary<Player, LightSourceToy> _playerLights = [];
    private readonly Dictionary<Player, GlowData> _cachedGlowData = [];

    private CoroutineHandle _handle;
    private bool _running;

    private const float UpdateIntervalSeconds = 1f;

    public void Start()
    {
        if (_running)
            return;

        _running = true;
        _handle = Timing.RunCoroutine(Run());

        Log.Debug($"PlayerGlowManager.Start", "Started");
    }

    public void Stop()
    {
        if (!_running)
            return;

        _playerLights.Clear();

        Timing.KillCoroutines(_handle);
        _running = false;

        Log.Debug($"PlayerGlowManager.Stop", "Stopped");
    }

    private IEnumerator<float> Run()
    {
        while (_running)
        {
            yield return Timing.WaitForSeconds(UpdateIntervalSeconds);
            yield return Timing.WaitUntilDone(Update());
        }
    }

    private IEnumerator<float> Update()
    {
        foreach (Player player in Player.ReadyList)
        {
            if (!_playerLights.TryGetValue(player, out LightSourceToy light) || light == null)
            {
                LightSourceToy lightObject = LightSourceToy.Create();

                lightObject.Transform.parent = player.GameObject.transform;
                lightObject.Transform.localPosition = new Vector3(0f, -0.3f, 0f);

                lightObject.Color = Color.clear;
                lightObject.ShadowType = LightShadows.Soft;
                lightObject.Intensity = 4f;
                lightObject.Range = 8f;

                _playerLights[player] = lightObject;
                _cachedGlowData[player] = null;

                light = lightObject;
            }

            GlowData data = player.GetGlowData();

            light.ShadowType = data.ShadowEnabled ? LightShadows.Soft : LightShadows.None;
            light.Range = data.Range;
            light.Intensity = data.Intensity;

            _cachedGlowData.TryGetValue(player, out GlowData cached);

            bool colourChanged = cached == null || cached.GlowColour != data.GlowColour;
            bool enabledChanged = cached == null || cached.GlowEnabled != data.GlowEnabled;

            if (colourChanged || enabledChanged)
            {
                Timing.RunCoroutine(FadeGlow(
                    light,
                    cached?.GlowEnabled ?? false,
                    data.GlowEnabled,
                    cached?.GlowColour ?? Colour.Clear,
                    data.GlowColour
                ));

                _cachedGlowData[player] = new()
                {
                    GlowEnabled = data.GlowEnabled,
                    GlowColour = data.GlowColour,
                    ShadowEnabled = data.ShadowEnabled,
                    Range = data.Range,
                    Intensity = data.Intensity
                };
            }

            yield return Timing.WaitForOneFrame;
        }
    }

    private IEnumerator<float> FadeGlow(
        LightSourceToy light,
        bool wasEnabled,
        bool isEnabled,
        Colour oldColour,
        Colour newColour
    )
    {
        Color start = wasEnabled ? ColourTypes.Get(oldColour).Colour : Color.clear;
        Color end = isEnabled ? ColourTypes.Get(newColour).Colour : Color.clear;

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            light.Color = Color.Lerp(start, end, t);

            float delta = 0.02f;
            elapsed += delta;
            yield return delta;
        }

        light.Color = end;

        Log.Debug("PlayerGlowManager.FadeGlow", $"{oldColour} {newColour}");
    }
}
