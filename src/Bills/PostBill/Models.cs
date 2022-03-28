using Tenda.Shared;
using Tenda.Shared.BaseModels;
using Tenda.Shared.Models;

namespace Tenda.Bills.PostBill;

public class PostBillRequest : FinancialTransactionRequestBase
{
    public RepeatRequest? RepeatSettings { get; init; }

    public override FinancialTransaction ToTransaction()
    {
        return new FinancialTransaction(this, false, TransactionType.Bill);
    }
}

public class PostBillRequestValidator : Validator<PostBillRequest>
{
    public PostBillRequestValidator()
    {
        Include(new FinancialTransactionRequestBaseValidator());
    }
}