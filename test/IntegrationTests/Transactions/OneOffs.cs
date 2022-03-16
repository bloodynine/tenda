using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FastEndpoints;
using FluentAssertions;
using NUnit.Framework;
using Tenda.All;
using Tenda.OneOffs;
using Tenda.OneOffs.GetOneOff;
using Tenda.OneOffs.PostOneOff;
using Tenda.OneOffs.PostOneOffs;
using Tenda.OneOffs.PutOneOff;
using Tenda.OneOffs.UpdateOneOff;
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
        var (response, result) = AdminClient.POSTAsync<PostOneOffsEndpoint, PostOneOffsRequest, Month>(
            new PostOneOffsRequest
            {
                OneOffs = new List<OneOffStub>
                {
                    new() { Amount = 1, Date = DateTime.Now, Name = "Seeded" }
                }
            }
        ).GetAwaiter().GetResult();

        seededTransaction = GetTransactionFromMonth(result, "Seeded", 1)?.ID!;
        transactionsToCleanUp.Add(seededTransaction!);
    }

    [Test]
    public void GetOneOffById_200()
    {
        var (response, result) = AdminClient.GETAsync<GetOneOffEndPoint, GetOneOffRequest, OneOffResponse>(
            new GetOneOffRequest
            {
                Id = seededTransaction
            }
        ).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Name.Should().Be("Seeded");
        result.Amount.Should().Be(1);
    }

    [Test]
    public void GetOneOffById_404()
    {
        var response = AdminClient.GETAsync<GetOneOffEndPoint, GetOneOffRequest>(
            new GetOneOffRequest
            {
                Id = "NoPe"
            }
        ).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public void GetOneOffById_400()
    {
        var response = UserClient.GETAsync<GetOneOffEndPoint, GetOneOffRequest>(
            new GetOneOffRequest
            {
                Id = seededTransaction
            }
        ).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public void PutOneOff_200()
    {
        var (response, result) = AdminClient.PUTAsync<PutOneOffEndpoint, PutOneOffRequest, Month>(
            new PutOneOffRequest
            {
                Name = "Updated",
                Amount = 7,
                Id = seededTransaction
            }
        ).GetAwaiter().GetResult();

        response!.StatusCode.Should().Be(HttpStatusCode.OK);
        var transaction = GetTransactionFromMonth(result, "Updated", 7);
        transaction!.Name.Should().Be("Updated");
        transaction!.Amount.Should().Be(7);
        transaction!.ID.Should().Be(seededTransaction);
    }

    [Test]
    public void PutOneOff_404()
    {
        var response = AdminClient.PUTAsync<PutOneOffEndpoint, PutOneOffRequest>(
            new PutOneOffRequest
            {
                Name = "Updated",
                Amount = 7,
                Id = "Nope"
            }
        ).GetAwaiter().GetResult();

        response!.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public void PostOneOffs_200()
    {
        var requests = new List<OneOffStub>
        {
            new() { Name = Guid.NewGuid().ToString(), Amount = 1, Date = DateTime.Now },
            new() { Name = Guid.NewGuid().ToString(), Amount = 1, Date = DateTime.Now },
            new() { Name = Guid.NewGuid().ToString(), Amount = 1, Date = DateTime.Now }
        };

        var (response, result) = AdminClient.POSTAsync<PostOneOffsEndpoint, PostOneOffsRequest, Month>(
            new PostOneOffsRequest
            {
                OneOffs = requests
            }
        ).GetAwaiter().GetResult();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var transaction1 = GetTransactionFromMonth(result, requests[0].Name, 1);
        var transaction2 = GetTransactionFromMonth(result, requests[1].Name, 1);
        var transaction3 = GetTransactionFromMonth(result, requests[2].Name, 1);
        transaction1.Should().NotBeNull();
        transaction2.Should().NotBeNull();
        transaction3.Should().NotBeNull();
        transactionsToCleanUp.AddRange(new[] { transaction1.ID, transaction2.ID, transaction3.ID });
    }

    [Test]
    public void PostOneOffs_400_Validator()
    {
        var requests = new List<PostOneOffsRequest>
        {
            new(),
            new() { OneOffs = new List<OneOffStub> { new() { Date = DateTime.Now, Name = "", Amount = 1 } } },
            new() { OneOffs = new List<OneOffStub> { new() { Date = DateTime.Now, Name = "Name", Amount = 0 } } },
            new() { OneOffs = new List<OneOffStub> { new() { Name = "Name", Amount = 0 } } }
        };

        requests.ForEach(request =>
        {
            var response = AdminClient.POSTAsync<PostOneOffsEndpoint, PostOneOffsRequest>(
                request).GetAwaiter().GetResult();
            response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        });
    }

    [Test]
    public void PostOneOffs_401()
    {
        var response = GuestClient.POSTAsync<PostOneOffsEndpoint, PostOneOffsRequest>(
            new PostOneOffsRequest()).GetAwaiter().GetResult();
        response!.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public void DeleteOneOff_200()
    {
        var (_, origMonth) = AdminClient.POSTAsync<PostOneOffEndpoint, PostOneOffRequest, Month>(
            new PostOneOffRequest
            {
                Name = "DeleteMe",
                Amount = 1,
                Date = DateTime.Now
            }
        ).GetAwaiter().GetResult();
        var oneOff = GetTransactionFromMonth(origMonth, "DeleteMe", 1);

        var response = AdminClient.DeleteAsync($"api/oneOffs/{oneOff.ID}?viewDate=2020-01-01").GetAwaiter().GetResult();
        var (_, modMonth) = AdminClient.GETAsync<GetMonthEndpoint, GetMonthRequest, Month>(
            new GetMonthRequest
            {
                Year = DateTime.Now.Year,
                Month = DateTime.Now.Month
            }).GetAwaiter().GetResult();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        GetTransactionFromMonth(modMonth, "DeleteMe", 1).Should().BeNull();
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

    private FinancialTransaction? GetTransactionFromMonth(Month? month, string name, decimal amount)
    {
        return (month?.Days.SelectMany(x => x.OneOffs)).FirstOrDefault(y => y.Name == name && y.Amount == amount);
    }
}