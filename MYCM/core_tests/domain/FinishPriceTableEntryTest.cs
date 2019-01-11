using Xunit;
using System;
using core.domain;
using NodaTime;

namespace core_tests.domain
{
    public class FinishPriceTableEntryTest
    {
        [Fact]
        public void ensureTableEntryIsntCreatedWithNullMaterialEID()
        {
            Action act = () => new FinishPriceTableEntry(materialEID: null, price: Price.valueOf(10),
                timePeriod: createTimePeriod(), finish: createFinish());

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureTableEntryIsntCreatedWithEmptyMaterialEID()
        {
            Action act = () => new FinishPriceTableEntry(materialEID: "   ", price: Price.valueOf(10),
                timePeriod: createTimePeriod(), finish: createFinish());

            Assert.Throws<ArgumentException>(act);
        }


        [Fact]
        public void ensureTableEntryIsntCreatedIfPriceIsNull()
        {
            Action act = () => new FinishPriceTableEntry(materialEID: "hi", price: null,
                timePeriod: createTimePeriod(), finish: createFinish());

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureTableEntryIsntCreatedIfTimePeriodIsNull()
        {
            Action act = () => new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                timePeriod: null, finish: createFinish());

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureTableEntryIsntCreatedIfFinishIsNull()
        {
            Action act = () => new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(20),
                timePeriod: createTimePeriod(), finish: null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsCreated()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(20),
                                                timePeriod: createTimePeriod(), finish: createFinish());
            Assert.NotNull(instance);
        }

        [Fact]
        public void ensureChangePriceDoesntChangePriceIfNewPriceIsNull()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(20),
                                                timePeriod: createTimePeriod(), finish: createFinish());

            Action act = () => instance.changePrice(null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureChangePriceChangesPrice()
        {
            Price oldPrice = Price.valueOf(20);
            Price newPrice = Price.valueOf(30);

            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: oldPrice,
                                               timePeriod: createTimePeriod(), finish: createFinish());

            instance.changePrice(newPrice);

            Assert.NotEqual(oldPrice, instance.price);
        }

        [Fact]
        public void ensureChangeTimePeriodDoesntChangeTimePeriodIfNewTimePeriodIsNull()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(20),
                                                timePeriod: createTimePeriod(), finish: createFinish());

            Action act = () => instance.changeTimePeriod(null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureChangeTimePeriodChangesTimePeriod()
        {
            TimePeriod oldTimePeriod = createTimePeriod();
            TimePeriod newTimePeriod = createOtherTimePeriod();

            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: oldTimePeriod, finish: createFinish());

            instance.changeTimePeriod(newTimePeriod);

            Assert.NotEqual(oldTimePeriod, instance.timePeriod);
        }

        [Fact]
        public void ensureIdReturnsTheBusinessIdentifier()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                               timePeriod: createTimePeriod(), finish: createFinish());

            FinishPriceTableEntry other = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                               timePeriod: createTimePeriod(), finish: createFinish());


            Assert.Equal(instance.id(), other.id());
        }

        [Fact]
        public void ensureSameAsReturnsTrueForEqualEntities()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                               timePeriod: createTimePeriod(), finish: createFinish());

            FinishPriceTableEntry other = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                               timePeriod: createTimePeriod(), finish: createFinish());


            Assert.True(instance.sameAs(other.eId));
        }

        [Fact]
        public void ensureSameAsReturnsFalseForDifferentEntities()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                               timePeriod: createTimePeriod(), finish: createFinish());

            FinishPriceTableEntry other = new FinishPriceTableEntry(materialEID: "hello", price: Price.valueOf(10),
                                               timePeriod: createOtherTimePeriod(), finish: createFinish());

            Assert.False(instance.sameAs(other.eId));
        }

        [Fact]
        public void ensureTableEntriesWithDifferentPricesHaveDifferentHashCodes()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(5),
                                                timePeriod: createTimePeriod(), finish: createFinish());

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTableEntriesWithDifferentTimePeriodsHaveDifferentHashCodes()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                               timePeriod: createOtherTimePeriod(), finish: createFinish());

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTableEntriesWithDifferentFinishesHaveDifferentHashCodes()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createOtherFinish());

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureEqualTableEntriesHaveEqualHashCodes()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTableEntryIsEqualToItself()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void ensureTableEntryIsntEqualToNull()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureTableEntryIsntEqualToInstanceOfOtherType()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());

            Assert.False(instance.Equals("bananas"));
        }

        [Fact]
        public void ensureTableEntriesWithDifferentPricesArentEqual()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(5),
                                                timePeriod: createTimePeriod(), finish: createFinish());

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureTableEntriesWithDifferentTimePeriodsArentEqual()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createOtherTimePeriod(), finish: createFinish());

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureTableEntriesWithDifferentFinishesArentEqual()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createOtherFinish());

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureTableEntriesWithEqualPropertiesAreEqual()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureToStringWorks()
        {
            FinishPriceTableEntry instance = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());
            FinishPriceTableEntry other = new FinishPriceTableEntry(materialEID: "hi", price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), finish: createFinish());

            Assert.Equal(instance.ToString(), other.ToString());
        }

        //!Purpose of this method is to make test code more readable
        private Finish createFinish()
        {
            return Finish.valueOf("finish", 12);
        }

        //!Purpose of this method is to make test code more readable
        private Finish createOtherFinish()
        {
            return Finish.valueOf("other finish", 50);
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