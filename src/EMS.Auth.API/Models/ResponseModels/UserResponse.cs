using System.Diagnostics.CodeAnalysis;

namespace EMS.Auth.API.Models.ResponseModels
{
    [ExcludeFromCodeCoverage]
    public class UserResponse: BaseResponse
    {
        public User User { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is not UserResponse)
            {
                return false;
            }
            UserResponse toCompare = obj as UserResponse;
            return Id == toCompare.Id
                && IsSucess == toCompare.IsSucess
                && ErrorMessage == toCompare.ErrorMessage
                && ((User == null && toCompare.User == null) || User.Equals(toCompare.User));
        }
    }
}
