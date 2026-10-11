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
    private readonly Dictionary<Player, CoroutineHandle> _colourHandles = [];

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

        foreach (CoroutineHandle handle in _colourHandles.Values)
            if (handle.IsRunning)
                Timing.KillCoroutines(handle);

        _colourHandles.Clear();

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
                if (_colourHandles.TryGetValue(player, out CoroutineHandle colourHandle) && colourHandle.IsRunning)
                    Timing.KillCoroutines(colourHandle);

                _colourHandles.Remove(player);

                _colourHandles[player] = Timing.RunCoroutine(TransitionGlow(
                    player,
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

    private IEnumerator<float> TransitionGlow(
        Player player,
        LightSourceToy light,
        bool wasEnabled,
        bool isEnabled,
        Colour oldColour,
        Colour newColour
    )
    {
        Color start = wasEnabled ? ColourTypes.Get(oldColour).Colour : Color.clear;

        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);

            Color target = isEnabled
                ? ColourTypes.Get(newColour).Colour
                : Color.clear;

            light.Color = Color.Lerp(start, target, t);

            elapsed += 0.02f;
            yield return 0.02f;
        }

        if (!isEnabled)
        {
            light.Color = Color.clear;
            yield break;
        }

        if (newColour != Colour.Rainbow)
        {
            light.Color = ColourTypes.Get(newColour).Colour;
            Log.Debug("PlayerGlowManager.TransitionGlow", $"{oldColour} {newColour}");
            yield break;
        }

        while (player != null)
        {
            GlowData data = player.GetGlowData();

            if (!data.GlowEnabled || data.GlowColour != Colour.Rainbow)
                yield break;

            light.Color = ColourTypes.Get(Colour.Rainbow).Colour;

            yield return Timing.WaitForOneFrame;
        }
    }
}
