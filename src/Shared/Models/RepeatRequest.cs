namespace Tenda.Shared.Models;

public record RepeatRequest
{
    public DateOnly StartDate { get; init; }
    public int Interval { get; set; }
    public RepeatType Type { get; init; }

    public DateOnly? EndDate { get; init; }
}