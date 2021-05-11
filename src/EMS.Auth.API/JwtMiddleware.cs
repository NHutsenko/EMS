using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EMS.Auth.API.Enums;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace EMS.Auth.API
{
    [ExcludeFromCodeCoverage]
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

#pragma warning disable IDE1006 // Naming Styles
        public async Task Invoke(HttpContext context, IUsersRepository usersRepository)
#pragma warning restore IDE1006 // Naming Styles
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (context.Request.Path.HasValue && context.Request.Path.Value.ToLower() == "/api/auth")
            {
                await _next(context);
                return;
            }
            if (!IsUserExistsByToken(token, usersRepository))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
            AttachUserToContext(context, token);
            await _next(context);
        }

        private bool IsUserExistsByToken(string token, IUsersRepository usersRepository)
        {
            JwtSecurityToken jwtToken = ParseTokenData(token);
            string userLogin = jwtToken.Claims.First(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            return usersRepository.GetByLogin(userLogin) != null;
        }

        private void AttachUserToContext(HttpContext httpContext, string token)
        {
            JwtSecurityToken jwtToken = ParseTokenData(token);
            string userLogin = jwtToken.Claims.First(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value;
            httpContext.Items["User"] = userLogin;
        }

        private static JwtSecurityToken ParseTokenData(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(TokenType.Access),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken;
        }
    }
}
