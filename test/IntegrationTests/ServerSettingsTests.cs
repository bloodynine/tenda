using System.Net;
using FastEndpoints;
using FluentAssertions;
using NUnit.Framework;
using Tenda.ServerSettings.GetServerSettings;
using Tenda.ServerSettings.PutServerSettings;
using static IntegrationTests.Setup;

namespace IntegrationTests;

public class ServerSettingsTests
{
    [SetUp]
    public void Setup()
    {
        var (_, settingsResponse) = AdminClient
            .GETAsync<GetServerSettingsEndpoint, GetServerSettingsResponse>().GetAwaiter().GetResult();

        AdminClient
            .PUTAsync<PutServerSettingsEndpoint, PutServerSettingsRequest>(
                new PutServerSettingsRequest
                {
                    Id = settingsResponse!.ID,
                    AllowSignUps = true
                }).GetAwaiter().GetResult();
    }

    [Test]
    public void ServerSettings_Get_200()
    {
        var (response, result) = AdminClient
            .GETAsync<GetServerSettingsEndpoint, GetServerSettingsResponse>()
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
            .GETAsync<GetServerSettingsEndpoint, GetServerSettingsResponse>().GetAwaiter().GetResult();

        var (response, result) = AdminClient
            .PUTAsync<PutServerSettingsEndpoint,
                PutServerSettingsRequest,
                PutServerSettingsResponse>(
                new PutServerSettingsRequest
                {
                    Id = settingsResponse!.ID,
                    AllowSignUps = true
                }).GetAwaiter().GetResult();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.AllowSignUps.Should().BeTrue();
    }

    [Test]
    public void ServerSettings_Put_Forbidden()
    {
        var (_, settingsResponse) = AdminClient
            .GETAsync<GetServerSettingsEndpoint, GetServerSettingsResponse>().GetAwaiter().GetResult();

        var response = UserClient
            .PUTAsync<PutServerSettingsEndpoint, PutServerSettingsRequest>(
                new PutServerSettingsRequest
                {
                    Id = settingsResponse!.ID,
                    AllowSignUps = false
                }
            )
            .GetAwaiter().GetResult();
        response!.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}