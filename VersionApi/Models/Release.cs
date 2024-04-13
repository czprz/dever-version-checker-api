using System.Text.Json.Serialization;

namespace VersionCheckApi.Models;

public record Release
{
    public string Url { get; set; } = string.Empty;
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("tag_name")]
    public string TagName { get; set; } = string.Empty;
    
    [JsonPropertyName("created_at")]
    public DateTime Created { get; set; }
    
    [JsonPropertyName("published_at")]
    public DateTime Published { get; set; }
}