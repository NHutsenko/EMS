using System;
using EMS.Common.Utils.DateTimeUtil;

namespace EMS.Core.API.Tests.Mocks
{
    public class DateTimeUtilMock: DateTimeUtil, IDateTimeUtil
    {
        public override DateTime GetCurrentDateTime()
        {
            return new DateTime(2020, 01, 01, 12, 00, 00);
        }
    }
}
