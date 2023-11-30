using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.HomeViewModels;

namespace BlogSimple.Model.ViewModels.PostViewModels;

public class PostDetailsViewModel
{
    public List<string> PostCategories { get; set; }
    public PostAndCreator PostAndCreator { get; set; }
    public IEnumerable<PostAndCreator> AllPosts { get; set; } = Enumerable.Empty<PostAndCreator>();
    public List<CommentAndCreator> Comments { get; set; } = new List<CommentAndCreator>();
    public Comment Comment { get; set; }
    public CommentReply CommentReply { get; set; }
    public List<CommentReplyAndCreator> CommentReplies { get; set; } = new List<CommentReplyAndCreator>();
    public User AccountUser { get; set; }
    public int CommentCount { get; set; }
    public List<AchievementsNotificationModel> AchievementsNotificationList { get; set; } = new List<AchievementsNotificationModel>();
}
