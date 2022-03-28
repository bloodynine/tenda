using Tenda.Shared.BaseModels;

namespace Tenda.Users.GetUser;

public class GetUserResponse
{
    public string Id { get; init; } = "";
    public bool IsAdmin { get; init; }
    public string Name { get; init; } = "";
    public string UserName { get; init; } = "";
}

public class GetUserRequest : RequestBase
{
}

public class GetUserMapper : Mapper<GetUserRequest, GetUserResponse, User>
{
    public override GetUserResponse FromEntity(User user) => new()
    {
        Id = user.ID,
        IsAdmin = user.IsAdmin,
        Name = user.Name,
        UserName = user.UserName
    };
}