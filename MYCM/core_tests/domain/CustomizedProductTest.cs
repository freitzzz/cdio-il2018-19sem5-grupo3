
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.dto;
using support.dto;
using Xunit;

namespace core_tests.domain
{
    /// <summary>
    /// Unit testing class for CustomizedProduct
    /// </summary>
    //TODO Create a method that returns a customized product to substantially reduce lines of code
    public class CustomizedProductTest
    {
        //These are all seperated into their own methods in order to allow for each property to be tested

        private ProductCategory buildValidCategory()
        {
            return new ProductCategory("Closets");
        }

        private Finish buildGlossyFinish()
        {
            return Finish.valueOf("Glossy");
        }

        private Finish buildMatteFinish()
        {
            return Finish.valueOf("Matte");
        }

        private Color buildRedColor()
        {
            return Color.valueOf("Deep Red", 255, 0, 0, 0);
        }

        private Color buildGreenColor()
        {
            return Color.valueOf("Totally Green", 0, 255, 0, 0);
        }

        private Material buildValidMaterial()
        {

            Finish glossy = buildGlossyFinish();
            Finish matte = buildMatteFinish();

            Color red = buildRedColor();
            Color green = buildGreenColor();


            return new Material("#123", "MDF", "ola.jpg", new List<Color>() { red, green }, new List<Finish>() { glossy, matte });
        }

        private Product buildValidProduct()
        {
            Dimension firstHeightDimension = new ContinuousDimensionInterval(50, 100, 2);
            Dimension firstWidthDimension = new DiscreteDimensionInterval(new List<double>() { 75, 80, 85, 90, 95, 120 });
            Dimension firstDepthDimension = new SingleValueDimension(25);

            Measurement firstMeasurement = new Measurement(firstHeightDimension, firstWidthDimension, firstDepthDimension);

            Dimension sideDimension = new SingleValueDimension(60);
            Measurement secondMeasurement = new Measurement(sideDimension, sideDimension, sideDimension);

            return new Product("#429", "Fabulous Closet", "fabcloset.glb",buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { firstMeasurement, secondMeasurement });
        }


        private CustomizedDimensions buildCustomizedProductDimensions()
        {
            return CustomizedDimensions.valueOf(76, 80, 25);
        }

        private CustomizedMaterial buildCustomizedMaterial()
        {
            Material material = buildValidMaterial();
            Finish selectedFinish = buildMatteFinish();
            Color selectedColor = buildRedColor();
            return CustomizedMaterial.valueOf(material, selectedColor, selectedFinish);
        }

        private CustomizedProduct buildValidInstance()
        {
            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            CustomizedDimensions selectedDimensions = buildCustomizedProductDimensions();

            return new CustomizedProduct("#429", "Fabulous Closet", customizedMaterial, selectedDimensions, buildValidProduct());
        }

