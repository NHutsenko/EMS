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

    }
}
