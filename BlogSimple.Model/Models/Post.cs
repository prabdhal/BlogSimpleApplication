﻿using MongoDB.Bson;
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
    public byte[] HeaderImage { get; set; } = Convert.FromBase64String(DefaultImageBase64String);
    public bool IsPublished { get; set; } = false;
    public bool IsFeatured { get; set; } = false;
    [DisplayName("Author")]
    public Guid CreatedById { get; set; }
    
    [DisplayName("Created On")]
    public DateTime CreatedOn { get; set; }
    
    [DisplayName("Last Updated")]
    public DateTime UpdatedOn { get; set; }

    public int WordCount { get; set; }

    private static string DefaultImageBase64String = "/9j/4AAQSkZJRgABAQEASABIAAD/4Qm+aHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJYTVAgQ29yZSA0LjQuMC1FeGl2MiI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyI+IDxkYzpjcmVhdG9yPiA8cmRmOlNlcT4gPHJkZjpsaT5WZWN0b3JTdG9jay5jb20vNDA1NDY1MzA8L3JkZjpsaT4gPC9yZGY6U2VxPiA8L2RjOmNyZWF0b3I+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDw/eHBhY2tldCBlbmQ9InciPz7/2wBDAAMCAgMCAgMDAwMEAwMEBQgFBQQEBQoHBwYIDAoMDAsKCwsNDhIQDQ4RDgsLEBYQERMUFRUVDA8XGBYUGBIUFRT/2wBDAQMEBAUEBQkFBQkUDQsNFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBT/wAARCAD6AU0DAREAAhEBAxEB/8QAHQABAAICAwEBAAAAAAAAAAAAAAYIBwkC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";
}
