using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.Models;

public class Post
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [Required(ErrorMessage = "Please enter the blog title.")]
    [StringLength(50, ErrorMessage = "Can only have a maximum of 50 characters", MinimumLength = 1)]
    public string Title { get; set; } = String.Empty;
    [Required(ErrorMessage = "Please enter the blog description.")]
    [StringLength(200, ErrorMessage = "Can only have a maximum of 200 characters", MinimumLength = 1)]
    public string Description { get; set; } = String.Empty;
    [Required(ErrorMessage = "Please select the appropriate category for the blog.")]
    public PostCategory Category { get; set; }
    [Required(ErrorMessage = "Please enter the blog content.")]
    public string Content { get; set; } = String.Empty;
    public bool IsPublished { get; set; } = false;
    public bool IsFeatured { get; set; } = false;

    [DisplayName("Author")]
    public User CreatedBy { get; set; } = new User();
    
    [DisplayName("Created On")]
    public DateTime CreatedOn { get; set; }
    
    [DisplayName("Last Updated")]
    public DateTime UpdatedOn { get; set; }
}
