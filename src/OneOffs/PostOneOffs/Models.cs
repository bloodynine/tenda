using Tenda.Shared;
using Tenda.Shared.BaseModels;
using Tenda.Shared.Models;

namespace Tenda.OneOffs.PostOneOffs;

public class PostOneOffsRequest : FinancialTransactionRequestBase
{
    public List<OneOffStub> OneOffs { get; init; } = new();

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
    public DateOnly Date { get; set; } = new ();
    public List<string> Tags { get; set; } = new List<string>();
}

public class PostOneOffsRequestValidator : Validator<PostOneOffsRequest>
{
    public PostOneOffsRequestValidator()
    {
        RuleFor(x => x.OneOffs.Count).GreaterThan(0);
        RuleFor(x => x.UserId).NotEmpty();
        RuleForEach(x => x.OneOffs).ChildRules(y =>
        {
            y.RuleFor(x => x.Name).NotEmpty();
            y.RuleFor(x => x.Amount).NotEqual(0);
            y.RuleFor(x => x.Date).GreaterThan(new DateOnly());
        });
    }
}