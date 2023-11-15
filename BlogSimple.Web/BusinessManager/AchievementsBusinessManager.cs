using BlogSimple.Model.Models;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services.Interfaces;

namespace BlogSimple.Web.BusinessManager;

public class AchievementsBusinessManager : IAchievementsBusinessManager
{
    private readonly IAchievementsService _achievementsService;
    //private readonly IPostBusinessManager _postBusinessManager;


    public AchievementsBusinessManager(
        IAchievementsService achivementsService
        )
    {
        _achievementsService = achivementsService;
        //_postBusinessManager = postBusinessManager;

        //postBusinessManager.OnFirstPostCreatedEvent += FirstPostCreated;
    }

    public async Task<Achievements> CreateAchievement()
    {
        return await _achievementsService.Create();
    }

    public async void FirstPostCreated(User user)
    {
        Achievements achievements = await _achievementsService.Get(user.AchievementId);
        achievements.CreatedPostFirstTime = true;
        await _achievementsService.Update(achievements.Id, achievements);
    }

    public async void FirstPostPublished(User user)
    {
        Achievements achievements = await _achievementsService.Get(user.AchievementId);
        achievements.PublishedPostFirstTime = true;
        await _achievementsService.Update(achievements.Id, achievements);
    }

    public async void FirstPostEdited(User user)
    {
        Achievements achievements = await _achievementsService.Get(user.AchievementId);
        achievements.EditPostFirstTime = true;
        await _achievementsService.Update(achievements.Id, achievements);
    }
}
