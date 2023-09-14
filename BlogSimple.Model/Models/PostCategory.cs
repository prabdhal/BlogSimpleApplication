using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.Models;

public enum PostCategory
{
    [Display(Name = "HTML")]
    HTML = 0,
    [Display(Name = "CSS")]
    CSS = 1,
    [Display(Name = "JavaScript")]
    JavaScript = 2,
    [Display(Name = "C#")]
    CSharp = 3,
    [Display(Name = "Object-Oriented Programming")]
    OOP = 4,
    [Display(Name = "Web Design")]
    WebDesign = 5,
    [Display(Name = "Tutorials")]
    Tutorials = 6,
    [Display(Name = "Freebies")]
    Freebies = 7,
    [Display(Name = "Other")]
    Other = 8
}
