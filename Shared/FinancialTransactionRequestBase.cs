using Tenda.Shared.BaseModels;
using Tenda.Shared.Models;

namespace Tenda.Shared;

public abstract class FinancialTransactionRequestBase : RequestBase
{
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }

    public List<string> Tags { get; set; } = new List<string>();
    public abstract FinancialTransaction ToTransaction();
}