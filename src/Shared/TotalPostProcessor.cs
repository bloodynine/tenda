using FastEndpoints.Validation.Results;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Tenda.OneOffs.PostOneOffs;
using Tenda.Shared.BaseModels;
using Tenda.Shared.Extensions;
using Tenda.Shared.Hubs;
using Tenda.Shared.Services;

namespace Tenda.Shared;

public class TotalPostProcessor<TRequest, TResponse> : IPostProcessor<TRequest, TResponse>
{
    public async Task PostProcessAsync(TRequest req, TResponse res, HttpContext ctx,
        IReadOnlyCollection<ValidationFailure> failures, CancellationToken ct)
    {
        var totalService = ctx.RequestServices.GetRequiredService<ITotalService>();
        var hubContext = ctx.RequestServices.GetRequiredService<IHubContext<ResolvedTotal>>();
        var cacheService = ctx.RequestServices.GetRequiredService<IMemoryCache>();
        if (req is FinancialTransactionRequestBase @base)
        {
            var result = cacheService.TryGetValue(@base.UserId.ToTagKey(), out List<string> cachedTags);
            var existingTags = result ? cachedTags : new List<string>();
            var newTags = GetTagsFromRequest(req);
            existingTags.AddRange(newTags);
            cacheService.Set(@base.UserId.ToTagKey(), existingTags.ToList(), TimeSpan.FromMinutes(10));
            var seed = await totalService.CalculateTotal(@base.UserId, @base.SeedId, ct);
            var boo = hubContext.Clients.User(@base.UserId);
            await hubContext.Clients.User(@base.UserId).SendAsync("ResolvedTotal", seed, ct);
        }
    }

    private IEnumerable<string> GetTagsFromRequest(TRequest request)
    {
        if (request is not FinancialTransactionRequestBase @financialTransactionRequestBase)
            return Enumerable.Empty<string>();
        if (request is PostOneOffsRequest @manyOneOffsRequest)
        {
            return @manyOneOffsRequest.OneOffs.SelectMany(x => x.Tags);
        }

        return @financialTransactionRequestBase.Tags;
    }
}