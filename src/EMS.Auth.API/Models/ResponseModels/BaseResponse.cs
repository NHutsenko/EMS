using System.Diagnostics.CodeAnalysis;

namespace EMS.Auth.API.Models.ResponseModels
{
    [ExcludeFromCodeCoverage]
    public class BaseResponse
    {
        public bool IsSucess { get; set; }
        public string ErrorMessage { get; set; }
        public long Id { get; set; }
    }
}
