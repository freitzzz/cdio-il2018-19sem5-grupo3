using Xunit;
using System;
using core.domain;
using System.Collections.Generic;
using support.utils;
using NodaTime;

namespace core_tests.domain
{
    /// <summary>
    /// Unit testing class for MaterialPriceTableEntry
    /// </summary>
    public class MaterialPriceTableEntryTest
    {
        [Fact]
        public void ensureTableEntryIsntCreatedIfPriceIsNull()
        {
            Action act = () => new MaterialPriceTableEntry(null,
                createTimePeriod(), createMaterial());

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureTableEntryIsntCreatedIfTimePeriodIsNull()
        {
            Action act = () => new MaterialPriceTableEntry(Price.valueOf(10),
                null, createMaterial());

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureTableEntryIsntCreatedIfMaterialIsNull()
        {
            Action act = () => new MaterialPriceTableEntry(Price.valueOf(20),
                createTimePeriod(), null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsCreated()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(Price.valueOf(20),
                                                createTimePeriod(), createMaterial());
            Assert.NotNull(instance);
        }

        [Fact]
        public void ensureTableEntriesWithDifferentPricesHaveDifferentHashCodes()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(Price.valueOf(5),
                                                createTimePeriod(), createMaterial());

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTableEntriesWithDifferentTimePeriodsHaveDifferentHashCodes()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(Price.valueOf(10),
                                               createOtherTimePeriod(), createMaterial());

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTableEntriesWithDifferentMaterialsHaveDifferentHashCodes()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createOtherMaterial());

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureEqualTableEntriesHaveEqualHashCodes()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTableEntryIsEqualToItself()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void ensureTableEntryIsntEqualToNull()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureTableEntryIsntEqualToInstanceOfOtherType()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());

            Assert.False(instance.Equals("bananas"));
        }

        [Fact]
        public void ensureTableEntriesWithDifferentPricesArentEqual()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(Price.valueOf(5),
                                                createTimePeriod(), createMaterial());

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureTableEntriesWithDifferentTimePeriodsArentEqual()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createOtherTimePeriod(), createMaterial());

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureTableEntriesWithDifferentMaterialsArentEqual()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createOtherMaterial());

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureTableEntriesWithEqualPropertiesAreEqual()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureToStringWorks()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(Price.valueOf(10),
                                                createTimePeriod(), createMaterial());

            Assert.Equal(instance.ToString(), other.ToString());
        }

        //!Purpose of this method is to make test code more readable
        private Material createMaterial()
        {
            String reference = "reference";
            String designation = "designation";
            Color color = Color.valueOf("color", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("finish");
            return new Material(reference, designation,
                                new List<Color>(new[] { color }),
                                 new List<Finish>(new[] { finish }));
        }

        //!Purpose of this method is to make test code more readable
        private Material createOtherMaterial()
        {
            String reference = "other reference";
            String designation = "other designation";
            Color color = Color.valueOf("other color", 14, 14, 12, 12);
            Finish finish = Finish.valueOf("other finish");
            return new Material(reference, designation,
                                new List<Color>(new[] { color }),
                                 new List<Finish>(new[] { finish }));
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