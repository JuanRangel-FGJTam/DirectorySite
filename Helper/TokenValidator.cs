using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace DirectorySite.Helper
{
    public class TokenValidator
    {
        public static ClaimsPrincipal ValidateToken(string token, JwtSettings jwtSettings)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes( jwtSettings.Key )
                    ),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };

                // Validate token
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // Check expiration
                if (validatedToken.ValidTo < DateTime.UtcNow)
                {
                    throw new SecurityTokenExpiredException("Token has expired.");
                }

                return principal;
            }
            catch (SecurityTokenException ex)
            {
                // Token validation failed
                throw new SecurityTokenValidationException("Token validation failed.", ex);
            }
        }
    }
}