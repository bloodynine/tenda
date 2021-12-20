using Tenda.Shared.BaseModels;

namespace Tenda.Shared.Models;

public class TransactionCollection
{
    public TransactionCollection(List<FinancialTransaction> transactions)
    {
        Transactions = transactions;
    }

    private List<FinancialTransaction> Transactions { get; }

    public List<FinancialTransaction> GetByDayAndType(DateTime day, TransactionType type)
    {
        return Transactions.Where(x => x.Date >= day.Date && x.Date <= day.AddDays(1).AddTicks(-1) && x.Type == type)
            .ToList();
    }

    public decimal GetRunningTotal(DateTime day)
    {
        return Transactions.Where(x => x.Date <= day.AddDays(1).AddTicks(-1)).Select(x => x.Amount).Sum();
    }
}