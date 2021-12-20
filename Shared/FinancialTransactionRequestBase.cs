using Tenda.Shared.BaseModels;
using Tenda.Shared.Models;

namespace Tenda.Shared;

public abstract class FinancialTransactionRequestBase : RequestBase
{
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public abstract FinancialTransaction ToTransaction();
}