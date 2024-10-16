using System.Net;
using FastEndpoints;
using FluentAssertions;
using NUnit.Framework;
using static IntegrationTests.Setup;

namespace IntegrationTests;

public class LoginTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void LoginTest()
    {
        var (response, result) = GuestClient.POSTAsync<
                    Tenda.Users.Login.LoginEndpoint,
                    Tenda.Users.Login.LoginRequest,
                    Tenda.Users.LoginResponse>(new()
                {
                    Username = "intUser",
                    Password = "intUserOne"
                })
                .GetAwaiter()
                .GetResult();
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.BearerToken.Should().NotBeNull();
    }

    [Test]
    public void Login_ForbiddenTest()
    {
        var response = GuestClient.POSTAsync<
                    Tenda.Users.Login.LoginEndpoint,
                    Tenda.Users.Login.LoginRequest
                    >(new()
                {
                    Username = "nope",
                    Password = "NoWay"
                })
                .GetAwaiter()
                .GetResult();
        response!.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}