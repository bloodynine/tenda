using MongoDB.Entities;

namespace Tenda.Shared.BaseModels;

public class Item : Entity
{
    public DateTime Date { get; set; }
    public Decimal Amount { get; set; }
}