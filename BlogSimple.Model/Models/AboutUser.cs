using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BlogSimple.Model.Models;

public class AboutUser
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public ApplicationUser User { get; set; }
    public string Description { get; set; }
    public string Content { get; set; }
    public string PortfolioLink { get; set; }
    public string TwitterLink { get; set; }
    public string GitHubLink { get; set; }
    public string LinkedInLink { get; set; }
}
