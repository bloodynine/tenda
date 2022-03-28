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

        var (response, result) = AdminClient.GETAsync<GetByMonthReportEndpoint, GetByMonthRequest, GetByMonthResponse>(
            new GetByMonthRequest { Year = Year }
        ).GetAwaiter().GetResult();

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        result!.IncomeExpensesByMonth.Count().Should().Be(12);
        result.IncomeExpensesByMonth.First().Name.Should().Be("January");
        result.IncomeExpensesByMonth.First().Series.Count().Should().Be(2);
        result.IncomeExpensesByMonth.First().Series.First().Name.Should().Be("Income");
        result.IncomeExpensesByMonth.First().Series.First().Value.Should().Be(2);
        result.IncomeExpensesByMonth.First().Series.Last().Name.Should().Be("Expenses");
        result.IncomeExpensesByMonth.First().Series.Last().Value.Should().Be(4);

        result.IncomeExpensesByMonth.Skip(1).First().Name.Should().Be("February");
        result.IncomeExpensesByMonth.Skip(1).First().Series.Count().Should().Be(2);
        result.IncomeExpensesByMonth.Skip(1).First().Series.First().Name.Should().Be("Income");
        result.IncomeExpensesByMonth.Skip(1).First().Series.First().Value.Should().Be(6);
        result.IncomeExpensesByMonth.Skip(1).First().Series.Last().Name.Should().Be("Expenses");
        result.IncomeExpensesByMonth.Skip(1).First().Series.Last().Value.Should().Be(4);

        result.IncomeExpensesByMonth.Skip(2).First().Name.Should().Be("March");
        result.IncomeExpensesByMonth.Skip(2).First().Series.Count().Should().Be(2);
        result.IncomeExpensesByMonth.Skip(2).First().Series.Last().Name.Should().Be("Expenses");
        result.IncomeExpensesByMonth.Skip(2).First().Series.Last().Value.Should().Be(4);

        result.AverageExpenses.Should().Be(1);
        result.AverageIncome.Should().BeApproximately(0.66666667m, 0.00000001m);
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