using EMS.Gateway.API.DAL;

namespace EMS.Gateway.API.Repositories
{
	public class BaseRepository
	{
		protected readonly ApplicationDbContext _context;
		public BaseRepository(ApplicationDbContext context)
		{
			_context = context;
		}
	}
}
