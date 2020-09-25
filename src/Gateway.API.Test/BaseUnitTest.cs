using EMS.Gateway.API.DAL;
using Gateway.API.Test.Mocks;
using Moq;

namespace Gateway.API.Test
{
	public class BaseUnitTest
	{
		protected Mock<IApplicationDbContext> _dbContextMock;
		protected IApplicationDbContext _dbContext;

		protected void InitializeMocks()
		{
			_dbContextMock = DbContextMock.SetupDbContext<IApplicationDbContext>();
			_dbContext = _dbContextMock.Object;
		}
	}
}
