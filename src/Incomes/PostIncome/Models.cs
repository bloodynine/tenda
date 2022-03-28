using Tenda.Shared;
using Tenda.Shared.BaseModels;
using Tenda.Shared.Models;

namespace Tenda.Incomes.PostIncome;

public class PostIncomeRequest : FinancialTransactionRequestBase
{
    public RepeatRequest? RepeatSettings { get; set; } = null;

    public override FinancialTransaction ToTransaction()
    {
        return new FinancialTransaction(this, false, TransactionType.Income);
    }
}

public class PostIncomeRequestValidator : Validator<PostIncomeRequest>
{
    public PostIncomeRequestValidator()
    {
        Include(new FinancialTransactionRequestBaseValidator());
    }
}