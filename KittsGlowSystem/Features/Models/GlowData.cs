#if MONGODB
using MongoDB.Bson.Serialization.Attributes;
#endif
using KittsGlowSystem.Features.Enums;

namespace KittsGlowSystem.Features.Models;

public sealed class GlowData
{
#if MONGODB
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string ObjectId = null;
#endif

    public string UserId = "";

    public bool GlowEnabled = false;

    public Colour GlowColour = Colour.Clear;
    public bool ShadowEnabled = true;
    public float Range = 8f;
    public float Intensity = 2f;
}
