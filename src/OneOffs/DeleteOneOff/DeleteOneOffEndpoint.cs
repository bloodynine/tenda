using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Errors;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.OneOffs.DeleteOneOff;

public class DeleteOneOffEndpoint : Endpoint<DeleteRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; } = null!;

    public override void Configure()
    {
        Delete("/api/OneOffs/{Id}");
        Claims("UserId", "SeedId");
        PostProcessors(new TotalPostProcessor<DeleteRequest, Month>());
    }

    public override async Task HandleAsync(DeleteRequest req, CancellationToken ct)
    {
        // ReSharper disable once MethodSupportsCancellation
        var result = await DB.DeleteAsync<FinancialTransaction>(req.Id);
        if (result.DeletedCount == 0)
        {
            await this.HandleApiErrorsAsync(new NotFoundException($"{req.Id} Not Found"), ct);
            return;
        }

        await SendAsync(await GetByMonthService.GetMonth(req.ViewDate, req.UserId, ct), cancellation: ct);
    }
}