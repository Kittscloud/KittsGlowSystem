#if MONGODB
using KittsGlowSystem.Features.Models;
using LabApi.Features.Wrappers;
using MongoDB.Driver;
using System.Collections.Generic;

namespace KittsGlowSystem.Features.Database;

public static class DatabaseMongo
{
    private static MongoClient _client;
    private static IMongoDatabase _db;
    private static IMongoCollection<GlowData> _playerGlowDataCollection;

    public static void Init()
    {
        _client = new MongoClient(KittsGlowSystem.Config.MongoDBURI);
        _db = _client.GetDatabase(KittsGlowSystem.Config.MongoDBName);
        _playerGlowDataCollection = _db.GetCollection<GlowData>(KittsGlowSystem.Config.MongoDBCollectionName);

        CreateIndexes();
    }

    private static void CreateIndexes()
    {
        _playerGlowDataCollection.Indexes.CreateOne(
            new CreateIndexModel<GlowData>(
                Builders<GlowData>.IndexKeys.Ascending(p => p.UserId),
                new CreateIndexOptions { Unique = true }
            )
        );
    }

    public static void Stop()
    {
        _playerGlowDataCollection = null;
        _client = null;
        _db = null;
    }

    private static readonly Dictionary<string, GlowData> _cache = [];

    public static IReadOnlyDictionary<string, GlowData> Cache => _cache;

    public static GlowData LoadGlowData(string userId)
    {
        GlowData data = _playerGlowDataCollection
            .Find(p => p.UserId == userId)
            .FirstOrDefault();

        if (data == null)
        {
            data = new() { UserId = userId };

            _playerGlowDataCollection.InsertOne(data);
        }

        _cache[userId] = data;

        Log.Debug("Database.LoadGlowData", data.UserId);

        return data;
    }
    public static GlowData LoadGlowData(this Player player) =>
        LoadGlowData(player.UserId);
    public static GlowData LoadGlowData(this ReferenceHub hub) =>
        Player.Get(hub).LoadGlowData();

    public static void UploadGlowData(this Player player)
    {
        GlowData GlowData = player.GetGlowData();

        if (GlowData == null)
            return;

        _playerGlowDataCollection.ReplaceOne(
            p => p.UserId == GlowData.UserId,
            GlowData,
            new ReplaceOptions { IsUpsert = true }
        );

        Log.Debug("Database.UploadGlowData", $"{player.UserId} | {player.DisplayName}");
    }
    public static GlowData GetGlowData(this Player player)
    {
        if (_cache.TryGetValue(player.UserId, out GlowData cached) && cached != null)
            return cached;

        return player.LoadGlowData();
    }
    public static GlowData GetGlowData(this ReferenceHub hub) =>
        Player.Get(hub).GetGlowData();
}
#endif
