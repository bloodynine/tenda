using FastEndpoints;
using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.OneOffs.UpdateOneOff;

public class PutOneOff : Endpoint<UpdateOneOffRequest, Month>
{
    public IGetByMonthService MonthService { get; set; } = null!;

    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("/api/OneOffs/{Id}");
        Claims("UserId");
        PostProcessors(new TotalPostProcessor<UpdateOneOffRequest, Month>());
    }

    public override async Task HandleAsync(UpdateOneOffRequest req, CancellationToken ct)
    {
        var transaction = req.ToTransaction();
        await DB.Update<FinancialTransaction>()
            .MatchID(req.Id)
            .ModifyWith(transaction)
            .ExecuteAsync(ct);
        await SendAsync(await  MonthService.GetMonth(req.Date, req.UserId, ct), cancellation: ct);
    }
}