using FastEndpoints;
using MongoDB.Entities;
using Tenda.ServerSettings;
using Tenda.Shared;

namespace Tenda.Users.CreateUser;

public class PostUser : Endpoint<CreateUserRequest, CreateUserResponse>
{
    public override void Configure()
    {
        Post("/api/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        var settings = await DB.Find<ServerSettingsDoc>().ExecuteFirstAsync(ct);
        if (!settings.AllowSignUps)
        {
            await SendErrorsAsync(ct);
            return;
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(req.Password);
        var userCount = await DB.CountAsync<User>(cancellation: ct);
        var user = new User() { Name = req.Name, Password = hashedPassword, UserName = req.Username, IsAdmin = userCount < 1 };
        await user.SaveAsync(cancellation: ct);
        var seed = new Seed() { UserId = user.ID, Amount = 0};
        await seed.SaveAsync(cancellation: ct);
        await SendAsync(new CreateUserResponse() { Id = user.ID, Username = user.UserName }, cancellation: ct);
    }
}