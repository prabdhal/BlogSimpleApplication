using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.Models;

public enum PostCategory
{
    [Display(Name = "Technology")]
    Technology = 0,
    [Display(Name = "Economy & Finance")]
    EconomyAndFinance = 1,
    [Display(Name = "Health & Fitness")]
    HealthAndFitness = 2,
    [Display(Name = "Food")]
    Food = 3,
    [Display(Name = "Politics")]
    Politics = 4,
    [Display(Name = "Travel")]
    Travel = 5,
    [Display(Name = "Human Science")]
    HumanScience = 6,
    [Display(Name = "Nature Science")]
    NatureScience = 7
}
