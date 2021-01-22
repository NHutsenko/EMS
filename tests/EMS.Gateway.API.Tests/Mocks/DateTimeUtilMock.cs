using System;
using System.Diagnostics.CodeAnalysis;
using EMS.Common.Utils.DateTimeUtil;

namespace EMS.Core.API.Tests.Mock
{
    [ExcludeFromCodeCoverage]
    public class DateTimeUtilMock: DateTimeUtil, IDateTimeUtil
    {
        public DateTimeUtilMock()
        {
            _currentDate = new DateTime(2020, 01, 01, 12, 00, 00);
        }
    }
}
