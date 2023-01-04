using VersionCheckApi.Models;

namespace VersionCheckApi.Services;

public interface IReleaseService
{
    Task<Release?> GetLatestRelease();
}