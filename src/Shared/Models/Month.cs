using Tenda.Shared.BaseModels;

namespace Tenda.Shared.Models;

public class Month
{
    public Month(DateOnly startDate, DateOnly endDate, decimal resolvedTotal, TransactionCollection transactions)
    {
        ResolvedTotal = resolvedTotal;
        for (var day = startDate; day <= endDate; day = day.AddDays(1))
        {
            var runningTotal = resolvedTotal + transactions.GetRunningTotal(day);
            Days.Add(
                new Day
                {
                    Date = day,
                    Incomes = transactions.GetByDayAndType(day, TransactionType.Income),
                    Bills = transactions.GetByDayAndType(day, TransactionType.Bill),
                    OneOffs = transactions.GetByDayAndType(day, TransactionType.OneOff),
                    RunningTotal = runningTotal
                });
        }
    }

    public Month()
    {
    }

    public decimal ResolvedTotal { get; set; }
    public List<Day> Days { get; set; } = new();
}