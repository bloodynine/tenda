using MongoDB.Entities;

namespace Tenda.Users;

public class User : Entity
{
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}