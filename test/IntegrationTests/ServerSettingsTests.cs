using System.Net;
using FastEndpoints;
using FluentAssertions;
using NUnit.Framework;
using Tenda.ServerSettings.GetServerSettings;
using Tenda.ServerSettings.UpdateServerSettings;
using static IntegrationTests.Setup;

namespace IntegrationTests;

public class ServerSettingsTests
{
    [SetUp]
    public void Setup()
    {
        var (_, settingsResponse) = AdminClient
            .GETAsync<GetServerSettings, ServerSettingsResponse>().GetAwaiter().GetResult();

        AdminClient
            .PUTAsync<UpdateServerSettings, UpdateServerSettingsRequest>(
                new UpdateServerSettingsRequest
                {
                    Id = settingsResponse!.ID,
                    AllowSignUps = true
                }).GetAwaiter().GetResult();
    }

    [Test]
    public void ServerSettings_Get_200()
    {
        var (response, result) = AdminClient
            .GETAsync<GetServerSettings, ServerSettingsResponse>()
            .GetAwaiter().GetResult();

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.IsFirstTime.Should().BeFalse();
        result!.ID.Should().NotBeNull();
        result!.UseKeyCloak.Should().BeFalse();
        result!.AllowSignUps.Should().BeTrue();
    }

    [Test]
    public void ServerSettings_Put_Ok()
    {
        var (_, settingsResponse) = AdminClient
            .GETAsync<GetServerSettings, ServerSettingsResponse>().GetAwaiter().GetResult();

        var (response, result) = AdminClient
            .PUTAsync<UpdateServerSettings,
                UpdateServerSettingsRequest,
                UpdateServerSettingsResponse>(
                new UpdateServerSettingsRequest
                {
                    Id = settingsResponse!.ID,
                    AllowSignUps = true
                }).GetAwaiter().GetResult();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.AllowSignUps.Should().BeTrue();
    }

    [Test]
    public void ServerSettings_Post_Forbidden()
    {
        var (_, settingsResponse) = AdminClient
            .GETAsync<GetServerSettings, ServerSettingsResponse>().GetAwaiter().GetResult();

        var response = UserClient
            .PUTAsync<UpdateServerSettings, UpdateServerSettingsRequest>(
                new UpdateServerSettingsRequest
                {
                    Id = settingsResponse!.ID,
                    AllowSignUps = false
                }
            )
            .GetAwaiter().GetResult();
        response!.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}