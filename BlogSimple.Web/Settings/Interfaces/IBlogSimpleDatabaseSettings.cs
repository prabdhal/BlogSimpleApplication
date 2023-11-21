namespace BlogSimple.Web.Settings.Interfaces;

public interface IBlogSimpleDatabaseSettings
{
    public string PostsCollectionName { get; set; }
    public string UsersCollectionName { get; set; }
    public string RolesCollectionName { get; set; }
    public string CommentsCollectionName { get; set; }
    public string RepliesCollectionName { get; set; }
    public string AchievementsCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
