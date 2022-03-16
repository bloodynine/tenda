using Tenda.Shared.Errors;

namespace Tenda.Users.Login;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    public ILoginService LoginService { get; set; } = null!;

    public override void Configure()
    {
        Post("/api/users/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        LoginResponse response;
        try
        {
            response = await LoginService.Login(req.Username, req.Password, ct);
        }
        catch (Exception e) when (ApiErrors.Filter(e))
        {
            await this.HandleApiErrorsAsync(e, ct);
            return;
        }


        await SendAsync(response, cancellation: ct);
    }
}

