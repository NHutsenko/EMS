using System.Diagnostics.CodeAnalysis;

namespace EMS.Auth.API.Models.RequestModels
{
    [ExcludeFromCodeCoverage]
    public class LoginUserRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is not LoginUserRequest)
            {
                return false;
            }
            LoginUserRequest toCompare = obj as LoginUserRequest;
            return Login == toCompare.Login && Password == toCompare.Password;
        }
    }
}
