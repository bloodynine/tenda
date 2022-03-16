using MongoDB.Entities;

namespace Tenda.Shared.Models;

public class ServerSettingsDoc : Entity
{
    public bool AllowSignUps { get; init; }
    public bool UseKeyCloak { get; init; }
}