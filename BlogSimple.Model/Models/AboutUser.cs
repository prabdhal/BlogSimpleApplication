using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BlogSimple.Model.Models;

public class AboutUser
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public ApplicationUser User { get; set; }
}
