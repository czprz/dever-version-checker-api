using Asp.Versioning.Builder;
using VersionCheckApi.Services;

namespace VersionCheckApi.Endpoints.Version;

public static class GetLatestVersion
{
    public static void MapLatestVersionEndpoints(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGet("version/latest", GetLatest)
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(1);
    }

    private static async Task<IResult> GetLatest(IReleaseService releaseService)
    {
        var release = await releaseService.GetLatestRelease();
        return release == null ? Results.BadRequest() : Results.Ok(release.TagName);
    }
}