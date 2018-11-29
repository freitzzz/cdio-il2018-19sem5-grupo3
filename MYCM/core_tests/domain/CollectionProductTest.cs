using System;
using System.Collections.Generic;
using core.domain;
using Xunit;
using System.Linq;

namespace core_tests.domain
{
    /// <summary>
    /// Tests of the class CollectionProductTest
    /// </summary>
    public class CollectionProductTest
    {
        [Fact]
        public void ensureCreationIsSucessfulWithAValidCustomizedProductCollectionAndCustomizedProduct()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list and customized product's customized material
            Color color = Color.valueOf("Blue", 1, 2, 3, 0);
            List<Color> colors = new List<Color>() { color };

            Finish finish = Finish.valueOf("Super shiny");
            List<Finish> finishes = new List<Finish>() { finish };

            Material material = new Material("123", "456, how original", "ola.jpg", colors, finishes);

            //Creates a product for the customized product collection's customized product
            Product product = new Product("0L4", "H4H4", new ProductCategory("Drawers"), new List<Material>() { material }, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Assert.NotNull(new CollectionProduct(new CustomizedProductCollection("Hang in there"),
            new CustomizedProduct("Or dont", "You choose", CustomizedMaterial.valueOf(material, color, finish),
            CustomizedDimensions.valueOf(500.0, 500.0, 500.0), product)));
        }

        [Fact]
        public void ensureNullCustomizedProductCollectionIsInvalid()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list and customized product's customized material
            Color color = Color.valueOf("Blue", 1, 2, 3, 0);
            List<Color> colors = new List<Color>() { color };

            Finish finish = Finish.valueOf("Super shiny");
            List<Finish> finishes = new List<Finish>() { finish };

            Material material = new Material("123", "456, how original", "ola.jpg", colors, finishes);

            //Creates a product for the customized product collection's customized product
            Product product = new Product("0L4", "H4H4", new ProductCategory("Drawers"), new List<Material>() { material }, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Assert.Throws<ArgumentException>(() => new CollectionProduct(null, new CustomizedProduct("Or dont", "You choose",
            CustomizedMaterial.valueOf(material, color, finish), CustomizedDimensions.valueOf(1.2, 1.5, 20.3), product)));
        }

        [Fact]
        public void ensureNullCustomizedProductIsInvalid()
        {
            Assert.Throws<ArgumentException>(() => new CollectionProduct(new CustomizedProductCollection("Hang in there"), null));
        }
    }
}