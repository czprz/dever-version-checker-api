using System.Text.Json.Serialization;

namespace VersionCheckApi.Models;

public class Release
{
    public string Url { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    
    [JsonPropertyName("tag_name")]
    public string TagName { get; set; }
    
    [JsonPropertyName("created_at")]
    public DateTime Created { get; set; }
    
    [JsonPropertyName("published_at")]
    public DateTime Published { get; set; }
}