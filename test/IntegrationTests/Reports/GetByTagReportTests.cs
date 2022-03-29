using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FastEndpoints;
using FluentAssertions;
using MongoDB.Entities;
using NUnit.Framework;
using Tenda.Reports;
using Tenda.Reports.GetByTagReport;
using Tenda.Shared.Models;
using Tenda.Users;
using static IntegrationTests.Setup;


namespace IntegrationTests.Reports;

public class GetByTagReportTests : BaseTest
{
    private DateOnly TestDate;

    [SetUp]
    public void Init()
    {
        DeleteAllTransactions();
        TestDate = new DateOnly(1908, 1, 1);
    }

    [Test]
    public void GetByTagReport_200Test()
    {
        CreateTransactionsWithTags(new List<string> { "tag1" }, 2, DateOnly.FromDateTime(DateTime.Now));
        CreateTransactionsWithTags(new List<string> { "tag1" }, 2, DateOnly.FromDateTime(DateTime.Now));
        CreateTransactionsWithTags(new List<string> { "tag1", "tag2" }, 2, DateOnly.FromDateTime(DateTime.Now));
        var (response, result) = AdminClient.POSTAsync<GetByTagReportEndpoint, GetReportRequest, GetByTagResponse>(
            new GetReportRequest()
        ).GetAwaiter().GetResult();

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.ByCount.Count.Should().Be(2);
        result.ByCount.Single(x => x.Name == "tag1").Value.Should().Be(3);
        result.ByCount.Single(x => x.Name == "tag2").Value.Should().Be(1);
        result.ByDollar.Count.Should().Be(2);
        result.ByDollar.Single(x => x.Name == "tag1").Value.Should().Be(6);
        result.ByDollar.Single(x => x.Name == "tag2").Value.Should().Be(2);
        DeleteAllTransactions();
    }

    [Test]
    public void GetByTagReport_200DateRangeTest()
    {
        CreateTransactionsWithTags(new List<string> { "tag1" }, 2, TestDate);
        CreateTransactionsWithTags(new List<string> { "tag1" }, 2, TestDate);
        CreateTransactionsWithTags(new List<string> { "tag1", "tag2" }, 2, TestDate);
        CreateTransactionsWithTags(new List<string> { "tag1", "tag2" }, 2, TestDate.AddMonths(2));
        CreateTransactionsWithTags(new List<string> { "tag1" }, 2, TestDate.AddMonths(2));
        CreateTransactionsWithTags(new List<string> { "tag1", "tag2" }, 2, TestDate.AddMonths(4));
        var (response, firstResult) = AdminClient.POSTAsync<GetByTagReportEndpoint, GetReportRequest, GetByTagResponse>(
            new GetReportRequest
            {
                StartDate = TestDate,
                EndDate = TestDate.AddMonths(1)
            }
        ).GetAwaiter().GetResult();

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        firstResult!.ByCount.Count.Should().Be(2);
        firstResult.ByCount.Single(x => x.Name == "tag1").Value.Should().Be(3);
        firstResult.ByCount.Single(x => x.Name == "tag2").Value.Should().Be(1);
        firstResult.ByDollar.Count.Should().Be(2);
        firstResult.ByDollar.Single(x => x.Name == "tag1").Value.Should().Be(6);
        firstResult.ByDollar.Single(x => x.Name == "tag2").Value.Should().Be(2);

        var (_, secondResult) = AdminClient.POSTAsync<GetByTagReportEndpoint, GetReportRequest, GetByTagResponse>(
            new GetReportRequest
            {
                StartDate = TestDate,
                EndDate = TestDate.AddMonths(2).AddDays(1)
            }
        ).GetAwaiter().GetResult();

        firstResult!.ByCount.Count.Should().Be(2);
        secondResult!.ByCount.Single(x => x.Name == "tag1").Value.Should().Be(5);
        secondResult.ByCount.Single(x => x.Name == "tag2").Value.Should().Be(2);
        secondResult.ByDollar.Count.Should().Be(2);
        secondResult.ByDollar.Single(x => x.Name == "tag1").Value.Should().Be(10);
        secondResult.ByDollar.Single(x => x.Name == "tag2").Value.Should().Be(4);

        DeleteAllTransactions();
    }

