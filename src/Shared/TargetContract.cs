using MongoDB.Bson;

namespace Tenda.Shared;

public class TargetContract
{
    public DateOnly Date { get; set; }
    public ObjectId? Id { get; set; }
}