using FastEndpoints;
using MongoDB.Entities;
using Tenda.Bills.CreateBill;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.Bills.UpdateBill;

public class UpdateBill : Endpoint<UpdateBillRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; } = null!;

    public override void Configure()
    {
        Put("api/bills/{Id}");
        Claims("UserId", "SeedId");
        PostProcessors(new TotalPostProcessor<UpdateBillRequest, Month>());
    }

    public override async Task HandleAsync(UpdateBillRequest req, CancellationToken ct)
    {
        var transaction = req.ToTransaction();
        var updatedBill = await DB.UpdateAndGet<FinancialTransaction>()
            .Match(x => x.ID == req.Id)
            .Match(x => x.UserId == req.UserId)
            .ModifyExcept(x => new { x.Type, x.AssociatedRepeatId }, transaction)
            .ExecuteAsync(ct);

        if (updatedBill is null)
        {
            await SendNotFoundAsync();
            return;
        }

        await SendAsync(await GetByMonthService.GetMonth(req.Date, req.UserId, ct), cancellation: ct);
    }
}