using EMS.Gateway.API.DAL;
using Gateway.API.Test.Mocks;
using Moq;

namespace Gateway.API.Test
{
	public class BaseUnitTest
	{
		protected Mock<ApplicationDbContext> _dbContextMock;
		protected ApplicationDbContext _dbContext;

		protected void InitializeMocks()
		{
			_dbContextMock = DbContextMock.SetupDbContext<ApplicationDbContext>();
			_dbContext = _dbContextMock.Object;
		}
	}
}
