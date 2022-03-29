using Tenda.Shared.BaseModels;
using Tenda.Shared.Models;

namespace Tenda.Reports;

public class GetReportRequest : DateRangeRequest
{
    public ReportTypes Type { get; set; }
    public IEnumerable<TransactionType>? Types { get; set; }
}

public class GetReportRequestValidator : Validator<GetReportRequest>
{
    public GetReportRequestValidator()
    {
        Include(new NullableDateRangeValidator());
    }
}

public enum ReportTypes
{
    Tag = 1
}

public class DateRangeRequest : RequestBase
{
    /// These values "should" be nullable. But there is an issue in Fast endpoints where it wont serialize a nullable
    /// query param
    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public DateRangeRequest()
    {
    }

}

public class NullableDateRangeValidator : Validator<DateRangeRequest>
{
    public NullableDateRangeValidator()
    {
        RuleFor(x => x.StartDate)
            .NotNull()
            .When(x => x.EndDate is not null);
        RuleFor(x => x.EndDate)
            .NotNull()
            .When(x => x.StartDate is not null);
        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .When(x => x.StartDate is not null && x.EndDate is not null);
    }
}