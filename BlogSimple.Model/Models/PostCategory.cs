using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.Models;

public enum PostCategory
{
    [Display(Name = "Programming")]
    Programming = 0,
    [Display(Name = "Game Development")]
    GameDevelopment = 1,
    [Display(Name = "Web Development")]
    WebDevelopment = 2,
    [Display(Name = "General Tutorials")]
    GeneralTutorials = 3,
    [Display(Name = "Other")]
    Other = 4
}
