using Tenda.Shared.BaseModels;

namespace Tenda.Reports.GetByMonthReport;

public class GetByMonthRequest : RequestBase
{
    public int Year { get; init; }
    public DateOnly GetStartDate() => new DateOnly(this.Year, 1, 1);
    public DateOnly GetEndDate() => new DateOnly(Year, 1, 1).AddYears(1);

}
