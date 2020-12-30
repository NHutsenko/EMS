using System;

namespace EMS.Common.Utils.DateTimeUtil
{
    public class DateTimeUtil: IDateTimeUtil
	{
        protected DateTime _currentDate = DateTime.Now;

        public DateTime GetCurrentDateTime()
        {
            return _currentDate;
        }
	}
}
