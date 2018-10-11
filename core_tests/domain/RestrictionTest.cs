using System;
using Xunit;
using core.domain;

namespace core_tests.domain
{
    /// <summary>
    /// Unit testing class for Restriction
    /// </summary>
    public class RestrictionTest
    {

        [Fact]
        public void ensureConstructorDetectsNullDescription()
        {
            Action action = () => new Restriction(null);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ensureConstructorDetectsEmptyDescription()
        {
            Action action = () => new Restriction(String.Empty);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ensureConstructorDetectsWhitespacesDescription()
        {
            Action action = () => new Restriction("         ");

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ensureInstanceIsCreated()
        {
            Restriction instance = new Restriction("restriction");

            Assert.NotNull(instance);
        }

        [Fact]
        public void ensureSameInstanceIsEqual()
        {
            Restriction instance = new Restriction("restriction");

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void ensureNullValueIsntEqual()
        {
            Restriction instance = new Restriction("restriction");

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureInstanceOfDifferentTypeIsntEqual()
        {
            Restriction instance = new Restriction("restriction");

            Assert.False(instance.Equals("bananas"));
        }

        [Fact]
        public void ensureInstanceWithDifferentDescriptionIsntEqual()
        {
            Restriction instance = new Restriction("restriction");
            Restriction other = new Restriction("bananas");

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstanceWithSameDescriptionIsEqual()
        {
            Restriction instance = new Restriction("restriction");
            Restriction other = new Restriction("restriction");

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureEqualRestrictionsHaveSameHashCode()
        {
            Restriction instance = new Restriction("restriction");
            Restriction other = new Restriction("restriction");

            Assert.True(instance.GetHashCode().Equals(other.GetHashCode()));
        }

        [Fact]
        public void ensureDifferentRestrictionsHaveDifferentHashCode()
        {
            Restriction instance = new Restriction("restriction");
            Restriction other = new Restriction("bananas");

            Assert.False(instance.GetHashCode().Equals(other.GetHashCode()));
        }

        [Fact]
        public void ensureToStringWorks()
        {
            Restriction instance = new Restriction("oh hi mark");
            Restriction other = new Restriction("oh hi mark");

            Assert.True(instance.ToString().Equals(other.ToString()));
        }

        [Fact]
        public void ensureToDTOWorks()
        {
            Restriction instance = new Restriction("lil pong");
            Restriction other = new Restriction("lil pong");

            Assert.True(instance.toDTO().description.Equals(other.toDTO().description));
        }
    }
}