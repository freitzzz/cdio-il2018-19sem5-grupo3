using core.domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace core_tests.domain {
    public class AlgorithmTest {
        //this class is for helping in the unit testing of the Algorithm abstract class
        //and as such the implementation of the abstract methods are very simple or non-existent
        public class AlgorithmImpl : Algorithm {
            public AlgorithmImpl() {
                InputValue val1 = new InputValue(Input.valueOf("one", "number 1"));
                InputValue val2 = new InputValue(Input.valueOf("two", "number 2"));
                this.inputValues = new List<InputValue> { val1, val2 };
            }
            public override Product apply(CustomizedProduct customProduct, Product product) {
                throw new NotImplementedException();
            }
            //if input name is one then value should be 1, if input name is two then value should be 2
            protected override void checkValue(Input input, string value) {
                string name = input.name;
                switch (name) {
                    case "one":
                        if (!value.Equals("1")) {
                            throw new ArgumentOutOfRangeException("should be one");
                        }
                        break;
                    case "two":
                        if (!value.Equals("2")) {
                            throw new ArgumentOutOfRangeException("should be two");
                        }
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }
        //tests of setInputValue method
        [Fact]
        public void ensureSetInputValueFailsIfInputArgumentIsNull() {
            AlgorithmImpl alg = new AlgorithmImpl();
            Action action = () => alg.setInputValue(null, "val");
            Assert.Throws<ArgumentNullException>(action);
        }
        [Fact]
        public void ensureSetInputValueFailsIfInputDoesNotExist() {
            AlgorithmImpl alg = new AlgorithmImpl();
            Action action = () => alg.setInputValue(Input.valueOf("three", "range"), "val");
            Assert.Throws<ArgumentException>(action);
        }
        [Fact]
        public void ensureSetInputValueFailsIfInputExistsButDoesNotSupportValue() {
            AlgorithmImpl alg = new AlgorithmImpl();
            Action action = () => alg.setInputValue(Input.valueOf("one", "range"), "2");
            Assert.Throws<ArgumentOutOfRangeException>(action);
        }
        [Fact]
        public void ensureSetInputValueSucceeds() {
            AlgorithmImpl alg = new AlgorithmImpl();
            alg.setInputValue(Input.valueOf("one", "range"), "1");
            Assert.Equal("1", alg.inputValues[0].value);
        }
        //tests of setInputValues method
        [Fact]
        public void ensureSetInputValuesFailsIfArgumentIsNull() {
            AlgorithmImpl alg = new AlgorithmImpl();
            Action action = () => alg.setInputValues(null);
            Assert.Throws<ArgumentNullException>(action);
        }
        [Fact]
        public void ensureSetInputValuesFailsIfAtLeastOneInputIsInvalid() {
            Input input1 = Input.valueOf("one", "number 1");
            Input input2 = Input.valueOf("three", "number 3");
            AlgorithmImpl alg = new AlgorithmImpl();
            Dictionary<Input, string> inputValues = new Dictionary<Input, string>();
            inputValues.Add(input1, "1");
            inputValues.Add(input2, "3");
            Action action = () => alg.setInputValues(inputValues);
            Assert.Throws<ArgumentException>(action);
        }
        [Fact]
        public void ensureSetInputValuesSucceeds() {
            Input input1 = Input.valueOf("one", "number 1");
            Input input2 = Input.valueOf("two", "number 2");
            AlgorithmImpl alg = new AlgorithmImpl();
            Dictionary<Input, string> inputValues = new Dictionary<Input, string>();
            inputValues.Add(input1, "1");
            inputValues.Add(input2, "2");
            alg.setInputValues(inputValues);
            Assert.Equal("1", alg.inputValues[0].value);
            Assert.Equal("2", alg.inputValues[1].value);
        }
        //tests of getRequiredInputs method
        [Fact]
        public void ensureGetRequiredInputsSucceeds() {
            AlgorithmImpl alg = new AlgorithmImpl();
            List<Input> inputs = alg.getRequiredInputs();
            Assert.Equal(2, inputs.Count);
            Assert.Equal("one", inputs[0].name);
            Assert.Equal("two", inputs[1].name);
        }
        //tests of ready method
        [Fact]
        public void ensureReadyFails() {
            AlgorithmImpl alg = new AlgorithmImpl();
            alg.setInputValue(Input.valueOf("one", "range"), "1");
            Action action = () => alg.ready();
            Assert.Throws<ArgumentNullException>(action);
        }
        [Fact]
        public void ensureReadySucceeds() {
            Input input1 = Input.valueOf("one", "number 1");
            Input input2 = Input.valueOf("two", "number 2");
            AlgorithmImpl alg = new AlgorithmImpl();
            Dictionary<Input, string> inputValues = new Dictionary<Input, string>();
            inputValues.Add(input1, "1");
            inputValues.Add(input2, "2");
            alg.setInputValues(inputValues);
            alg.ready();
            //this method does not need an assert
        }
    }
}
