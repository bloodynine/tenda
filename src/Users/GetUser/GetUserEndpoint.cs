using MongoDB.Entities;

namespace Tenda.Users.GetUser;

public class GetUserEndpoint : Endpoint<GetUserRequest, GetUserResponse, GetUserMapper >
{
    public override void Configure()
    {
        Get("/api/users");
        Claims("UserId");
    }

    public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
    {
        var user = await DB.Find<User>().Match(x => x.ID == req.UserId).ExecuteFirstAsync(ct);
        await SendAsync(Map.FromEntity(user), cancellation: ct);
    }
}