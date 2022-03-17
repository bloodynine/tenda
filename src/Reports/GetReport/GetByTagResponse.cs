namespace Tenda.Reports.GetReport;

public class GetByTagResponse
{
    public List<LabelDecimal> ByCount { get; set; } = new();
    public List<LabelDecimal> ByDollar { get; set; } = new();
}

public class LabelDecimal
{
    public string Name { get; set; }
    public decimal Value { get; set; }
}