#if !MONGODB
using KittsGlowSystem.Features.Models;
using LabApi.Features.Wrappers;
using LabApi.Loader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KittsGlowSystem.Features.Database;

public static class DatabaseJson
{
    private static List<GlowData> _jsonCache = [];
    private static string JsonFilePath => Path.Combine(
        KittsGlowSystem.Instance.GetConfigDirectory().FullName, "GlowData.json");

    public static void Init() =>
        LoadJsonCache();

    public static void Stop()
    {
        SaveJsonCache();
        _jsonCache.Clear();
    }

    private static void EnsureJsonFile()
    {
        string dir = KittsGlowSystem.Instance.GetConfigDirectory().FullName;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        if (!File.Exists(JsonFilePath))
            File.WriteAllText(JsonFilePath, "[]");
    }

    private static void LoadJsonCache()
    {
        try
        {
            EnsureJsonFile();

            List<GlowData> jsonCache = JsonConvert.DeserializeObject<List<GlowData>>(File.ReadAllText(JsonFilePath));
            if (jsonCache != null)
                _jsonCache = jsonCache;
        }
        catch (Exception e)
        {
            Log.Error("DatabaseJson.LoadJsonCache", $"Error loading JSON cache: {e.Message}");
            Log.Debug("DatabaseJson.LoadJsonCache", e.ToString());
        }
    }

    public static void SaveJsonCache()
    {
        try
        {
            File.WriteAllText(JsonFilePath, JsonConvert.SerializeObject(_jsonCache, Formatting.Indented));
        }
        catch (Exception e)
        {
            Log.Error("DatabaseJson.SaveJsonCache", $"Error saving JSON cache: {e.Message}");
            Log.Debug("DatabaseJson.SaveJsonCache", e.ToString());
        }
    }

    public static GlowData GetGlowData(string userId)
    {
        GlowData data = _jsonCache
            .Where(d => d.UserId == userId)
            .FirstOrDefault();

        if (data == null)
        {
            data = new()
            {
                UserId = userId
            };

            _jsonCache.Add(data);
        }

        return data;
    }

    public static GlowData GetGlowData(this ReferenceHub hub) =>
        GetGlowData(Player.Get(hub).UserId);

    public static GlowData GetGlowData(this Player player) =>
        GetGlowData(player.UserId);
}
#endif
