using MongoDB.Entities;

namespace Tenda.Shared.Models;

public class Seed : Entity
{
    public string UserId { get; set; } = "";
    public decimal Amount { get; set; } = 0;
}