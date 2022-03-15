using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Users;

namespace SeedScripts;

public class Program
{
    public static void Main(string[] args)
    {
        MainAsync(args).GetAwaiter().GetResult();
    }

    public static async Task MainAsync(string[] args)
    {
        await DB.InitAsync("IntTests", "localhost", 27017);
        await ClearDB();

        await CreateUsers();
        Console.WriteLine("Created Users");
    }

    private static async Task ClearDB()
    {
        await DB.DeleteAsync<User>(x => x.ID != null);
    }

    private static async Task CreateUsers()
    {
        var users = new List<User>()
       {
           new() { UserName = "intAdmin", Password = BCrypt.Net.BCrypt.HashPassword("intAdminOne"), IsAdmin = true },
           new() { UserName = "intUser", Password = BCrypt.Net.BCrypt.HashPassword("intUserOne"), IsAdmin = false },
       };
       await users.SaveAsync();
       foreach (var seed in users.Select(user => new Seed() { UserId = user.ID, Amount = 0 }))
       {
           await seed.SaveAsync();
       }
    }
}