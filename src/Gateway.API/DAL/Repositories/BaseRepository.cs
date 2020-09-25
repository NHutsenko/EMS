using EMS.Gateway.API.DAL;

namespace EMS.Gateway.API.Repositories
{
	public class BaseRepository
	{
		protected readonly IApplicationDbContext _context;
		public BaseRepository(IApplicationDbContext context)
		{
			_context = context;
		}
	}
}
