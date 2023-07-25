namespace BlogSimple.Web.Settings;

public class BlogSimpleDatabaseSettings : IBlogSimpleDatabaseSettings
{
    public string BlogsCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}
