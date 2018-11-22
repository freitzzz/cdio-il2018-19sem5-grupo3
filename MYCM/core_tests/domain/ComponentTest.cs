using System;
using Xunit;
using System.Collections.Generic;
using core.domain;
using core.dto;
using support.dto;

namespace core_tests.domain
{
    /// <summary>
    /// Tests of the class Component
    /// </summary>
    public class ComponentTest
    {
        //checkComponentProperties tests
        [Fact]
        public void ensureNullParentProductIsNotValid()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product product = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Assert.Throws<ArgumentException>(() => new Component(null, product));
            Assert.Throws<ArgumentException>(() => new Component(null, product, true));
            Assert.Throws<ArgumentException>(() => new Component(null, product, new List<Restriction>() { new Restriction("LÁ ESTÁ") }));
            Assert.Throws<ArgumentException>(() => new Component(null, product, new List<Restriction>() { new Restriction("LÁ ESTÁ") }, true));
        }

        [Fact]
        public void ensureNullChildProductIsNotValid()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Assert.Throws<ArgumentException>(() => new Component(parent, null));
            Assert.Throws<ArgumentException>(() => new Component(parent, null, true));
            Assert.Throws<ArgumentException>(() => new Component(parent, null, new List<Restriction>() { new Restriction("ISTO É") }));
            Assert.Throws<ArgumentException>(() => new Component(parent, null, new List<Restriction>() { new Restriction("ISTO É") }, true));
        }

        [Fact]
        public void ensureCreationIsSuccessfulIsBothChildAndParentAreValid()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Assert.NotNull(new Component(parent, child));
        }

        [Fact]
        public void ensureEmptyRestrictionsListIsNotValid()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Assert.Throws<ArgumentException>(() => new Component(parent, child, new List<Restriction>()));
            Assert.Throws<ArgumentException>(() => new Component(parent, child, new List<Restriction>(), true));
        }

        [Fact]
        public void ensureNullRestrictionsListIsNotValid()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

           //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            List<Restriction> restrictions = null;

            Assert.Throws<ArgumentException>(() => new Component(parent, child, restrictions));
            Assert.Throws<ArgumentException>(() => new Component(parent, child, restrictions, true));
        }

        [Fact]
        public void ensureCreationIsSuccessfulWithRestrictionListAndAValidProduct()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Assert.NotNull(new Component(parent, child, new List<Restriction>() { new Restriction("LÁ ESTÁ") }));
        }

        [Fact]
        public void ensureCreationIsSuccessfulWithValidRestrictionListAValidProductAndAMandatoryOption()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Assert.NotNull(new Component(parent, child, new List<Restriction>() { new Restriction("POIS") }, true));
        }

        //GetHashCode tests
        [Fact]
        public void ensureGetHashCodeWorks()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Component component1 = new Component(parent, child);
            Component component2 = new Component(parent, child);

            Assert.Equal(component1.GetHashCode(), component2.GetHashCode());
        }

        //Equals tests
        [Fact]
        public void ensureComponentsWithDifferentReferencesAreNotEqual()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Component component1 = new Component(parent, child);
            Component component2 = new Component(child, parent);

            Assert.False(component1.Equals(component2));
        }

        [Fact]
        public void ensureComponentsWithSameReferencesAreEqual()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

           //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Component component1 = new Component(parent, child);
            Component component2 = new Component(parent, child);
            Assert.True(component1.Equals(component2));
        }

        [Fact]
        public void ensureComponentsWithDiferentReferencesAreEqual()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Component component1 = new Component(parent, child);
            Component component2 = new Component(child, parent);
            Assert.False(component1.Equals(component2));
        }

        [Fact]
        public void ensureNullObjectIsNotEqual()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Component component = new Component(parent, child);
            Assert.False(component.Equals(null));
        }

        [Fact]
        public void ensureDifferentTypesAreNotEqual()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"),materials, measurements, 
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Component component = new Component(parent, child);
            Assert.False(component.Equals("Def not a component"));
        }

        //ToString tests
        [Fact]
        public void ensureToStringWorks()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list and customized product's customized material
            Color color = Color.valueOf("Blue", 1, 2, 3, 0);
            List<Color> colors = new List<Color>() { color };

            Finish finish = Finish.valueOf("Super shiny");
            List<Finish> finishes = new List<Finish>() { finish };

            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin",  new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Component component1 = new Component(parent, child);
            Component component2 = new Component(parent, child);

            Assert.Equal(component1.ToString(), component2.ToString());
        }

        //addRestriction tests
        [Fact]
        public void ensureNullRestrictionCantBeAddedToTheComponentsRestrictionsList()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements, 
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Assert.Throws<ArgumentException>(() => new Component(parent, child,
            new List<Restriction>() { new Restriction("FUNCIONE") }).addRestriction(null));
        }

        [Fact]
        public void ensureAlreadyExistentRestrictionCantBeAddedToTheComponentsRestrictionsList()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements, 
            CustomizedDimensions.valueOf(1, 1, 1),  CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"), materials, measurements, 
             CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Restriction restriction = new Restriction("FUNCIONE");
            Assert.Throws<ArgumentException>(() => new Component(parent, child,
            new List<Restriction>() { restriction }).addRestriction(restriction));
        }

        [Fact]
        public void ensureValidRestrictionCanBeAddedToTheComponentsRestrictionsList()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };

            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Assert.True(new Component(parent, child, new List<Restriction>() { new Restriction("FUNCIONE") }).
            addRestriction(new Restriction("FUNCIONOU")));
        }

        [Fact]
        public void testToDTO()
        {
            //Creates measurements and a material for the product
            List<Measurement> measurements = new List<Measurement>() { new Measurement(new DiscreteDimensionInterval( new List<Double>() { 500.0 }),
            new DiscreteDimensionInterval( new List<Double>() { 500.0 }), new DiscreteDimensionInterval( new List<Double>() { 500.0 })) };

            //Creates colors and finishes for the product's material list 
            List<Color> colors = new List<Color>() { Color.valueOf("Blue", 1, 2, 3, 0) };
            List<Finish> finishes = new List<Finish>() { Finish.valueOf("Super shiny") };
            //Creates a material list
            List<Material> materials = new List<Material>() { new Material("123", "456, how original", colors, finishes) };


            //Creates a parent product 
            Product parent = new Product("Kinda bad", "Anakin", new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            //Creates a child product
            Product child = new Product("Not so bad", "Luke",  new ProductCategory("Drawers"), materials, measurements,
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(5, 5, 5), CustomizedDimensions.valueOf(4, 4, 4));

            Component component = new Component(parent, child);

            ComponentDTO expected = new ComponentDTO();
            expected.product = child.toDTO();

            ComponentDTO actual = component.toDTO();
            Assert.Equal(expected.product.complements, actual.product.complements);
            Assert.Equal(expected.product.designation, actual.product.designation);
            Assert.Equal(expected.product.dimensions.Capacity, actual.product.dimensions.Capacity);
            Assert.Equal(expected.product.productCategory.name, actual.product.productCategory.name);
            Assert.Equal(expected.product.productMaterials.Count, actual.product.productMaterials.Count);
            Assert.Equal(expected.product.productMaterials.Capacity, actual.product.productMaterials.Capacity);
            Assert.Equal(expected.product.reference, actual.product.reference);
            Assert.Equal(expected.product.id, actual.product.id);
        }
    }
}