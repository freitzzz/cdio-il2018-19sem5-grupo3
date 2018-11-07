using System;
using core.domain;
using Xunit;

namespace core_tests.domain
{
    public class MeasurementTest
    {
        [Fact]
        public void ensureArgumentExceptionIsThrownIfHeightDimensionIsNull()
        {

            Dimension height = null;
            Dimension width = new SingleValueDimension(21.5);
            Dimension depth = new SingleValueDimension(12.3);

            Action action = () => new Measurement(height, width, depth);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ensureArgumentExceptionIsThrownIfWidthDimensionIsNull()
        {

            Dimension height = new SingleValueDimension(21.5);
            Dimension width = null;
            Dimension depth = new SingleValueDimension(12.3);

            Action action = () => new Measurement(height, width, depth);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ensureArgumentExceptionIsThrownIfDepthDimensionIsNull()
        {

            Dimension height = new SingleValueDimension(21.5);
            Dimension width = new SingleValueDimension(12.3);
            Dimension depth = null;

            Action action = () => new Measurement(height, width, depth);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ensureInstanceIsCreatedIfDimensionsAreNotNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);


            Measurement measurement = new Measurement(height, width, depth);

            Assert.NotNull(measurement);
        }

        [Fact]
        public void ensureAddRestrictionReturnsFalseIfRestrictionIsNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            Restriction restriction = null;

            Assert.False(measurement.addRestriction(restriction));
        }

        [Fact]
        public void ensureAddRestrictionDoesNotAddRestrictionIfRestrictionIsNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            Restriction restriction = null;

            measurement.addRestriction(restriction);

            Assert.Equal(0, measurement.restrictions.Count);
        }

        [Fact]
        public void ensureAddRestrictionReturnsIfRestrictionIsAddedSuccessfully()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            Restriction restriction = new Restriction("This is a restriction");

            Assert.True(measurement.addRestriction(restriction));
        }

        [Fact]
        public void ensureAddRestrictionAddsRestriction()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            Restriction restriction = new Restriction("This is a restriction");

            measurement.addRestriction(restriction);

            Assert.Equal(1, measurement.restrictions.Count);
        }

        [Fact]
        public void ensureRemoveNullRestrictionReturnsFalse()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            Restriction restriction = null;

            Assert.False(measurement.removeRestriction(restriction));
        }

        [Fact]
        public void ensureRemoveRestrictionReturnsFalseIfRestrictionWasNotAddedBeforehand()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);
            Restriction restriction = new Restriction("This is a restriction");

            Assert.False(measurement.removeRestriction(restriction));
        }

        [Fact]
        public void ensureRemoveRestrictionReturnsTrueIfRestrictionWasAddedBeforehand()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);
            Restriction restriction = new Restriction("This is a restriction");

            measurement.addRestriction(restriction);

            Assert.True(measurement.removeRestriction(restriction));
        }

        [Fact]
        public void ensureRemoveRestrictionRemovesRestriction()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            Restriction restriction = new Restriction("This is a restriction");

            measurement.addRestriction(restriction);
            measurement.removeRestriction(restriction);

            Assert.Equal(0, measurement.restrictions.Count);
        }
    }
}