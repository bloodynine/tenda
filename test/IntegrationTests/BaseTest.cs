using MongoDB.Entities;
using NUnit.Framework;
using Tenda.Shared.Models;

namespace IntegrationTests;

public class BaseTest
{
    [SetUp]
    public void BaseSetup()
    {
        DB.InitAsync("IntTests", "localhost").GetAwaiter().GetResult();
    }

    protected void DeleteAllTransactions()
    {
        DB.DeleteAsync<FinancialTransaction>(x => x.UserId != "").GetAwaiter().GetResult();
    }
}