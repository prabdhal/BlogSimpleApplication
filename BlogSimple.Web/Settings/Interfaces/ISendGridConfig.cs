namespace BlogSimple.Model.Models;

public interface ISendGridConfig
{
    public string APIKey { get; set; }
    public string SenderAddress { get; set; }
    public string SenderDisplayName { get; set; }
}
