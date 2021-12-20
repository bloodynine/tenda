using FastEndpoints;
using MongoDB.Entities;
using Tenda.Shared.Models;

namespace Tenda.OneOffs.GetOneOffById;

public class GetOneOffById : Endpoint<OneOffByIdRequest, OneOffResponse>
{
    public override void Configure()
    {
        Get("api/OneOffs/{Id}");
        Claims("UserId");
    }

    public override async Task HandleAsync(OneOffByIdRequest req, CancellationToken ct)
    {
        var transaction = await DB.Find<FinancialTransaction>().OneAsync(req.Id, ct);
        if (transaction is null) {await SendNotFoundAsync(); return;}

        if (transaction!.UserId != req.UserId) { await SendErrorsAsync(ct); return;}

        await SendAsync(new OneOffResponse(transaction), cancellation: ct);
    }
}