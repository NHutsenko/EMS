using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace EMS.Common.Logger.Models
{
    [ExcludeFromCodeCoverage]
    public class LogData
    {
        public string CallSide { get; set; }
        public string CallerMethodName { get; set; }
        public static string AppName => Assembly.GetCallingAssembly().ManifestModule.Name;
        public DateTime CreatedOn { get; set; }
        public object Request { get; set; }
        public object Response { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is not LogData)
            {
                return false;
            }
            LogData toCompare = obj as LogData;

            if (Response is Exception && toCompare.Response is Exception)
            {
                string exceptionMessage = (Response as Exception).Message;
                string toCompareExceptionMessage = (toCompare.Response as Exception).Message;

                return CallSide == toCompare.CallSide &&
                    CallerMethodName == toCompare.CallerMethodName &&
                    CreatedOn == toCompare.CreatedOn &&
                    ((Request == null && toCompare.Request == null) || Request.Equals(toCompare.Request)) &&
                    exceptionMessage == toCompareExceptionMessage;
            }

            return CallSide == toCompare.CallSide &&
                CallerMethodName == toCompare.CallerMethodName &&
                CreatedOn == toCompare.CreatedOn &&
                ((Request == null && toCompare.Request == null) || Request.Equals(toCompare.Request)) &&
                ((Response == null && toCompare.Response == null) || Response.Equals(toCompare.Response));
        }
    }
}
