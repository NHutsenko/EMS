using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace EMS.Gateway.API.Models
{
    [ExcludeFromCodeCoverage]
    public class Person: BaseModel
    {
        [IgnoreDataMember]
        public ICollection<Staff> Staff { get; set; }
    }
}
