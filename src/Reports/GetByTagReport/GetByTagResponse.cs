namespace Tenda.Reports.GetByTagReport;

public class GetByTagResponse
{
    public List<LabelDecimal> ByCount { get; set; } = new();
    public List<LabelDecimal> ByDollar { get; set; } = new();
}