        [Fact]
        public void ensureCustomizedProductCannotBeCreatedWithNullReference()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            Assert.Throws<ArgumentException>(() => new CustomizedProduct(null, "Shelf", custMaterial1, customizedDimensions, product));
        }
        [Fact]
        public void ensureCustomizedProductCannotBeCreatedWithNullDesignation()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            Assert.Throws<ArgumentException>(() => new CustomizedProduct("K6205", null, custMaterial1, customizedDimensions, product));
        }
        [Fact]
        public void ensureCustomizedProductCannotBeCreatedWithEmptyReference()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            Assert.Throws<ArgumentException>(() => new CustomizedProduct("", "Shelf", custMaterial1, customizedDimensions, product));
        }
        [Fact]
        public void ensureCustomizedProductCannotBeCreatedWithEmptyDesignation()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            Assert.Throws<ArgumentException>(() => new CustomizedProduct("K6205", "", custMaterial1, customizedDimensions, product));
        }
        /// <summary>
        /// Test to ensure that a null CustomizedMaterial is not valid when creating a CustomizedProduct
        /// </summary>
        [Fact]
        public void ensureNullCustomizedMaterialFails()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            finishes.Add(finish);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);
            string condition = ";";
            try
            {
                CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", null, customizedDimensions, product);

            }
            catch (ArgumentException) // The argument was thrown
            {
                condition = "True";

            }
            catch (NullReferenceException)
            {
                condition = "True";

            }
            Assert.Equal("True", condition);
        }

        /// <summary>
        /// Test to ensure that a null CustomizedDimensions is not valid when creating a CustomizedProduct
        /// </summary>
        [Fact]
        public void ensureNullCustomizedDimensionsFails()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            //CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);
            string condition = ";";
            try
            {
                CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, null, product);
            }
            catch (ArgumentException) // The argument was thrown
            {
                condition = "True";
            }
            catch (NullReferenceException)
            {
                condition = "True";
            }
            Assert.Equal("True", condition);
        }

        /// <summary>
        /// Test to ensure that a null Product is not valid when creating a CustomizedProduct
        /// </summary>
        [Fact]
        public void ensureNullProductFails()
        {
            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            List<Color> colors = new List<Color>();
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish2);

            Material material = new Material("11", "mat", "ola.jpg", colors, finishes);
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);
            string condition = ";";
            try
            {
                CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, null);
            }
            catch (ArgumentException) // The argument was thrown
            {
                condition = "True";
            }
            catch (NullReferenceException)
            {
                condition = "True";
            }
            Assert.Equal("True", condition);
        }

        /// <summary>
        /// Test to ensure a valid Slot is added to the CustomizedProduct's list of Slots
        /// </summary>
        [Fact]
        public void ensureAddSlotWorksForValidSlot()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements,
            ProductSlotWidths.valueOf(1, 5, 4));
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.True(cp.addSlot(new Slot(CustomizedDimensions.valueOf(5, 5, 5))));
        }

        /// <summary>
        /// Test to ensure a valid Slot is not added to the CustomizedProduct's list of Slots if the Product doesn't support Slots
        /// </summary>
        [Fact]
        public void ensureAddSlotFailsForProductThatDoesntSupportSlots()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category,
            matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Action act = () => cp.addSlot(new Slot(CustomizedDimensions.valueOf(5, 5, 5)));

            Assert.Throws<ArgumentException>(act);
        }

        /// <summary>
        /// Test to ensure a null Slot is not added to the CustomizedProduct's list of Slots
        /// </summary>
        [Fact]
        public void ensureAddSlotFailsForNullSlot()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Action action = () => cp.addSlot(null);

            Assert.Throws<ArgumentException>(action);
        }

        /// <summary>
        /// Test to ensure a null Slot is not removed from the CustomizedProduct's list of Slots
        /// </summary>
        [Fact]
        public void ensureRemoveSlotFailsForNullSlot()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.False(cp.removeSlot(null));
        }

        /// <summary>
        /// Test to ensure a valid Slot is not removed from the CustomizedProduct's list of Slots if the Product doesn't support Slots
        /// </summary>
        [Fact]
        public void ensureRemoveSlotFailsForProductThatDoesntSupportSlots()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category,
            matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.False(cp.removeSlot(new Slot(CustomizedDimensions.valueOf(5, 5, 5))));
        }

        /// <summary>
        /// Test to ensure a valid Slot is removed from the CustomizedProduct's list of Slots
        /// </summary>
        [Fact]
        public void ensureRemoveSlotWorksForValidSlot()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements,
            ProductSlotWidths.valueOf(1, 5, 4));
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);
            Slot slot = new Slot(CustomizedDimensions.valueOf(5, 5, 5));

            cp.addSlot(slot);
            Assert.True(cp.removeSlot(slot));
        }
        /// <summary>
        /// Test to ensure the number of Slots in the CustomizedProduct's list of Slots is the expected
        /// </summary>
        [Fact]
        public void ensureNumberOfSlotsWorks()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements,
            ProductSlotWidths.valueOf(1, 5, 4));
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);
            cp.addSlot(new Slot(CustomizedDimensions.valueOf(5, 5, 5)));

            Assert.Equal(1, cp.numberOfSlots());
        }

        /// <summary>
        /// Test to ensure the returned id of the CustomizedProduct is the expected
        /// </summary>
        [Fact]
        public void ensureIdWorks()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements,
            ProductSlotWidths.valueOf(1, 5, 4));
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.Equal("#666", cp.id());
        }

        /// <summary>
        /// Test to ensure the designation of the CustomizedProduct can't be changed if the string is empty
        /// </summary>
        [Fact]
        public void ensureChangeDesignationWorksForEmptyString()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements,
            ProductSlotWidths.valueOf(1, 5, 4));
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            Assert.Throws<ArgumentException>(() =>
            new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).changeDesignation(""));
        }

        /// <summary>
        /// Test to ensure the designation of the CustomizedProduct can't be changed if the string is null
        /// </summary>
        [Fact]
        public void ensureChangeDesignationWorksForNullString()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements,
            ProductSlotWidths.valueOf(1, 5, 4));
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            Assert.Throws<ArgumentException>(() =>
            new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).changeDesignation(null));
        }
        [Fact]
        public void ensureChangeDesignationSucceeds()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements,
            ProductSlotWidths.valueOf(1, 5, 4));
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            CustomizedProduct custom = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);
            string newDesignation = "Amadeus";
            custom.changeDesignation(newDesignation);
            Assert.Equal(newDesignation, custom.designation);
        }

        /// <summary>
        /// Test to ensure the reference of the CustomizedProduct can't be changed if the string is empty
        /// </summary>
        [Fact]
        public void ensureChangeReferenceWorksForEmptyString()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements,
            ProductSlotWidths.valueOf(1, 5, 4));
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            Assert.Throws<ArgumentException>(() =>
            new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).changeReference(""));
        }

        /// <summary>
        /// Test to ensure the reference of the CustomizedProduct can't be changed if the string is null
        /// </summary>
        [Fact]
        public void ensureChangeReferenceWorksForNullString()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements,
            ProductSlotWidths.valueOf(1, 5, 4));
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            Assert.Throws<ArgumentException>(() =>
            new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).changeReference(null));
        }
        [Fact]
        public void ensureChangeReferenceSucceeds()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements,
            ProductSlotWidths.valueOf(1, 5, 4));
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            CustomizedProduct custom = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);
            string newReference = "K6205";
            custom.changeReference(newReference);
            Assert.Equal(newReference, custom.reference);
        }

        /// <summary>
        /// Test to ensure that two equal CustomizedProducts are equal
        /// </summary>
        [Fact]
        public void ensureEqualCustomizedProductsAreEqual()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.True(cp.Equals(cp));
        }

        /// <summary>
        /// Test to ensure that a CustomizedProduct and a null object are not equal
        /// </summary>
        [Fact]
        public void ensureCustomizedProductIsNotEqualToNullObject()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            Assert.False(new CustomizedProduct("#666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product).Equals(null));
        }

        /// <summary>
        /// Test to ensure that two different CustomizedProducts are not equal
        /// </summary>
        [Fact]
        public void ensureDifferentCustomizedProductsAreNotEqual()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            Assert.False(new CustomizedProduct("#666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product).Equals(
            new CustomizedProduct("#66666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product)));
        }

        /// <summary>
        /// Test to ensure that a CustomizedProduct and a different type object are not equal
        /// </summary>
        [Fact]
        public void ensureDifferentTypeObjectAndCustomizedProductsAreNotEqual()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            Assert.False(new CustomizedProduct("#666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product).
            Equals("Different type"));
        }

        /// <summary>
        /// Test to ensure that two CustomizedProducts with different Products are not equal
        /// </summary>
        [Fact]
        public void ensureCustomizedProductProductIsNotEqual()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();
            List<Double> values3 = new List<Double>();

            values2.Add(500.0); //Width
            values3.Add(666.1); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
            DiscreteDimensionInterval d3 = new DiscreteDimensionInterval(values3);

            Measurement measurement1 = new Measurement(d2, d2, d2);
            Measurement measurement2 = new Measurement(d3, d3, d3);


            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, new List<Measurement>() { measurement1 });

            CustomizedDimensions customizedDimensions1 = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);
            CustomizedDimensions customizedDimensions2 = CustomizedDimensions.valueOf(666.1, 666.1, 666.1);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            //Product2 
            Product product2 = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, new List<Measurement>() { measurement2 });

            CustomizedProduct cp = new CustomizedProduct("#666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions1, product);


            CustomizedProduct cp1 = new CustomizedProduct("#66666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions2, product2);

            Assert.NotEqual(cp, cp1);
        }

        /// <summary>
        /// Test to ensure that the CustomizedProduct's identity is the same as a given identity.
        /// </summary>
        [Fact]
        public void ensureSameAsWorks()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            Assert.True(new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).sameAs("#666"));
        }

        /// <summary>
        /// Test to ensure two equal CustomizedProducts' textual description is the same
        /// </summary>
        [Fact]
        public void ensureToStringWorks()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);

            Assert.Equal(new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).ToString(),
            new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).ToString());
        }
        [Fact]
        public void ensureCustomProductIsCreatedSuccessfullyWithSlots()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;
            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(15, 80, 50);

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements, slotWidths);

            //these dimensions have to be coherent with the Product's available dimensions
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //all the values in this dimension are in the range defined by the min and max sizes
            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(40, 80, 25);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            Slot slot = new Slot(slotDimensions);
            Assert.NotNull(new CustomizedProduct("#666", "Shelf", custMaterial1, customizedProductDimensions, product, new List<Slot>(new[] { slot })));
        }
        [Fact]
        public void ensureGetHashCodeWorks()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            Assert.Equal(new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).GetHashCode(), new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).GetHashCode());
        }
        [Fact]
        public void ensuretoDTOWorks()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            CustomizedProduct custom = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.Equal(custom.reference, custom.toDTO().reference);
            Assert.Equal(custom.designation, custom.toDTO().designation);
            Assert.Equal(custom.product.toDTO().reference, custom.toDTO().productDTO.reference);
            Assert.Equal(custom.customizedDimensions.toDTO().Id, custom.toDTO().customizedDimensionsDTO.Id);
            Assert.Equal(custom.customizedMaterial.toDTO().material.reference, custom.toDTO().customizedMaterialDTO.material.reference);
            Assert.Equal(DTOUtils.parseToDTOS(custom.slots).ToList().Count(), custom.toDTO().slotListDTO.Count());
            Assert.Equal(custom.Id, custom.toDTO().id);
        }

        [Fact]
        public void ensureCustomizedProductWithSlotsCantBeCreatedIfTheSlotWhereItIsInsertedIsNull()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(100, 500, 300);

            Product product = new Product("#555", "designation", "product555.glb", category, matsList, measurements, slotWidths);

            Slot slot = new Slot(CustomizedDimensions.valueOf(500, 500, 500));
            List<Slot> slotList = new List<Slot>();
            slotList.Add(slot);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color1, finish2);

            Action act = () => new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product, slotList, null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureCustomizedProductWithSlotsCanBeCreatedIfTheSlotWhereItIsInsertedIsValid()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(100, 500, 300);

            Product product = new Product("#555", "designation", "product555.glb", category, matsList, measurements, slotWidths);

            Slot slot = new Slot(CustomizedDimensions.valueOf(500, 500, 500));
            Slot otherSlot = new Slot(CustomizedDimensions.valueOf(300, 300, 300));
            List<Slot> slotList = new List<Slot>();
            List<Slot> otherSlotList = new List<Slot>();
            slotList.Add(slot);
            otherSlotList.Add(otherSlot);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);
            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct father = new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product, slotList);
            CustomizedProduct child = new CustomizedProduct("#444", "hello", customizedMaterial, customizedDimensions, product, otherSlotList, slot);

            Assert.NotNull(child);
            Assert.Equal(slot.customizedProducts.First(), child);
        }

        [Fact]
        public void ensureCustomizedProductWithoutSlotsCantBeCreatedIfTheSlotWhereItIsInsertedIsNull()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(100, 500, 300);

            Product product = new Product("#555", "designation", "product555.glb", category, matsList, measurements, slotWidths);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color1, finish2);

            Slot instance = null;
            Action act = () => new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product, instance);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureCustomizedProductWithoutSlotsCanBeCreatedIfTheSlotWhereItIsInsertedIsValid()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(100, 500, 300);

            Slot slot = new Slot(CustomizedDimensions.valueOf(500, 500, 500));
            List<Slot> slotList = new List<Slot>();
            slotList.Add(slot);

            Product product = new Product("#555", "designation", "product555.glb", category, matsList, measurements, slotWidths);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct father = new CustomizedProduct("#444", "Hi", customizedMaterial, customizedDimensions, product, slotList);
            CustomizedProduct child = new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product, slot);

            Assert.NotNull(child);
            Assert.Equal(slot.customizedProducts.First(), child);
        }

        [Fact]
        public void ensureChangeCustomizedDimensionsChangesDimensions()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval discreteDimension = new DiscreteDimensionInterval(values2);

            ContinuousDimensionInterval continuousDimension = new ContinuousDimensionInterval(100.0, 350.0, 2.0);

            Measurement measurement = new Measurement(discreteDimension, discreteDimension, discreteDimension);
            Measurement otherMeasurement = new Measurement(continuousDimension, discreteDimension, continuousDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement, otherMeasurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#555", "designation", "product555.glb", category,
                matsList, measurements);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct customizedProduct = new CustomizedProduct("#444", "Hi", customizedMaterial, customizedDimensions, product);

            CustomizedDimensions otherCustomizedDimensions = CustomizedDimensions.valueOf(182.0, 500.0, 200.0);

            Assert.True(customizedProduct.changeCustomizedDimensions(otherCustomizedDimensions));
            Assert.NotEqual(customizedProduct.customizedDimensions, customizedDimensions);
        }

        [Fact]
        public void ensureChangeCustomizedDimensionsDoesNotChangeDimensionsWhenTheyArentValid()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#555", "designation", "product555.glb", category,
                matsList, measurements);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct customizedProduct = new CustomizedProduct("#444", "Hi", customizedMaterial, customizedDimensions, product);

            Action act = () => customizedProduct.changeCustomizedDimensions(null);

            Assert.Throws<ArgumentException>(act);
            Assert.Equal(customizedProduct.customizedDimensions, customizedDimensions);
        }

        [Fact]
        public void ensureChangeCustomizedMaterialChangesColorAndFinish()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#555", "designation", "product555.glb", category,
                matsList, measurements);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color1, finish2);
            CustomizedMaterial otherCustomizedMaterial = CustomizedMaterial.valueOf(material, color, finish);

            CustomizedProduct customizedProduct = new CustomizedProduct("#444", "Hi", customizedMaterial, customizedDimensions, product);

            Assert.True(customizedProduct.changeCustomizedMaterial(otherCustomizedMaterial));
            Assert.NotEqual(customizedMaterial.color, customizedProduct.customizedMaterial.color);
            Assert.NotEqual(customizedMaterial.finish, customizedProduct.customizedMaterial.finish);
        }

        [Fact]
        public void ensureChangeCustomizedMaterialChangesMaterialColorAndFinish()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            List<Color> otherColors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            otherColors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            List<Finish> otherFinishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            otherFinishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            Material otherMaterial = new Material("hello", "goodbye", "ola.jpg", otherColors, otherFinishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);
            materials.Add(otherMaterial);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#555", "designation", "product555.glb", category,
                matsList, measurements);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedMaterial otherCustomizedMaterial = CustomizedMaterial.valueOf(otherMaterial, color1, finish2);

            CustomizedProduct customizedProduct = new CustomizedProduct("#444", "Hi", customizedMaterial, customizedDimensions, product);

            Assert.True(customizedProduct.changeCustomizedMaterial(otherCustomizedMaterial));
            Assert.NotEqual(customizedMaterial.color, customizedProduct.customizedMaterial.color);
            Assert.NotEqual(customizedMaterial.finish, customizedProduct.customizedMaterial.finish);
            Assert.NotEqual(customizedMaterial.material, customizedProduct.customizedMaterial.material);
        }

        [Fact]
        public void ensureChangeCustomizedMaterialDoesNotChangeCustomizedMaterialWhenNewCusomizedMaterialIsNull()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#555", "designation", "product555.glb", category,
                matsList, measurements);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct customizedProduct = new CustomizedProduct("#444", "Hi", customizedMaterial, customizedDimensions, product);

            Action act = () => customizedProduct.changeCustomizedMaterial(null);

            Assert.Throws<ArgumentException>(act);
            Assert.Equal(customizedMaterial, customizedProduct.customizedMaterial);
        }

        [Fact]
        public void ensureChangeFinishDoesNotChangeFinishWhenNewFinishIsNull()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#555", "designation", "product555.glb", category,
                matsList, measurements);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct customizedProduct = new CustomizedProduct("#444", "Hi", customizedMaterial, customizedDimensions, product);

            Action act = () => customizedProduct.changeFinish(null);

            Assert.Throws<ArgumentException>(act);
            Assert.Equal(customizedMaterial.finish, customizedProduct.customizedMaterial.finish);
        }

        [Fact]
        public void ensureChangeFinishDoesNotChangeFinishWhenNewFinishDoesNotBelongToTheMaterial()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#555", "designation", "product555.glb", category,
                matsList, measurements);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color1, finish);

            CustomizedProduct customizedProduct = new CustomizedProduct("#444", "Hi", customizedMaterial, customizedDimensions, product);

            Action act = () => customizedProduct.changeFinish(finish2);

            Assert.Throws<ArgumentException>(act);
            Assert.Equal(customizedMaterial.finish, customizedProduct.customizedMaterial.finish);
        }

        [Fact]
        public void ensureChangeFinishChangesFinish()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#555", "designation", "product555.glb", category,
                matsList, measurements);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct customizedProduct = new CustomizedProduct("#444", "Hi", customizedMaterial, customizedDimensions, product);

            Assert.True(customizedProduct.changeFinish(finish));
            Assert.NotEqual(finish2, customizedProduct.customizedMaterial.finish);
        }

        [Fact]
        public void ensureChangeColorDoesNotChangeColorWhenNewColorIsNull()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#555", "designation", "product555.glb", category,
                matsList, measurements);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct customizedProduct = new CustomizedProduct("#444", "Hi", customizedMaterial, customizedDimensions, product);

            Action act = () => customizedProduct.changeColor(null);

            Assert.Throws<ArgumentException>(act);
            Assert.Equal(customizedMaterial.color, customizedProduct.customizedMaterial.color);
        }

        [Fact]
        public void ensureChangeColorDoesNotChangeColorWhenNewColorDoesNotBelongToTheMaterial()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#555", "designation", "product555.glb", category,
                matsList, measurements);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish2);

            CustomizedProduct customizedProduct = new CustomizedProduct("#444", "Hi", customizedMaterial, customizedDimensions, product);

            Action act = () => customizedProduct.changeColor(null);

            Assert.Throws<ArgumentException>(act);
            Assert.Equal(customizedMaterial.color, customizedProduct.customizedMaterial.color);
        }

        [Fact]
        public void ensureChangeColorChangesColor()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            Finish finish2 = Finish.valueOf("Acabamento polido");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#555", "designation", "product555.glb", category,
                matsList, measurements);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color1, finish2);

            CustomizedProduct customizedProduct = new CustomizedProduct("#444", "Hi", customizedMaterial, customizedDimensions, product);

            Assert.True(customizedProduct.changeColor(color));
            Assert.NotEqual(color1, customizedProduct.customizedMaterial.color);
        }
    }
}