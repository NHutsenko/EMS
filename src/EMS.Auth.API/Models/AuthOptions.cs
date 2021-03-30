using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EMS.Auth.API.Models
{
    [ExcludeFromCodeCoverage]
    public class AuthOptions
    {
        public static string Issuer => "EMS.Auth.API";
        public static string Audience => "EMS.Gateway.API";
        private static string Key => Environment.GetEnvironmentVariable("AuthKey");
        public static int LifeTime => Convert.ToInt32(Environment.GetEnvironmentVariable("TokenLifeTime"));

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
