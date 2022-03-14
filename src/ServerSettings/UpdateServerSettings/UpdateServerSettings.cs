using MongoDB.Entities;

namespace Tenda.ServerSettings.UpdateServerSettings;

public class UpdateServerSettings: Endpoint<UpdateServerSettingsRequest, UpdateServerSettingsResponse>
{
    public override void Configure()
    {
        Put("/api/settings/server/{id}");
        ClaimsAll("UserId", "IsAdmin");
    }

    public override async Task HandleAsync(UpdateServerSettingsRequest req, CancellationToken ct)
    {
        var doc = new ServerSettingsDoc() { AllowSignUps = req.AllowSignUps, ID = req.Id };
        await DB.Update<ServerSettingsDoc>().MatchID(req.Id).ModifyWith(doc).ExecuteAsync(ct);

        await SendAsync(new(doc, false), cancellation: ct);
    }
}