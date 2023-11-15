using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogSimple.Model.Models;

public class Achievements
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string CreatedPostFirstTimeName { get; set; } = "Postman";
    public string CreatedPostFirstTimeDescription { get; set; } = "You created your first blog post!";
    public string CreatedPostFirstTimeImagePath { get; set; } 
    public bool CreatedPostFirstTime { get; set; } = false;
    public bool CreatedPostFirstTimeActive { get; set; } = true;

    public string PublishedPostFirstTimeName { get; set; } = "Publisher";
    public string PublishedPostFirstTimeDescription { get; set; } = "You published your first blog post!";
    public string PublishedPostFirstTimeImagePath { get; set; }
    public bool PublishedPostFirstTime { get; set; } = false;
    public bool PublishedPostFirstTimeActive { get; set; } = true;

    public string EditPostFirstTimeName { get; set; } = "Editor";
    public string EditPostFirstTimeDescription { get; set; } = "You edited your blog post for the first time!";
    public string EditPostFirstTimeImagePath { get; set; }
    public bool EditPostFirstTime { get; set; } = false;
    public bool EditPostFirstTimeActive { get; set; } = true;

    public string FavoritePostFirstTimeName { get; set; } = "Favorite";
    public string FavoritePostFirstTimeDescription { get; set; } = "You saved a blog post for the first time!";
    public string FavoritePostFirstTimeImagePath { get; set; }
    public bool FavoritePostFirstTime { get; set; } = false;
    public bool FavoritePostFirstTimeActive { get; set; } = true;

    public string PublishedCommentFirstTimeName { get; set; } = "Commenter";
    public string PublishedCommentFirstTimeDescription { get; set; } = "You published your first comment!";
    public string PublishedCommentFirstTimeImagePath { get; set; }
    public bool PublishedCommentFirstTime { get; set; } = false;
    public bool PublishedCommentFirstTimeActive { get; set; } = true;

    public string PublishedReplyFirstTimeName { get; set; } = "Replier";
    public string PublishedReplyFirstTimeDescription { get; set; } = "You published your first reply!";
    public string PublishedReplyFirstTimeImagePath { get; set; }
    public bool PublishedReplyFirstTime { get; set; } = false;
    public bool PublishedReplyFirstTimeActive { get; set; } = true;

    public string PublishedOver500TotalWordsName { get; set; } = "Words Smith";
    public string PublishedOver500TotalWordsDescription { get; set; } = "You have published over 500 total words!";
    public string PublishedOver500TotalWordsImagePath { get; set; }
    public bool PublishedOver500TotalWords { get; set; } = false;
    public bool PublishedOver500TotalWordsActive { get; set; } = true;

    public string PublishedOver1000WordsName { get; set; } = "Intermediate Words Smith";
    public string PublishedOver1000WordsDescription { get; set; } = "You have published over 1,000 total words!";
    public string PublishedOver1000WordsImagePath { get; set; }
    public bool PublishedOver1000Words { get; set; } = false;
    public bool PublishedOver1000WordsActive { get; set; } = true;

    public string PublishedOver5000WordsName { get; set; } = "Senior Words Smith";
    public string PublishedOver5000WordsDescription { get; set; } = "You have published over 5,000 total words!";
    public string PublishedOver5000WordsImagePath { get; set; }
    public bool PublishedOver5000Words { get; set; } = false;
    public bool PublishedOver5000WordsActive { get; set; } = true;

    public string PublishedOver10000WordsName { get; set; } = "Master Words Smith";
    public string PublishedOver10000WordsDescription { get; set; } = "You have published over 10,000 words!";
    public string PublishedOver10000WordsImagePath { get; set; }
    public bool PublishedOver10000Words { get; set; } = false;
    public bool PublishedOver10000WordsActive { get; set; } = true;

    public string PublishedOver50000WordsName { get; set; } = "God-Like Words Smith";
    public string PublishedOver50000WordsDescription { get; set; } = "You have published over 50,000 words!";
    public string PublishedOver50000WordsImagePath { get; set; }
    public bool PublishedOver50000Words { get; set; } = false;
    public bool PublishedOver50000WordsActive { get; set; } = true;

    public string PublishedFivePostsName { get; set; } = "Intermediate Poster";
    public string PublishedFivePostsDescription { get; set; } = "You have published five blog posts!";
    public string PublishedFivePostsImagePath { get; set; }
    public bool PublishedFivePosts { get; set; } = false;
    public bool PublishedFivePostsActive { get; set; } = true;

    public string PublishedFiveCommentsName { get; set; } = "Intermediate Commenter";
    public string PublishedFiveCommentsDescription { get; set; } = "You have published five comments!";
    public string PublishedFiveCommentsImagePath { get; set; }
    public bool PublishedFiveComments { get; set; } = false;
    public bool PublishedFiveCommentsActive { get; set; } = true;

    public string PublishedTenPostsName { get; set; } = "Senior Poster";
    public string PublishedTenPostsDescription { get; set; } = "You have published five blog posts!";
    public string PublishedTenPostsImagePath { get; set; }
    public bool PublishedTenPosts { get; set; } = false;
    public bool PublishedTenPostsActive { get; set; } = true;

    public string LikeACommentName { get; set; } = "Supporter";
    public string LikeACommentDescription { get; set; } = "You have liked a comment for the first time!";
    public string LikeACommentImagePath { get; set; }
    public bool LikeAComment { get; set; } = false;
    public bool LikeACommentActive { get; set; } = true;

    public string LikedFiveCommentsName { get; set; } = "Intermediate Supporter";
    public string LikedFiveCommentsDescription { get; set; } = "You have published five blog posts!";
    public string LikedFiveCommentsImagePath { get; set; }
    public bool LikedFiveComments { get; set; } = false;
    public bool ActiveActive { get; set; } = true;

    public string LikedTenCommentsName { get; set; } = "Senior Supporter";
    public string LikedTenCommentsDescription { get; set; } = "You have published ten comments!";
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