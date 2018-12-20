using core.domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace core_tests.domain {
    public class InputValueTest {
        [Fact]
        public void ensureCreationFailsIfInputIsNull() {
            Action creation = () => new InputValue(null);
            Assert.Throws<ArgumentNullException>(creation);
        }
        [Fact]
        public void ensureCreationSucceeds() {
            string name = "der alte würfelt nicht";
            string range = "Deneb";
            Input input = Input.valueOf(name, range);
            InputValue val = new InputValue(input);
            Assert.Equal(input, val.input);
        }
    }
}
