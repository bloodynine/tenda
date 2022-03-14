using System.Net.Http;
using System.Net.Http.Headers;
using FastEndpoints;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Tenda.Users;
using Tenda.Users.Login;

namespace IntegrationTests;

public static class Setup
{
    private static readonly WebApplicationFactory<Program> Factory = new();

    public static HttpClient AdminClient { get; } = Factory
        .WithWebHostBuilder(x => { x.UseSetting("DatabaseNameOverride", "IntTests"); }).CreateClient();

    public static HttpClient GuestClient { get; } = Factory
        .WithWebHostBuilder(x => { x.UseSetting("DatabaseNameOverride", "IntTests"); }).CreateClient();

    public static HttpClient UserClient { get; } = Factory
        .WithWebHostBuilder(x => x.Configure(y => { x.UseSetting("DatabaseNameOverride", "IntTests"); }))
        .CreateClient();

    static Setup()
    {
        var (_, result) = GuestClient.POSTAsync<
                Login,
                LoginRequest,
                LoginResponse>(new LoginRequest
            {
                Username = "intAdmin",
                Password = "intAdminOne"
            })
            .GetAwaiter()
            .GetResult();
        var (_, userResult) = GuestClient.POSTAsync<
                Login,
                LoginRequest,
                LoginResponse>(new LoginRequest
            {
                Username = "intUser",
                Password = "intUserOne"
            })
            .GetAwaiter()
            .GetResult();
        AdminClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result?.BearerToken);
        UserClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", userResult?.BearerToken);
    }
}