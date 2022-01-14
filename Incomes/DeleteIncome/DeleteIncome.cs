using FastEndpoints;
using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.Incomes.DeleteIncome;

public class DeleteIncome : Endpoint<DeleteRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; } = null!;

    public override void Configure()
    {
        Delete("api/incomes/{Id}");
        Claims("UserId", "SeedId");
    }

    public override async Task HandleAsync(DeleteRequest req, CancellationToken ct)
    {
        var transaction = await DB.Find<FinancialTransaction>().MatchID(req.Id).Match(x => x.UserId == req.UserId)
            .ExecuteFirstAsync(ct);
        if (transaction is null)
        {
            await SendNotFoundAsync();
            return;
        }

        await transaction.DeleteAsync();
        await SendAsync(await GetByMonthService.GetMonth(transaction.Date, req.UserId, ct), cancellation: ct);
    }
}