using Tenda.Shared;
using Tenda.Shared.Models;

namespace Tenda.OneOffs.GetOneOffById;

public class OneOffByIdRequest : FinancialTransactionRequestBase
{
    public string Id { get; set; } = "";

    public override FinancialTransaction ToTransaction()
    {
        throw new NotImplementedException();
    }
}

public class OneOffByIdRequestValidator : Validator<OneOffByIdRequest>
{
    public OneOffByIdRequestValidator()
    {
        Include(new FinancialTransactionRequestBaseValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}
