
using Microsoft.IdentityModel.Tokens;

namespace RickYMorty.token
{
    public class ValidateToken
    {
        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {

            var tokenKey = configuration["AppSettings:Token"];
            if (string.IsNullOrEmpty(tokenKey))
                throw new Exception("JWT token key is not configured in AppSettings:Token");

            return new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(tokenKey)
                ),
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
