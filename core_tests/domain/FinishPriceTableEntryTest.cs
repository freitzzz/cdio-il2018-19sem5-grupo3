using Xunit;
using System;
using core.domain;
using NodaTime;
using support.utils;

namespace core_tests.domain
{
    public class FinishPriceTableEntryTest
    {
        [Fact]
        public void ensureTableEntryIsntCreatedIfPriceIsNull()
        {
            Action act = () => new FinishPriceTableEntry(null,
                createTimePeriod(), createFinish());

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureTableEntryIsntCreatedIfTimePeriodIsNull()
        {
            Action act = () => new FinishPriceTableEntry(Price.valueOf(10),
                null, createFinish());

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureTableEntryIsntCreatedIfFinishIsNull()
        {
            Action act = () => new FinishPriceTableEntry(Price.valueOf(20),
                createTimePeriod(), null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsCreated()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(Price.valueOf(20),
                                                createTimePeriod(), createFinish());
            Assert.NotNull(instance);
        }

        [Fact]
        public void ensureTableEntriesWithDifferentPricesHaveDifferentHashCodes()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(Price.valueOf(5),
                                                createTimePeriod(), createFinish());

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTableEntriesWithDifferentTimePeriodsHaveDifferentHashCodes()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(Price.valueOf(10),
                                               createOtherTimePeriod(), createFinish());

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTableEntriesWithDifferentFinishesHaveDifferentHashCodes()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createOtherFinish());

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureEqualTableEntriesHaveEqualHashCodes()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTableEntryIsEqualToItself()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void ensureTableEntryIsntEqualToNull()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureTableEntryIsntEqualToInstanceOfOtherType()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());

            Assert.False(instance.Equals("bananas"));
        }

        [Fact]
        public void ensureTableEntriesWithDifferentPricesArentEqual()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(Price.valueOf(5),
                                                createTimePeriod(), createFinish());

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureTableEntriesWithDifferentTimePeriodsArentEqual()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(Price.valueOf(10),
                                                createOtherTimePeriod(), createFinish());

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureTableEntriesWithDifferentFinishesArentEqual()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createOtherFinish());

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureTableEntriesWithEqualPropertiesAreEqual()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureToStringWorks()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createFinish());

            Assert.Equal(instance.ToString(), other.ToString());
        }

        //!Purpose of this method is to make test code more readable
        private Finish createFinish()
        {
            return Finish.valueOf("finish");
        }

        //!Purpose of this method is to make test code more readable
        private Finish createOtherFinish()
        {
            return Finish.valueOf("other finish");
        }

        //!Purpose of this method is to make test code more readable
        private TimePeriod createTimePeriod()
        {
            return TimePeriod.valueOf(new LocalDateTime(2000, 10, 10, 10, 10),
                        new LocalDateTime(2001, 10, 10, 10, 10));
        }

        //!Purpose of this method is to make test code more readable
        private TimePeriod createOtherTimePeriod()
        {
            return TimePeriod.valueOf(new LocalDateTime(1999, 9, 9, 9, 9),
                        new LocalDateTime(2000, 9, 9, 9, 9));
        }
    }
}