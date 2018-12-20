using System;
using Xunit;
using core.domain;

namespace core_tests.domain {
    /// <summary>
    /// Unit testing class for Restriction
    /// </summary>
    public class RestrictionTest {

        [Fact]
        public void ensureConstructorDetectsNullDescription() {
            Action action = () => new Restriction(null, new WidthPercentageAlgorithm());

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ensureConstructorDetectsEmptyDescription() {
            Action action = () => new Restriction(String.Empty, new SameMaterialAndFinishAlgorithm());

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ensureConstructorDetectsWhitespacesDescription() {
            Action action = () => new Restriction("         ", new WidthPercentageAlgorithm());

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ensureInstanceIsCreated() {
            Restriction instance = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());

            Assert.NotNull(instance);
        }

        [Fact]
        public void ensureSameInstanceIsEqual() {
            Restriction instance = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void ensureNullValueIsntEqual() {
            Restriction instance = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureInstanceOfDifferentTypeIsntEqual() {
            Restriction instance = new Restriction("restriction", new WidthPercentageAlgorithm());

            Assert.False(instance.Equals("bananas"));
        }

        [Fact]
        public void ensureInstanceWithDifferentDescriptionIsntEqual() {
            Restriction instance = new Restriction("restriction", new WidthPercentageAlgorithm());
            Restriction other = new Restriction("bananas", new WidthPercentageAlgorithm());

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstanceWithSameDescriptionIsEqual() {
            Restriction instance = new Restriction("restriction", new WidthPercentageAlgorithm());
            Restriction other = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureEqualRestrictionsHaveSameHashCode() {
            Restriction instance = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());
            Restriction other = new Restriction("restriction", new WidthPercentageAlgorithm());

            Assert.True(instance.GetHashCode().Equals(other.GetHashCode()));
        }

        [Fact]
        public void ensureDifferentRestrictionsHaveDifferentHashCode() {
            Restriction instance = new Restriction("restriction", new WidthPercentageAlgorithm());
            Restriction other = new Restriction("bananas", new WidthPercentageAlgorithm());

            Assert.False(instance.GetHashCode().Equals(other.GetHashCode()));
        }

        [Fact]
        public void ensureToStringWorks() {
            Restriction instance = new Restriction("oh hi mark", new WidthPercentageAlgorithm());
            Restriction other = new Restriction("oh hi mark", new WidthPercentageAlgorithm());
            Assert.Equal(instance.ToString(), other.ToString());
        }
    }
}