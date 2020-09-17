using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Gateway.API.Models
{
	public class Position: BaseModel
	{
		[Column("team")]
		public Team Team { get; set; }

		[Column("teamId")]
		public long TeamId { get; set; }
	}
}
