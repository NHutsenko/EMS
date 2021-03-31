using System;
using System.Diagnostics.CodeAnalysis;

namespace EMS.Auth.API.Models.ResponseModels
{
    [ExcludeFromCodeCoverage]
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresIn { get; set; } 
    }
}
