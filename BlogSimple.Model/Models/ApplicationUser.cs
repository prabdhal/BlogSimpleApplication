using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using MongoDbGenericRepository.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BlogSimple.Model.Models;

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<Guid>
{
    public ApplicationUser() : base()
    {
    }

    public ApplicationUser(string userName, string email) : base(userName, email)
    {
    }
}