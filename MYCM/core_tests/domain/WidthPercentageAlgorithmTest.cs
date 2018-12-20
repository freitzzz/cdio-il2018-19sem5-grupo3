﻿//TODO: FIX ME
// using core.domain;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Xunit;
// using static core.domain.CustomizedProduct;

// namespace core_tests.domain
// {
//     public class WidthPercentageAlgorithmTest
//     {
//         /// <summary>
//         /// Ensures getRequiredInputs returns the correct list of inputs
//         /// </summary>
//         [Fact]
//         public void ensureGetRequiredInputsSucceeds()
//         {
//             List<Input> inputs = new WidthPercentageAlgorithm().getRequiredInputs();
//             Assert.True(2 == inputs.Count());
//             Assert.Equal("Minimum Percentage", inputs.ElementAt(0).name);
//             Assert.Equal("Maximum Percentage", inputs.ElementAt(1).name);
//         }
//         /// <summary>
//         /// Ensures setInputValues throws ArgumentException if argument is null
//         /// </summary>
//         [Fact]
//         public void ensureSetInputValuesThrowsArgumentExceptionIfArgumentNull()
//         {
//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Action setValues = () => algorithm.setInputValues(null);
//             Assert.Throws<ArgumentException>(setValues);
//         }
//         /// <summary>
//         /// Ensures setInputValues throws ArgumentException if argument is empty list
//         /// </summary>
//         [Fact]
//         public void ensureSetInputValuesThrowsArgumentExceptionIfArgumentIsEmpty()
//         {
//             List<Input> inputs = new List<Input>();
//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Action setValues = () => algorithm.setInputValues(inputs);
//             Assert.Throws<ArgumentException>(setValues);
//         }
//         /// <summary>
//         /// Ensures setInputValues throws ArgumentException if list has more than the required inputs
//         /// </summary>
//         [Fact]
//         public void ensureSetInputValuesThrowsArgumentExceptionIfListTooBig()
//         {
//             List<Input> inputs = new List<Input>();
//             inputs.Add(new Input("Altair"));
//             inputs.Add(new Input("Vega"));
//             inputs.Add(new Input("Deneb"));
//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Action setValues = () => algorithm.setInputValues(inputs);
//             Assert.Throws<ArgumentException>(setValues);
//         }
//         /// <summary>
//         /// Ensures setInputValues succeeds
//         /// </summary>
//         [Fact]
//         public void ensureSetInputValuesSucceeds()
//         {
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "0.1";
//             Input maxInput = new Input("Maximum Percentage");
//             maxInput.value = "0.2";
//             List<Input> inputs = new List<Input>();
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             Assert.True(new WidthPercentageAlgorithm().setInputValues(inputs));
//         }
//         /// <summary>
//         /// Ensures isWithinDataRange throws ArgumentException if argument is null
//         /// </summary>
//         [Fact]
//         public void ensureIsWithinDataRangeThrowsArgumentExceptionIfArgumentNull()
//         {
//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Action withinRange = () => algorithm.isWithinDataRange(null);
//             Assert.Throws<ArgumentException>(withinRange);
//         }
//         /// <summary>
//         /// Ensures isWithinDataRange throws ArgumentException if argument is empty list
//         /// </summary>
//         [Fact]
//         public void ensureIsWithinDataRangeThrowsArgumentExceptionIfArgumentIsEmpty()
//         {
//             List<Input> inputs = new List<Input>();
//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Action withinRange = () => algorithm.isWithinDataRange(inputs);
//             Assert.Throws<ArgumentException>(withinRange);
//         }
//         /// <summary>
//         /// Ensures isWithinDataRange throws ArgumentException if list has incorrect number of inputs
//         /// </summary>
//         [Fact]
//         public void ensureIsWithinDataRangeThrowsArgumentExceptionIfIncorretNumberOfArguments()
//         {
//             List<Input> inputs = new List<Input>();
//             inputs.Add(new Input("Altair"));
//             inputs.Add(new Input("Vega"));
//             inputs.Add(new Input("Deneb"));
//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Action withinRange = () => algorithm.isWithinDataRange(inputs);
//             Assert.Throws<ArgumentException>(withinRange);
//         }
//         /// <summary>
//         /// Ensures isWithinDataRange throws FormatException if any input value is not double
//         /// </summary>
//         [Fact]
//         public void ensureIsWithinDataRangeFailsIfValueIsNotDouble()
//         {
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "AAAA";
//             Input maxInput = new Input("Maximum Percentage");
//             maxInput.value = "BBBB";
//             List<Input> inputs = new List<Input>();
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             Action withinRange = () => new WidthPercentageAlgorithm().isWithinDataRange(inputs);
//             Assert.Throws<FormatException>(withinRange);
//         }
//         /// <summary>
//         /// Ensures isWithinDataRange throws ArgumentException if any input has incorrect name
//         /// </summary>
//         [Fact]
//         public void ensureIsWithinDataRangeThrowsArgumentExceptionIfInputNamesAreIncorrect()
//         {
//             List<Input> inputs = new List<Input>();
//             inputs.Add(new Input("Altair"));
//             inputs.Add(new Input("Vega"));
//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Action withinRange = () => algorithm.isWithinDataRange(inputs);
//             Assert.Throws<ArgumentException>(withinRange);
//         }
//         /// <summary>
//         /// Ensures isWithinDataRange throws ArgumentException any input name is null or empty
//         /// </summary>
//         [Fact]
//         public void ensureIsWithinDataRangeThrowsArgumentExceptionIfInputNameNullOrEmpty()
//         {
//             List<Input> inputs = new List<Input>();
//             Input minInput = new Input("Minimum Percentage");
//             minInput.name = "";
//             Input maxInput = new Input("Maximum Percentage");
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Action withinRange = () => algorithm.isWithinDataRange(inputs);
//             Assert.Throws<ArgumentException>(withinRange);
//         }
//         /// <summary>
//         /// Ensures isWithinDataRange throws ArgumentException input minimum percentage value is below zero
//         /// </summary>
//         [Fact]
//         public void ensureIsWithinDataRangeThrowsArgumentExceptionIfMinimumPercentageBelowZero()
//         {
//             List<Input> inputs = new List<Input>();
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "-1";
//             Input maxInput = new Input("Maximum Percentage");
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Action withinRange = () => algorithm.isWithinDataRange(inputs);
//             Assert.Throws<ArgumentOutOfRangeException>(withinRange);
//         }
//         /// <summary>
//         /// Ensures isWithinDataRange throws ArgumentException if minimum percentage value is above one
//         /// </summary>
//         [Fact]
//         public void ensureIsWithinDataRangeThrowsArgumentExceptionIfMinimumPercentageAboveOne()
//         {
//             List<Input> inputs = new List<Input>();
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "2";
//             Input maxInput = new Input("Maximum Percentage");
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Action withinRange = () => algorithm.isWithinDataRange(inputs);
//             Assert.Throws<ArgumentOutOfRangeException>(withinRange);
//         }
//         /// <summary>
//         /// Ensures isWithinDataRange throws ArgumentException if minimum percentage is greater than maximum percentage
//         /// </summary>
//         [Fact]
//         public void ensureIsWithinDataRangeThrowsArgumentExceptionIfMinimumPercentageGreaterThanMaximumPercentage()
//         {
//             List<Input> inputs = new List<Input>();
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "0.5";
//             Input maxInput = new Input("Maximum Percentage");
//             maxInput.value = "0";
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Action withinRange = () => algorithm.isWithinDataRange(inputs);
//             Assert.Throws<ArgumentOutOfRangeException>(withinRange);
//         }
//         /// <summary>
//         /// Ensures isWithinDataRange throws ArgumentException if maximum percentage above one
//         /// </summary>
//         [Fact]
//         public void ensureIsWithinDataRangeThrowsArgumentExceptionIfMaximumPercentageAboveOne()
//         {
//             List<Input> inputs = new List<Input>();
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "0.5";
//             Input maxInput = new Input("Maximum Percentage");
//             maxInput.value = "2";
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Action withinRange = () => algorithm.isWithinDataRange(inputs);
//             Assert.Throws<ArgumentOutOfRangeException>(withinRange);
//         }
//         /// <summary>
//         /// Ensures isWithinDataRange succeeds
//         /// </summary>
//         [Fact]
//         public void ensureIsWithinDataRangeSucceeds()
//         {
//             Console.WriteLine("ensureIsWithinDataRangeSucceeds");
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "0.1";
//             Input maxInput = new Input("Maximum Percentage");
//             maxInput.value = "0.2";
//             List<Input> inputs = new List<Input>();
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             Assert.True(new WidthPercentageAlgorithm().isWithinDataRange(inputs));
//         }
//         /// <summary>
//         /// Ensures method apply removes all single value dimensions that dont fit
//         /// </summary>
//         [Fact]
//         public void ensureApplyRestrictsSingleValueDimensions()
//         {
//             Finish finish = Finish.valueOf("der alte wurfelt nicht", 20);
//             Color color = Color.valueOf("Missing Link of the Annihilator: Absolute Zero", 100, 100, 100, 100);

