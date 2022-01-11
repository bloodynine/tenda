using MongoDB.Entities;
using Tenda.Repeats.UpdateRepeatContract;
using Tenda.Shared.Models;

namespace Tenda.Shared;

public class RepeatContracts : Entity
{
    private static DateTime EndDate => DateTime.Parse("2050-01-01");
    public string UserId { get; set; }
    public TransactionType Type { get; set; }
    public RepeatType RepeatType { get; set; }
    public int Interval { get; set; }
    public DateTime StartDate { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public IEnumerable<string> Tags { get; set; }

    public RepeatContracts(RepeatRequest request, FinancialTransactionRequestBase transaction, TransactionType type)
    {
        StartDate = request.StartDate;
        Interval = request.Interval;
        RepeatType = request.Type;
        Name = transaction.Name;
        Amount = transaction.Amount;
        UserId = transaction.UserId;
        Type = type;
        Tags = transaction.Tags;
    }

    public RepeatContracts(UpdateRepeatContractRequest request)
    {
        StartDate = request.StartDate;
        Interval = request.Interval;
        RepeatType = request.RepeatType;
        Name = request.Name;
        Amount = request.Amount;
        UserId = request.UserId;
        ID = request.ContractId;
        Tags = request.Tags;
    }

    public IEnumerable<FinancialTransaction> CalculateTargets()
    {
        var newBills = RepeatType switch
        {
            RepeatType.ByMonth => ByMonth(),
            RepeatType.ByWeek => ByWeek(),
            RepeatType.ByDay => ByDay(),
            _ => new List<FinancialTransaction>()
        };
        return newBills;
    }

    private IEnumerable<FinancialTransaction> ByDay()
    {
        for (var day = StartDate; day <= EndDate; day = day.AddDays(Interval))
            yield return new FinancialTransaction(this, day, ID);
    }

    private IEnumerable<FinancialTransaction> ByWeek()
    {
        for (var day = StartDate; day <= EndDate; day = day.AddDays(7 * Interval))
            yield return new FinancialTransaction(this, day, ID);
    }

    private IEnumerable<FinancialTransaction> ByMonth()
    {
        for (var day = StartDate; day <= EndDate; day = day.AddMonths(Interval))
            yield return new FinancialTransaction(this, day, ID);
    }
}