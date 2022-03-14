using Tenda.Shared.BaseModels;

namespace Tenda.ServerSettings.UpdateServerSettings;

public class UpdateServerSettingsRequest: RequestBase
{
    public string Id { get; init; }
    public bool AllowSignUps { get; init; }
}

public class UpdateServerSettingsResponse : ServerSettingsDoc
{
    public bool IsFirstTime { get; init; }

    public UpdateServerSettingsResponse(ServerSettingsDoc doc, bool isFirstTime)
    {
        this.IsFirstTime = isFirstTime;
        this.AllowSignUps = doc.AllowSignUps;
        this.UseKeyCloak = doc.UseKeyCloak;
    }

    public UpdateServerSettingsResponse()
    {
    }
}