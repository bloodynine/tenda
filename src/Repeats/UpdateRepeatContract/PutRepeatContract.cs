using FastEndpoints;
using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.Repeats.UpdateRepeatContract;

public class PutRepeatContract : Endpoint<UpdateRepeatContractRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; } = null!;

    public override void Configure()
    {
        Put("api/repeats/{ContractId}");
        Claims("UserId", "SeedId");
    }

    public override async Task HandleAsync(UpdateRepeatContractRequest req, CancellationToken ct)
    {
        var fullContract = new RepeatContracts(req);
        var updatedContract = await DB.UpdateAndGet<RepeatContracts>()
            .MatchID(req.ContractId)
            .ModifyExcept(x => new {x.Type}, fullContract)
            .ExecuteAsync(ct);

        if (updatedContract is null)
        {
            await SendNotFoundAsync();
            return;
        }

        await DB.DeleteAsync<FinancialTransaction>(x => x.AssociatedRepeatId == updatedContract.ID && !x.IsResolved);

        await updatedContract.CalculateTargets().SaveAsync(cancellation: ct);
        var response =
            await GetByMonthService.GetMonth(req.CurrentViewDate, req.UserId, ct);
        await SendAsync(response, cancellation: ct);
    }
}