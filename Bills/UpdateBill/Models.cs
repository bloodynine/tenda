using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.Bills.UpdateBill;

public class UpdateBillRequest : FinancialTransactionRequestBase
{
    public string Id { get; set; }
    public bool IsResolved { get; set; }

    public override FinancialTransaction ToTransaction()
    {
        return new FinancialTransaction(this, IsResolved, TransactionType.Bill)
        {
            Updated = DateTime.Now
        };
    }
}