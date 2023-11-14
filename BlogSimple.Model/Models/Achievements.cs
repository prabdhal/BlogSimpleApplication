using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace BlogSimple.Model.Models;

public class Achievements
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [DisplayName("First Post Created")]
    public bool CreatedPostFirstTime { get; set; } = false;

    [DisplayName("First Post Published")]
    public bool PublishedPostFirstTime { get; set; } = false;

    [DisplayName("First Post Editted")]
    public bool EditPostFirstTime { get; set; } = false;

    [DisplayName("First Post Favorited")]
    public bool FavoritePostFirstTime { get; set; } = false;

    [DisplayName("First Commented Published")]
    public bool PublishedCommentFirstTime { get; set; } = false;

    [DisplayName("First Reply Published")]
    public bool PublishedReplyFirstTime { get; set; } = false;

    [DisplayName("Published Over 500 Words")]
    public bool PublishedOver500TotalWords { get; set; } = false;

    [DisplayName("Published Over 1000 Words")]
    public bool PublishedOver1000Words { get; set; } = false;

    [DisplayName("Published Over 5000 Words")]
    public bool PublishedOver5000Words { get; set; } = false;

    [DisplayName("Published Over 10000 Words")]
    public bool PublishedOver10000Words { get; set; } = false;

    [DisplayName("Published Over 50000 Words")]
    public bool PublishedOver50000Words { get; set; } = false;

    [DisplayName("Published Over 5 Posts")]
    public bool PublishedFivePosts { get; set; } = false;

    [DisplayName("Published Over 5 Comments")]
    public bool PublishedFiveComments { get; set; } = false;

    [DisplayName("Published Over 10 Posts")]
    public bool PublishedTenPosts { get; set; } = false;

    [DisplayName("First Comment Liked")]
    public bool LikeAComment { get; set; } = false;

    [DisplayName("Five Comments Liked")]
    public bool LikedFiveComments { get; set; } = false;

    [DisplayName("Ten Comments Liked")]
    public bool LikedTenComments { get; set; } = false;
}


//CreatedPostFirstTime 
//PublishedPostFirstTime 
//EditPostFirstTime 
//FavoritePostFirstTime 
//PublishedCommentFirstTime 
//PublishedReplyFirstTime 
//PublishedOver500TotalWords 
//PublishedOver1000Words 
//PublishedOver5000Words 
//PublishedOver10000Words
//PublishedOver50000Words
//PublishedFivePosts 
//PublishedFiveComments 
//PublishedTenPosts 
//LikeAComment 
//LikedFiveComments
//LikedTenComments 