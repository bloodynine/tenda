namespace Tenda.All.Models;

public class GetMonthRequest
{
    public int Year { get; set; } = 0;
    public int Month { get; set; } = 0;

    [FromClaim("UserId")] 
    public string UserId { get; set; } = "";
}

public class GetMonthRequestValidator : Validator<GetMonthRequest>
{
    public GetMonthRequestValidator()
    {
        RuleFor(x => x.Year).GreaterThan(0);
        RuleFor(x => x.Month).GreaterThan(0);
        RuleFor(x => x.UserId).NotEmpty();
    }
}