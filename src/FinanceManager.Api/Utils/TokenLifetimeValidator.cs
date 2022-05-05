using Microsoft.IdentityModel.Tokens;
using System;

namespace FinanceManager.Api.Utils
{
    public static class TokenLifetimeValidator
    {
        public static bool Validate(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param) =>
            expires != null && expires > DateTime.UtcNow;
    }
}
