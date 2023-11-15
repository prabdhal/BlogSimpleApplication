using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogSimple.Model.Models;

public class Achievements
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string CreatedPostFirstTimeName { get; set; } = "Postman";
    public string CreatedPostFirstTimeDescription { get; set; } = "Created your first blog post ever!";
    public string CreatedPostFirstTimeImagePath { get; set; } 
    public bool CreatedPostFirstTime { get; set; } = false;
    public bool CreatedPostFirstTimeActive { get; set; } = true;

    public string PublishedPostFirstTimeName { get; set; } 
    public string PublishedPostFirstTimeDescription { get; set; }
    public string PublishedPostFirstTimeImagePath { get; set; }
    public bool PublishedPostFirstTime { get; set; } = false;
    public bool PublishedPostFirstTimeActive { get; set; } = true;

    public string EditPostFirstTimeName { get; set; }
    public string EditPostFirstTimeDescription { get; set; }
    public string EditPostFirstTimeImagePath { get; set; }
    public bool EditPostFirstTime { get; set; } = false;
    public bool EditPostFirstTimeActive { get; set; } = true;

    public string FavoritePostFirstTimeName { get; set; }
    public string FavoritePostFirstTimeDescription { get; set; }
    public string FavoritePostFirstTimeImagePath { get; set; }
    public bool FavoritePostFirstTime { get; set; } = false;
    public bool FavoritePostFirstTimeActive { get; set; } = true;

    public string PublishedCommentFirstTimeName { get; set; }
    public string PublishedCommentFirstTimeDescription { get; set; }
    public string PublishedCommentFirstTimeImagePath { get; set; }
    public bool PublishedCommentFirstTime { get; set; } = false;
    public bool PublishedCommentFirstTimeActive { get; set; } = true;

    public string PublishedReplyFirstTimeName { get; set; }
    public string PublishedReplyFirstTimeDescription { get; set; }
    public string PublishedReplyFirstTimeImagePath { get; set; }
    public bool PublishedReplyFirstTime { get; set; } = false;
    public bool PublishedReplyFirstTimeActive { get; set; } = true;

    public string PublishedOver500TotalWordsName { get; set; }
    public string PublishedOver500TotalWordsDescription { get; set; }
    public string PublishedOver500TotalWordsImagePath { get; set; }
    public bool PublishedOver500TotalWords { get; set; } = false;
    public bool PublishedOver500TotalWordsActive { get; set; } = true;

    public string PublishedOver1000WordsName { get; set; }
    public string PublishedOver1000WordsDescription { get; set; }
    public string PublishedOver1000WordsImagePath { get; set; }
    public bool PublishedOver1000Words { get; set; } = false;
    public bool PublishedOver1000WordsActive { get; set; } = true;

    public string PublishedOver5000WordsName { get; set; }
    public string PublishedOver5000WordsDescription { get; set; }
    public string PublishedOver5000WordsImagePath { get; set; }
    public bool PublishedOver5000Words { get; set; } = false;
    public bool PublishedOver5000WordsActive { get; set; } = true;

    public string PublishedOver10000WordsName { get; set; }
    public string PublishedOver10000WordsDescription { get; set; }
    public string PublishedOver10000WordsImagePath { get; set; }
    public bool PublishedOver10000Words { get; set; } = false;
    public bool PublishedOver10000WordsActive { get; set; } = true;

    public string PublishedOver50000WordsName { get; set; }
    public string PublishedOver50000WordsDescription { get; set; }
    public string PublishedOver50000WordsImagePath { get; set; }
    public bool PublishedOver50000Words { get; set; } = false;
    public bool PublishedOver50000WordsActive { get; set; } = true;

    public string PublishedFivePostsName { get; set; }
    public string PublishedFivePostsDescription { get; set; }
    public string PublishedFivePostsImagePath { get; set; }
    public bool PublishedFivePosts { get; set; } = false;
    public bool PublishedFivePostsActive { get; set; } = true;

    public string PublishedFiveCommentsName { get; set; }
    public string PublishedFiveCommentsDescription { get; set; }
    public string PublishedFiveCommentsImagePath { get; set; }
    public bool PublishedFiveComments { get; set; } = false;
    public bool PublishedFiveCommentsActive { get; set; } = true;

    public string PublishedTenPostsName { get; set; }
    public string PublishedTenPostsDescription { get; set; }
    public string PublishedTenPostsImagePath { get; set; }
    public bool PublishedTenPosts { get; set; } = false;
    public bool PublishedTenPostsActive { get; set; } = true;

    public string LikeACommentName { get; set; }
    public string LikeACommentDescription { get; set; }
    public string LikeACommentImagePath { get; set; }
    public bool LikeAComment { get; set; } = false;
    public bool LikeACommentActive { get; set; } = true;

    public string LikedFiveCommentsName { get; set; }
    public string LikedFiveCommentsDescription { get; set; }
    public string LikedFiveCommentsImagePath { get; set; }
    public bool LikedFiveComments { get; set; } = false;
    public bool ActiveActive { get; set; } = true;

    public string LikedTenCommentsName { get; set; }
    public string LikedTenCommentsDescription { get; set; }
    public string LikedTenCommentsImagePath { get; set; }
    public bool LikedTenComments { get; set; } = false;
    public bool LikedTenCommentsActive { get; set; } = true;
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