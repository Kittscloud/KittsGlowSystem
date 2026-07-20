using KittsGlowSystem.Features.Enums;
using System;
using UnityEngine;

namespace KittsGlowSystem.Features.Models;

public readonly struct ColourInfo
{
    public string DisplayName { get; }
    public string InternalName { get; }

    public Colour EnumColour { get; }

    private readonly Func<Color> _colourGetter;
    private readonly Color? _cachedColour;

    public Color Colour => _cachedColour ?? _colourGetter();

    public ColourInfo(string name, string internalName, Colour enumColour, Color staticColour)
    {
        DisplayName = name;
        InternalName = internalName;
        EnumColour = enumColour;
        _colourGetter = () => staticColour;
        _cachedColour = staticColour;
    }

    public ColourInfo(string name, string internalName, Colour enumColour, Func<Color> colourFunc)
    {
        DisplayName = name;
        InternalName = internalName;
        EnumColour = enumColour;
        _colourGetter = colourFunc;
        _cachedColour = null;
    }
}
