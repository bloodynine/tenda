using Tenda.Shared;
using Tenda.Shared.BaseModels;
using Tenda.Shared.Models;

namespace Tenda.Repeats.GetRepeatContract;

public class GetRepeatContractRequest : RequestBase
{
    public string Id { get; set; } = "";
}

public class GetRepeatContractRequestValidator : Validator<GetRepeatContractRequest>
{
    public GetRepeatContractRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class GetRepeatContractResponse
{
    public GetRepeatContractResponse(RepeatContracts contract)
    {
        Name = contract.Name;
        Amount = contract.Amount;
        Interval = contract.Interval;
        Type = contract.Type;
        RepeatType = contract.RepeatType;
        StartDate = contract.StartDate;
        Id = contract.ID;
        Tags = contract.Tags;
        EndDate = contract.EndDate;
    }

    public GetRepeatContractResponse()
    {
    }

    public string Name { get; set; } = "";
    public int Interval { get; set; }
    public TransactionType Type { get; set; }
    public RepeatType RepeatType { get; set; }
    public DateTime StartDate { get; set; }
    public decimal Amount { get; set; }
    public string Id { get; set; } = "";
    public DateTime? EndDate { get; set; }
    public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();
}