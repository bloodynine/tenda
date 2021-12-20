namespace Tenda.Users.CreateUser;

public class CreateUserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
}

public class CreateUserResponse
{
    public string Id { get; set; }
    public string Username { get; set; }
}