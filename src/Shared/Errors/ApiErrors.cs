namespace Tenda.Shared.Errors;

public static class ApiErrors
{
    public static bool Filter(Exception e)
    {
        return e is ForbiddenException or BadHttpRequestException or NotFoundException;
    }

    public static async Task HandleApiErrorsAsync(this BaseEndpoint endpoint, Exception e, CancellationToken ct = default)
    {
        switch (e)
        {
            case ForbiddenException:
                await endpoint.HttpContext.Response.SendForbiddenAsync(ct);
                break;
            case NotFoundException:
                await endpoint.HttpContext.Response.SendNotFoundAsync(ct);
                break;
            case BadHttpRequestException:
                await endpoint.HttpContext.Response.SendAsync<string?>(null, 400, cancellation:ct);
                break;
            default:
                throw e;
        }
    }
}