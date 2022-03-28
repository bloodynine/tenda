using Tenda.Shared.BaseModels;

namespace Tenda.Reports.GetByMonthReport;

public class GetByMonthRequest : RequestBase
{
    public int Year { get; init; }
    public DateTime GetStartDate() => new DateTime(this.Year, 1, 1);
    public DateTime GetEndDate() => new DateTime(Year, 1, 1).AddYears(1).AddTicks(-1);

}
