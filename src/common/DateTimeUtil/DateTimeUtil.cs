using System;

namespace EMS.Common.Utils.DateTimeUtil
{
    public class DateTimeUtil: IDateTimeUtil
	{
        public virtual DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
	}
}
