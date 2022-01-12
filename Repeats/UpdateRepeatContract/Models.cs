using FastEndpoints;
using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.Repeats.UpdateRepeatContract;

public class UpdateRepeatContractRequest
{
    public string ContractId { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public int Interval { get; set; }
    public RepeatType RepeatType { get; set; }
    public DateTime CurrentViewDate { get; set; }
    public IEnumerable<string> Tags { get; set; }
    [FromClaim("UserId")] public string UserId { get; set; }
}