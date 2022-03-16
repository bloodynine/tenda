using Tenda.Shared;
using Tenda.Shared.BaseModels;
using Tenda.Shared.Models;

namespace Tenda.ServerSettings.PutServerSettings;

public class PutServerSettingsRequest : RequestBase
{
    public string Id { get; init; }
    public bool AllowSignUps { get; init; }
}

public class PutServerSettingsResponse : ServerSettingsDoc
{
    public bool IsFirstTime { get; init; }

    public PutServerSettingsResponse(ServerSettingsDoc doc, bool isFirstTime)
    {
        IsFirstTime = isFirstTime;
        AllowSignUps = doc.AllowSignUps;
        UseKeyCloak = doc.UseKeyCloak;
    }

    public PutServerSettingsResponse()
    {
    }
}

public class PutServerSettingsRequestValidator : Validator<PutServerSettingsRequest>
{
    public PutServerSettingsRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}