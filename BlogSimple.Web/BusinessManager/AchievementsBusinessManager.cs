using BlogSimple.Model.Models;
using BlogSimple.Model.Services.Interfaces;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services.Interfaces;

namespace BlogSimple.Web.BusinessManager;

public class AchievementsBusinessManager : IAchievementsBusinessManager
{
    private readonly IAchievementsService _achievementsService;
    private readonly IPostService _postService;
    private readonly ICommentService _commentService;
    private readonly IUserService _userService;


    public AchievementsBusinessManager(
        IAchievementsService achivementsService,
        IPostService postService,
        ICommentService commentService,
        IUserService userService
        )
    {
        _achievementsService = achivementsService;
        _postService = postService;
        _commentService = commentService;
        _userService = userService;
    }

    public async Task<Achievements> CreateAchievement()
    {
        return await _achievementsService.Create();
    }

    public async void PostCreated(User user)
    {
        Achievements achievements = await _achievementsService.Get(user.AchievementId);
        achievements.CreatedPostFirstTime = true;
        await _achievementsService.Update(achievements.Id, achievements);
    }

    public async void PostPublished(User user)
    {
        Achievements achievements = await _achievementsService.Get(user.AchievementId);
        achievements.PublishedPostFirstTime = true;

        var posts = await _postService.GetAllByUser(user);
        PostCountAchievemetnCheck(achievements, posts);
        TotalWordsAchievementCheck(achievements, posts);

        await _achievementsService.Update(achievements.Id, achievements);
    }
    public async void PostEdited(User user)
    {
        Achievements achievements = await _achievementsService.Get(user.AchievementId);
        achievements.EditedPostFirstTime = true;

        var posts = await _postService.GetAllByUser(user);
        TotalWordsAchievementCheck(achievements, posts);
        await _achievementsService.Update(achievements.Id, achievements);
    }

    public async void PostFavorited(User user)
    {
        Achievements achievements = await _achievementsService.Get(user.AchievementId);
        achievements.FavoritePostFirstTime = true;
        await _achievementsService.Update(achievements.Id, achievements);
    }

    public async void CommentPublished(User user)
    {
        Achievements achievements = await _achievementsService.Get(user.AchievementId);
        achievements.PublishedCommentFirstTime = true;

        var comments = await _commentService.GetAllByUser(user);
        CommentCountAchievementCheck(achievements, comments);
        await _achievementsService.Update(achievements.Id, achievements);
    }

    public async void ReplyPublished(User user)
    {
        Achievements achievements = await _achievementsService.Get(user.AchievementId);
        achievements.PublishedReplyFirstTime = true;
        await _achievementsService.Update(achievements.Id, achievements);
    }

    public async void CommentLiked(User user)
    {
        Achievements achievements = await _achievementsService.Get(user.AchievementId);
        achievements.LikedCommentFirstTime = true;

        var comments = await _commentService.GetAllByUser(user);
        CommentsLikedCountAchievementCheck(achievements, comments, user);
        await _achievementsService.Update(achievements.Id, achievements);
    }



    #region Private Achievement Helper Methods

    private void PostCountAchievemetnCheck(Achievements achievements, List<Post> posts)
    {
        // Posts count achievements
        if (posts.Count >= 10 && achievements.PublishedTenPosts == false)
        {
            achievements.PublishedTenPosts = true;
        }
        else if (posts.Count >= 5 && achievements.PublishedFivePosts == false)
        {
            achievements.PublishedFivePosts = true;
        }
    }

    private void TotalWordsAchievementCheck(Achievements achievements, List<Post> posts)
    {
        var totalWords = 0;

        foreach (var post in posts)
        {
            totalWords += post.WordCount;
        }

        // Posts total words achievements
        if (totalWords >= 50000 && achievements.PublishedOver50000TotalWords == false)
        {
            achievements.PublishedOver50000TotalWords = true;
        }
        else if (totalWords >= 10000 && achievements.PublishedOver10000TotalWords == false)
        {
            achievements.PublishedOver10000TotalWords = true;
        }
        else if (totalWords >= 5000 && achievements.PublishedOver5000TotalWords == false)
        {
            achievements.PublishedOver5000TotalWords = true;
        }
        else if (totalWords >= 1000 && achievements.PublishedOver1000TotalWords == false)
        {
            achievements.PublishedOver1000TotalWords = true;
        }
        else if (totalWords >= 500 && achievements.PublishedOver500TotalWords == false)
        {
            achievements.PublishedOver500TotalWords = true;
        }
    }

    private void CommentCountAchievementCheck(Achievements achievements, List<Comment> comments)
    {
        if (comments.Count >= 20 && achievements.Published20Comments == false)
        {
            achievements.Published20Comments = true;
        }
        if (comments.Count >= 10 && achievements.PublishedTenComments == false)
        {
            achievements.PublishedTenComments = true;
        }
        if (comments.Count >= 5 && achievements.PublishedFiveComments == false)
        {
            achievements.PublishedFiveComments = true;
        }
    }

    private async void CommentsLikedCountAchievementCheck(Achievements achievements, List<Comment> comments, User user)
    {
        int likedCommentsCount = 0;

        foreach (Comment comment in comments)
        {
            foreach (var u in comment.CommentLikedByUserNames)
            {
                var userLiked = await _userService.Get(u);
                if (userLiked.Id == user.Id)
                {
                    likedCommentsCount += 1;
                }
            }
        }

        if (likedCommentsCount >= 10 && achievements.LikedTenComments == false)
        {
            achievements.LikedTenComments = true;

        }
        else if (likedCommentsCount >= 5 && achievements.LikedFiveComments == false)
        {
            achievements.LikedFiveComments = true;
        }

    }
    #endregion

}