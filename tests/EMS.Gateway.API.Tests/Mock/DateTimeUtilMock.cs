using System;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Utils.DateTimeUtil;

namespace EMS.Gateway.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class DateTimeUtilMock : DateTimeUtil, IDateTimeUtil
    {
        public DateTimeUtilMock()
        {
            _currentDate = new DateTime(2021, 01, 01, 12, 00, 00, DateTimeKind.Utc);
        }
    }
}
