using Tenda.Shared;
using Tenda.Shared.BaseModels;
using Tenda.Shared.Models;

namespace Tenda.Bills.PutBill;

public class PutBillRequest : FinancialTransactionRequestBase
{
    public string Id { get; set; } = "";
    public bool IsResolved { get; set; } = false;

    public override FinancialTransaction ToTransaction()
    {
        return new FinancialTransaction(this, IsResolved, TransactionType.Bill)
        {
            Updated = DateTime.Now
        };
    }
}

public class PutBillRequestValidator : Validator<PutBillRequest>
{
    public PutBillRequestValidator()
    {
        Include(new FinancialTransactionRequestBaseValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}

