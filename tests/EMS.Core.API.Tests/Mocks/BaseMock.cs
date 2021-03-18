using System;
using System.Diagnostics.CodeAnalysis;

namespace EMS.Core.API.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class BaseMock
    {
        public static bool ShouldThrowException { get; set; }


        protected static void ThrowExceptionIfNeeded()
        {
            if (ShouldThrowException)
            {
                throw new Exception("Test exception");
            }
        }
    }
}
