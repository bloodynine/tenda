using FastEndpoints;
using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.Repeats.GetRepeatContract;

public class GetRepeatContract : Endpoint<GetRepeatContractRequest, GetRepeatContractResponse>
{
    public override void Configure()
    {
        Get("api/repeats/{Id}");
        Claims("UserId", "SeedId");
    }

    public override async Task HandleAsync(GetRepeatContractRequest req, CancellationToken ct)
    {
        var contract = await DB.Find<RepeatContracts>().OneAsync(req.Id, ct);
        if (contract is null){ await SendNotFoundAsync(); return;}

        await SendAsync(new GetRepeatContractResponse(contract), cancellation: ct);
    }
}