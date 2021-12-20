using Tenda.Shared.Models;

namespace Tenda.Shared.BaseModels;

public class ResponseBase
{
    public ResponseBase()
    {
    }

    public ResponseBase(FinancialTransaction transaction)
    {
        Id = transaction.ID;
        UserId = transaction.UserId;
    }

    public string Id { get; set; }
    public string UserId { get; set; }
}