//             Material material = new Material("#12", "K6205", "12.jpg", new List<Color>() { color }, new List<Finish>() { finish });
//             ProductCategory cat = new ProductCategory("AI");
//             Measurement measurement = new Measurement(new SingleValueDimension(200), new SingleValueDimension(100), new SingleValueDimension(50));
//             Measurement measurement1 = new Measurement(new SingleValueDimension(100), new SingleValueDimension(200), new SingleValueDimension(50));
//             Measurement measurement2 = new Measurement(new SingleValueDimension(100), new SingleValueDimension(50), new SingleValueDimension(200));

//             List<Measurement> measurements = new List<Measurement>() { measurement, measurement1, measurement2 };
//             Product component = new Product("#16", "Altair of the Point at Infinity: Vega and Altair", "16.gltf", cat, new List<Material>(new[] { material }), measurements);
//             Product product = new Product("#23", "Arclight of the Point at Infinity: Arclight of the Sky", "23.glb", cat, new List<Material>(new[] { material }), measurements, new List<Product>(){component});


//             CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(200, 100, 50);
//             CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);
//             CustomizedProduct customizedProduct = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("12345", product, customizedProductDimensions).withMaterial(customizedMaterial).build();

//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "0.9";
//             Input maxInput = new Input("Maximum Percentage");
//             maxInput.value = "1.0";
//             List<Input> inputs = new List<Input>();
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             algorithm.setInputValues(inputs);
//             Product alteredProduct = algorithm.apply(customizedProduct, component);
//             Assert.Single(alteredProduct.productMeasurements);
//             double remainingValue = ((SingleValueDimension)alteredProduct.productMeasurements[0].measurement.width).value;
//             Assert.True(remainingValue == 100);
//         }
//         /// <summary>
//         /// Ensures method apply returns null if the component does not have any compatible dimension
//         /// </summary>
//         [Fact]
//         public void ensureApplyReturnsNullIfComponentDoesNotHaveCompatibleDimensions()
//         {

