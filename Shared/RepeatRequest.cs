namespace Tenda.Shared;

public record RepeatRequest
{
    public DateTime StartDate { get; init; }
    public int Interval { get; set; }
    public RepeatType Type { get; init; }

    public DateTime? EndDate { get; init; }
}