using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogSimple.Model.Models;

public class Achievements
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public bool CreatedPostFirstTime { get; set; }
    public bool PublishedPostFirstTime { get; set; }
    public bool EditPostFirstTime { get; set; }
    public bool FavoritePostFirstTime { get; set; }
    public bool PublishedCommentFirstTime { get; set; }
    public bool PublishedReplyFirstTime { get; set; }
    public bool PublishedOver500TotalWords { get; set; }
    public bool PublishedOver1000Words { get; set; }
    public bool PublishedOver5000Words { get; set; }
    public bool PublishedOver10000Words { get; set; }
    public bool PublishedOver50000Words { get; set; }
    public bool PublishedFivePosts { get; set; }
    public bool PublishedFiveComments { get; set; }
    public bool PublishedTenPosts { get; set; }
    public bool LikeAComment { get; set; }
    public bool LikedFiveComments { get; set; }
    public bool LikedTenComments { get; set; }
}
