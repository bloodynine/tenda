using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
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
        .WithWebHostBuilder(x => { x.UseSetting("DatabaseNameOverride", "IntTests"); }).CreateClient();

    public static TestServer Server { get; } = Factory.Server;

    static Setup()
    {
        var (_, result) = GuestClient.POSTAsync<
                LoginEndpoint,
                LoginRequest,
                LoginResponse>(new LoginRequest
            {
                Username = "intAdmin",
                Password = "intAdminOne"
            })
            .GetAwaiter()
            .GetResult();
        var (_, userResult) = GuestClient.POSTAsync<
                LoginEndpoint,
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

    public static async Task<HubConnection> StartConnectionAsync(HttpMessageHandler handler, string hubName, string token)
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl($"https://localhost:7139/api/{hubName}", o =>
            {
                o.HttpMessageHandlerFactory = _ => handler;
                o.AccessTokenProvider = () => Task.FromResult(token)!;
            })
            .Build();

        await hubConnection.StartAsync();

        return hubConnection;
    }
}