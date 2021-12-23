namespace Tenda.Shared.Models;

public class DatabaseSettings
{
    public string Host { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public int  Port { get; set; }
}