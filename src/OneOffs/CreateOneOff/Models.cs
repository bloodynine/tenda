using Tenda.Shared;
using Tenda.Shared.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Tenda.OneOffs.CreateOneOff;

public class CreateOneOffRequest : FinancialTransactionRequestBase
{
    public bool? IsResolved { get; set; } = false;

    public override FinancialTransaction ToTransaction()
    {
        return new FinancialTransaction(this, IsResolved.GetValueOrDefault(true), TransactionType.OneOff)
        {
            Created = DateTime.Now,
            Updated = DateTime.Now
        };
    }
}

public class CreateOneOffRequestValidator : Validator<CreateOneOffRequest>
{
    public CreateOneOffRequestValidator()
    {
        Include(new FinancialTransactionRequestBaseValidator());
    }
}