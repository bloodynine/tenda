namespace Tenda.Users.PostUser;

public class PostUserRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Name { get; set; } = "";
}

public class PostUserRequestValidator : Validator<PostUserRequest>
{
    public PostUserRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class PostUserResponse
{
    public string Id { get; set; } = "";
    public string Username { get; set; } = "";
}