//             Finish finish = Finish.valueOf("der alte wurfelt nicht", 20);
//             Color color = Color.valueOf("Missing Link of the Annihilator: Absolute Zero", 100, 100, 100, 100);

//             Material material = new Material("#12", "K6205", "12.jpg", new List<Color>() { color }, new List<Finish>() { finish });
//             ProductCategory cat = new ProductCategory("AI");
//             Measurement measurement = new Measurement(new SingleValueDimension(200), new SingleValueDimension(120), new SingleValueDimension(50));
//             Measurement measurement1 = new Measurement(new SingleValueDimension(120), new SingleValueDimension(200), new SingleValueDimension(50));
//             Measurement measurement2 = new Measurement(new SingleValueDimension(120), new SingleValueDimension(50), new SingleValueDimension(200));

//             List<Measurement> measurements = new List<Measurement>() { measurement, measurement1 };
//             List<Measurement> measurements2 = new List<Measurement>() { measurement2 };

//             Product component = new Product("#16", "Altair of the Point at Infinity: Vega and Altair", "16.glb", cat, new List<Material>(new[] { material }), measurements2);
//             Product product = new Product("#23", "Arclight of the Point at Infinity: Arclight of the Sky", "23.glb", cat, new List<Material>(new[] { material }), measurements, new List<Product>(){component});
            
//             CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);
//             CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(200, 120, 50);
//             CustomizedProduct custom = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("12345", product, customizedProductDimensions).withMaterial(customizedMaterial).build();

