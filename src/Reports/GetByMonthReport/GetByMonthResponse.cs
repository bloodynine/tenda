namespace Tenda.Reports.GetByMonthReport;

public class GetByMonthResponse
{
    public decimal AverageIncome { get; set; } = 0;
    public decimal AverageExpenses { get; set; } = 0;
    public IEnumerable<GroupedItem> IncomeExpensesByMonth { get; set; } = Enumerable.Empty<GroupedItem>();
}