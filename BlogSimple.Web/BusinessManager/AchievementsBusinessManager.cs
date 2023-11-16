using BlogSimple.Model.Models;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services.Interfaces;

namespace BlogSimple.Web.BusinessManager;

public class AchievementsBusinessManager : IAchievementsBusinessManager
{
    private readonly IAchievementsService _achievementsService;


    public AchievementsBusinessManager(
        IAchievementsService achivementsService
        )
    {
        _achievementsService = achivementsService;
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

        //achievements.PublishedTenPosts = true;
        //achievements.PublishedFivePosts = true;
        //achievements.PublishedOver500TotalWords = true;
        //achievements.PublishedOver1000TotalWords = true;
        //achievements.PublishedOver5000TotalWords = true;
        //achievements.PublishedOver10000TotalWords = true;
        //achievements.PublishedOver50000TotalWords = true;

        await _achievementsService.Update(achievements.Id, achievements);
    }

    public async void PostEdited(User user)
    {
        Achievements achievements = await _achievementsService.Get(user.AchievementId);
        achievements.EditedPostFirstTime = true;

        //achievements.PublishedOver500TotalWords = true;
        //achievements.PublishedOver1000TotalWords = true;
        //achievements.PublishedOver5000TotalWords = true;
        //achievements.PublishedOver10000TotalWords = true;
        //achievements.PublishedOver50000TotalWords = true;

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

        //achievements.PublishedFiveComments = true;

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

        //achievements.LikedFiveComments = true; 
        //achievements.LikedTenComments = true;

        await _achievementsService.Update(achievements.Id, achievements);
    }
}