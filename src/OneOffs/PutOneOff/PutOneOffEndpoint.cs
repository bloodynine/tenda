using MongoDB.Entities;
using Tenda.OneOffs.UpdateOneOff;
using Tenda.Shared;
using Tenda.Shared.Errors;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.OneOffs.PutOneOff;

public class PutOneOffEndpoint : Endpoint<PutOneOffRequest, Month>
{
    public IGetByMonthService MonthService { get; set; } = null!;

    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("/api/OneOffs/{Id}");
        Claims("UserId");
        PostProcessors(new TotalPostProcessor<PutOneOffRequest, Month>());
    }

    public override async Task HandleAsync(PutOneOffRequest req, CancellationToken ct)
    {
        var transaction = req.ToTransaction();
        var result = await DB.Update<FinancialTransaction>()
            .MatchID(req.Id)
            .ModifyWith(transaction)
            .ExecuteAsync(ct);
        if (result.ModifiedCount == 0)
        {
            await this.HandleApiErrorsAsync(new NotFoundException(), ct);
            return;
        }

        await SendAsync(await  MonthService.GetMonth(req.Date, req.UserId, ct), cancellation: ct);
    }
}