using FastEndpoints;
using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.Repeats.UpdateRepeatContract;

public class UpdateRepeatContractRequest
{
    public string ContractId { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Amount { get; set; } = 0;
    public DateTime StartDate { get; set; } = new ();
    public int Interval { get; set; } = 0;
    public RepeatType RepeatType { get; set; } = RepeatType.None;
    public DateTime CurrentViewDate { get; set; } = new();
    public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();
    [FromClaim("UserId")] public string UserId { get; set; } = "";
}

public class UpdateRepeatContractRequestValidator : Validator<UpdateRepeatContractRequest>
{
    public UpdateRepeatContractRequestValidator()
    {
        RuleFor(x => x.ContractId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Amount).NotEqual(0);
        RuleFor(x => x.StartDate).GreaterThan(new DateTime());
        RuleFor(x => x.RepeatType).NotEqual(RepeatType.None);
        RuleFor(x => x.CurrentViewDate).GreaterThan(new DateTime());
        RuleFor(x => x.UserId).NotEmpty();
    }
}