namespace Tenda.Reports;

public class LabelDecimal
{
    public string Name { get; set; }
    public decimal Value { get; set; }

    public LabelDecimal()
    {
    }

    public LabelDecimal(string name, decimal value)
    {
        Name = name;
        Value = value;
    }
}