using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.Incomes.CreateIncome;

public class CreateIncomeRequest : FinancialTransactionRequestBase
{
    public RepeatRequest? RepeatRequest { get; set; }

    public override FinancialTransaction ToTransaction()
    {
        return new FinancialTransaction(this, false, TransactionType.Income);
    }
}