using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.Incomes.CreateIncome;

public class CreateIncomeRequest : FinancialTransactionRequestBase
{
    public RepeatRequest? RepeatSettings { get; set; }

    public override FinancialTransaction ToTransaction()
    {
        return new FinancialTransaction(this, false, TransactionType.Income);
    }
}