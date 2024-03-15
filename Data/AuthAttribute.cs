using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DirectorySite.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace DirectorySite.Data
{
    [AttributeUsage(AttributeTargets.All)]
    public class AuthAttribute : Attribute, IAuthorizationFilter 
    {  
        
        private readonly JwtSettings jwtSettings;

        public AuthAttribute()
        {
            jwtSettings = StaticSettings
                .GetConfiguration()
                .GetSection("JwtSettings")
                .Get<JwtSettings>()!;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(context != null){
                
                // Validate session
                var token = context.HttpContext.Session.GetString("JWTToken");
                if(token == null){
                    context.HttpContext.Response.Redirect("/authentication");
                    return;
                }

                // Validate token
                try {
                    context.HttpContext.User = TokenValidator.ValidateToken( token, jwtSettings );
                }
                catch (Exception ) {
                    context.HttpContext.Response.Redirect("/authentication");
                    return;
                }
            }
        }
    }  
}