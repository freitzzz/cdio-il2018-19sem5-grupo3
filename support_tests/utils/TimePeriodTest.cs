using System;
using NodaTime;
using support.utils;
using Xunit;

namespace support_tests.utils
{
    /// <summary>
    /// Unit testing class for TimePeriod
    /// </summary>
    public class TimePeriodTest
    {
        [Fact]
        public void ensureFiniteTimePeriodCantBeCreatedWithStartingDateAndEndingDateBeingEqual()
        {
            LocalDateTime localDateTime = new LocalDateTime(2000, 10, 10, 10, 10);

            Action act = () => TimePeriod.valueOf(localDateTime, localDateTime);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureFiniteTimePeriodCantBeCreatedWithStartingDateBeingLaterThanEndingDate()
        {
            LocalDateTime startingDate = new LocalDateTime(2000, 10, 10, 10, 10);
            LocalDateTime endingDate = new LocalDateTime(1999, 12, 31, 23, 59);

            Action act = () => TimePeriod.valueOf(startingDate, endingDate);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureFiniteTimePeriodCanBeCreated()
        {
            LocalDateTime startingDate = new LocalDateTime(1999, 12, 31, 23, 59);
            LocalDateTime endingDate = new LocalDateTime(3000, 12, 12, 12, 12);

            Assert.NotNull(TimePeriod.valueOf(startingDate, endingDate));
        }

        [Fact]
        public void ensureInfiniteTimePeriodCanBeCreated()
        {
            LocalDateTime startingDate = new LocalDateTime(1999, 12, 31, 23, 59);

            Assert.NotNull(TimePeriod.valueOf(startingDate));
        }

        [Fact]
        public void ensureTimePeriodsWithSameStartingAndEndingDatesHaveEqualHashCodes()
        {
            LocalDateTime startingDate = new LocalDateTime(1999, 12, 31, 23, 59);
            LocalDateTime endingDate = new LocalDateTime(3000, 12, 12, 12, 12);

            TimePeriod instance = TimePeriod.valueOf(startingDate, endingDate);
            TimePeriod other = TimePeriod.valueOf(startingDate, endingDate);

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTimePeriodsWithDifferentStartingDatesHaveDifferentHashCodes()
        {
            LocalDateTime startingDate = new LocalDateTime(1999, 12, 31, 23, 59);
            LocalDateTime otherStartingDate = new LocalDateTime(1999, 1, 1, 1, 1);
            LocalDateTime endingDate = new LocalDateTime(3000, 12, 12, 12, 12);

            TimePeriod instance = TimePeriod.valueOf(startingDate, endingDate);
            TimePeriod other = TimePeriod.valueOf(otherStartingDate, endingDate);

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTimePeriodsWithDifferentEndingDatesHaveDifferentHashCodes()
        {
            LocalDateTime startingDate = new LocalDateTime(1999, 12, 31, 23, 29);
            LocalDateTime endingDate = new LocalDateTime(3000, 12, 12, 12, 12);
            LocalDateTime otherEndingDate = new LocalDateTime(2999, 12, 12, 12, 12);

            TimePeriod instance = TimePeriod.valueOf(startingDate, endingDate);
            TimePeriod other = TimePeriod.valueOf(startingDate, otherEndingDate);

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTimePeriodsWithDifferentStartingAndEndingDatesHaveDifferentHashCodes()
        {
            LocalDateTime startingDate = new LocalDateTime(1999, 12, 31, 23, 29);
            LocalDateTime otherStartingDate = new LocalDateTime(2000, 1, 1, 1, 1);
            LocalDateTime endingDate = new LocalDateTime(3000, 12, 12, 12, 12);
            LocalDateTime otherEndingDate = new LocalDateTime(2999, 12, 12, 12, 12);

            TimePeriod instance = TimePeriod.valueOf(startingDate, endingDate);
            TimePeriod other = TimePeriod.valueOf(otherStartingDate, otherEndingDate);

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTimePeriodIsEqualToItself()
        {
            LocalDateTime startingDate = new LocalDateTime(1999, 12, 31, 23, 29);
            LocalDateTime endingDate = new LocalDateTime(3000, 12, 12, 12, 12);

            TimePeriod instance = TimePeriod.valueOf(startingDate, endingDate);

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void ensureTimePeriodIsntEqualToNull()
        {
            LocalDateTime startingDate = new LocalDateTime(1999, 12, 31, 23, 29);
            LocalDateTime endingDate = new LocalDateTime(3000, 12, 12, 12, 12);

            TimePeriod instance = TimePeriod.valueOf(startingDate, endingDate);

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureTimePeriodIsntEqualToDifferentTypeInstance()
        {
            LocalDateTime startingDate = new LocalDateTime(1999, 12, 31, 23, 29);
            LocalDateTime endingDate = new LocalDateTime(3000, 12, 12, 12, 12);

            TimePeriod instance = TimePeriod.valueOf(startingDate, endingDate);

            Assert.False(instance.Equals("bananas"));
        }

        [Fact]
        public void ensureTimePeriodsArentEqualIfTheyHaveDifferentStartingDates()
        {
            LocalDateTime startingDate = new LocalDateTime(1999, 12, 31, 23, 29);
            LocalDateTime otherStartingDate = new LocalDateTime(2000, 12, 12, 23, 59);
            LocalDateTime endingDate = new LocalDateTime(3000, 12, 12, 12, 12);

            TimePeriod instance = TimePeriod.valueOf(startingDate, endingDate);
            TimePeriod otherInstance = TimePeriod.valueOf(otherStartingDate, endingDate);

            Assert.False(instance.Equals(otherInstance));
        }

        [Fact]
        public void ensureTimePeriodsArentEqualIfTheyHaveDifferentEndingDates()
        {
            LocalDateTime startingDate = new LocalDateTime(1999, 12, 31, 23, 29);
            LocalDateTime endingDate = new LocalDateTime(3000, 12, 12, 12, 12);
            LocalDateTime otherEndingDate = new LocalDateTime(3001, 12, 12, 12, 12);

            TimePeriod instance = TimePeriod.valueOf(startingDate, endingDate);
            TimePeriod otherInstance = TimePeriod.valueOf(startingDate, otherEndingDate);

            Assert.False(instance.Equals(otherInstance));
        }

        [Fact]
        public void ensureTimePeriodsAreEqualIfTheyHaveTheSameStartingAndEndingDates()
        {
            LocalDateTime startingDate = new LocalDateTime(1999, 12, 31, 23, 29);
            LocalDateTime endingDate = new LocalDateTime(3000, 12, 12, 12, 12);

            TimePeriod instance = TimePeriod.valueOf(startingDate, endingDate);
            TimePeriod otherInstance = TimePeriod.valueOf(startingDate, endingDate);

            Assert.True(instance.Equals(otherInstance));
        }

        [Fact]
        public void ensureToStringWorks()
        {
            LocalDateTime startingDate = new LocalDateTime(1999, 12, 31, 23, 29);
            LocalDateTime endingDate = new LocalDateTime(3000, 12, 12, 12, 12);

            TimePeriod instance = TimePeriod.valueOf(startingDate, endingDate);
            TimePeriod otherInstance = TimePeriod.valueOf(startingDate, endingDate);

            Assert.Equal(instance.ToString(), otherInstance.ToString());
        }
    }
}