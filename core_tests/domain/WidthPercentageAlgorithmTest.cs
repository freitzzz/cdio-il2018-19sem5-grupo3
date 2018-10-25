// using core.domain;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Xunit;

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
        /// <summary>
        /// Ensures method apply removes all single value dimensions that dont fit
        /// </summary>
        [Fact]
        public void ensureApplyRestrictsSingleValueDimensions() {
            Console.WriteLine("ensureApplyRestrictsSingleValueDimensions");
            Material material = new Material("#12", "K6205", new List<Color>(new[] { Color.valueOf("Missing Link of the Annihilator: Absolute Zero", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht") }));
            ProductCategory cat = new ProductCategory("AI");
            List<Dimension> dimensions = new List<Dimension>(new[] { new SingleValueDimension(200), new SingleValueDimension(100), new SingleValueDimension(50) });
            Product product = new Product("#23", "Arclight of the Point at Infinity: Arclight of the Sky", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            Product component = new Product("#16", "Altair of the Point at Infinity: Vega and Altair", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            CustomizedProduct custom = new CustomizedProduct("#17", "Altair of the Hyperbolic Plane: Beltrami Pseudosphere", CustomizedMaterial.valueOf(material, Color.valueOf("Missing Link of the Annihilator: Absolute Zero", 100, 100, 100, 100), Finish.valueOf("der alte wurfelt nicht")), CustomizedDimensions.valueOf(100, 100, 100), product);
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "0.9";
            Input maxInput = new Input("Maximum Percentage");
            maxInput.value = "1.0";
            List<Input> inputs = new List<Input>();
            inputs.Add(minInput);
            inputs.Add(maxInput);
            algorithm.setInputValues(inputs);
            Product alteredProduct = algorithm.apply(custom, component);
            Assert.True(alteredProduct.widthValues.Count == 1);
            double remainingValue = ((SingleValueDimension)alteredProduct.widthValues[0]).value;
            Assert.True(remainingValue == 100);
        }
        /// <summary>
        /// Ensures method apply returns null if the component does not have any compatible dimension
        /// </summary>
        [Fact]
        public void ensureApplyReturnsNullIfComponentDoesNotHaveCompatibleDimensions() {
            Console.WriteLine("ensureApplyReturnsNullIfComponentDoesNotHaveCompatibleDimensions");
            Material material = new Material("#12", "K6205", new List<Color>(new[] { Color.valueOf("Missing Link of the Annihilator: Absolute Zero", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht") }));
            ProductCategory cat = new ProductCategory("AI");
            List<Dimension> dimensions = new List<Dimension>(new[] { new SingleValueDimension(200), new SingleValueDimension(120), new SingleValueDimension(50) });
            Product product = new Product("#23", "Arclight of the Point at Infinity: Arclight of the Sky", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            Product component = new Product("#16", "Altair of the Point at Infinity: Vega and Altair", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            CustomizedProduct custom = new CustomizedProduct("#17", "Altair of the Hyperbolic Plane: Beltrami Pseudosphere", CustomizedMaterial.valueOf(material, Color.valueOf("Missing Link of the Annihilator: Absolute Zero", 100, 100, 100, 100), Finish.valueOf("der alte wurfelt nicht")), CustomizedDimensions.valueOf(100, 100, 100), product);
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "0.9";
            Input maxInput = new Input("Maximum Percentage");
            maxInput.value = "1.0";
            List<Input> inputs = new List<Input>();
            inputs.Add(minInput);
            inputs.Add(maxInput);
            algorithm.setInputValues(inputs);
            Assert.Null(algorithm.apply(custom, component));
        }
        /// <summary>
        /// Ensures method apply removes discrete dimension values outside of set percentages
        /// </summary>
        [Fact]
        public void ensureApplyRemovesValuesFromDiscreteDimensions() {
            Console.WriteLine("ensureApplyRemovesValuesFromDiscreteDimensions");
            Material material = new Material("#24", "K6205", new List<Color>(new[] { Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht") }));
            ProductCategory cat = new ProductCategory("AI");
            DiscreteDimensionInterval discrete = new DiscreteDimensionInterval(new List<double>(new[] { 50.0, 90.0, 100.0, 150.0 }));
            List<Dimension> dimensions = new List<Dimension>();
            dimensions.Add(discrete);
            Product product = new Product("#12", "Mother Goose of Mutual Recursion: Recursive Mother Goose ", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            Product component = new Product("#13", "Mother Goose of Diffractive Recitavo: Diffraction Mother Goose", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            CustomizedProduct custom = new CustomizedProduct("#8", "Dual of Antinomy: Antinomic Dual", CustomizedMaterial.valueOf(material, Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100), Finish.valueOf("der alte wurfelt nicht")), CustomizedDimensions.valueOf(100, 100, 100), product);
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "0.9";
            Input maxInput = new Input("Maximum Percentage");
            maxInput.value = "1.0";
            List<Input> inputs = new List<Input>();
            inputs.Add(minInput);
            inputs.Add(maxInput);
            algorithm.setInputValues(inputs);
            Product alteredProduct = algorithm.apply(custom, component);
            DiscreteDimensionInterval discreteDimension = (DiscreteDimensionInterval)alteredProduct.widthValues[0];
            DiscreteDimensionInterval expected = new DiscreteDimensionInterval(new List<double>(new[] { 90.0, 100.0 }));
            Assert.True(discreteDimension.Equals(expected));
        }
        /// <summary>
        /// Ensures method apply condenses discrete dimensions left with a single value into single value dimensions
        /// </summary>
        [Fact]
        public void ensureApplyCondensesDiscreteDimensions() {
            Console.WriteLine("ensureApplyCondensesDiscreteDimensions");
            Material material = new Material("#12", "K6205", new List<Color>(new[] { Color.valueOf("Silver", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht") }));
            ProductCategory cat = new ProductCategory("AI");
            DiscreteDimensionInterval discrete = new DiscreteDimensionInterval(new List<double>(new[] { 50.0, 100.0, 150.0 }));
            List<Dimension> dimensions = new List<Dimension>();
            dimensions.Add(discrete);
            Product product = new Product("#20", "Rinascimento of the Unwavering Promise: Promised Rinascimento", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            Product component = new Product("#21", "Rinascimento of Image Formation: Return of Phoenix", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            CustomizedProduct custom = new CustomizedProduct("#22", "Rinascimento of Projection: Project Amadeus", CustomizedMaterial.valueOf(material, Color.valueOf("Silver", 100, 100, 100, 100), Finish.valueOf("der alte wurfelt nicht")), CustomizedDimensions.valueOf(100, 100, 100), product);
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "0.9";
            Input maxInput = new Input("Maximum Percentage");
            maxInput.value = "1.0";
            List<Input> inputs = new List<Input>();
            inputs.Add(minInput);
            inputs.Add(maxInput);
            algorithm.setInputValues(inputs);
            Product alteredProduct = algorithm.apply(custom, component);
            Assert.True(alteredProduct.widthValues[0].GetType() == typeof(SingleValueDimension));
            Assert.True(((SingleValueDimension)alteredProduct.widthValues[0]).value == 100);
        }
        /// <summary>
        /// Ensures method apply removes discrete dimensions with no values
        /// </summary>
        [Fact]
        public void ensureApplyRemovesDiscreteDimensions() {
            Console.WriteLine("ensureApplyRemovesDiscreteDimensions");
            Material material = new Material("#12", "K6205", new List<Color>(new[] { Color.valueOf("Durpa", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht") }));
            ProductCategory cat = new ProductCategory("AI");
            DiscreteDimensionInterval discrete = new DiscreteDimensionInterval(new List<double>(new[] { 50.0, 110.0, 150.0 }));
            List<Dimension> dimensions = new List<Dimension>();
            dimensions.Add(discrete);
            Product product = new Product("#9", "Pandora of Eternal Return: Pandora's Box", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            Product component = new Product("#10", "Pandora of Provable Existence: Forbidden Cubicle", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            CustomizedProduct custom = new CustomizedProduct("#11", "Pandora of Forgotten Existence: Sealed Reliquary", CustomizedMaterial.valueOf(material, Color.valueOf("Durpa", 100, 100, 100, 100), Finish.valueOf("der alte wurfelt nicht")), CustomizedDimensions.valueOf(100, 100, 100), product);
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "0.9";
            Input maxInput = new Input("Maximum Percentage");
            maxInput.value = "1.0";
            List<Input> inputs = new List<Input>();
            inputs.Add(minInput);
            inputs.Add(maxInput);
            algorithm.setInputValues(inputs);
            Assert.Null(algorithm.apply(custom, component));
        }
        /// <summary>
        /// Ensures method apply restricts continuous dimensions interval minimum and maximum limit
        /// </summary>
        [Fact]
        public void ensureApplyChangesContinuousDimensionsLimits() {
            Console.WriteLine("ensureApplyChangesContinuousDimensionsLimits");
            Material material = new Material("#12", "K6205", new List<Color>(new[] { Color.valueOf("Open the Missing Link", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht") }));
            ProductCategory cat = new ProductCategory("AI");
            ContinuousDimensionInterval continuous = new ContinuousDimensionInterval(50.0, 150.0, 2.0);
            List<Dimension> dimensions = new List<Dimension>();
            dimensions.Add(continuous);
            Product product = new Product("#18", "Altair of Translational Symmetry: Translational Symmetry", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            Product component = new Product("#19", "Altair of the Cyclic Coordinate: Time-leap Machine", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            CustomizedProduct custom = new CustomizedProduct("#3", "Protocol of the Two-sided Gospel: X-Day Protocol", CustomizedMaterial.valueOf(material, Color.valueOf("Open the Missing Link", 100, 100, 100, 100), Finish.valueOf("der alte wurfelt nicht")), CustomizedDimensions.valueOf(100, 100, 100), product);
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "0.9";
            Input maxInput = new Input("Maximum Percentage");
            maxInput.value = "1.0";
            List<Input> inputs = new List<Input>();
            inputs.Add(minInput);
            inputs.Add(maxInput);
            algorithm.setInputValues(inputs);
            Product alteredProduct = algorithm.apply(custom, component);
            Assert.True(((ContinuousDimensionInterval)alteredProduct.widthValues[0]).minValue == 90);
            Assert.True(((ContinuousDimensionInterval)alteredProduct.widthValues[0]).maxValue == 100);
        }
        /// <summary>
        /// Ensures method apply condenses continuous dimension intervals with same minimum and maximum value into a single value dimension
        /// </summary>
        [Fact]
        public void ensureApplyCondensesContinuousDimensions() {
            Console.WriteLine("ensureApplyCondensesContinuousDimensions");
            Material material = new Material("#12", "K6205", new List<Color>(new[] { Color.valueOf("Open the Steins Gate", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht") }));
            ProductCategory cat = new ProductCategory("AI");
            ContinuousDimensionInterval continuous = new ContinuousDimensionInterval(100.0, 150.0, 2.0);
            List<Dimension> dimensions = new List<Dimension>();
            dimensions.Add(continuous);
            Product product = new Product("#4", "Solitude of the Mournful Flow: A Stray Sheep", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            Product component = new Product("#5", "Solitude of the Astigmatism: Entangled Sheep", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            CustomizedProduct custom = new CustomizedProduct("#24", "Achievement Point", CustomizedMaterial.valueOf(material, Color.valueOf("Open the Steins Gate", 100, 100, 100, 100), Finish.valueOf("der alte wurfelt nicht")), CustomizedDimensions.valueOf(100, 100, 100), product);
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "0.9";
            Input maxInput = new Input("Maximum Percentage");
            maxInput.value = "1.0";
            List<Input> inputs = new List<Input>();
            inputs.Add(minInput);
            inputs.Add(maxInput);
            algorithm.setInputValues(inputs);
            Product alteredProduct = algorithm.apply(custom, component);
            Assert.True(alteredProduct.widthValues[0].GetType() == typeof(SingleValueDimension));
            Assert.True(((SingleValueDimension)alteredProduct.widthValues[0]).value == 100);
        }
        /// <summary>
        /// Ensures method apply removes continuous dimension intervals whose interval does not fit allowed dimension
        /// </summary>
        [Fact]
        public void ensureApplyRemovesContinuousDimensions() {
            Console.WriteLine("ensureApplyRemovesContinuousDimensions");
            Material material = new Material("#12", "K6205", new List<Color>(new[] { Color.valueOf("Open the Steins Gate", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht") }));
            ProductCategory cat = new ProductCategory("AI");
            ContinuousDimensionInterval continuous1 = new ContinuousDimensionInterval(110.0, 150.0, 2.0);
            ContinuousDimensionInterval continuous2 = new ContinuousDimensionInterval(50.0, 80.0, 2.0);
            List<Dimension> dimensions = new List<Dimension>();
            dimensions.Add(continuous1);
            dimensions.Add(continuous2);
            Product product = new Product("#4", "Solitude of the Mournful Flow: A Stray Sheep", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            Product component = new Product("#5", "Solitude of the Astigmatism: Entangled Sheep", cat, new List<Material>(new[] { material }), dimensions, dimensions, dimensions);
            CustomizedProduct custom = new CustomizedProduct("#24", "Achievement Point", CustomizedMaterial.valueOf(material, Color.valueOf("Open the Steins Gate", 100, 100, 100, 100), Finish.valueOf("der alte wurfelt nicht")), CustomizedDimensions.valueOf(100, 100, 100), product);
            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Input minInput = new Input("Minimum Percentage");
            minInput.value = "0.9";
            Input maxInput = new Input("Maximum Percentage");
            maxInput.value = "1.0";
            List<Input> inputs = new List<Input>();
            inputs.Add(minInput);
            inputs.Add(maxInput);
            algorithm.setInputValues(inputs);
            Assert.Null(algorithm.apply(custom, component));
        }
    }
}
