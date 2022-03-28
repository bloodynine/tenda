using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FastEndpoints;
using FluentAssertions;
using MongoDB.Entities;
using NUnit.Framework;
using Tenda.Reports.GetReportSettings;
using Tenda.Shared.Models;
using Tenda.Users;
using static IntegrationTests.Setup;

namespace IntegrationTests.Reports;

public class GetReportSettingsTests : BaseTest
{
    [SetUp]
    public void Init()
    {
        DeleteAllTransactions();
    }

    [Test]
    public void GetReportSettings_200Test()
    {
        CreateTransaction(new DateTime(2020, 1, 1), new DateTime(2021, 1, 1), new DateTime(1900,1,1), new DateTime(2021, 2,2));
        var (response, result) = UserClient
            .GETAsync<GetReportSettingsEndpoint, GetReportSettingsRequest, GetReportSettingsResponse>(
                new()).GetAwaiter().GetResult();

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.ValidYears.Count().Should().Be(3);
        result.ValidYears.First().Should().Be(2021);
        result.ValidYears.Last().Should().Be(1900);
    }

    private void CreateTransaction(params DateTime[] dates)
    {
        var user = DB.Find<User>().Match(x => x.UserName == "intUser").ExecuteFirstAsync().GetAwaiter().GetResult();
        foreach (var date in dates)
        {
            var transaction = new FinancialTransaction(Guid.NewGuid().ToString(), 1, date,
                true,
                TransactionType.OneOff,
                user.ID, new List<string>());
            transaction.SaveAsync().GetAwaiter().GetResult();
        }
    }

    [TearDown]
    public void Finish()
    {
        DeleteAllTransactions();
    }
}