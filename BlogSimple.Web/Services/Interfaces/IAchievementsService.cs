using BlogSimple.Model.Models;

namespace BlogSimple.Web.Services.Interfaces;

public interface IAchievementsService
{
    Task<Achievements> Create();
    Task<Achievements> Get(string id);
    Task<List<Achievements>> GetAll();
    Task<Achievements> Update(string id, Achievements achievements);
    void Remove(string id);
}
