using FastEndpoints;
using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.Incomes.UpdateIncome;

public class UpdateIncome : Endpoint<UpdateIncomeRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; }

    public override void Configure()
    {
        Put("api/incomes/{Id}");
        Claims("UserId", "SeedId");
        PostProcessors(new TotalPostProcessor<UpdateIncomeRequest, Month>());
    }

    public override async Task HandleAsync(UpdateIncomeRequest req, CancellationToken ct)
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