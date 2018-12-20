using System;
using Xunit;
using core.domain;
using System.Collections.Generic;
using static core.domain.CustomizedProduct;

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
        public void ensureCreationFailsIfAlgorithmIsNull() {
            Action action = () => new Restriction("description", null);
            Assert.Throws<ArgumentNullException>(action);
        }
        [Fact]
        public void ensureCreationFailsIfAlgorithmIsNotReady() {
            Action action = () => new Restriction("description", new WidthPercentageAlgorithm());
            Assert.Throws<ArgumentNullException>(action);
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
            Restriction instance = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());

            Assert.False(instance.Equals("bananas"));
        }

        [Fact]
        public void ensureInstanceWithDifferentDescriptionIsntEqual() {
            Restriction instance = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());
            Restriction other = new Restriction("bananas", new SameMaterialAndFinishAlgorithm());

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureInstanceWithSameDescriptionIsEqual() {
            Restriction instance = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());
            Restriction other = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureEqualRestrictionsHaveSameHashCode() {
            Restriction instance = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());
            Restriction other = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());

            Assert.True(instance.GetHashCode().Equals(other.GetHashCode()));
        }

        [Fact]
        public void ensureDifferentRestrictionsHaveDifferentHashCode() {
            Restriction instance = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());
            Restriction other = new Restriction("bananas", new SameMaterialAndFinishAlgorithm());

            Assert.False(instance.GetHashCode().Equals(other.GetHashCode()));
        }

        [Fact]
        public void ensureToStringWorks() {
            Restriction instance = new Restriction("oh hi mark", new SameMaterialAndFinishAlgorithm());
            Restriction other = new Restriction("oh hi mark", new SameMaterialAndFinishAlgorithm());
            Assert.Equal(instance.ToString(), other.ToString());
        }

        [Fact]
        public void ensureApplyWorks() {
            Restriction instance = new Restriction("oh hi mark", new SameMaterialAndFinishAlgorithm());
            Material material = new Material("#24", "K6205", "ola.jpg", new List<Color>(new[] { Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht", 12) }));
            Material material1 = new Material("#22", "Amadeus", "ola.jpg", new List<Color>(new[] { Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100) }), new List<Finish>(new[] { Finish.valueOf("der alte wurfelt nicht", 12) }));
            ProductCategory cat = new ProductCategory("AI");
            DiscreteDimensionInterval discrete = new DiscreteDimensionInterval(new List<double>(new[] { 50.0, 90.0, 100.0, 150.0 }));
            Measurement measurement = new Measurement(discrete, discrete, discrete);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            Product product = new Product("#12", "Mother Goose of Mutual Recursion: Recursive Mother Goose", "product12.glb", cat, new List<Material>(new[] { material }), measurements);
            Product component = new Product("#13", "Mother Goose of Diffractive Recitavo: Diffraction Mother Goose", "product13.gltf", cat, new List<Material>(new[] { material1, material }), measurements);
            CustomizedProduct custom = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("#8", product, CustomizedDimensions.valueOf(100, 100, 100)).withMaterial(CustomizedMaterial.valueOf(material, Color.valueOf("Epigraph of the Closed Curve: Close Epigraph", 100, 100, 100, 100), Finish.valueOf("der alte wurfelt nicht", 12))).build();
            Assert.True(instance.applyAlgorithm(custom, component).productMaterials[0].material.Equals(material));
        }
    }
}