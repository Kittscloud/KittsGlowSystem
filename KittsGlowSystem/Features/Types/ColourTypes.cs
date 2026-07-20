using KittsGlowSystem.Features.Enums;
using KittsGlowSystem.Features.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KittsGlowSystem.Features.Types;

public static class ColourTypes
{
    public static readonly List<string> RainbowColours =
    [
        "pink", "magenta", "crimson", "tomato", "orange", "yellow",
        "lime", "emerald", "light_green", "mint",
        "blue_green", "aqua", "cyan",
        "aqua", "blue_green", "mint", "green", "light_green",
        "emerald", "lime", "yellow", "orange", "tomato",
        "crimson", "magenta"
    ];

    public static readonly IReadOnlyDictionary<Colour, ColourInfo> All = new Dictionary<Colour, ColourInfo>
    {
        [Colour.Red] = new("Red", "red", Colour.Red, new Color(0.77f, 0f, 0f)),
        [Colour.Brown] = new("Brown", "brown", Colour.Brown, new Color(0.58f, 0.28f, 0.06f)),
        [Colour.Crimson] = new("Crimson", "crimson", Colour.Crimson, new Color(0.86f, 0.08f, 0.24f)),
        [Colour.Blue] = new("Blue", "cyan", Colour.Blue, new Color(0f, 0.72f, 0.92f)),
        [Colour.Cyan] = new("Cyan", "aqua", Colour.Cyan, Color.cyan),
        [Colour.Magenta] = new("Magenta", "magenta", Colour.Magenta, new Color(1f, 0f, 0.56f)),
        [Colour.Yellow] = new("Yellow", "yellow", Colour.Yellow, new Color(0.98f, 1f, 0.52f)),
        [Colour.Orange] = new("Orange", "pumpkin", Colour.Orange, new Color(0.93f, 0.46f, 0f)),
        [Colour.Pink] = new("Pink", "pink", Colour.Pink, new Color(1f, 0.59f, 0.87f)),
        [Colour.Lime] = new("Lime", "lime", Colour.Lime, new Color(0.75f, 1f, 0f)),
        [Colour.LightGreen] = new("Light Green", "mint", Colour.LightGreen, new Color(0.596f, 0.984f, 0.596f)),
        [Colour.Green] = new("Green", "light_green", Colour.Green, new Color(0.2f, 0.8f, 0.2f)),
        [Colour.DarkGreen] = new("Dark Green", "green", Colour.DarkGreen, new Color(0.13f, 0.55f, 0.13f)),
        [Colour.Carmine] = new("Carmine", "carmine", Colour.Carmine, new Color(0.59f, 0f, 0.09f)),
        [Colour.Nickel] = new("Nickel", "nickel", Colour.Nickel, new Color(0.45f, 0.45f, 0.45f)),
        [Colour.BlueGreen] = new("Blue Green", "blue_green", Colour.BlueGreen, new Color(0.302f, 1f, 0.721f)),
        [Colour.Purple] = new("Purple", "purple", Colour.Purple, new Color(0.6f, 0f, 1f)),
        [Colour.Clear] = new("Clear", "default", Colour.Clear, Color.clear),
        [Colour.Rainbow] = new("Rainbow", "rainbow", Colour.Rainbow, () => Color.HSVToRGB(Mathf.Repeat(Time.time * 0.125f, 1f), 1f, 1f))
    };
    public static readonly IReadOnlyDictionary<Colour, ColourInfo> AllValid = All.Where(c => c.Key != Colour.Clear && c.Key != Colour.Rainbow).ToDictionary(k => k.Key, v => v.Value);

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