//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "0.9";
//             Input maxInput = new Input("Maximum Percentage");
//             maxInput.value = "1.0";
//             List<Input> inputs = new List<Input>();
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             algorithm.setInputValues(inputs);
//             Assert.Null(algorithm.apply(custom, component));
//         }
//         /// <summary>
//         /// Ensures method apply removes discrete dimension values outside of set percentages
//         /// </summary>
//         [Fact]
//         public void ensureApplyRemovesValuesFromDiscreteDimensions()
//         {
//             Color color = Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100);
//             Finish finish = Finish.valueOf("der alte wurfelt nicht", 20);

//             Material material = new Material("#24", "K6205", "12.jpg", new List<Color>(new[] { color }), new List<Finish>(new[] { finish }));
//             ProductCategory cat = new ProductCategory("AI");
//             DiscreteDimensionInterval discrete = new DiscreteDimensionInterval(new List<double>(new[] { 50.0, 90.0, 100.0, 150.0 }));
//             Measurement measurement = new Measurement(new SingleValueDimension(200), discrete, new SingleValueDimension(50));

//             List<Measurement> measurements = new List<Measurement>() { measurement };

//             Product component = new Product("#13", "Mother Goose of Diffractive Recitavo: Diffraction Mother Goose", "13.glb", cat, new List<Material>(new[] { material }), measurements);
//             Product product = new Product("#12", "Mother Goose of Mutual Recursion: Recursive Mother Goose ", "12.fbx", cat, new List<Material>(new[] { material }), measurements, new List<Product>(){component});

//             CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);
//             CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(200, 100, 50);
//             CustomizedProduct custom = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("12345", product, customizedDimensions).withMaterial(customizedMaterial).build();

//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "0.9";
//             Input maxInput = new Input("Maximum Percentage");
//             maxInput.value = "1.0";
//             List<Input> inputs = new List<Input>();
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             algorithm.setInputValues(inputs);
//             Product alteredProduct = algorithm.apply(custom, component);
//             DiscreteDimensionInterval discreteDimension = (DiscreteDimensionInterval)alteredProduct.productMeasurements[0].measurement.width;
//             DiscreteDimensionInterval expected = new DiscreteDimensionInterval(new List<double>(new[] { 90.0, 100.0 }));
//             Assert.True(discreteDimension.Equals(expected));
//         }
//         /// <summary>
//         /// Ensures method apply condenses discrete dimensions left with a single value into single value dimensions
//         /// </summary>
//         [Fact]
//         public void ensureApplyCondensesDiscreteDimensions()
//         {
//             Color color = Color.valueOf("Silver", 100, 100, 100, 100);
//             Finish finish = Finish.valueOf("der alte wurfelt nicht", 20);
//             Material material = new Material("#12", "K6205", "12.jpg", new List<Color>(new[] { color }), new List<Finish>(new[] { finish }));
//             ProductCategory cat = new ProductCategory("AI");
//             DiscreteDimensionInterval discrete = new DiscreteDimensionInterval(new List<double>(new[] { 50.0, 100.0, 150.0 }));
//             Measurement measurement = new Measurement(new SingleValueDimension(200), discrete, new SingleValueDimension(50));
//             List<Measurement> measurements = new List<Measurement>() { measurement };

//             Product component = new Product("#21", "Rinascimento of Image Formation: Return of Phoenix", "21.glb", cat, new List<Material>(new[] { material }), measurements);
//             Product product = new Product("#20", "Rinascimento of the Unwavering Promise: Promised Rinascimento", "20.gltf", cat, new List<Material>(new[] { material }), measurements, new List<Product>(){component});

//             CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);
//             CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(200, 100, 50);
//             CustomizedProduct custom = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("1235", product, customizedDimensions).withMaterial(customizedMaterial).build();

