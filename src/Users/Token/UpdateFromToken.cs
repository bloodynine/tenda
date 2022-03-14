using FastEndpoints;

namespace Tenda.Users.Token;

public class UpdateFromToken : Endpoint<TokenRequest, LoginResponse>
{
    public ILoginService LoginService { get; set; } = null!;

    public override void Configure()
    {
        Post("api/users/token");
        AllowAnonymous();
    }

    public override async Task HandleAsync(TokenRequest req, CancellationToken ct)
    {
        await SendAsync(await LoginService.RefreshToken(req.Token, ct), cancellation: ct);
    }
}