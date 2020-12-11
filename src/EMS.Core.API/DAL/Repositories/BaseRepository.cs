namespace EMS.Core.API.DAL.Repositories
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
