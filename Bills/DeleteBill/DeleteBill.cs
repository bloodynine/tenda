using FastEndpoints;
using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.Bills.DeleteBill;

public class DeleteBill : Endpoint<DeleteRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; }

    public override void Configure()
    {
       Delete("api/bills/{Id}");
       Claims("UserId", "SeedId");
    }

    public override async Task HandleAsync(DeleteRequest req, CancellationToken ct)
    {
        var transaction = await DB.Find<FinancialTransaction>().OneAsync(req.Id, ct);
        if(transaction.UserId != req.UserId) ThrowError("Nope!");

        // ReSharper disable once MethodSupportsCancellation
        await transaction.DeleteAsync();
        await SendAsync(await GetByMonthService.GetMonth(transaction.Date, req.UserId, ct), cancellation: ct);
    }
}