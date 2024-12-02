using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DirectorySite.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace DirectorySite.Data
{
    [AttributeUsage(AttributeTargets.All)]
    public class AdminAttribute : Attribute, IAuthorizationFilter
    {  
        
        private readonly JwtSettings jwtSettings;

        public AdminAttribute()
        {
            jwtSettings = StaticSettings
                .GetConfiguration()
                .GetSection("JwtSettings")
                .Get<JwtSettings>()!;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
                
            // Validate session
            var token = context.HttpContext.Session.GetString("JWTToken");
            if(token == null){
                context.HttpContext.Response.Redirect("/authentication");
                return;
            }

            // Validate role claim admin
            try
            {
                var userPrincipal = TokenValidator.ValidateToken(token, jwtSettings);
                if (!userPrincipal.IsInRole("Admin"))
                {
                    context.HttpContext.Response.Redirect("/forbid");
                    return;
                }

                context.HttpContext.User = userPrincipal;
            }
            catch (Exception)
            {
                context.HttpContext.Response.Redirect("/authentication");
            }

        }
    }
}