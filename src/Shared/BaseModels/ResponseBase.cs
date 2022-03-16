using Tenda.Shared.Models;

namespace Tenda.Shared.BaseModels;

public class ResponseBase
{
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;

    public ResponseBase()
    {
    }

    public ResponseBase(FinancialTransaction transaction)
    {
        Id = transaction.ID;
        UserId = transaction.UserId;
    }
}