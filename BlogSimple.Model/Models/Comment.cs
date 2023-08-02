namespace BlogSimple.Model.Models;

public class Comment
{
    public int Id { get; set; }
    public ApplicationUser User { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string Content { get; set; }
    public IEnumerable<CommentReply> Replies { get; set; }
    public Blog CommentedBlog { get; set; }
}
