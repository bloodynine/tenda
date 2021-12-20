using FastEndpoints;

namespace Tenda;

public class Test : Endpoint<Bah, Bahreturn>
{
    public override void Configure()
    {
       Get("api/test"); 
    }

    public override async Task HandleAsync(Bah req, CancellationToken ct)
    {
        await SendAsync(new Bahreturn(), cancellation: ct);
    }
}



public class Bah
{
    
}

public class Bahreturn
{
    public DateOnly DateOnly { get; set; } = new DateOnly(2020, 1, 1);
}