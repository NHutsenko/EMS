using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using EMS.Core.API.Models;

namespace EMS.Core.API.Models
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is not Staff)
            {
                return false;
            }
            Staff toCompare = obj as Staff;

            return Id.Equals(toCompare.Id) &&
                PersonId.Equals(toCompare.PersonId) &&
                ManagerId.Equals(toCompare.ManagerId) &&
                PositionId.Equals(toCompare.PositionId) &&
                CreatedOn.Equals(toCompare.CreatedOn);
        }
    }
}
