using System;
using System.Diagnostics.CodeAnalysis;

namespace EMS.Common.Models.BaseModel
{
    [ExcludeFromCodeCoverage]
    public class RequestResponseObject
    {
        public DateTime CreatedOn { get; set; }
        public object Request { get; set; }
        public object Response { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is not RequestResponseObject)
            {
                return false;
            }
            RequestResponseObject toCompare = obj as RequestResponseObject;

            if (Response is Exception && toCompare.Response is Exception)
            {
                string exceptionMessage = (Response as Exception).Message;
                string toCompareExceptionMessage = (toCompare.Response as Exception).Message;

                return CreatedOn == toCompare.CreatedOn &&
                ((Request == null && toCompare.Request == null) || Request.Equals(toCompare.Request)) &&
                exceptionMessage == toCompareExceptionMessage;
            }

            return CreatedOn == toCompare.CreatedOn &&
                ((Request == null && toCompare.Request == null) || Request.Equals(toCompare.Request)) &&
                ((Response == null && toCompare.Response == null) || Response.Equals(toCompare.Response));
        }
    }
}
