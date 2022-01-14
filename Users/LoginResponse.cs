namespace Tenda.Users;

public class LoginResponse
{
    public LoginResponse(string bearerToken, string refreshToken)
    {
        BearerToken = bearerToken;
        RefreshToken = refreshToken;
    }

    public LoginResponse()
    {
        BearerToken = "";
        RefreshToken = "";
    }

    public string BearerToken { get; set; }
    public string RefreshToken { get; set; }

    public DateTime BearerExpiresAt { get; set; } = DateTime.Now.AddMinutes(5);

    public DateTime RefreshExpiresAt { get; set; } = DateTime.Now.AddDays(1);
}