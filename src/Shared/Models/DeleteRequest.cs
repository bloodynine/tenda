using Microsoft.AspNetCore.Mvc;
using Tenda.Shared.BaseModels;

namespace Tenda.Shared.Models;

public class DeleteRequest : RequestBase
{
    public string Id { get; set; } = "";

    [FromQuery] public DateOnly ViewDate { get; set; } = new DateOnly();
}

public class DeleteRequestValidator : Validator<DeleteRequest>
{
    public DeleteRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ViewDate).GreaterThan(new DateOnly());
    }
}