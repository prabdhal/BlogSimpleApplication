using BlogSimple.Model.Models;
using BlogSimple.Model.ViewModels.HomeViewModels;

namespace BlogSimple.Model.ViewModels.PostViewModels;

public class PostDetailsViewModel
{
    public List<string> PostCategories { get; set; }
    public PostAndCreator PostAndCreator { get; set; }
    public IEnumerable<PostAndCreator> AllPosts { get; set; } = Enumerable.Empty<PostAndCreator>();
    public IEnumerable<Comment> Comments { get; set; }
    public Comment Comment { get; set; }
    public CommentReply CommentReply { get; set; }
    public List<CommentReply> CommentReplies { get; set; } = new List<CommentReply>();
    public User AccountUser { get; set; }
    public int CommentCount { get; set; }
    public List<AchievementsNotificationModel> AchievementsNotificationList { get; set; } = new List<AchievementsNotificationModel>();
}
