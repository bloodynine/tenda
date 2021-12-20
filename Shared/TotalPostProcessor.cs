using FastEndpoints;
using FastEndpoints.Validation.Results;
using Microsoft.AspNetCore.SignalR;
using Tenda.Shared.Hubs;
using Tenda.Shared.Models;
using Tenda.Shared.Services;

namespace Tenda.Shared;

public class TotalPostProcessor<TRequest, TResponse> : IPostProcessor<TRequest, TResponse>
{
    public async Task PostProcessAsync(TRequest req, TResponse res, HttpContext ctx,
        IReadOnlyCollection<ValidationFailure> failures, CancellationToken ct)
    {
        var totalService = ctx.RequestServices.GetRequiredService<ITotalService>();
        var hubContext = ctx.RequestServices.GetRequiredService<IHubContext<ResolvedTotal>>();
        if (req is FinancialTransactionRequestBase @base)
        {
            var seed = await totalService.CalculateTotal(@base.UserId, @base.SeedId, ct);
            await hubContext.Clients.User(@base.UserId).SendAsync("Foo", seed, ct);
        }
    }
}