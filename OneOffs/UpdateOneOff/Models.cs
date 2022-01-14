using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.OneOffs.UpdateOneOff;

public class UpdateOneOffRequest : FinancialTransactionRequestBase
{
    public string Id { get; set; } = "";
    public bool IsResolved { get; set; } = false;

    public override FinancialTransaction ToTransaction()
        {
        return new FinancialTransaction(this, IsResolved, TransactionType.OneOff)
        {
            Updated = DateTime.Now
        };
    }
}

public class UpdateOneOffRequestValidator : Validator<UpdateOneOffRequest>
{
    public UpdateOneOffRequestValidator()
    {
        Include(new FinancialTransactionRequestBaseValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}
