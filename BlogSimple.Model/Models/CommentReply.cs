namespace BlogSimple.Model.Models;

public class CommentReply
{
    public int Id { get; set; }
    public ApplicationUser User { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string Content { get; set; }
    public Comment RepliedComment { get; set; }
    public Blog RepliedBlog { get; set; }
}
