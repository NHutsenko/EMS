using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EMS.Common.Utils.Tests
{
    [ExcludeFromCodeCoverage]
    public class LocalDateTimeUtil: DateTimeUtil.DateTimeUtil
    {
        public LocalDateTimeUtil(DateTime current)
        {
            _currentDate = current;
        }
    }

    [ExcludeFromCodeCoverage]
    public class DateTimeUtilsTets
	{
        private LocalDateTimeUtil _dateTimeUtil;
        private DateTime _expected;
        [SetUp]
		public void Setup()
		{
            _expected = DateTime.Now;
            _dateTimeUtil = new LocalDateTimeUtil(_expected);
		}

		[Test]
		public void GetCurrentDateTime_should_return_current_datetime()
		{
            // Act
            DateTime actual = _dateTimeUtil.GetCurrentDateTime();

            // Assert
            Assert.AreEqual(_expected, actual, "Returned current date time as expected");
		}
	}
}