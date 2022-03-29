using Tenda.Shared.BaseModels;

namespace Tenda.Shared.Models;

public class TransactionCollection
{
    public TransactionCollection(List<FinancialTransaction> transactions)
    {
        Transactions = transactions;
    }

    private List<FinancialTransaction> Transactions { get; }

    public List<FinancialTransaction> GetByDayAndType(DateOnly day, TransactionType type)
    {
        return Transactions.Where(x => x.Date == day && x.Type == type)
            .ToList();
    }

    public decimal GetRunningTotal(DateOnly day)
    {
        return Transactions.Where(x => x.Date <= day).Select(x => x.Amount).Sum();
    }
}