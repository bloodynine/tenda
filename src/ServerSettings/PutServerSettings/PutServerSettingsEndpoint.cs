using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.ServerSettings.PutServerSettings;

public class PutServerSettingsEndpoint: Endpoint<PutServerSettingsRequest, PutServerSettingsResponse>
{
    public override void Configure()
    {
        Put("/api/settings/server/{id}");
        ClaimsAll("UserId", "IsAdmin");
    }

    public override async Task HandleAsync(PutServerSettingsRequest req, CancellationToken ct)
    {
        var doc = new ServerSettingsDoc() { AllowSignUps = req.AllowSignUps, ID = req.Id };
        await DB.Update<ServerSettingsDoc>().MatchID(req.Id).ModifyWith(doc).ExecuteAsync(ct);

        await SendAsync(new(doc, false), cancellation: ct);
    }
}