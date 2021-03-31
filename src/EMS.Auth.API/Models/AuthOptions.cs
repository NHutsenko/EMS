using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using EMS.Auth.API.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EMS.Auth.API.Models
{
    [ExcludeFromCodeCoverage]
    public class AuthOptions
    {
        public static string Issuer => "EMS.Auth.API";
        public static string Audience => "EMS.Gateway.API";
        private static string Key
        {
            get
            {
                string key = Environment.GetEnvironmentVariable("AuthKey");
                if (string.IsNullOrEmpty(key))
                {
                    IConfiguration configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();
                    key = configuration["Settings:AuthKey"];
                }
                return key;
            }
        }
        public static int LifeTime
        {
            get
            {
                string lifeTimeVal = Environment.GetEnvironmentVariable("TokenLifeTime");
                if (string.IsNullOrEmpty(lifeTimeVal))
                {
                    IConfiguration configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();
                    lifeTimeVal = configuration["Settings:TokenLifeTime"];
                }
                return Convert.ToInt32(lifeTimeVal);
            }
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey(TokenType tokenType)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key + tokenType.ToString()));
        }
    }
}
