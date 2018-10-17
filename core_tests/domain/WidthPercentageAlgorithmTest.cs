using core.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace core_tests.domain {
    public class WidthPercentageAlgorithmTest {
        /// <summary>
        /// Ensures getRequiredInputs returns the correct list of inputs
        /// </summary>
        [Fact]
        public void ensureGetRequiredInputsSucceeds() {
            Console.WriteLine("ensureGetRequiredInputsSucceeds");
            List<Input> inputs = new WidthPercentageAlgorithm().getRequiredInputs();
            Assert.True(2 == inputs.Count());
            Assert.Equal("Minimum Percentage", inputs.ElementAt(0).name);
            Assert.Equal("Maximum Percentage", inputs.ElementAt(1).name);
        }
        /// <summary>
        /// Ensures setInputValues throws ArgumentException if argument is null
        /// </summary>
        [Fact]
        public void ensureSetInputValuesThrowsArgumentExceptionIfArgumentNull() {
            Console.WriteLine("ensureSetInputValuesThrowsArgumentExceptionIfArgumentNull");
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Action setValues = () => algorithm.setInputValues(null);
            Assert.Throws<ArgumentException>(setValues);
        }
        /// <summary>
        /// Ensures setInputValues throws ArgumentException if argument is empty list
        /// </summary>
        [Fact]
        public void ensureSetInputValuesThrowsArgumentExceptionIfArgumentIsEmpty() {
            Console.WriteLine("ensureSetInputValuesThrowsArgumentExceptionIfArgumentIsEmpty");
            List<Input> inputs = new List<Input>();
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Action setValues = () => algorithm.setInputValues(inputs);
            Assert.Throws<ArgumentException>(setValues);
        }
        /// <summary>
        /// Ensures setInputValues throws ArgumentException if list has more than the required inputs
        /// </summary>
        [Fact]
        public void ensureSetInputValuesThrowsArgumentExceptionIfListTooBig() {
            Console.WriteLine("ensureSetInputValuesThrowsArgumentExceptionIfListTooBig");
            List<Input> inputs = new List<Input>();
            inputs.Add(new Input("Altair"));
            inputs.Add(new Input("Vega"));
            inputs.Add(new Input("Deneb"));
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Action setValues = () => algorithm.setInputValues(inputs);
            Assert.Throws<ArgumentException>(setValues);
        }
        /// <summary>
        /// Ensures setInputValues succeeds
        /// </summary>
        [Fact]
        public void ensureSetInputValuesSucceeds() {
            Console.WriteLine("ensureSetInputValuesSucceeds");
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "0.1";
            Input maxInput = new Input("Maximum Percentage");
            maxInput.value = "0.2";
            List<Input> inputs = new List<Input>();
            inputs.Add(minInput);
            inputs.Add(maxInput);
            Assert.True(new WidthPercentageAlgorithm().setInputValues(inputs));
        }
        /// <summary>
        /// Ensures isWithinDataRange throws ArgumentException if argument is null
        /// </summary>
        [Fact]
        public void ensureIsWithinDataRangeThrowsArgumentExceptionIfArgumentNull() {
            Console.WriteLine("ensureIsWithinDataRangeThrowsArgumentExceptionIfArgumentNull");
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Action withinRange = () => algorithm.isWithinDataRange(null);
            Assert.Throws<ArgumentException>(withinRange);
        }
        /// <summary>
        /// Ensures isWithinDataRange throws ArgumentException if argument is empty list
        /// </summary>
        [Fact]
        public void ensureIsWithinDataRangeThrowsArgumentExceptionIfArgumentIsEmpty() {
            Console.WriteLine("ensureIsWithinDataRangeThrowsArgumentExceptionIfArgumentIsEmpty");
            List<Input> inputs = new List<Input>();
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Action withinRange = () => algorithm.isWithinDataRange(inputs);
            Assert.Throws<ArgumentException>(withinRange);
        }
        /// <summary>
        /// Ensures isWithinDataRange throws ArgumentException if list has incorrect number of inputs
        /// </summary>
        [Fact]
        public void ensureIsWithinDataRangeThrowsArgumentExceptionIfIncorretNumberOfArguments() {
            Console.WriteLine("ensureIsWithinDataRangeThrowsArgumentExceptionIfIncorretNumberOfArguments");
            List<Input> inputs = new List<Input>();
            inputs.Add(new Input("Altair"));
            inputs.Add(new Input("Vega"));
            inputs.Add(new Input("Deneb"));
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Action withinRange = () => algorithm.isWithinDataRange(inputs);
            Assert.Throws<ArgumentException>(withinRange);
        }
        /// <summary>
        /// Ensures isWithinDataRange throws FormatException if any input value is not double
        /// </summary>
        [Fact]
        public void ensureIsWithinDataRangeFailsIfValueIsNotDouble() {
            Console.WriteLine("ensureIsWithinDataRangeFailsIfValueIsNotDouble");
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "AAAA";
            Input maxInput = new Input("Maximum Percentage");
            maxInput.value = "BBBB";
            List<Input> inputs = new List<Input>();
            inputs.Add(minInput);
            inputs.Add(maxInput);
            Action withinRange = () => new WidthPercentageAlgorithm().isWithinDataRange(inputs);
            Assert.Throws<FormatException>(withinRange);
        }
        /// <summary>
        /// Ensures isWithinDataRange throws ArgumentException if any input has incorrect name
        /// </summary>
        [Fact]
        public void ensureIsWithinDataRangeThrowsArgumentExceptionIfInputNamesAreIncorrect() {
            Console.WriteLine("ensureIsWithinDataRangeThrowsArgumentExceptionIfInputNamesAreIncorrect");
            List<Input> inputs = new List<Input>();
            inputs.Add(new Input("Altair"));
            inputs.Add(new Input("Vega"));
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Action withinRange = () => algorithm.isWithinDataRange(inputs);
            Assert.Throws<ArgumentException>(withinRange);
        }
        /// <summary>
        /// Ensures isWithinDataRange throws ArgumentException any input name is null or empty
        /// </summary>
        [Fact]
        public void ensureIsWithinDataRangeThrowsArgumentExceptionIfInputNameNullOrEmpty() {
            Console.WriteLine("ensureIsWithinDataRangeThrowsArgumentExceptionIfInputNameNullOrEmpty");
            List<Input> inputs = new List<Input>();
            Input minInput = new Input("Minimum Percentage");
            minInput.name = "";
            Input maxInput = new Input("Maximum Percentage");
            inputs.Add(minInput);
            inputs.Add(maxInput);
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Action withinRange = () => algorithm.isWithinDataRange(inputs);
            Assert.Throws<ArgumentException>(withinRange);
        }
        /// <summary>
        /// Ensures isWithinDataRange throws ArgumentException input minimum percentage value is below zero
        /// </summary>
        [Fact]
        public void ensureIsWithinDataRangeThrowsArgumentExceptionIfMinimumPercentageBelowZero() {
            Console.WriteLine("ensureIsWithinDataRangeThrowsArgumentExceptionIfMinimumPercentageBelowZero");
            List<Input> inputs = new List<Input>();
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "-1";
            Input maxInput = new Input("Maximum Percentage");
            inputs.Add(minInput);
            inputs.Add(maxInput);
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Action withinRange = () => algorithm.isWithinDataRange(inputs);
            Assert.Throws<ArgumentOutOfRangeException>(withinRange);
        }
        /// <summary>
        /// Ensures isWithinDataRange throws ArgumentException if minimum percentage value is above one
        /// </summary>
        [Fact]
        public void ensureIsWithinDataRangeThrowsArgumentExceptionIfMinimumPercentageAboveOne() {
            Console.WriteLine("ensureIsWithinDataRangeThrowsArgumentExceptionIfMinimumPercentageAboveOne");
            List<Input> inputs = new List<Input>();
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "2";
            Input maxInput = new Input("Maximum Percentage");
            inputs.Add(minInput);
            inputs.Add(maxInput);
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Action withinRange = () => algorithm.isWithinDataRange(inputs);
            Assert.Throws<ArgumentOutOfRangeException>(withinRange);
        }
        /// <summary>
        /// Ensures isWithinDataRange throws ArgumentException if minimum percentage is greater than maximum percentage
        /// </summary>
        [Fact]
        public void ensureIsWithinDataRangeThrowsArgumentExceptionIfMinimumPercentageGreaterThanMaximumPercentage() {
            Console.WriteLine("ensureIsWithinDataRangeThrowsArgumentExceptionIfMinimumPercentageGreaterThanMaximumPercentage");
            List<Input> inputs = new List<Input>();
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "0.5";
            Input maxInput = new Input("Maximum Percentage");
            maxInput.value = "0";
            inputs.Add(minInput);
            inputs.Add(maxInput);
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Action withinRange = () => algorithm.isWithinDataRange(inputs);
            Assert.Throws<ArgumentOutOfRangeException>(withinRange);
        }
        /// <summary>
        /// Ensures isWithinDataRange throws ArgumentException if maximum percentage above one
        /// </summary>
        [Fact]
        public void ensureIsWithinDataRangeThrowsArgumentExceptionIfMaximumPercentageAboveOne() {
            Console.WriteLine("ensureIsWithinDataRangeThrowsArgumentExceptionIfMaximumPercentageAboveOne");
            List<Input> inputs = new List<Input>();
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "0.5";
            Input maxInput = new Input("Maximum Percentage");
            maxInput.value = "2";
            inputs.Add(minInput);
            inputs.Add(maxInput);
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Action withinRange = () => algorithm.isWithinDataRange(inputs);
            Assert.Throws<ArgumentOutOfRangeException>(withinRange);
        }
        /// <summary>
        /// Ensures isWithinDataRange succeeds
        /// </summary>
        [Fact]
        public void ensureIsWithinDataRangeSucceeds() {
            Console.WriteLine("ensureIsWithinDataRangeSucceeds");
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "0.1";
            Input maxInput = new Input("Maximum Percentage");
            maxInput.value = "0.2";
            List<Input> inputs = new List<Input>();
            inputs.Add(minInput);
            inputs.Add(maxInput);
            Assert.True(new WidthPercentageAlgorithm().isWithinDataRange(inputs));
        }
    }
}
