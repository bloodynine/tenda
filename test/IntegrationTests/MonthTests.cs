using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FastEndpoints;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR.Client;
using MongoDB.Entities;
using NUnit.Framework;
using Tenda.All;
using Tenda.All.Models;
using Tenda.OneOffs.PostOneOff;
using Tenda.Shared.Models;
using static IntegrationTests.Setup;

namespace IntegrationTests;

public class MonthTests : BaseTest
{
    private DateOnly TestDate => new(1908, 1, 1);

    [SetUp]
    public void Init()
    {
        DeleteAllTransactions();
    }

    [Test]
    public async Task SignalR_Connects_Test()
    {
        var token = AdminClient.DefaultRequestHeaders.Authorization!.Parameter;
        var connection = await StartConnectionAsync(Server.CreateHandler(), "ResolvedTotal", token);
        connection.Closed += async error =>
        {
            await Task.Delay(new Random().Next(0, 5) * 1000);
            await connection.StartAsync();
        };

        Seed message = new();
        connection.On<Seed>("ResolvedTotal", x => { message = x; });

        var (_, origMonth) = await AdminClient.POSTAsync<PostOneOffEndpoint, PostOneOffRequest, Month>(
            new PostOneOffRequest
            {
                Name = "DeleteMe",
                Amount = -1,
                Date = DateOnly.FromDateTime(DateTime.Now),
                IsResolved = true
            }
        );
        message.UserId.Should().NotBeNull();
    }

    [Test]
    public void GetMonth_200Test()
    {
        var (response, result) = UserClient.GETAsync<GetMonthEndpoint, GetMonthRequest, Month>(
            new GetMonthRequest
            {
                Year = TestDate.Year,
                Month = TestDate.Month
            }).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Days.Count.Should().Be(31);
    }

    [Test]
    public void GetMonth_TotalMathTest()
    {
        CreateTransaction(TestDate, 10);
        CreateTransaction(TestDate.AddDays(2), -3);
        CreateTransaction(TestDate.AddDays(4), -3);
        var (response, result) = UserClient.GETAsync<GetMonthEndpoint, GetMonthRequest, Month>(
            new GetMonthRequest
            {
                Year = TestDate.Year,
                Month = TestDate.Month
            }).GetAwaiter().GetResult();

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.Days.Single(x => x.Date.Day == TestDate.Day).RunningTotal.Should().Be(10);
        result!.Days.Single(x => x.Date.Day == TestDate.AddDays(1).Day).RunningTotal.Should().Be(10);
        result!.Days.Single(x => x.Date.Day == TestDate.AddDays(2).Day).RunningTotal.Should().Be(7);
        result!.Days.Single(x => x.Date.Day == TestDate.AddDays(4).Day).RunningTotal.Should().Be(4);
        result!.Days.Single(x => x.Date.Day == TestDate.AddDays(20).Day).RunningTotal.Should().Be(4);

        CreateTransaction(TestDate.AddDays(2), -3);
        var (response2, result2) = UserClient.GETAsync<GetMonthEndpoint, GetMonthRequest, Month>(
            new GetMonthRequest
            {
                Year = TestDate.Year,
                Month = TestDate.Month
            }).GetAwaiter().GetResult();
        response2!.StatusCode.Should().Be(HttpStatusCode.OK);
        result2!.Days.Single(x => x.Date.Day == TestDate.AddDays(2).Day).RunningTotal.Should().Be(4);
        result2!.Days.Single(x => x.Date.Day == TestDate.AddDays(4).Day).RunningTotal.Should().Be(1);
        result2!.Days.Single(x => x.Date.Day == TestDate.AddDays(20).Day).RunningTotal.Should().Be(1);

        var (response3, result3) = UserClient.GETAsync<GetMonthEndpoint, GetMonthRequest, Month>(
            new GetMonthRequest
            {
                Year = TestDate.AddYears(1).Year,
                Month = TestDate.AddYears(1).Month
            }).GetAwaiter().GetResult();
        response3!.StatusCode.Should().Be(HttpStatusCode.OK);
        result3!.Days.Single(x => x.Date.Day == TestDate.AddYears(1).Day).RunningTotal.Should().Be(1);
    }

    private void CreateTransaction(DateOnly date, decimal amount)
    {
        var (_, origMonth) = UserClient.POSTAsync<PostOneOffEndpoint, PostOneOffRequest, Month>(
            new PostOneOffRequest
            {
                Name = Guid.NewGuid().ToString(),
                Amount = amount,
                Date = date,
                IsResolved = true
            }
        ).GetAwaiter().GetResult();
    }

    [TearDown]
    public void CleanUp()
    {
        DeleteAllTransactions();
    }
}