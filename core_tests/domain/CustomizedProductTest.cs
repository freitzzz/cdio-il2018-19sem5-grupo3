
using System;
using System.Collections;
using System.Collections.Generic;
using core.domain;
using Xunit;

namespace core_tests.domain
{
    public class CustomizedProductTest
    {
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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", category, matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);
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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", category, matsList, heightValues, widthValues, depthValues);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);
            //CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);
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
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);
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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", category,
            matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.False(cp.addSlot(new Slot(CustomizedDimensions.valueOf(5, 5, 5))));
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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", category, matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.False(cp.addSlot(null));
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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", category, matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", category,
            matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

            Assert.Throws<ArgumentException>(() =>
            new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).changeDesignation(null));
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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", true, CustomizedDimensions.valueOf(5, 5, 5),
            CustomizedDimensions.valueOf(1, 1, 1), CustomizedDimensions.valueOf(4, 4, 4), category,
            matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

            Assert.Throws<ArgumentException>(() =>
            new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).changeReference(null));
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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", category, matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", category, matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", category, matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

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

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", category, matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            List<Dimension> valuest2 = new List<Dimension>();
            valuest2.Add(d3);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;


            IEnumerable<Dimension> heightValues1 = valuest2;

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

            Product product = new Product("#666", "Shelf", category, matsList, heightValues, widthValues, depthValues);

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

            //Product2 
            Product product2 = new Product("#666", "Shelf", category, matsList, heightValues1, widthValues, depthValues);

            CustomizedProduct cp = new CustomizedProduct("#666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product);


            CustomizedProduct cp1 = new CustomizedProduct("#66666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product2);

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

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", category, matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

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

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

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

            Product product = new Product("#666", "Shelf", category, matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);

            Assert.Equal(new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).ToString(),
            new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product).ToString());
        }
    }
}