using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.Models;

public class Comment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public ApplicationUser CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    [Required(ErrorMessage = "Comment is empty!")]
    public string Content { get; set; }
    public IEnumerable<CommentReply> Replies { get; set; } = Enumerable.Empty<CommentReply>();
    public Blog CommentedBlog { get; set; }
}
