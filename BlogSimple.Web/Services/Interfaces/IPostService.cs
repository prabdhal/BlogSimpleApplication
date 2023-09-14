using BlogSimple.Model.Models;

namespace BlogSimple.Model.Services.Interfaces;

public interface IPostService
{
    Task<List<Post>> GetAll();
    Task<List<Post>> GetAll(string searchString);
    Task<List<Post>> GetPublishedOnly(string searchString);
    Task<List<Post>> GetAll(User user);
    Task<Post> Get(string id);
    Task<Post> Create(Post post);
    Task<Post> Update(string id, Post post);
    void Remove(string id);
    void Remove(Post post);
}
