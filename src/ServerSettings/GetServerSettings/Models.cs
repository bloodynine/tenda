namespace Tenda.ServerSettings.GetServerSettings;

public class GetServerSettingsRequest
{
}

public class ServerSettingsResponse : ServerSettingsDoc
{
    public bool IsFirstTime { get; init; }
    public ServerSettingsResponse(ServerSettingsDoc doc, bool isFirstTime)
    {
        this.AllowSignUps = doc.AllowSignUps;
        this.UseKeyCloak = doc.UseKeyCloak;
        this.ID = doc.ID;
        this.IsFirstTime = isFirstTime;
    }

    public ServerSettingsResponse()
    {
    }
}