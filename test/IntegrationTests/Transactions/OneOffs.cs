using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FastEndpoints;
using FluentAssertions;
using NUnit.Framework;
using Tenda.OneOffs;
using Tenda.OneOffs.CreateManyOneOffs;
using Tenda.OneOffs.GetOneOffById;
using Tenda.Shared.Models;
using static IntegrationTests.Setup;

namespace IntegrationTests.Transactions;

public class OneOffs
{
    private string seededTransaction;
    private List<string> transactionsToCleanUp;

    [SetUp]
    public void Setup()
    {
        transactionsToCleanUp = new List<string>();
        var (response, result) = AdminClient.POSTAsync<CreateManyOneOffs, CreateManyOneOffsRequest, Month>(
            new CreateManyOneOffsRequest
            {
                OneOffs = new List<OneOffStub>
                {
                    new() { Amount = 1, Date = DateTime.Now, Name = "Seeded" }
                }
            }
        ).GetAwaiter().GetResult();

        seededTransaction = GetTransactionIdFromMonth(result, "Seeded", 1);
        transactionsToCleanUp.Add(seededTransaction);
    }

    [Test]
    public void GetOneOffById_200()
    {
        var (response, result) = AdminClient.GETAsync<GetOneOffById, OneOffByIdRequest, OneOffResponse>(
            new OneOffByIdRequest
            {
                Id = seededTransaction
            }
        ).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Name.Should().Be("Seeded");
        result.Amount.Should().Be(1);
    }

    [Test]
    public void DeleteOneOff_404()
    {
        var response = AdminClient.DeleteAsync("api/oneOffs/Nope?viewDate=2020-01-01").GetAwaiter().GetResult();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TearDown]
    public void TearDown()
    {
        transactionsToCleanUp.ForEach(x =>
        {
            AdminClient.DeleteAsync($"api/oneOffs/{x}?viewDate=2020-01-01").GetAwaiter().GetResult();
        });
    }

    private string GetTransactionIdFromMonth(Month? month, string name, decimal amount)
    {
        return month?.Days.SelectMany(x => x.OneOffs).Where(y => y.Name == name && y.Amount == amount).Select(z => z.ID)
            .FirstOrDefault();
    }
}