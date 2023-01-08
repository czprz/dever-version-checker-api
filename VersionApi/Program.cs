using VersionCheckApi.Endpoints.Version;
using VersionCheckApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapLatestVersionEndpoints();

app.MapHealthChecks("/healthz");

app.Run();