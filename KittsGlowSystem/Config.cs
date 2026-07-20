using System.ComponentModel;

namespace KittsGlowSystem;

public class Config
{
    /// <summary>
    /// Sends debug logs to console.
    /// </summary>
    [Description("Sends debug logs to console")]
    public bool Debug { get; set; } = false;

#if MONGODB
    /// <summary>
    /// MongoDB URI.
    /// </summary>
    [Description("MongoDB URI")]
    public string MongoDBURI { get; set; } = "mongodb://username:password@ip:port/";

    /// <summary>
    /// MongoDB name.
    /// </summary>
    [Description("MongoDB name")]
    public string MongoDBName { get; set; } = "KittsGlowSystem";

    /// <summary>
    /// MongoDB collection name.
    /// </summary>
    [Description("MongoDB collection name")]
    public string MongoDBCollectionName { get; set; } = "GlowData";
#endif

    /// <summary>
    /// Permission for player glow command.
    /// </summary>
    [Description("Permission for player glow command")]
    public string PlayerGlowPermission { get; set; } = "kts.glow";

    /// <summary>
    /// Permission for admin glow command.
    /// </summary>
    [Description("Permission for admin glow command")]
    public string AdminGlowPermission { get; set; } = "kts.glowadmin";
}
