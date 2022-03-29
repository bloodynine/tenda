using Tenda.Shared.BaseModels;
using Tenda.Shared.Models;

namespace Tenda.OneOffs;

public class OneOffResponse : ResponseBase
{
    public OneOffResponse(FinancialTransaction transaction) : base(transaction)
    {
        Name = transaction.Name;
        Amount = transaction.Amount;
        IsResolved = transaction.IsResolved;
        Type = transaction.Type;
        Created = transaction.Created;
        Updated = transaction.Updated;
        Date = transaction.Date;
    }

    public OneOffResponse()
    {
    }

    public string Name { get; set; } = "";
    public decimal Amount { get; set; }
    public bool IsResolved { get; set; }
    public TransactionType Type { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;

    public DateTime Updated { get; set; } = DateTime.Now;

    public DateOnly Date { get; set; }
}