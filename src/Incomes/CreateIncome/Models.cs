using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.Incomes.CreateIncome;

public class CreateIncomeRequest : FinancialTransactionRequestBase
{
    public RepeatRequest? RepeatSettings { get; set; } = null;

    public override FinancialTransaction ToTransaction()
    {
        return new FinancialTransaction(this, false, TransactionType.Income);
    }
}

public class CreateIncomeRequestValidator : Validator<CreateIncomeRequest>
{
    public CreateIncomeRequestValidator()
    {
        Include(new FinancialTransactionRequestBaseValidator());
    }
}