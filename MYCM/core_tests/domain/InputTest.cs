using core.domain;
using core.dto;
using System;
using Xunit;

namespace core_tests.domain {
    public class InputTest {
        /// <summary>
        /// Ensures Input constructor throws ArgumentException when name is null
        /// </summary>
        [Fact]
        public void ensureCreationThrowsArgumentExceptionWhenNameIsNull() {
            Console.WriteLine("ensureConstructorThrowsArgumentExceptionWhenArgumentIsNull");
            Action invalidNullInputNameCreation = () => Input.valueOf(null, "range");
            //Since the input was created with a null name then it should throw an ArgumentException
            Assert.Throws<ArgumentException>(invalidNullInputNameCreation);
        }
        /// <summary>
        /// Ensures Input constructor throws ArgumentException when name is empty string
        /// </summary>
        [Fact]
        public void ensureCreationThrowsArgumentExceptionWhenNameIsEmpty() {
            Console.WriteLine("ensureConstructorThrowsArgumentExceptionWhenArgumentIsEmpty");
            Action invalidNullInputNameCreation = () => Input.valueOf("", "range");
            //Since the input was created with name as an empty string then it should throw an ArgumentException
            Assert.Throws<ArgumentException>(invalidNullInputNameCreation);
        }
        /// <summary>
        /// Ensures Input constructor throws ArgumentException when range is null
        /// </summary>
        [Fact]
        public void ensureCreationThrowsArgumentExceptionWhenRangeIsNull() {
            Console.WriteLine("ensureConstructorThrowsArgumentExceptionWhenArgumentIsNull");
            Action invalidNullInputNameCreation = () => Input.valueOf("name", null);
            //Since the input was created with a null range then it should throw an ArgumentException
            Assert.Throws<ArgumentException>(invalidNullInputNameCreation);
        }
        /// <summary>
        /// Ensures Input constructor throws ArgumentException when range is empty string
        /// </summary>
        [Fact]
        public void ensureCreationThrowsArgumentExceptionWhenRangeIsEmpty() {
            Console.WriteLine("ensureConstructorThrowsArgumentExceptionWhenArgumentIsEmpty");
            Action invalidNullInputNameCreation = () => Input.valueOf("name", "");
            //Since the input was created with range as an empty string then it should throw an ArgumentException
            Assert.Throws<ArgumentException>(invalidNullInputNameCreation);
        }
        /// <summary>
        /// Ensure Input is  created successfully with a name and range
        /// </summary>
        [Fact]
        public void ensureInputCreationIsSuccessful() {
            Console.WriteLine("ensureInputNameConstructorIsSuccessful");
            string name = "der alte würfelt nicht";
            string range = "Deneb";
            Input input = Input.valueOf(name, range);
            Assert.Equal(name, input.name);
            Assert.Equal(range, input.range);
        }
        /// <summary>
        /// Ensure toDTO succeeds
        /// </summary>
        [Fact]
        public void ensureToDTOSucceeds() {
            Console.WriteLine("ensureToDTOSucceeds");
            Input input = Input.valueOf("Altair", "Vega");
            InputDTO dto = input.toDTO();
            Assert.Equal(input.name, dto.name);
            Assert.Equal(input.range, dto.range);
        }

        [Fact]
        public void ensureEqualsReturnsTrueIfSameInstance() {
            string name = "der alte würfelt nicht";
            string range = "Deneb";
            Input input = Input.valueOf(name, range);
            Assert.True(input.Equals(input));
        }
        [Fact]
        public void ensureEqualsReturnsFalseIfNullArgument() {
            string name = "der alte würfelt nicht";
            string range = "Deneb";
            Input input = Input.valueOf(name, range);
            Assert.False(input.Equals(null));
        }
        [Fact]
        public void ensureEqualsReturnsFalseIfArgumentNotSameClass() {
            string name = "der alte würfelt nicht";
            string range = "Deneb";
            Input input = Input.valueOf(name, range);
            Assert.False(input.Equals("i'm a string"));
        }
        [Fact]
        public void ensureEqualsReturnsFalseIfArgumentNotEqual() {
            string name = "der alte würfelt nicht";
            string range = "Deneb";
            Input input = Input.valueOf(name, range);
            Input input2 = Input.valueOf(range, name);
            Assert.False(input.Equals(input2));
        }
        [Fact]
        public void ensureEqualsReturnsTrueIfArgumentIsEqual() {
            string name = "der alte würfelt nicht";
            string range = "Deneb";
            Input input = Input.valueOf(name, range);
            Input input2 = Input.valueOf(name, range);
            Assert.True(input.Equals(input2));
        }
    }
}
