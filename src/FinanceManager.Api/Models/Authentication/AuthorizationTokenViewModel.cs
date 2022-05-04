using System;
using System.IdentityModel.Tokens.Jwt;

namespace FinanceManager.Api.Models
{
    public class AuthorizationTokenViewModel
    {
        public AuthorizationTokenViewModel(JwtSecurityToken token)
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token);
            Expiration = token.ValidTo;
        }

        public string Token { get; set; }

        public DateTime Expiration { get; set; }
    }
}
