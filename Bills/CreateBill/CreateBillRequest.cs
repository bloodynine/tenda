using Tenda.OneOffs.CreateManyOneOffs;
using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.Bills.CreateBill;

public class CreateBillRequest : FinancialTransactionRequestBase
{
    public RepeatRequest? RepeatSettings { get; init; }

    public override FinancialTransaction ToTransaction()
    {
        return new FinancialTransaction(this, false, TransactionType.Bill);
    }
}

public class CreateBillRequestValidator : Validator<CreateBillRequest>
{
    public CreateBillRequestValidator()
    {
        Include(new FinancialTransactionRequestBaseValidator());
    }
}