using BlogSimple.Model.Models;
using BlogSimple.Web.Services.Interfaces;
using BlogSimple.Web.Settings.Interfaces;
using MongoDB.Driver;
using NuGet.ProjectModel;

namespace BlogSimple.Web.Services;

public class AchievementsService : IAchievementsService
{
    private readonly IMongoCollection<Achievements> _achievements;

    public AchievementsService(
        IBlogSimpleDatabaseSettings postSettings,
        IMongoClient mongoClient
        )
    {
        var db = mongoClient.GetDatabase(postSettings.DatabaseName);
        _achievements = db.GetCollection<Achievements>(postSettings.AchievementsCollectionName);
    }

    public async Task<Achievements> Create()
    {
        Achievements achievements = new Achievements();
        await _achievements.InsertOneAsync(achievements);
        return achievements;
    }

    public async Task<Achievements> Get(string id)
    {
        Achievements achievements;
        try
        {
            achievements = await _achievements.Find(u => u.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
        return achievements;
    }

    public async Task<List<Achievements>> GetAll()
    {
        return await _achievements.Find(_ => true).ToListAsync();
    }

    public async Task<Achievements> Update(string id, Achievements achievements)
    {
        await _achievements.ReplaceOneAsync(a => a.Id == id, achievements);
        return achievements;
    }

    public async void Remove(string id)
    {
        await _achievements.DeleteOneAsync(u => u.Id == id);
    }
}
