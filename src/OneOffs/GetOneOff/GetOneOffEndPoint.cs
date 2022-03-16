using MongoDB.Entities;
using Tenda.Shared.Errors;
using Tenda.Shared.Models;

namespace Tenda.OneOffs.GetOneOff;

public class GetOneOffEndPoint : Endpoint<GetOneOffRequest, OneOffResponse>
{
    public override void Configure()
    {
        Get("/api/OneOffs/{Id}");
        Claims("UserId");
    }

    public override async Task HandleAsync(GetOneOffRequest req, CancellationToken ct)
    {
        var transaction = await DB.Find<FinancialTransaction>().OneAsync(req.Id, ct);
        if (transaction is null)
        {
            await this.HandleApiErrorsAsync(new NotFoundException(), ct); 
            return;
        }

        if (transaction!.UserId != req.UserId) { await SendErrorsAsync(ct); return;}

        await SendAsync(new OneOffResponse(transaction), cancellation: ct);
    }
}