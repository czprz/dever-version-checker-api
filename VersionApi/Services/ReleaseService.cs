using VersionCheckApi.Models;

namespace VersionCheckApi.Services;

public class ReleaseService : IReleaseService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ReleaseService> _logger;

    public ReleaseService(IHttpClientFactory httpClientFactory, ILogger<ReleaseService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
    
    public async Task<Release?> GetLatestRelease()
    {
        var client = _httpClientFactory.CreateClient("github-api");
        try
        {
            return await client.GetFromJsonAsync<Release>("repos/czprz/dever/releases/latest");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogTrace("Could not get latest release due to {0}", ex.StatusCode);
            return default;
        } 
    }
}