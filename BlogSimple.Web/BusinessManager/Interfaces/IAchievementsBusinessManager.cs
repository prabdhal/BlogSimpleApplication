using BlogSimple.Model.Models;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IAchievementsBusinessManager
{
    void FirstPostCreated(User user);
    void FirstPostPublished(User user);
    void FirstPostEdited(User user);
    Task<Achievements> CreateAchievement();
}
