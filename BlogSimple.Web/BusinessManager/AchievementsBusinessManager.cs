using BlogSimple.Model.Models;
using BlogSimple.Web.BusinessManager.Interfaces;
using BlogSimple.Web.Services.Interfaces;

namespace BlogSimple.Web.BusinessManager;

public class AchievementsBusinessManager : IAchievementsBusinessManager
{
    private readonly IAchievementsService _achievementsService;
    //private readonly IPostBusinessManager _postBusinessManager;


    public AchievementsBusinessManager(
        IAchievementsService achivementsService,
        IPostBusinessManager postBusinessManager
        )
    {
        _achievementsService = achivementsService;
        //_postBusinessManager = postBusinessManager;

        //postBusinessManager.OnFirstPostCreatedEvent += FirstPostCreated;
    }

    public async void FirstPostCreated(User user)
    {
        // Fetch Users Achievement Model
        Achievements achievements = await _achievementsService.Get(user.AchievementId);
        achievements.CreatedPostFirstTime = true;
        await _achievementsService.Update(achievements.Id, achievements);
    }
}
