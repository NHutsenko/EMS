using EMS.Common.Logger.Models;

namespace EMS.Common.Logger
{
    public interface IEMSLogger<T>
    {
        void AddLog(LogData requestResponseObject);
        void AddErrorLog(LogData requestResponseObject);
    }
}
