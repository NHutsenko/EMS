using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

namespace EMS.Gateway.API.Models
{
    [ExcludeFromCodeCoverage]
    public class Staff: BaseModel
    {
        [Column("personId")]
        public long? PersonId { get; set; }
        [Column("person")]
        public Person Person { get; set; }
        [Column("managerId")]
        public long ManagerId { get; set; }
        [Column("manager")]
        public Person Manager { get; set; }
        [Column("positionId")]
        public long PositionId { get; set; }
        [Column("position")]
        public Position Position { get; set; }
        [IgnoreDataMember]
        public IQueryable<DayOff> DayOff { get; set; }
    }
}
