
using System;
using System.Collections;
using System.Collections.Generic;
using core.domain;
using Xunit;

namespace core_tests.domain
{
    public class CustomizedProductTest
    {
        [Fact]
        public void ensureCustomizedMaterialIsNull()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);

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
                CustomizedProduct cp = CustomizedProduct.valueOf("#666", "Shelf", null, customizedDimensions, product);

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



        [Fact]
        public void ensureCustomizedDimensionsIsNull()
        {
            var category = new ProductCategory("Drawers");

            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);

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
                CustomizedProduct cp = CustomizedProduct.valueOf("#666", "Shelf", custMaterial1, null, product);

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

        [Fact]
        public void ensureProductIsInvalid()
        {

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);
            string condition = ";";
            try
            {
                CustomizedProduct cp = CustomizedProduct.valueOf("#666", "Shelf", custMaterial1, customizedDimensions, null);

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

        [Fact]
        public void ensureCustomizedProductIsEqual()
        {
            var category = new ProductCategory("Drawers");



            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);

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



            CustomizedProduct cp = CustomizedProduct.valueOf("#666", "Shelf", custMaterial1, customizedDimensions, product);


            CustomizedProduct cp1 = CustomizedProduct.valueOf("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.Equal(cp, cp1);
        }

        [Fact]
        public void ensureCustomizedProductReferenceAreNotEqual()
        {
            var category = new ProductCategory("Drawers");



            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);

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



            CustomizedProduct cp = CustomizedProduct.valueOf("#666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product);


            CustomizedProduct cp1 = CustomizedProduct.valueOf("#666", "Shelf", custMaterial1, customizedDimensions, product);

            Assert.NotEqual(cp, cp1);
        }

        [Fact]
        public void ensureCustomizedProductDesignationAreNotEqual()
        {
            var category = new ProductCategory("Drawers");



            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);

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



            CustomizedProduct cp = CustomizedProduct.valueOf("#666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product);


            CustomizedProduct cp1 = CustomizedProduct.valueOf("#66666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product);

            Assert.NotEqual(cp, cp1);
        }

        [Fact]
        public void ensureCustomizedProductProductIsNotEqual()
        {
            var category = new ProductCategory("Drawers");



            //Creating Dimensions
            List<Double> values2 = new List<Double>();
            List<Double> values3 = new List<Double>();

            values2.Add(500.0); //Width
            values3.Add(666.1); //Width

            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            DiscreteDimensionInterval d3 = DiscreteDimensionInterval.valueOf(values3);

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

            CustomizedProduct cp = CustomizedProduct.valueOf("#666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product);


            CustomizedProduct cp1 = CustomizedProduct.valueOf("#66666", "AND READ-ER-BIBLE", custMaterial1, customizedDimensions, product2);

            Assert.NotEqual(cp, cp1);
        }
    }
}