using Tenda.Shared;
using Tenda.Shared.BaseModels;
using Tenda.Shared.Models;

namespace Tenda.Incomes.PutIncome;

public class PutIncomeRequest : FinancialTransactionRequestBase
{
    public string Id { get; set; } = "";
    public bool IsResolved { get; set; } = false;

    public override FinancialTransaction ToTransaction()
    {
        return new FinancialTransaction(this, IsResolved, TransactionType.Income);
    }
}

public class PutIncomeRequestValidator : Validator<PutIncomeRequest>
{
    public PutIncomeRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        Include(new FinancialTransactionRequestBaseValidator());
    }
}