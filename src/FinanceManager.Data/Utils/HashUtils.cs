using System.Security.Cryptography;
using System.Text;

namespace FinanceManager.Data.Utils;

public static class HashUtils
{
    public static string GetHash(params object[] values)
    {
        var hash = new StringBuilder();
        foreach (var value in values)
        {
            hash.Append(value);
        }

        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(hash.ToString());
        var hashBytes = sha256.ComputeHash(bytes);
        return BitConverter.ToString(hashBytes).Replace("-", "");
    }
}