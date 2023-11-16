using BlogSimple.Model.Models;

namespace BlogSimple.Web.BusinessManager.Interfaces;

public interface IAchievementsBusinessManager
{
    void PostCreated(User user);
    void PostPublished(User user);
    void PostEdited(User user);
    void PostFavorited(User user);
    void CommentPublished(User user);
    void ReplyPublished(User user);
    void CommentLiked(User user);
    Task<Achievements> CreateAchievement();
}