    [Test]
    public void GetByTagReport_200TypesTest()
    {
        CreateTransactionsWithTags(new List<string> { "tag1" }, 2, TestDate, TransactionType.Bill);
        CreateTransactionsWithTags(new List<string> { "tag1" }, 2, TestDate, TransactionType.Income);
        CreateTransactionsWithTags(new List<string> { "tag1", "tag2" }, 2, TestDate, TransactionType.OneOff);
        var (response, firstResult) = AdminClient.POSTAsync<GetByTagReportEndpoint, GetReportRequest, GetByTagResponse>(
            new GetReportRequest
            {
                StartDate = TestDate,
                EndDate = TestDate.AddMonths(1),
                Types = new [] { TransactionType.Bill , TransactionType.Income }
            }
        ).GetAwaiter().GetResult();

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        firstResult!.ByCount.Count.Should().Be(1);
        firstResult.ByCount.Single(x => x.Name == "tag1").Value.Should().Be(2);
        firstResult.ByDollar.Count.Should().Be(1);
        firstResult.ByDollar.Single(x => x.Name == "tag1").Value.Should().Be(4);

        var (_, secondResult) = AdminClient.POSTAsync<GetByTagReportEndpoint, GetReportRequest, GetByTagResponse>(
            new GetReportRequest
            {
                StartDate = TestDate,
                EndDate = TestDate.AddMonths(2).AddDays(1)
            }
        ).GetAwaiter().GetResult();

        secondResult!.ByCount.Count.Should().Be(2);
        secondResult!.ByCount.Single(x => x.Name == "tag1").Value.Should().Be(3);
        secondResult.ByCount.Single(x => x.Name == "tag2").Value.Should().Be(1);
        secondResult.ByDollar.Count.Should().Be(2);
        secondResult.ByDollar.Single(x => x.Name == "tag1").Value.Should().Be(6);
        secondResult.ByDollar.Single(x => x.Name == "tag2").Value.Should().Be(2);

        DeleteAllTransactions();
    }

    [Test]
    public void GetByTagsReport_400DateRangeTest()
    {
        var badDateRanges = new List<(DateRangeRequest dateRange, string because)>
        {
            (new DateRangeRequest { EndDate = TestDate }, "Only start date can't be null"),
            (new DateRangeRequest { StartDate = TestDate }, "Only end date can't be null"),
            (new DateRangeRequest { StartDate = TestDate, EndDate = TestDate.AddDays(-1) },
                "End date must be after start date")
        };

        badDateRanges.ForEach(dateRange =>
        {
            var response = AdminClient.POSTAsync<GetByTagReportEndpoint, GetReportRequest>(
                new GetReportRequest
                {
                    StartDate = dateRange.dateRange.StartDate,
                    EndDate = dateRange.dateRange.EndDate
                }
            ).GetAwaiter().GetResult();
            response!.StatusCode.Should().Be(HttpStatusCode.BadRequest, dateRange.because);
        });
    }


    private void CreateTransactionsWithTags(List<string> tags, decimal amount, DateOnly date, TransactionType type = TransactionType.OneOff)
    {
        var user = DB.Find<User>().Match(x => x.UserName == "intAdmin").ExecuteFirstAsync().GetAwaiter().GetResult();
        var transaction = new FinancialTransaction(Guid.NewGuid().ToString(), amount, date, true,
            type,
            user.ID, tags);
        transaction.SaveAsync().GetAwaiter().GetResult();
    }

    [TearDown]
    public void CleanUp()
    {
        DeleteAllTransactions();
    }
}