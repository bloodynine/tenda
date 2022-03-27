using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FastEndpoints;
using FluentAssertions;
using MongoDB.Entities;
using NUnit.Framework;
using Tenda.Reports;
using Tenda.Reports.GetByMonthReport;
using Tenda.Shared.Models;
using Tenda.Users;
using static IntegrationTests.Setup;

namespace IntegrationTests.Reports;

public class GetByMonthReport : BaseTest
{
    private int Year => 2222;

    [SetUp]
    public void Init()
    {
        DeleteAllTransactions();
    }

    [Test]
    public void GetByMonthReport_200Test()
    {
        CreateTransaction(1, 2, TransactionType.Bill);
        CreateTransaction(1, 2, TransactionType.OneOff);
        CreateTransaction(1, 2, TransactionType.Income);
        CreateTransaction(2, 2, TransactionType.OneOff);
        CreateTransaction(2, 2, TransactionType.OneOff);
        CreateTransaction(2, 2, TransactionType.Income);
        CreateTransaction(2, 4, TransactionType.Income);
        CreateTransaction(3, 4, TransactionType.Bill);

        var (response, result) = AdminClient.GETAsync<GetByMonthReportEndpoint, GetByMonthRequest, List<GroupedItem>>(
            new GetByMonthRequest { Year = Year }
        ).GetAwaiter().GetResult();

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.Count.Should().Be(12);
        result[0].Name.Should().Be("January");
        result[0].Series.Count().Should().Be(2);
        result[0].Series.First().Name.Should().Be("Income");
        result[0].Series.First().Value.Should().Be(2);
        result[0].Series.Last().Name.Should().Be("Expenses");
        result[0].Series.Last().Value.Should().Be(4);

        result[1].Name.Should().Be("February");
        result[1].Series.Count().Should().Be(2);
        result[1].Series.First().Name.Should().Be("Income");
        result[1].Series.First().Value.Should().Be(6);
        result[1].Series.Last().Name.Should().Be("Expenses");
        result[1].Series.Last().Value.Should().Be(4);

        result[2].Name.Should().Be("March");
        result[2].Series.Count().Should().Be(2);
        result[2].Series.Last().Name.Should().Be("Expenses");
        result[2].Series.Last().Value.Should().Be(4);
    }

    private void CreateTransaction(int month, decimal amount, TransactionType type)
    {
        var user = DB.Find<User>().Match(x => x.UserName == "intAdmin").ExecuteFirstAsync().GetAwaiter().GetResult();
        var transaction = new FinancialTransaction(Guid.NewGuid().ToString(), amount, new DateTime(Year, month, 3),
            true,
            type,
            user.ID, new List<string>());
        transaction.SaveAsync().GetAwaiter().GetResult();
    }
}