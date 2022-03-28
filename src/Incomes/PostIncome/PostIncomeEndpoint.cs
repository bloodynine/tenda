using MongoDB.Entities;
using Tenda.Incomes.PostIncome;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.Incomes.CreateIncome;

public class PostIncomeEndpoint : Endpoint<PostIncomeRequest, Month>
{
    public IGetByMonthService GetByMonthService { get; set; } = null!;

    public override void Configure()
    {
        Post("api/incomes");
        Claims("UserId", "SeedId");
        PostProcessors(new TotalPostProcessor<PostIncomeRequest, Month>());
    }

    public override async Task HandleAsync(PostIncomeRequest req, CancellationToken ct)
    {
        var transaction = req.ToTransaction();

        if (req.RepeatSettings is not null)
        {
            RepeatContracts repeatSettings = new(req.RepeatSettings, req, TransactionType.Income);
            await repeatSettings.SaveAsync(cancellation: ct);
            var incomes = repeatSettings.CalculateTargets();
            await incomes.SaveAsync(cancellation: ct);
        }
        else
        {
            await transaction.SaveAsync(cancellation: ct);
        }

        await SendAsync(await GetByMonthService.GetMonth(req.Date, req.UserId, ct), cancellation: ct);
    }
}