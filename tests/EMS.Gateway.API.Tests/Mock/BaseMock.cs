using System;
using System.Diagnostics.CodeAnalysis;

namespace EMS.Gateway.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
	public class BaseMock
	{
        public static bool ShouldThrowException { get; set; }
        public static object Response { get; set; }
        public static string ExceptionMessage => "Test exception";

        protected static void ThrowExceptionIfNeeded()
        {
            if (ShouldThrowException)
            {
                throw new Exception(ExceptionMessage);
            }
        }
	}
}
