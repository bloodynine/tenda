using FastEndpoints;
using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.OneOffs.DeleteOneOff;

public class DeleteOneOff : Endpoint<DeleteRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; }

    public override void Configure()
    {
        Delete("api/OneOffs/{Id}");
        Claims("UserId", "SeedId");
    }

    public override async Task HandleAsync(DeleteRequest req, CancellationToken ct)
    {
        // ReSharper disable once MethodSupportsCancellation
        await DB.DeleteAsync<FinancialTransaction>(req.Id);
        // var date = req.ViewDate ?? DateTime.Now;
        await SendAsync(await GetByMonthService.GetMonth(req.ViewDate, req.UserId, ct), cancellation: ct);
    }
}