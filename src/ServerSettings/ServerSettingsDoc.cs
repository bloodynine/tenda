using MongoDB.Entities;

namespace Tenda.ServerSettings;

public class ServerSettingsDoc : Entity
{
    public bool AllowSignUps { get; init; }
    public bool UseKeyCloak { get; init; }
}