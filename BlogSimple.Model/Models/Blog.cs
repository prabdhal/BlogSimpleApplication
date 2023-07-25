using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace BlogSimple.Model.Models;

public class Blog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = String.Empty;
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string Content { get; set; } = String.Empty;

    [DisplayName("Author")]
    public ApplicationUser CreatedBy { get; set; } = new ApplicationUser();
    
    [DisplayName("Created On")]
    public DateTime CreatedOn { get; set; }
    
    [DisplayName("Last Updated")]
    public DateTime UpdatedOn { get; set; }
}
