using BlogSimple.Model.Models;

namespace BlogSimple.Web.Services.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAll();
    Task<User> Get(string id);
    Task<User> Create(User user);
    Task<User> Update(string userName, User user);
    void Remove(string userName);
}
