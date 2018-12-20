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
        public void ensureAddRestrictionThrowsExceptionIfRestrictionIsNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            Restriction restriction = null;

            Action addNullRestrictionAction = () => measurement.addRestriction(restriction);

            Assert.Throws<ArgumentException>(addNullRestrictionAction);
        }

        [Fact]
        public void ensureAddRestrictionDoesNotAddRestrictionIfRestrictionIsNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            Restriction restriction = null;

            try
            {
                measurement.addRestriction(restriction);
            }
            catch (Exception) { }

            Assert.Empty(measurement.restrictions);
        }

        [Fact]
        public void ensureAddRestrictionDoesNotThrowExceptionIfRestrictionIsAddedSuccessfully()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            Restriction restriction = new Restriction("This is a restriction", new WidthPercentageAlgorithm());

            Action addValidRestrictionAction = () => measurement.addRestriction(restriction);

            Exception exception = Record.Exception(addValidRestrictionAction);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddRestrictionAddsRestriction()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            measurement.addRestriction(restriction);

            Assert.Single(measurement.restrictions);
        }

        [Fact]
        public void ensureRemoveNullRestrictionThrowsException()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            Restriction restriction = null;

            Action nullRestrictionRemovalAction = () => measurement.removeRestriction(restriction);

            Assert.Throws<ArgumentException>(nullRestrictionRemovalAction);
        }

        [Fact]
        public void ensureRemoveRestrictionThrowsExceptionIfRestrictionWasNotAddedBeforehand()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action notAddedRestrictionRemovalAction = () => measurement.removeRestriction(restriction);

            Assert.Throws<ArgumentException>(notAddedRestrictionRemovalAction);
        }

        [Fact]
        public void ensureRemoveRestrictionDoesNotThrowExceptionIfRestrictionWasAddedBeforehand()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);
            Restriction restriction = new Restriction("This is a restriction", new WidthPercentageAlgorithm());

            measurement.addRestriction(restriction);

            Action removeValidRetrictionAction = () => measurement.removeRestriction(restriction);

            Exception e = Record.Exception(removeValidRetrictionAction);

            Assert.Null(e);
        }

        [Fact]
        public void ensureRemoveRestrictionRemovesRestriction()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            measurement.addRestriction(restriction);
            measurement.removeRestriction(restriction);

            Assert.Empty(measurement.restrictions);
        }

        [Fact]
        public void ensureChangeHeightDimensionReturnsFalseNewValueIsNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension newHeight = null;

            Assert.False(measurement.changeHeightDimension(newHeight));
        }

        [Fact]
        public void ensureChangeHeightDimensionDoesNotChangeOldValueIfNewValueIsNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension newHeight = null;

            measurement.changeHeightDimension(newHeight);

            Assert.Equal(height, measurement.height);
        }

        [Fact]
        public void ensureChangeHeightDimensionReturnsTrueIfNewValueIsNotNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension newHeight = new SingleValueDimension(25);

            Assert.True(measurement.changeHeightDimension(newHeight));
        }

        [Fact]
        public void ensureChangeHeightDimensionChangesValueIfNewValueIsNotNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension newHeight = new SingleValueDimension(25);

            measurement.changeHeightDimension(newHeight);

            Assert.Equal(newHeight, measurement.height);
        }

        [Fact]
        public void ensureChangeWidthDimensionReturnsFalseIfNewValueIsNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension newWidth = null;

            Assert.False(measurement.changeWidthDimension(newWidth));
        }

        [Fact]
        public void ensureChangeWidthDimensionDoesNotChangeOldValueIfNewValueIsNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension newWidth = null;

            measurement.changeWidthDimension(newWidth);

            Assert.Equal(width, measurement.width);
        }

        [Fact]
        public void ensureChangeWidthDimensionReturnsTrueIfNewValueIsNotNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension newWidth = new SingleValueDimension(17);

            Assert.True(measurement.changeWidthDimension(newWidth));
        }

        [Fact]
        public void ensureChangeWidthDimensionChangesValueIfNewValueIsNotNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension newWidth = new SingleValueDimension(17);

            measurement.changeWidthDimension(newWidth);

            Assert.Equal(newWidth, measurement.width);
        }

        [Fact]
        public void ensureChangeDepthDimensionReturnsFalseIfNewValueIsNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension newDepth = null;

            Assert.False(measurement.changeDepthDimension(newDepth));
        }

        [Fact]
        public void ensureChangeDepthDimensionDoesNotChangeOldValueIfNewValueIsNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension newDepth = null;

            measurement.changeDepthDimension(newDepth);

            Assert.Equal(depth, measurement.depth);
        }

        [Fact]
        public void ensureChangeDepthDimensionReturnsTrueIfNewValueIsNotNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension newDepth = new SingleValueDimension(12);

            Assert.True(measurement.changeDepthDimension(newDepth));
        }

        [Fact]
        public void ensureChangeDepthDimensionChangesValueIfNewValueIsNotNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension newDepth = new SingleValueDimension(12);

            measurement.changeDepthDimension(newDepth);

            Assert.Equal(newDepth, measurement.depth);
        }

        [Fact]
        public void ensureMeaurementIsNotEqualIfComparingObjecttIsNull()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            Assert.False(measurement.Equals(null));
        }

        [Fact]
        public void ensureMeasuremnetIsNotEqualIfComparingObjectIsNullAndNotAnInstanceOfMeasurement()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            string other = null;

            Assert.False(measurement.Equals(other));
        }

        [Fact]
        public void ensureMeasurementIsNotEqualIfComparingObjectIsNotInstanceOfMeasurement()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            //just to make sure Measurement's Equals() is being called and not Dimension's
            Assert.False(measurement.Equals(height));
        }

        [Fact]
        public void ensureMeasurementIsEqualIfComparingObjectHasTheSameReference()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            Assert.Equal(measurement, measurement);
        }

        [Fact]
        public void ensureMeasurementIsNotEqualIfComparingMeasurementHeightIsNotEqual()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension otherHeight = new SingleValueDimension(27);
            Measurement otherMeasurement = new Measurement(otherHeight, width, depth);

            Assert.NotEqual(measurement, otherMeasurement);
        }

        [Fact]
        public void ensureMeasurementIsNotEqualIfComparingMeasurementWidthIsNotEqual()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension otherWidth = new SingleValueDimension(15);
            Measurement otherMeasurement = new Measurement(height, otherWidth, depth);

            Assert.NotEqual(measurement, otherMeasurement);
        }

        [Fact]
        public void ensureMeasurementIsNotEqualIfComparingMeasurementDepthIsNotEqual()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);

            SingleValueDimension otherDepth = new SingleValueDimension(5);
            Measurement otherMeasurement = new Measurement(height, width, otherDepth);

            Assert.NotEqual(measurement, otherMeasurement);
        }

        [Fact]
        public void ensureMeasurementHasDifferentHashCodeIfDimensionsAreDifferent()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);


            SingleValueDimension otherHeight = new SingleValueDimension(12);
            SingleValueDimension otherWidth = new SingleValueDimension(16);
            SingleValueDimension otherDepth = new SingleValueDimension(6);

            Measurement otherMeasurement = new Measurement(otherHeight, otherWidth, otherDepth);

            Assert.NotEqual(measurement.GetHashCode(), otherMeasurement.GetHashCode());
        }

        [Fact]
        public void ensureMeasuerementHasEqualHashCodeIfDimensionsAreEqual()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);


            SingleValueDimension otherHeight = new SingleValueDimension(21.5);
            SingleValueDimension otherWidth = new SingleValueDimension(12.3);
            SingleValueDimension otherDepth = new SingleValueDimension(8.0);

            Measurement otherMeasurement = new Measurement(otherHeight, otherWidth, otherDepth);

            Assert.Equal(measurement.GetHashCode(), otherMeasurement.GetHashCode());
        }

        [Fact]
        public void ensureMeasurementsWithEqualDimensionsHaveEqualToString()
        {
            SingleValueDimension height = new SingleValueDimension(21.5);
            SingleValueDimension width = new SingleValueDimension(12.3);
            SingleValueDimension depth = new SingleValueDimension(8.0);

            Measurement measurement = new Measurement(height, width, depth);
            Measurement otherMeasurement = new Measurement(height, width, depth);

            Assert.Equal(measurement.ToString(), otherMeasurement.ToString());
        }
    }
}