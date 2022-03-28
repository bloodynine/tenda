using Tenda.Shared.Models;

namespace Tenda.Shared.Extensions;

public static class TransactionTypeExtensions
{
    public static IEnumerable<TransactionType> ExpenseTransactions =>
        new[] { TransactionType.OneOff, TransactionType.Bill };
}