//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "0.9";
//             Input maxInput = new Input("Maximum Percentage");
//             maxInput.value = "1.0";
//             List<Input> inputs = new List<Input>();
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             algorithm.setInputValues(inputs);
//             Product alteredProduct = algorithm.apply(custom, component);
//             Assert.True(alteredProduct.productMeasurements[0].measurement.width.GetType() == typeof(SingleValueDimension));
//             Assert.True(((SingleValueDimension)alteredProduct.productMeasurements[0].measurement.width).value == 100);
//         }
//         /// <summary>
//         /// Ensures method apply removes discrete dimensions with no values
//         /// </summary>
//         [Fact]
//         public void ensureApplyRemovesDiscreteDimensions()
//         {
//             Color color = Color.valueOf("Durpa", 100, 100, 100, 100);
//             Finish finish = Finish.valueOf("der alte wurfelt nicht", 35);
//             Material material = new Material("#12", "K6205", "12.jpg", new List<Color>(new[] { color }), new List<Finish>(new[] { finish }));
//             ProductCategory cat = new ProductCategory("AI");
//             DiscreteDimensionInterval discrete = new DiscreteDimensionInterval(new List<double>(new[] { 50.0, 110.0, 150.0 }));
//             DiscreteDimensionInterval discrete2 = new DiscreteDimensionInterval(new List<double>(new[] { 50.0, 150.0, 150.0 }));

//             Measurement measurement = new Measurement(new SingleValueDimension(200), discrete, new SingleValueDimension(50));
//             Measurement measurement2 = new Measurement(new SingleValueDimension(200), discrete2, new SingleValueDimension(50));

//             List<Measurement> measurements = new List<Measurement>() { measurement };
//             List<Measurement> measurements2 = new List<Measurement>() { measurement2 };
//             Product component = new Product("#10", "Pandora of Provable Existence: Forbidden Cubicle", "10.gltf", cat, new List<Material>(new[] { material }), measurements2);
//             Product product = new Product("#9", "Pandora of Eternal Return: Pandora's Box", "9.fbx", cat, new List<Material>(new[] { material }), measurements, new List<Product>(){component});

//             CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(200, 110, 50);
//             CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);
//             CustomizedProduct custom = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("12345", product, customizedDimensions).withMaterial(customizedMaterial).build();

//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "0.9";
//             Input maxInput = new Input("Maximum Percentage");
//             maxInput.value = "1.0";
//             List<Input> inputs = new List<Input>();
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             algorithm.setInputValues(inputs);
//             Assert.Null(algorithm.apply(custom, component));
//         }
//         /// <summary>
//         /// Ensures method apply restricts continuous dimensions interval minimum and maximum limit
//         /// </summary>
//         [Fact]
//         public void ensureApplyChangesContinuousDimensionsLimits()
//         {
//             Color color = Color.valueOf("Open the Missing Link", 100, 100, 100, 100);
//             Finish finish = Finish.valueOf("der alte wurfelt nicht", 20);
//             Material material = new Material("#12", "K6205", "12.png", new List<Color>(new[] { color }), new List<Finish>(new[] { finish }));
//             ProductCategory cat = new ProductCategory("AI");
//             ContinuousDimensionInterval continuous = new ContinuousDimensionInterval(50.0, 150.0, 2.0);
//             Measurement measurement = new Measurement(new SingleValueDimension(200), continuous, new SingleValueDimension(50));
//             List<Measurement> measurements = new List<Measurement>() { measurement };

//             Product component = new Product("#19", "Altair of the Cyclic Coordinate: Time-leap Machine", "19.glb", cat, new List<Material>(new[] { material }), measurements);

//             Product product = new Product("#18", "Altair of Translational Symmetry: Translational Symmetry", "18.glb", cat, new List<Material>(new[] { material }), measurements,
//                 new List<Product>() { component });

//             CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(200, 100, 50);
//             CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);
//             CustomizedProduct custom = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("12345", product, customizedDimensions).withMaterial(customizedMaterial).build();
            
//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "0.9";
//             Input maxInput = new Input("Maximum Percentage");
//             maxInput.value = "1.0";
//             List<Input> inputs = new List<Input>();
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             algorithm.setInputValues(inputs);
//             Product alteredProduct = algorithm.apply(custom, component);
//             Assert.True(alteredProduct.productMeasurements[0].measurement.width.getMinValue() == 90);
//             Assert.True(alteredProduct.productMeasurements[0].measurement.width.getMaxValue() == 100);
//         }
//         /// <summary>
//         /// Ensures method apply condenses continuous dimension intervals with same minimum and maximum value into a single value dimension
//         /// </summary>
//         [Fact]
//         public void ensureApplyCondensesContinuousDimensions() {
//             Color color = Color.valueOf("Open the Steins Gate", 100, 100, 100, 100);
//             Finish finish = Finish.valueOf("der alte wurfelt nicht", 45);
//             Material material = new Material("#12", "K6205", "12.jpg", new List<Color>(new[] { color }), new List<Finish>(new[] { finish }));

