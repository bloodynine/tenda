using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.Bills.UpdateBill;

public class UpdateBillRequest : FinancialTransactionRequestBase
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

public class UpdateBillRequestValidator : Validator<UpdateBillRequest>
{
    public UpdateBillRequestValidator()
    {
        Include(new FinancialTransactionRequestBaseValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}

