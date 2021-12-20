using System.Security.Cryptography;
using FastEndpoints.Security;
using MongoDB.Entities;
using Tenda.Shared;

namespace Tenda.Users;

public class LoginService : ILoginService
{
    public async Task<LoginResponse> Login(string userName, string password, CancellationToken ct)
    {
        var user = await GetUser(userName, ct);
        var seed = await GetSeedValue(user.ID, ct);
        if (BCrypt.Net.BCrypt.Verify(password, user!.Password)) return await GetLoginResponse(user, seed, ct);

        throw new ApplicationException("Not Authorized");
    }

    public async Task<LoginResponse> RefreshToken(string refreshToken, CancellationToken ct)
    {
        var tokenItem = await DB.Find<RefreshToken>()
            .Match(x => x.Token == refreshToken).ExecuteFirstAsync(ct);
        if (tokenItem is null || tokenItem.ExpiresAt < DateTime.Now) throw new ApplicationException("Token Expired");
        if (!tokenItem.IsValid) await InvalidateTokenFamily(tokenItem, ct);

        var user = await DB.Find<User>().Match(x => x.ID == tokenItem.UserId).ExecuteFirstAsync(ct)
                   ?? throw new ApplicationException("User not found");
        var seed = await GetSeedValue(user.ID, ct);
        return await GetLoginResponse(user, seed, ct);
    }

    private async Task InvalidateTokenFamily(RefreshToken tokenItem, CancellationToken ct)
    {
        await DB.Update<RefreshToken>()
            .Match(x => x.UserId == tokenItem.UserId)
            .Match(x => x.IsValid)
            .Modify(x => x.IsValid, false)
            .ExecuteAsync(cancellation:ct);
        throw new ApplicationException($"Invalid token submitted: Token Id {tokenItem.Token}");
    }

    private async Task<LoginResponse> GetLoginResponse(User user, Seed seed, CancellationToken ct)
    {
        //TODO Add sig to env
        var token = JWTBearer.CreateToken(
            "FDJSKLfdjsklw93nfugh84hrew99",
            DateTime.Now.AddDays(1),
            claims: new[]
                { ("Username", user.UserName), ("UserId", user.ID), ("SeedId", seed.ID), ("Name", user.ID) }
        );
        var tokenData = new byte[64];
        RandomNumberGenerator.Fill(tokenData);
        var refreshToken = new RefreshToken(Convert.ToBase64String(tokenData), user.ID);
        await refreshToken.SaveAsync(cancellation: ct);
        await DB.Update<RefreshToken>()
            .Match(x => x.ID != refreshToken.ID)
            .Match(x => x.IsValid)
            .Modify(x => x.IsValid, false)
            .ExecuteAsync(cancellation: ct);

        return new LoginResponse(token, refreshToken.Token);
    }

    private async Task<User> GetUser(string username, CancellationToken ct)
    {
        var user = await DB.Find<User>()
            .Match(x => x.UserName == username).ExecuteFirstAsync(ct);
        if (user is null) throw new ApplicationException("User Not found");
        return user;
    }

    private async Task<Seed> GetSeedValue(string userId, CancellationToken ct)
    {
        var seed = await DB.Find<Seed>().Match(x => x.UserId == userId).ExecuteFirstAsync(ct);
        if (seed is null) throw new ApplicationException();
        return seed;
    }
}

public interface ILoginService
{
    Task<LoginResponse> Login(string userId, string password, CancellationToken ct);
    Task<LoginResponse> RefreshToken(string refreshToken, CancellationToken ct);
}