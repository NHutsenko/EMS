using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace EMS.Gateway.API.Models
{
	public class Team: BaseModel
	{
		[Column("description")]
		public string Description { get; set; }

		[IgnoreDataMember]
		public ICollection<Position> Positions { get; set; }
	}
}
