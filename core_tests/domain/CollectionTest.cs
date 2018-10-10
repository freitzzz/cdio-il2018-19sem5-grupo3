using System;
using System.Collections.Generic;
using core.domain;
using Xunit;

namespace core_tests.domain
{
    public class CollectionTest
    {
        //ensureProductisInvalid
        [Fact]
        public void ensureProductisInvalid()
        {
            string condition = ";";
            CustomizedProduct cp = null;
            try
            {
                CustomizedProductCollection c = CustomizedProductCollection.valueOf("#666", "Shelf", cp);
            }
            catch (ArgumentException)
            {
                condition = "True";
            }
            catch (NullReferenceException)
            {
                condition = "True";

            }
            Assert.Equal("True", condition);
        }
        //ensureReferenceisInvalid
        [Fact]
        public void ensureReferenceisInvalid()
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


            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            string condition = ";";

            try
            {
                CustomizedProductCollection c = CustomizedProductCollection.valueOf(null, "Shelf", cp);
            }
            catch (ArgumentException)
            {
                condition = "True";
            }
            catch (NullReferenceException)
            {
                condition = "True";

            }
            Assert.Equal("True", condition);
        }
        //ensureDesignationisInvalid
        [Fact]
        public void ensureDesignationisInvalid()
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


            CustomizedProduct cp =new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);

            string condition = ";";

            try
            {
                CustomizedProductCollection c = CustomizedProductCollection.valueOf("Lucy", null, cp);
            }
            catch (ArgumentException)
            {
                condition = "True";
            }
            catch (NullReferenceException)
            {
                condition = "True";

            }
            Assert.Equal("True", condition);
        }
        //ensureListisInvalid
        [Fact]
        public void ensureListisInvalid()
        {

            string condition = ";";
            List<CustomizedProduct> list = null;

            try
            {
                CustomizedProductCollection c = CustomizedProductCollection.valueOf("Lucy", "666", list);
            }
            catch (ArgumentException)
            {
                condition = "True";
            }
            catch (NullReferenceException)
            {
                condition = "True";

            }
            Assert.Equal("True", condition);
        }
        //EnsureEqualsWorks
        [Fact]
        public void ensureEqualsWorks()
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

            List<CustomizedProduct> list = new List<CustomizedProduct>();
            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);
            list.Add(cp);

            CustomizedProductCollection c = CustomizedProductCollection.valueOf("Lucy", "666", list);
            CustomizedProductCollection c1 = CustomizedProductCollection.valueOf("Lucy", "666", list);

            Assert.Equal(c, c1);
        }

    }
}