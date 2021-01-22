using EMS.Common.Models.BaseModel;

namespace EMS.Common.Logger
{
    public interface IEMSLogger<T>
    {
        void AddLog(RequestResponseObject requestResponseObject);
        void AddErrorLog(RequestResponseObject requestResponseObject);
    }
}
