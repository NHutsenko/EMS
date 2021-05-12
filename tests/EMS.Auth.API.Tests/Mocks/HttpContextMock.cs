using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Moq;

namespace EMS.Auth.API.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class HttpContextMock
    {
        public static Mock<HttpContext> SetupHttpContextMock()
        {

            Mock<HttpContext> httpContext = new();
            Mock<HttpResponse> response = new();
            Mock<HttpRequest> request = new();

               
            response.Setup(m => m.Headers).Returns(new HeaderDictionary());
            httpContext.Setup(m => m.Request).Returns(request.Object);
            httpContext.Setup(m => m.Response).Returns(response.Object);
            httpContext.Setup(m => m.Items).Returns(new Dictionary<object, object>());

            return httpContext;
        }
    }
}
