using System.Diagnostics.CodeAnalysis;
using EMS.Core.API.DAL;
using EMS.Core.API.Tests.Mocks;
using Moq;

namespace EMS.Core.API.Tests
{
    [ExcludeFromCodeCoverage]
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
