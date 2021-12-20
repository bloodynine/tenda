using FastEndpoints;

namespace Tenda.Users.Login;

public class Login : Endpoint<LoginRequest, LoginResponse>
{
    public ILoginService LoginService { get; set; }

    public override void Configure()
    {
        Post("/api/users/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        await SendAsync(await LoginService.Login(req.Username, req.Password, ct), cancellation: ct);
    }
}