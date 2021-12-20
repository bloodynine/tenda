using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.OneOffs.UpdateOneOff;

public class UpdateOneOffRequest : FinancialTransactionRequestBase
{
    public string Id { get; set; }
    public bool IsResolved { get; set; }

    public override FinancialTransaction ToTransaction()
    {
        return new FinancialTransaction(this, IsResolved, TransactionType.OneOff)
        {
            Updated = DateTime.Now
        };
    }
}