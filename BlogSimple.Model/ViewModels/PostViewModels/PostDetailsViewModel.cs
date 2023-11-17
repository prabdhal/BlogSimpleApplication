using BlogSimple.Model.Models;

namespace BlogSimple.Model.ViewModels.PostViewModels;

public class PostDetailsViewModel
{
    public List<string> PostCategories { get; set; }
    public Post Post { get; set; }
    public IEnumerable<Post> AllPosts { get; set; } = Enumerable.Empty<Post>();
    public IEnumerable<Comment> Comments { get; set; }
    public Comment Comment { get; set; }
    public CommentReply CommentReply { get; set; }
    public List<CommentReply> CommentReplies { get; set; } = new List<CommentReply>();
    public User AccountUser { get; set; }
    public int CommentCount { get; set; }
    public List<AchievementsNotificationModel> AchievementsNotificationList { get; set; } = new List<AchievementsNotificationModel>();
}
