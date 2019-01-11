using core.domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static core.domain.CustomizedProduct;

namespace core_tests.domain {
    public class RestrictableTest {
        public class RestrictableImpl : Restrictable {
            public RestrictableImpl() {
                this.restrictions = new List<Restriction>();
            }
        }
        [Fact]
        public void ensureApplyAllRestrictionsReturnsNullIfCustomizedProductArgumentNull() {
            ProductCategory cat = new ProductCategory("All Products");

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            Material material = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);

            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Product product = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));

            Assert.Null(new RestrictableImpl().applyAllRestrictions(null, product));
        }
        [Fact]
        public void ensureApplyAllRestrictionsReturnsNullIfProductArgumentNull() {
            ProductCategory cat = new ProductCategory("All Products");

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            Material material = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);

            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Product product = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));

            CustomizedDimensions customDimension = CustomizedDimensions.valueOf(50, 80, 25);
            CustomizedMaterial customMaterial = CustomizedMaterial.valueOf(material, white, matte);
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", product, customDimension).build();

            customizedProduct.changeCustomizedMaterial(customMaterial);

            customizedProduct.finalizeCustomization();

            Assert.Null(new RestrictableImpl().applyAllRestrictions(customizedProduct, null));
        }
        [Fact]
        public void ensureApplyAllRestrictionsReturnsNullIfProductDoesNotObeyRestrictions() {
            ProductCategory cat = new ProductCategory("All Products");

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            Material material = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);
            Material material2 = new Material("#002", "Expensive Wood", "ola.jpg", colors, finishes);

            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Product product = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            Product product2 = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));

            CustomizedDimensions customDimension = CustomizedDimensions.valueOf(50, 80, 25);
            CustomizedMaterial customMaterial = CustomizedMaterial.valueOf(material2, white, matte);
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", product, customDimension).build();

            customizedProduct.changeCustomizedMaterial(customMaterial);

            customizedProduct.finalizeCustomization();
            RestrictableImpl instance = new RestrictableImpl();
            instance.addRestriction(new Restriction("same material", new SameMaterialAndFinishAlgorithm()));
            Assert.Null(instance.applyAllRestrictions(customizedProduct, product2));
        }
        [Fact]
        public void ensureApplyAllRestrictionsReturnsRestrictedProduct() {
            ProductCategory cat = new ProductCategory("All Products");

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            Material material = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);
            Material material2 = new Material("#002", "Expensive Wood", "ola.jpg", colors, finishes);

            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Product product = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            Product product2 = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));

            CustomizedDimensions customDimension = CustomizedDimensions.valueOf(50, 80, 25);
            CustomizedMaterial customMaterial = CustomizedMaterial.valueOf(material, white, matte);
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", product, customDimension).build();

            customizedProduct.changeCustomizedMaterial(customMaterial);

            customizedProduct.finalizeCustomization();
            RestrictableImpl instance = new RestrictableImpl();
            instance.addRestriction(new Restriction("same material", new SameMaterialAndFinishAlgorithm()));

            WidthPercentageAlgorithm algorithm = new WidthPercentageAlgorithm();
            Input minInput = Input.valueOf("Minimum Percentage", "From 0 to 1");
            Input maxInput = Input.valueOf("Maximum Percentage", "From 0 to 1");
            Dictionary<Input, string> inputs = new Dictionary<Input, string>();
            inputs.Add(minInput, "0.8");
            inputs.Add(maxInput, "1.0");
            algorithm.setInputValues(inputs);
            instance.addRestriction(new Restriction("width percentage", algorithm));

            Product returned = instance.applyAllRestrictions(customizedProduct, product2);
            Assert.True(returned.productMaterials.Count == 1);
            Assert.True(returned.productMaterials[0].material.Equals(material));
            Assert.True(returned.productMeasurements[0].measurement.width.getMinValue() == 65);
            Assert.True(returned.productMeasurements[0].measurement.width.getMaxValue() == 80);
        }
    }
}
