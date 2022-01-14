using Microsoft.AspNetCore.Mvc;
using Tenda.Shared.BaseModels;

namespace Tenda.Shared;

public class DeleteRequest : RequestBase
{
    public string Id { get; set; } = "";

    [FromQuery] public DateTime ViewDate { get; set; } = new DateTime();
}

public class DeleteRequestValidator : Validator<DeleteRequest>
{
    public DeleteRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ViewDate).GreaterThan(new DateTime());
    }
}