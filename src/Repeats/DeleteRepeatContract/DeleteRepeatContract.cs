using FastEndpoints;
using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.Repeats.DeleteRepeatContract;

public class DeleteRepeatContract : Endpoint<DeleteRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; } = null!;

    public override void Configure()
    {
       Delete("api/repeats/{Id}");
       Claims("UserId", "SeedId");
    }

    public override async Task HandleAsync(DeleteRequest req, CancellationToken ct)
    {
        await DB.DeleteAsync<FinancialTransaction>(x =>
            x.AssociatedRepeatId == req.Id && x.UserId == req.UserId && !x.IsResolved);
        await DB.DeleteAsync<RepeatContracts>(req.Id);
        await DB.Update<FinancialTransaction>()
            .Match(x => x.AssociatedRepeatId == req.Id && x.UserId == req.UserId && x.IsResolved)
            .Modify(x => x.AssociatedRepeatId, "")
            .ExecuteAsync(ct);
        await SendAsync(await GetByMonthService.GetMonth(req.ViewDate, req.UserId, ct), cancellation: ct);
    }
}