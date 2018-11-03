
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.dto;
using support.dto;
using Xunit;

namespace core_tests.domain {
    public class CustomizedProductTest {
        [Fact]
        public void ensureCustomizedProductCannotBeCreatedWithNullReference() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            Assert.Throws<ArgumentException>(() => new CustomizedProduct(null, "Shelf", custMaterial1, customizedDimensions, product));
        }
        [Fact]
        public void ensureCustomizedProductCannotBeCreatedWithNullDesignation() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            Assert.Throws<ArgumentException>(() => new CustomizedProduct("K6205", null, custMaterial1, customizedDimensions, product));
        }
        [Fact]
        public void ensureCustomizedProductCannotBeCreatedWithEmptyReference() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            Assert.Throws<ArgumentException>(() => new CustomizedProduct("", "Shelf", custMaterial1, customizedDimensions, product));
        }
        [Fact]
        public void ensureCustomizedProductCannotBeCreatedWithEmptyDesignation() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            Assert.Throws<ArgumentException>(() => new CustomizedProduct("K6205", "", custMaterial1, customizedDimensions, product));
        }
        /// <summary>
        /// Test to ensure that a null CustomizedMaterial is not valid when creating a CustomizedProduct
        /// </summary>
        [Fact]
        public void ensureNullCustomizedMaterialFails() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            finishes.Add(finish);

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);
            string condition = ";";
            try {
                CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", null, customizedDimensions, product);

            } catch (ArgumentException) // The argument was thrown
              {
                condition = "True";

            } catch (NullReferenceException) {
                condition = "True";

            }
            Assert.Equal("True", condition);
        }

        /// <summary>
        /// Test to ensure that a null CustomizedDimensions is not valid when creating a CustomizedProduct
        /// </summary>
        [Fact]
        public void ensureNullCustomizedDimensionsFails() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);
            //CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);
            string condition = ";";
            try {
                CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, null, product);
            } catch (ArgumentException) // The argument was thrown
              {
                condition = "True";
            } catch (NullReferenceException) {
                condition = "True";
            }
            Assert.Equal("True", condition);
        }

        /// <summary>
        /// Test to ensure that a null Product is not valid when creating a CustomizedProduct
        /// </summary>
        [Fact]
        public void ensureNullProductFails() {
            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            List<Color> colors = new List<Color>();
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish2);

            Material material = new Material("11", "mat", colors, finishes);
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);
            string condition = ";";
            try {
                CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, null);
            } catch (ArgumentException) // The argument was thrown
              {
                condition = "True";
            } catch (NullReferenceException) {
                condition = "True";
            }
            Assert.Equal("True", condition);
        }

        /// <summary>
        /// Test to ensure a valid Slot is added to the CustomizedProduct's list of Slots
        /// </summary>
        [Fact]
        public void ensureAddSlotWorksForValidSlot() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.True(cp.addSlot(new Slot(CustomizedDimensions.valueOf(5, 5, 5))));
        }

        /// <summary>
        /// Test to ensure a valid Slot is not added to the CustomizedProduct's list of Slots if the Product doesn't support Slots
        /// </summary>
        [Fact]
        public void ensureAddSlotFailsForProductThatDoesntSupportSlots() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category,
            matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.False(cp.addSlot(new Slot(CustomizedDimensions.valueOf(5, 5, 5))));
        }

        /// <summary>
        /// Test to ensure a null Slot is not added to the CustomizedProduct's list of Slots
        /// </summary>
        [Fact]
        public void ensureAddSlotFailsForNullSlot() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.False(cp.addSlot(null));
        }

        /// <summary>
        /// Test to ensure a null Slot is not removed from the CustomizedProduct's list of Slots
        /// </summary>
        [Fact]
        public void ensureRemoveSlotFailsForNullSlot() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.False(cp.removeSlot(null));
        }

        /// <summary>
        /// Test to ensure a valid Slot is not removed from the CustomizedProduct's list of Slots if the Product doesn't support Slots
        /// </summary>
        [Fact]
        public void ensureRemoveSlotFailsForProductThatDoesntSupportSlots() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category,
            matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.False(cp.removeSlot(new Slot(CustomizedDimensions.valueOf(5, 5, 5))));
        }

        /// <summary>
        /// Test to ensure a valid Slot is removed from the CustomizedProduct's list of Slots
        /// </summary>
        [Fact]
        public void ensureRemoveSlotWorksForValidSlot() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);
            Slot slot = new Slot(CustomizedDimensions.valueOf(5, 5, 5));

            cp.addSlot(slot);
            Assert.True(cp.removeSlot(slot));
        }
        /// <summary>
        /// Test to ensure the number of Slots in the CustomizedProduct's list of Slots is the expected
        /// </summary>
        [Fact]
        public void ensureNumberOfSlotsWorks() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);
            cp.addSlot(new Slot(CustomizedDimensions.valueOf(5, 5, 5)));

            Assert.Equal(1, cp.numberOfSlots());
        }

        /// <summary>
        /// Test to ensure the returned id of the CustomizedProduct is the expected
        /// </summary>
        [Fact]
        public void ensureIdWorks() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.Equal("#666", cp.id());
        }

        /// <summary>
        /// Test to ensure the designation of the CustomizedProduct can't be changed if the string is empty
        /// </summary>
        [Fact]
        public void ensureChangeDesignationWorksForEmptyString() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            Assert.Throws<ArgumentException>(() =>
            new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).changeDesignation(""));
        }

        /// <summary>
        /// Test to ensure the designation of the CustomizedProduct can't be changed if the string is null
        /// </summary>
        [Fact]
        public void ensureChangeDesignationWorksForNullString() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            Assert.Throws<ArgumentException>(() =>
            new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).changeDesignation(null));
        }
        [Fact]
        public void ensureChangeDesignationSucceeds() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

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
        public void ensureChangeReferenceWorksForEmptyString() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            Assert.Throws<ArgumentException>(() =>
            new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).changeReference(""));
        }

        /// <summary>
        /// Test to ensure the reference of the CustomizedProduct can't be changed if the string is null
        /// </summary>
        [Fact]
        public void ensureChangeReferenceWorksForNullString() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            Assert.Throws<ArgumentException>(() =>
            new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).changeReference(null));
        }
        [Fact]
        public void ensureChangeReferenceSucceeds() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

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
        public void ensureEqualCustomizedProductsAreEqual() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.True(cp.Equals(cp));
        }

        /// <summary>
        /// Test to ensure that a CustomizedProduct and a null object are not equal
        /// </summary>
        [Fact]
        public void ensureCustomizedProductIsNotEqualToNullObject() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            Assert.False(new CustomizedProduct("#666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product).Equals(null));
        }

        /// <summary>
        /// Test to ensure that two different CustomizedProducts are not equal
        /// </summary>
        [Fact]
        public void ensureDifferentCustomizedProductsAreNotEqual() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            Assert.False(new CustomizedProduct("#666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product).Equals(
            new CustomizedProduct("#66666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product)));
        }

        /// <summary>
        /// Test to ensure that a CustomizedProduct and a different type object are not equal
        /// </summary>
        [Fact]
        public void ensureDifferentTypeObjectAndCustomizedProductsAreNotEqual() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            Assert.False(new CustomizedProduct("#666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product).
            Equals("Different type"));
        }

        /// <summary>
        /// Test to ensure that two CustomizedProducts with different Products are not equal
        /// </summary>
        [Fact]
        public void ensureCustomizedProductProductIsNotEqual() {
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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, new List<Measurement>(){measurement1});

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            //Product2 
            Product product2 = new Product("#666", "Shelf", category, matsList, new List<Measurement>(){measurement2});

            CustomizedProduct cp = new CustomizedProduct("#666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product);


            CustomizedProduct cp1 = new CustomizedProduct("#66666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product2);

            Assert.NotEqual(cp, cp1);
        }

        /// <summary>
        /// Test to ensure that the CustomizedProduct's identity is the same as a given identity.
        /// </summary>
        [Fact]
        public void ensureSameAsWorks() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            Assert.True(new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).sameAs("#666"));
        }

        /// <summary>
        /// Test to ensure two equal CustomizedProducts' textual description is the same
        /// </summary>
        [Fact]
        public void ensureToStringWorks() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material,color1, finish2);

            Assert.Equal(new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).ToString(),
            new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).ToString());
        }
        [Fact]
        public void ensureCustomProductIsCreatedSuccessfullyWithSlots() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            Slot slot = new Slot(customizedDimensions);
            Assert.NotNull(new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product, new List<Slot>(new[] { slot })));
        }
        [Fact]
        public void ensureGetHashCodeWorks() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color1, finish2);
            Assert.Equal(new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).GetHashCode(), new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).GetHashCode());
        }
        [Fact]
        public void ensuretoDTOWorks() {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() {measurement};

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

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

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
    }
}