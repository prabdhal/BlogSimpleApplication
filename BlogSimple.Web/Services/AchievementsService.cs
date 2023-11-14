using BlogSimple.Model.Models;
using BlogSimple.Web.Services.Interfaces;
using BlogSimple.Web.Settings.Interfaces;
using MongoDB.Driver;

namespace BlogSimple.Web.Services;

public class AchievementsService : IAchievementsService
{
    private readonly IMongoCollection<Achievements> _achievements;

    public AchievementsService(
        IPostSimpleDatabaseSettings blogSettings,
        IMongoClient mongoClient
        )
    {
        var db = mongoClient.GetDatabase(blogSettings.DatabaseName);
        _achievements = db.GetCollection<Achievements>(blogSettings.AchievementsCollectionName);
    }

    public async Task<Achievements> Create(Achievements achievements)
    {
        await _achievements.InsertOneAsync(achievements);
        return achievements;
    }

    public async Task<Achievements> Get(string id)
    {
        var achievements = await _achievements.Find(u => u.Id == Guid.Parse(id)).FirstOrDefaultAsync();
        return achievements;
    }

    public async Task<Achievements> Get(Guid id)
    {
        var achievements = await _achievements.Find(u => u.Id == id).FirstOrDefaultAsync();
        return achievements;
    }

    public async Task<List<Achievements>> GetAll()
    {
        return await _achievements.Find(_ => true).ToListAsync();
    }

    public async Task<Achievements> Update(Guid id, Achievements achievements)
    {
        await _achievements.ReplaceOneAsync(a => a.Id == id, achievements);
        return achievements;
    }

    public async void Remove(Guid id)
    {
        await _achievements.DeleteOneAsync(u => u.Id == id);
    }
}
