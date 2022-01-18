using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.OneOffs.CreateManyOneOffs;

public class CreateManyOneOffsRequest : FinancialTransactionRequestBase
{
    public List<OneOffStub> OneOffs { get; set; } = new List<OneOffStub>();

    public IEnumerable<FinancialTransaction> ToTransactions()
    {
        return OneOffs.Select(stub => new FinancialTransaction(stub.Name, stub.Amount, stub.Date, true, TransactionType.OneOff,
            UserId, stub.Tags)
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
    public string Name { get; set; } = "";
    public decimal Amount { get; set; } = 0;
    public DateTime Date { get; set; } = new ();
    public List<string> Tags { get; set; } = new List<string>();
}

public class CreateManyOneOffsRequestValidator : Validator<CreateManyOneOffsRequest>
{
    public CreateManyOneOffsRequestValidator()
    {
        RuleFor(x => x.OneOffs.Count).GreaterThan(0);
        RuleForEach(x => x.OneOffs).ChildRules(y =>
        {
            y.RuleFor(x => x.Name).NotEmpty();
            y.RuleFor(x => x.Amount).NotEqual(0);
            y.RuleFor(x => x.Date).GreaterThan(new DateTime());
        });
    }
}