using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.ServerSettings.GetServerSettings;

public class GetServerSettingsResponse : ServerSettingsDoc
{
    public bool IsFirstTime { get; init; }

    public GetServerSettingsResponse(ServerSettingsDoc doc, bool isFirstTime)
    {
        AllowSignUps = doc.AllowSignUps;
        UseKeyCloak = doc.UseKeyCloak;
        ID = doc.ID;
        IsFirstTime = isFirstTime;
    }

    public GetServerSettingsResponse()
    {
    }
}