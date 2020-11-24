using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace EMS.Gateway.API.Models
{
    [ExcludeFromCodeCoverage]
    public class Position: BaseModel
	{
		[Column("team")]
		public Team Team { get; set; }

		[Column("teamId")]
		public long TeamId { get; set; }
        [IgnoreDataMember]
        public ICollection<Staff> Staff { get; set; }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(!(obj is Position))
            {
                return false;
            }
            Position toCompare = obj as Position;

            return Id == toCompare.Id && Name.Equals(toCompare.Name) && TeamId == toCompare.TeamId && CreatedOn == toCompare.CreatedOn;
        }
    }
}
