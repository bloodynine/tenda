namespace Tenda.Shared.Extensions;

public static class StringExtensions
{
    public static string ToTagKey(this string userId) => $"{userId}-Tag";
}