using MongoDB.Entities;

namespace Tenda.Users;

public class User : Entity
{
    public string Name { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;

    public bool IsAdmin { get; init; }
}