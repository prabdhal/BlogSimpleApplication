using BlogSimple.Model.Models;

namespace BlogSimple.Web.Services.Interfaces;

public interface IAchievementsService
{
    Task<Achievements> Create(Achievements achievements);
    Task<Achievements> Get(string id);
    Task<Achievements> Get(Guid id);
    Task<List<Achievements>> GetAll();
    Task<Achievements> Update(Guid id, Achievements achievements);
    void Remove(Guid id);
}
