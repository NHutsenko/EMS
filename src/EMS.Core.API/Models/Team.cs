using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using EMS.Common.Models.BaseModel;

namespace EMS.Core.API.Models
{
    [ExcludeFromCodeCoverage]
	public class Team: BaseModel
	{
		[Column("description")]
		public string Description { get; set; }

		[IgnoreDataMember]
		public ICollection<Position> Positions { get; set; }
    }
}
