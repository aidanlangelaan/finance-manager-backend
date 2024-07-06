using System;
using System.Security.Cryptography;
using System.Text;

namespace FinanceManager.Business.Utils;

public static class SecurityUtils
{
    private const int keySize = 64;
    private const int iterations = 350000;
    private static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
    
    public static string HashValue(string value, string salt)
    {
        var saltBytes = Encoding.UTF8.GetBytes(salt);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(value),
            saltBytes,
            iterations,
            hashAlgorithm,
            keySize);
        return Convert.ToHexString(hash);
    }
}