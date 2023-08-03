namespace BlogSimple.Web.Settings.Interfaces;

public interface IBlogSimpleDatabaseSettings
{
    public string BlogsCollectionName { get; set; }
    public string CommentsCollectionName { get; set; }
    public string RepliesCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
