using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.Models;

public class CommentReply
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public Guid CreatedById { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    [Required(ErrorMessage = "Reply is empty!")]
    [StringLength(300, ErrorMessage = "Can only have a maximum of 300 characters", MinimumLength = 1)]
    public string Content { get; set; }
    public string RepliedCommentId { get; set; }
    public string RepliedPostId { get; set; }
}


public class CommentReplyAndCreator
{
    public CommentReply CommentReply { get; set; }
    public User Creator { get; set; }
}
