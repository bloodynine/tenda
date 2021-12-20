using FastEndpoints;

namespace Tenda.All;

public class GetAllRequest
{
    public int Year { get; set; }
    public int Month { get; set; }

    [FromClaim("UserId")]
    public string UserId { get; set; }
}