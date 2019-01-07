using core.domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static core.domain.CustomizedProduct;

namespace core_tests.domain {
    public class SameMaterialAndFinishAlgorithmTest {
        /// <summary>
        /// Ensures getRequiredInputs returns an empty list due to the algorithm not needing any inputs
        /// </summary>
        [Fact]
        public void ensureGetRequiredInputsReturnsEmptyList() {
            Console.WriteLine("ensureGetRequiredInputsReturnsNull");
            Algorithm alg = new AlgorithmFactory().createAlgorithm(RestrictionAlgorithm.SAME_MATERIAL_AND_FINISH_ALGORITHM);
            Assert.Empty(alg.getRequiredInputs());
        }
        [Fact]
        public void ensureSetInputValueFails() {
            Algorithm alg = new AlgorithmFactory().createAlgorithm(RestrictionAlgorithm.SAME_MATERIAL_AND_FINISH_ALGORITHM);
            Input input = Input.valueOf("one", "number 1");
            Action action = () => alg.setInputValue(input, "1");
            Assert.Throws<ArgumentException>(action);
        }
        [Fact]
        public void ensureSetInputValuesFails() {
            Input input1 = Input.valueOf("one", "number 1");
            Input input2 = Input.valueOf("two", "number 2");
            Algorithm alg = new AlgorithmFactory().createAlgorithm(RestrictionAlgorithm.SAME_MATERIAL_AND_FINISH_ALGORITHM);
            Dictionary<Input, string> inputValues = new Dictionary<Input, string>();
            inputValues.Add(input1, "1");
            inputValues.Add(input2, "2");
            Action action = () => alg.setInputValues(inputValues);
            Assert.Throws<ArgumentException>(action);
        }
        /// <summary>
        /// Ensure apply method returns null if the component does not have the material required
        /// </summary>
        [Fact]
        public void ensureApplyReturnsNullIfComponentDoesNotHaveRequiredMaterial() {
            Console.WriteLine("ensureApplyReturnsNullIfComponentDoesNotHaveRequiredMaterial");
            Material material = new Material("#24", "K6205", "ola.jpg", new List<Color>(new[] { Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht", 12) }));
            Material material1 = new Material("#22", "Amadeus", "ola.jpg", new List<Color>(new[] { Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht", 12) }));
            ProductCategory cat = new ProductCategory("AI");
            DiscreteDimensionInterval discrete = new DiscreteDimensionInterval(new List<double>(new[] { 50.0, 90.0, 100.0, 150.0 }));
            Measurement measurement = new Measurement(discrete, discrete, discrete);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            Product product = new Product("#12", "Mother Goose of Mutual Recursion: Recursive Mother Goose", "product12.glb", cat, new List<Material>(new[] { material }), measurements);
            Product component = new Product("#13", "Mother Goose of Diffractive Recitavo: Diffraction Mother Goose", "product13.gltf", cat, new List<Material>(new[] { material1 }), measurements);
            CustomizedProduct custom = CustomizedProductBuilder.createCustomizedProduct("#8", product, CustomizedDimensions.valueOf(100, 100, 100)).withMaterial(CustomizedMaterial.valueOf(material, Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100), Finish.valueOf("der alte wurfelt nicht", 12))).build();

            Algorithm algorithm = new AlgorithmFactory().createAlgorithm(RestrictionAlgorithm.SAME_MATERIAL_AND_FINISH_ALGORITHM);
            Assert.Null(algorithm.apply(custom, component));
        }
        /// <summary>
        /// Ensures apply method returns null if component material does not have the required finish
        /// </summary>
        [Fact]
        public void ensureApplyReturnsNullIfComponentDoesNotHaveRequiredFinish() {
            Console.WriteLine("ensureApplyReturnsNullIfComponentDoesNotHaveRequiredFinish");
            Color color = Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100);
            Finish finish = Finish.valueOf("der alte wurfelt nicht", 12);
            Material material = new Material("#24", "K6205", "ola.jpg", new List<Color>(new[] { color }), new List<Finish>(new[] { finish, Finish.valueOf("schrödinger's box", 12) }));
            Material otherMaterial = new Material("#24", "K6205", "ola.jpg", new List<Color>(new[] { color }), new List<Finish>(new[] { Finish.valueOf("schrödinger's box", 12) }));
            ProductCategory cat = new ProductCategory("AI");
            DiscreteDimensionInterval discrete = new DiscreteDimensionInterval(new List<double>(new[] { 50.0, 90.0, 100.0, 150.0 }));
            Measurement measurement = new Measurement(discrete, discrete, discrete);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            Product product = new Product("#12", "Mother Goose of Mutual Recursion: Recursive Mother Goose", "product12.glb", cat, new List<Material>(new[] { material }), measurements);
            Product component = new Product("#13", "Mother Goose of Diffractive Recitavo: Diffraction Mother Goose", "product13.gltf", cat, new List<Material>(new[] { otherMaterial }), measurements);
            CustomizedProduct custom = CustomizedProductBuilder.createCustomizedProduct("#8", product, CustomizedDimensions.valueOf(100, 100, 100)).withMaterial(CustomizedMaterial.valueOf(material, Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100), Finish.valueOf("der alte wurfelt nicht", 12))).build();
            Algorithm algorithm = new AlgorithmFactory().createAlgorithm(RestrictionAlgorithm.SAME_MATERIAL_AND_FINISH_ALGORITHM);
            Assert.Null(algorithm.apply(custom, component));
        }
        /// <summary>
        /// Ensure apply method removes all unnecessary materials from component
        /// </summary>
        [Fact]
        public void ensureApplyRemovesUnnecessaryMaterials() {
            Console.WriteLine("ensureApplyRemovesUnnecessaryMaterials");
            Material material = new Material("#24", "K6205", "ola.jpg", new List<Color>(new[] { Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht", 12) }));
            Material material1 = new Material("#22", "Amadeus", "ola.jpg", new List<Color>(new[] { Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht", 12) }));
            ProductCategory cat = new ProductCategory("AI");
            DiscreteDimensionInterval discrete = new DiscreteDimensionInterval(new List<double>(new[] { 50.0, 90.0, 100.0, 150.0 }));
            Measurement measurement = new Measurement(discrete, discrete, discrete);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            Product product = new Product("#12", "Mother Goose of Mutual Recursion: Recursive Mother Goose", "product12.glb", cat, new List<Material>(new[] { material }), measurements);
            Product component = new Product("#13", "Mother Goose of Diffractive Recitavo: Diffraction Mother Goose", "product13.gltf", cat, new List<Material>(new[] { material1, material }), measurements);
            CustomizedProduct custom = CustomizedProductBuilder.createCustomizedProduct("#8", product, CustomizedDimensions.valueOf(100, 100, 100)).withMaterial(CustomizedMaterial.valueOf(material, Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100), Finish.valueOf("der alte wurfelt nicht", 12))).build();
            Algorithm algorithm = new AlgorithmFactory().createAlgorithm(RestrictionAlgorithm.SAME_MATERIAL_AND_FINISH_ALGORITHM);
            Assert.True(algorithm.apply(custom, component).productMaterials[0].material.Equals(material));
        }
        /// <summary>
        /// Ensures apply method removes all unnecessary finishes from component's material
        /// </summary>
        [Fact]
        public void ensureApplyRemovesUnnecessaryFinishes() {
            Console.WriteLine("ensureApplyRemovesUnnecessaryFinishes");
            Material material = new Material("#24", "K6205", "ola.jpg", new List<Color>(new[] { Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht", 12), Finish.valueOf("schrödinger's box", 13) }));
            ProductCategory cat = new ProductCategory("AI");
            DiscreteDimensionInterval discrete = new DiscreteDimensionInterval(new List<double>(new[] { 50.0, 90.0, 100.0, 150.0 }));
            Measurement measurement = new Measurement(discrete, discrete, discrete);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            Product product = new Product("#12", "Mother Goose of Mutual Recursion: Recursive Mother Goose", "product12.glb", cat, new List<Material>(new[] { material }), measurements);
            Product component = new Product("#13", "Mother Goose of Diffractive Recitavo: Diffraction Mother Goose", "product13.gltf", cat, new List<Material>(new[] { material }), measurements);
            CustomizedProduct custom = CustomizedProductBuilder.createCustomizedProduct("#8", product, CustomizedDimensions.valueOf(100, 100, 100)).withMaterial(CustomizedMaterial.valueOf(material, Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100), Finish.valueOf("schrödinger's box", 13))).build();
            Algorithm algorithm = new AlgorithmFactory().createAlgorithm(RestrictionAlgorithm.SAME_MATERIAL_AND_FINISH_ALGORITHM);
            Assert.Equal("schrödinger's box", algorithm.apply(custom, component).productMaterials[0].material.Finishes[0].description);
        }
    }
}
