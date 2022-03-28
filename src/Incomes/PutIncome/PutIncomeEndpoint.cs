using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.Incomes.PutIncome;

public class PutIncomeEndpoint : Endpoint<PutIncomeRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; } = null!;

    public override void Configure()
    {
        Put("api/incomes/{Id}");
        Claims("UserId", "SeedId");
        PostProcessors(new TotalPostProcessor<PutIncomeRequest, Month>());
    }

    public override async Task HandleAsync(PutIncomeRequest req, CancellationToken ct)
    {
        var transaction = await DB.UpdateAndGet<FinancialTransaction>()
            .MatchID(req.Id)
            .Match(x => x.UserId == req.UserId)
            .ModifyWith(req.ToTransaction())
            .ExecuteAsync(ct);
        if (transaction is null)
        {
            await SendNotFoundAsync();
            return;
        }

        await SendAsync(await GetByMonthService.GetMonth(req.Date, req.UserId, ct), cancellation: ct);
    }
}