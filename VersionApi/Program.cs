using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using VersionCheckApi.Endpoints.Version;
using VersionCheckApi.OpenApi;
using VersionCheckApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());
builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new(1);
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new QueryStringApiVersionReader("api-version"),
            new HeaderApiVersionReader("api-version"),
            new MediaTypeApiVersionReader()
        );
    })
    .AddApiExplorer(options => { options.GroupNameFormat = "'v'VVV"; });

builder.Services.AddHealthChecks();

builder.Services.AddScoped<IReleaseService, ReleaseService>();

builder.Services.AddHttpClient("github-api", client =>
{
    client.BaseAddress = new("https://api.github.com");
    client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
    client.DefaultRequestHeaders.Add("User-Agent", "request");
    client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
    var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
});

var app = builder.Build();

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(1, 0)
    .HasApiVersion(2, 0)
    .Build();

app.MapLatestVersionEndpoints(versionSet);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.MapHealthChecks("/healthz");

app.Run();