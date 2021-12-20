using MongoDB.Entities;

namespace Tenda.Users;

public class RefreshToken : Entity
{
    public RefreshToken(string token, string userId)
    {
        Token = token;
        UserId = userId;
    }

    public string Token { get; set; }
    public string UserId { get; set; }

    public DateTime ExpiresAt { get; set; } = DateTime.Now.AddHours(3);

    public bool IsValid { get; set; } = true;
}