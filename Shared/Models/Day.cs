namespace Tenda.Shared.Models;

public class Day
{
    public DateTime Date { get; set; }
    public List<FinancialTransaction>? OneOffs { get; set; }
    public List<FinancialTransaction>? Bills { get; set; }
    public List<FinancialTransaction>? Incomes { get; set; }
    public decimal RunningTotal { get; set; }
}