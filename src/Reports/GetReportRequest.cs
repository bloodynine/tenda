using Tenda.Shared.BaseModels;

namespace Tenda.Reports;

public class GetReportRequest : RequestBase
{
    public ReportTypes Type { get; set; }
    public NullableDateRange DateRange { get; set; } = new();
}

public class GetReportRequestValidator : Validator<GetReportRequest>
{
    public GetReportRequestValidator()
    {
        RuleFor(x => x.DateRange).SetValidator(new NullableDateRangeValidator());
    }
}


public enum ReportTypes
{
    Tag = 1
}

public class NullableDateRange
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public NullableDateRange()
    {
    }

    public NullableDateRange(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }
}

public class NullableDateRangeValidator : Validator<NullableDateRange>
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