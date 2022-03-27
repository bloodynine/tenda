namespace Tenda.Reports;

public class GroupedItem
{
    public string Name { get; set; }
    public IEnumerable<LabelDecimal> Series { get; set; }
}
