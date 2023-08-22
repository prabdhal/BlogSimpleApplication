using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.Models;

public class Comment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public User CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    [Required(ErrorMessage = "Comment is empty!")]
    [StringLength(300, ErrorMessage = "Can only have a maximum of 300 characters", MinimumLength = 1)]
    public string Content { get; set; }
    public List<User> CommentLikedByUsers { get; set; } = new List<User>();
    public Blog CommentedBlog { get; set; }
}
