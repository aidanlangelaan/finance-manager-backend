using System;
using System.IdentityModel.Tokens.Jwt;

namespace FinanceManager.Api.Models
{
    public class AuthorizationTokenViewModel
    {
        public AuthorizationTokenViewModel(JwtSecurityToken token)
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
