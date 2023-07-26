using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.Models;

public class Blog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [Required(ErrorMessage = "Please enter the blog title.")]
    public string Title { get; set; } = String.Empty;
    [Required(ErrorMessage = "Please enter the blog description.")]
    public int CategoryId { get; set; }
    //[Required(ErrorMessage = "Please select the appropriate category of the blog.")]
    public BlogCategory Category { get; set; }
    public string Description { get; set; } = String.Empty;
    [Required(ErrorMessage = "Please enter the blog content.")]
    public string Content { get; set; } = String.Empty;
    public bool isPublished { get; set; } = false;
    public bool isFeatured { get; set; } = false;

    [DisplayName("Author")]
    public ApplicationUser CreatedBy { get; set; } = new ApplicationUser();
    
    [DisplayName("Created On")]
    public DateTime CreatedOn { get; set; }
    
    [DisplayName("Last Updated")]
    public DateTime UpdatedOn { get; set; }
}
