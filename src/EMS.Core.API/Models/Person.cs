using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace EMS.Core.API.Models
{
    [ExcludeFromCodeCoverage]
    public class Person: BaseModel
    {
        [IgnoreDataMember]
        public ICollection<Staff> Staff { get; set; }
    }
}
