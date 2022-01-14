namespace Tenda.Users.Token;

public class TokenRequest
{
    public string Token { get; set; } = "";
}

public class TokenRequestValidator : Validator<TokenRequest>
{
    public TokenRequestValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
    }
}