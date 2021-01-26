using System;

namespace EMS.Common.Utils.DateTimeUtil
{
    public class DateTimeUtil: IDateTimeUtil
	{
        protected DateTime _currentDate = DateTime.UtcNow;

        public DateTime GetCurrentDateTime()
        {
            return _currentDate;
        }
	}
}
