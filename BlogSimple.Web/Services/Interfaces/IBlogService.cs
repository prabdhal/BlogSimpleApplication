using BlogSimple.Model.Models;

namespace BlogSimple.Model.Services.Interfaces;

public interface IBlogService
{
    Task<List<Blog>> GetAll();
    Task<List<Blog>> GetAll(string searchString);
    Task<List<Blog>> GetPublishedOnly(string searchString);
    Task<List<Blog>> GetAll(User user);
    Task<Blog> Get(string id);
    Task<Blog> Create(Blog blog);
    Task<Blog> Update(string id, Blog blog);
    void Remove(string id);
    void Remove(Blog blog);
}
