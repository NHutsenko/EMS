using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EMS.Gateway.API.Models
{
    public class Person: BaseModel
    {
        [IgnoreDataMember]
        public ICollection<Staff> Staff { get; set; }
    }
}
