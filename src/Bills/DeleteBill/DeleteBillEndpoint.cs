using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Errors;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.Bills.DeleteBill;

public class DeleteBillEndpoint : Endpoint<DeleteRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; } = null!;

    public override void Configure()
    {
        Delete("/api/bills/{Id}");
        Claims("UserId", "SeedId");
        PostProcessors(new TotalPostProcessor<DeleteRequest, Month>());
    }

    public override async Task HandleAsync(DeleteRequest req, CancellationToken ct)
    {
        var transaction = await DB.Find<FinancialTransaction>().OneAsync(req.Id, ct);
        if (transaction.UserId != req.UserId)
        {
            await this.HandleApiErrorsAsync(new ForbiddenException($"User does not own {req.Id}"), ct);
            return;
        }

        ;

        // ReSharper disable once MethodSupportsCancellation
        await transaction.DeleteAsync();
        await SendAsync(await GetByMonthService.GetMonth(transaction.Date, req.UserId, ct), cancellation: ct);
    }
}