//             ProductCategory cat = new ProductCategory("AI");
//             ContinuousDimensionInterval continuous = new ContinuousDimensionInterval(100.0, 150.0, 2.0);
//             Measurement measurement = new Measurement(new SingleValueDimension(200), continuous, new SingleValueDimension(50));
//             List<Measurement> measurements = new List<Measurement>() { measurement };
//             Product component = new Product("#5", "Solitude of the Astigmatism: Entangled Sheep", "5.gltf", cat, new List<Material>(new[] { material }), measurements);
//             Product product = new Product("#4", "Solitude of the Mournful Flow: A Stray Sheep", "4.fbx", cat, new List<Material>(new[] { material }), measurements, new List<Product>(){component});

//             CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(200, 100, 50);
//             CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish); 
//             CustomizedProduct custom = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("12345", product, customizedDimensions).withMaterial(customizedMaterial).build();

//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "0.9";
//             Input maxInput = new Input("Maximum Percentage");
//             maxInput.value = "1.0";
//             List<Input> inputs = new List<Input>();
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             algorithm.setInputValues(inputs);
//             Product alteredProduct = algorithm.apply(custom, component);
//             Assert.True(alteredProduct.productMeasurements[0].measurement.width.GetType() == typeof(SingleValueDimension));
//             Assert.True(((SingleValueDimension)alteredProduct.productMeasurements[0].measurement.width).value == 100);
//         }
//         /// <summary>
//         /// Ensures method apply removes continuous dimension intervals whose interval does not fit allowed dimension
//         /// </summary>
//         [Fact]
//         public void ensureApplyRemovesContinuousDimensions() {
//             Color color = Color.valueOf("Open the Steins Gate", 100, 100, 100, 100);
//             Finish finish = Finish.valueOf("der alte wurfelt nicht", 15);
//             Material material = new Material("#12", "K6205", "12.jpg", new List<Color>(new[] { color }), new List<Finish>(new[] { finish }));
//             ProductCategory cat = new ProductCategory("AI");
//             ContinuousDimensionInterval continuous1 = new ContinuousDimensionInterval(110.0, 150.0, 2.0);
//             ContinuousDimensionInterval continuous2 = new ContinuousDimensionInterval(50.0, 80.0, 2.0);
//             Measurement measurement1 = new Measurement(continuous1, continuous1, continuous1);
//             Measurement measurement2 = new Measurement(continuous2, continuous2, continuous2);
//             ContinuousDimensionInterval continuous3 = new ContinuousDimensionInterval(35.0, 45.0, 1.0);
//             ContinuousDimensionInterval continuous4 = new ContinuousDimensionInterval(10.0, 20.0, 2.0);
//             Measurement measurement3 = new Measurement(continuous3, continuous3, continuous3);
//             Measurement measurement4 = new Measurement(continuous4, continuous4, continuous4);
//             List<Measurement> measurements = new List<Measurement>() { measurement1, measurement2 };
//             List<Measurement> measurements2 = new List<Measurement>() { measurement3, measurement4 };

//             Product component = new Product("#5", "Solitude of the Astigmatism: Entangled Sheep", "5.glb", cat, new List<Material>(new[] { material }), measurements2);
//             Product product = new Product("#4", "Solitude of the Mournful Flow: A Stray Sheep", "4.gltf", cat, new List<Material>(new[] { material }), measurements, new List<Product>(){component});

//             CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(110, 110, 110);
//             CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);
//             CustomizedProduct custom = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("12345", product, customizedDimensions).withMaterial(customizedMaterial).build();
//             WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
//             Input minInput = new Input("Minimum Percentage");
//             minInput.value = "0.9";
//             Input maxInput = new Input("Maximum Percentage");
//             maxInput.value = "1.0";
//             List<Input> inputs = new List<Input>();
//             inputs.Add(minInput);
//             inputs.Add(maxInput);
//             algorithm.setInputValues(inputs);
//             Assert.Null(algorithm.apply(custom, component));
//         }
//     }
// }
