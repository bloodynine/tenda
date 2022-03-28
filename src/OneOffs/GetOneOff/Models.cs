using Tenda.Shared;
using Tenda.Shared.BaseModels;
using Tenda.Shared.Models;

namespace Tenda.OneOffs.GetOneOff;

public class GetOneOffRequest : FinancialTransactionRequestBase
{
    public string Id { get; set; } = "";

    public override FinancialTransaction ToTransaction()
    {
        throw new NotImplementedException();
    }
}

public class OneOffByIdRequestValidator : Validator<GetOneOffRequest>
{
    public OneOffByIdRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
