using System.Collections.Generic;
using core.domain;
using Xunit;

namespace core_tests.domain
{
    public class DimensionTest
    {
        [Fact]
        public void ensureAddRestrictionReturnsFalseIfRestrictionIsNull()
        {
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(new List<double>() { 12, 13, 14, 15 });
            Restriction restriction = null;

            Assert.False(instance.addRestriction(restriction));
        }

        [Fact]
        public void ensureAddRestrictionDoesNotAddRestrictionIfRestrictionIsNull()
        {
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(new List<double>() { 12, 13, 14, 15 });
            Restriction restriction = null;

            instance.addRestriction(restriction);

            Assert.Equal(0, instance.restrictions.Count);
        }

        [Fact]
        public void ensureAddRestrictionReturnsIfRestrictionIsAddedSuccessfully()
        {
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(new List<double>() { 12, 13, 14, 15 });
            Restriction restriction = new Restriction("This is a restriction");

            Assert.True(instance.addRestriction(restriction));
        }

        [Fact]
        public void ensureAddRestrictionAddsRestriction()
        {
            DiscreteDimensionInterval instance = new DiscreteDimensionInterval(new List<double>() { 12, 13, 14, 15 });
            Restriction restriction = new Restriction("This is a restriction");

            instance.addRestriction(restriction);

            Assert.Equal(1, instance.restrictions.Count);
        }

        [Fact]
        public void ensureRemoveNullRestrictionReturnsFalse()
        {
            SingleValueDimension instance = new SingleValueDimension(1.0);
            Restriction restriction = null;

            Assert.False(instance.removeRestriction(restriction));
        }

        [Fact]
        public void ensureRemoveRestrictionReturnsFalseIfRestrictionWasNotAddedBeforehand()
        {
            SingleValueDimension instance = new SingleValueDimension(1.0);
            Restriction restriction = new Restriction("This is a restriction");

            Assert.False(instance.removeRestriction(restriction));
        }

        [Fact]
        public void ensureRemoveRestrictionReturnsTrueIfRestrictionWasAddedBeforehand()
        {
            SingleValueDimension instance = new SingleValueDimension(1.0);
            Restriction restriction = new Restriction("This is a restriction");

            instance.addRestriction(restriction);

            Assert.True(instance.removeRestriction(restriction));
        }

        [Fact]
        public void ensureRemoveRestrictionRemovesRestriction()
        {
            SingleValueDimension instance = new SingleValueDimension(1.0);
            Restriction restriction = new Restriction("This is a restriction");

            instance.addRestriction(restriction);
            instance.removeRestriction(restriction);

            Assert.Equal(0, instance.restrictions.Count);
        }
    }
}