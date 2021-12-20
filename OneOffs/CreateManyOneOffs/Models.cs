using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.OneOffs.CreateManyOneOffs;

public class CreateManyOneOffsRequest : FinancialTransactionRequestBase
{
    public List<OneOffStub> OneOffs { get; set; }

    public IEnumerable<FinancialTransaction> ToTransactions()
    {
        return OneOffs.Select(stub => new FinancialTransaction(stub.Name, stub.Amount, stub.Date, true, TransactionType.OneOff,
            UserId)
        {
            Created = DateTime.Now,
            Updated = DateTime.Now
        });
    }

    public override FinancialTransaction ToTransaction()
    {
        throw new NotImplementedException();
    }
}

public class OneOffStub
{
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}