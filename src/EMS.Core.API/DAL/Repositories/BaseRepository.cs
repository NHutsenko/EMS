using EMS.Common.Models.BaseModel;
using EMS.Common.Utils.DateTimeUtil;

namespace EMS.Core.API.DAL.Repositories
{
    public class BaseRepository
	{
		protected readonly IApplicationDbContext _context;
        protected readonly IDateTimeUtil _dateTimeUtil;
		public BaseRepository(IApplicationDbContext context, IDateTimeUtil dateTimeUtil)
		{
			_context = context;
            _dateTimeUtil = dateTimeUtil;
		}
    }
}
