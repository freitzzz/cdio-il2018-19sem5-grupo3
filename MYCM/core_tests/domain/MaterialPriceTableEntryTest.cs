using Xunit;
using System;
using core.domain;
using System.Collections.Generic;
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
            Action act = () => new MaterialPriceTableEntry(price: null,
                timePeriod: createTimePeriod(), material: createMaterial());

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureTableEntryIsntCreatedIfTimePeriodIsNull()
        {
            Action act = () => new MaterialPriceTableEntry(price: Price.valueOf(10),
                timePeriod: null, material: createMaterial());

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureTableEntryIsntCreatedIfMaterialIsNull()
        {
            Action act = () => new MaterialPriceTableEntry(price: Price.valueOf(20),
                timePeriod: createTimePeriod(), material: null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsCreated()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(20),
                                                timePeriod: createTimePeriod(), material: createMaterial());
            Assert.NotNull(instance);
        }

        [Fact]
        public void ensureChangePriceDoesntChangePriceIfNewPriceIsNull()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(20),
                                                timePeriod: createTimePeriod(), material: createMaterial());

            Action act = () => instance.changePrice(null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureChangePriceChangesPrice()
        {
            Price oldPrice = Price.valueOf(20);
            Price newPrice = Price.valueOf(30);

            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: oldPrice,
                                               timePeriod: createTimePeriod(), material: createMaterial());

            instance.changePrice(newPrice);

            Assert.NotEqual(oldPrice, instance.price);
        }

        [Fact]
        public void ensureChangeTimePeriodDoesntChangeTimePeriodIfNewTimePeriodIsNull()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(20),
                                                timePeriod: createTimePeriod(), material: createMaterial());

            Action act = () => instance.changeTimePeriod(null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureChangeTimePeriodChangesTimePeriod()
        {
            TimePeriod oldTimePeriod = createTimePeriod();
            TimePeriod newTimePeriod = createOtherTimePeriod();

            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: oldTimePeriod, material: createMaterial());

            instance.changeTimePeriod(newTimePeriod);

            Assert.NotEqual(oldTimePeriod, instance.timePeriod);
        }

        [Fact]
        public void ensureEntriesWithDifferentTimePeriodsHaveDifferentIds()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createOtherTimePeriod(), material: createMaterial());

            Assert.NotEqual(instance.id(), other.id());
        }

        [Fact]
        public void ensureEntriesWithDifferentMaterialsHaveDifferentIds()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createOtherMaterial());

            Assert.NotEqual(instance.id(), other.id());
        }

        [Fact]
        public void ensureEqualEntriesHaveSameIds()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());

            Assert.Equal(instance.id(), other.id());
        }

        [Fact]
        public void ensureSameAsReturnsTrueForEqualEntityIds()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());

            Assert.True(instance.sameAs(instance.id()));
        }

        [Fact]
        public void ensureSameAsReturnsFalseForDifferentEntityIds()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());

            Assert.False(instance.sameAs("bananas"));
        }

        [Fact]
        public void ensureTableEntriesWithDifferentPricesHaveDifferentHashCodes()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(price: Price.valueOf(5),
                                                timePeriod: createTimePeriod(), material: createMaterial());

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTableEntriesWithDifferentTimePeriodsHaveDifferentHashCodes()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                               timePeriod: createOtherTimePeriod(), material: createMaterial());

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTableEntriesWithDifferentMaterialsHaveDifferentHashCodes()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createOtherMaterial());

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureEqualTableEntriesHaveEqualHashCodes()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureTableEntryIsEqualToItself()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void ensureTableEntryIsntEqualToNull()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureTableEntryIsntEqualToInstanceOfOtherType()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());

            Assert.False(instance.Equals("bananas"));
        }

        [Fact]
        public void ensureTableEntriesWithDifferentPricesArentEqual()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(price: Price.valueOf(5),
                                                timePeriod: createTimePeriod(), material: createMaterial());

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureTableEntriesWithDifferentTimePeriodsArentEqual()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createOtherTimePeriod(), material: createMaterial());

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureTableEntriesWithDifferentMaterialsArentEqual()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createOtherMaterial());

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureTableEntriesWithEqualPropertiesAreEqual()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureToStringWorks()
        {
            MaterialPriceTableEntry instance = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());
            MaterialPriceTableEntry other = new MaterialPriceTableEntry(price: Price.valueOf(10),
                                                timePeriod: createTimePeriod(), material: createMaterial());

            Assert.Equal(instance.ToString(), other.ToString());
        }

        //!Purpose of this method is to make test code more readable
        private Material createMaterial()
        {
            String reference = "reference";
            String designation = "designation";
            Color color = Color.valueOf("color", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("finish");
            return new Material(reference, designation, "ola.jpg",
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
            return new Material(reference, designation, "ola.jpg",
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