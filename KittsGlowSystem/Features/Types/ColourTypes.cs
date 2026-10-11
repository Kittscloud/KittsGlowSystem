using KittsGlowSystem.Features.Enums;
using KittsGlowSystem.Features.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KittsGlowSystem.Features.Types;

public static class ColourTypes
{
    public static readonly IReadOnlyDictionary<Colour, ColourInfo> ServerColours = new Dictionary<Colour, ColourInfo>
    {
        [Colour.Pink] = new("Pink", "pink", Colour.Pink, new Color(1f, 0.588235f, 0.858824f)),
        [Colour.Red] = new("Red", "red", Colour.Red, new Color(0.772549f, 0f, 0f)),
        [Colour.Brown] = new("Brown", "brown", Colour.Brown, new Color(0.580392f, 0.278431f, 0.062745f)),
        [Colour.Silver] = new("Silver", "silver", Colour.Silver, new Color(0.627451f, 0.627451f, 0.627451f)),
        [Colour.LightGreen] = new("Light Green", "light_green", Colour.LightGreen, new Color(0.196078f, 0.803922f, 0.196078f)),
        [Colour.Crimson] = new("Crimson", "crimson", Colour.Crimson, new Color(0.862745f, 0.078431f, 0.235294f)),
        [Colour.Cyan] = new("Cyan", "cyan", Colour.Cyan, new Color(0f, 0.717647f, 0.921569f)),
        [Colour.Aqua] = new("Aqua", "aqua", Colour.Aqua, new Color(0f, 1f, 1f)),
        [Colour.DeepPink] = new("Deep Pink", "deep_pink", Colour.DeepPink, new Color(1f, 0.078431f, 0.576471f)),
        [Colour.Tomato] = new("Tomato", "tomato", Colour.Tomato, new Color(1f, 0.392157f, 0.282353f)),
        [Colour.Yellow] = new("Yellow", "yellow", Colour.Yellow, new Color(0.980392f, 1f, 0.52549f)),
        [Colour.Magenta] = new("Magenta", "magenta", Colour.Magenta, new Color(1f, 0f, 0.564706f)),
        [Colour.BlueGreen] = new("Blue Green", "blue_green", Colour.BlueGreen, new Color(0.301961f, 0.972549f, 0.721569f)),
        [Colour.Orange] = new("Orange", "orange", Colour.Orange, new Color(1f, 0.6f, 0.4f)),
        [Colour.Lime] = new("Lime", "lime", Colour.Lime, new Color(0.74902f, 1f, 0f)),
        [Colour.Green] = new("Green", "green", Colour.Green, new Color(0.168627f, 0.545098f, 0.133333f)),
        [Colour.Emerald] = new("Emerald", "emerald", Colour.Emerald, new Color(0.313725f, 0.784314f, 0.470588f)),
        [Colour.Carmine] = new("Carmine", "carmine", Colour.Carmine, new Color(0.588235f, 0f, 0.094118f)),
        [Colour.Nickel] = new("Nickel", "nickel", Colour.Nickel, new Color(0.447059f, 0.454902f, 0.447059f)),
        [Colour.Mint] = new("Mint", "mint", Colour.Mint, new Color(0.596078f, 0.984314f, 0.596078f)),
        [Colour.ArmyGreen] = new("Army Green", "army_green", Colour.ArmyGreen, new Color(0.294118f, 0.32549f, 0.12549f)),
        [Colour.Pumpkin] = new("Pumpkin", "pumpkin", Colour.Pumpkin, new Color(0.933333f, 0.462745f, 0f))
    };
    public static readonly IReadOnlyDictionary<Colour, ColourInfo> GlobalColours = new Dictionary<Colour, ColourInfo>
    {
        [Colour.Gold] = new("Gold", "gold", Colour.Gold, new Color(1f, 0.752941f, 0.101961f)),
        [Colour.Teal] = new("Teal", "teal", Colour.Teal, new Color(0f, 0.501961f, 0.501961f)),
        [Colour.Blue] = new("Blue", "blue", Colour.Blue, new Color(0f, 0.368627f, 0.737255f)),
        [Colour.Purple] = new("Purple", "purple", Colour.Purple, new Color(0.505882f, 0.215686f, 0.807843f)),
        [Colour.LightRed] = new("Light Red", "light_red", Colour.LightRed, new Color(0.847059f, 0.152941f, 0.176471f)),
        [Colour.SilverBlue] = new("Silver Blue", "silver_blue", Colour.SilverBlue, new Color(0.4f, 0.4f, 0.6f)),
        [Colour.PoliceBlue] = new("Police Blue", "police_blue", Colour.PoliceBlue, new Color(0f, 0.176471f, 0.701961f))
    };
    public static readonly IReadOnlyDictionary<Colour, ColourInfo> OtherColours = new Dictionary<Colour, ColourInfo>
    {
        [Colour.Clear] = new("Clear", "default", Colour.Clear, Color.clear),
        [Colour.Rainbow] = new("Rainbow", "rainbow", Colour.Rainbow, () => Color.HSVToRGB(Mathf.Repeat(Time.time * 0.125f, 1f), 1f, 1f))
    };

    public static readonly IReadOnlyDictionary<Colour, ColourInfo> All = ServerColours.Concat(GlobalColours).Concat(OtherColours).ToDictionary(x => x.Key, x => x.Value);
    public static readonly IReadOnlyDictionary<Colour, ColourInfo> AllValid = ServerColours.Concat(GlobalColours).Concat(OtherColours).Where(x => x.Key != Colour.Clear).ToDictionary(x => x.Key, x => x.Value);

    public static ColourInfo Get(Colour colour) => All[colour];
    public static ColourInfo GetValid(Colour colour) => AllValid[colour];

    public static ColourInfo Random() => All.ElementAt(UnityEngine.Random.Range(0, All.Count)).Value;
    public static ColourInfo RandomValid() => AllValid.ElementAt(UnityEngine.Random.Range(0, All.Count)).Value;

    private static readonly IReadOnlyDictionary<string, ColourInfo> _lookup = AllValid.Values
        .Select(c => new KeyValuePair<string, ColourInfo>(c.DisplayName, c))
        .Concat(
            AllValid.Values.Select(c => new KeyValuePair<string, ColourInfo>(c.InternalName, c))
        )
        .GroupBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
        .ToDictionary(x => x.Key, x => x.First().Value, StringComparer.OrdinalIgnoreCase);

    public static bool TryGet(string value, out ColourInfo colour) =>
        _lookup.TryGetValue(value, out colour);
}
