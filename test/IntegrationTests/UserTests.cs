using System;
using System.Net;
using FastEndpoints;
using FluentAssertions;
using NUnit.Framework;
using Tenda.ServerSettings.GetServerSettings;
using Tenda.ServerSettings.PutServerSettings;
using Tenda.Users.CreateUser;
using Tenda.Users.GetUser;
using Tenda.Users.PostUser;
using static IntegrationTests.Setup;

namespace IntegrationTests;

public class UserTests
{
    [SetUp]
    public void Setup()
    {
        UpdateAllowedSignups(true);
    }

    [Test]
    public void Users_GetLoggedInUser_200OK()
    {
        var (response, result) = UserClient.GETAsync<GetUserEndpoint, GetUserRequest, GetUserResponse>(
            new GetUserRequest()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.UserName.Should().Be("intUser");
        result.IsAdmin.Should().BeFalse();
    }

    [Test]
    public void Users_GetLoggedInUser_Unauthorized()
    {
        var response = GuestClient.GETAsync<GetUserEndpoint, GetUserRequest>(
            new GetUserRequest()).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public void Users_Post_Success()
    {
        var username = Guid.NewGuid().ToString();
        var (response, result) = GuestClient.POSTAsync<PostUserEndpoint, PostUserRequest, PostUserResponse>(
            new PostUserRequest
            {
                Username = username,
                Password = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            }
        ).GetAwaiter().GetResult();

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.Id.Should().NotBeNull();
        result.Username.Should().Be(username);
    }

    [Test]
    public void Users_Post_ShouldFailOnNotAllowSignups()
    {
        UpdateAllowedSignups(false);
        var response = GuestClient.POSTAsync<PostUserEndpoint, PostUserRequest>(
            new PostUserRequest
            {
                Username = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            }
        ).GetAwaiter().GetResult();
        response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        // Cleanup
        UpdateAllowedSignups(true);
    }


    private void UpdateAllowedSignups(bool allowSignups)
    {
        var (_, settingsResponse) = AdminClient
            .GETAsync<GetServerSettingsEndpoint, GetServerSettingsResponse>().GetAwaiter().GetResult();

        AdminClient.PUTAsync<PutServerSettingsEndpoint, PutServerSettingsRequest>(
            new PutServerSettingsRequest
            {
                Id = settingsResponse!.ID,
                AllowSignUps = allowSignups
            }).GetAwaiter().GetResult();
    }
}