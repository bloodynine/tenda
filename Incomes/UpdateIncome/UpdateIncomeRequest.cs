using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.Incomes.UpdateIncome;

public class UpdateIncomeRequest : FinancialTransactionRequestBase
{
    public string Id { get; set; }
    public bool IsResolved { get; set; }

    public override FinancialTransaction ToTransaction()
    {
        return new FinancialTransaction(this, IsResolved, TransactionType.Income);
    }
}