
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
        public void ensureCustomizedProductCategoryIsNull()
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
            Assert.True(condition.Equals("True"));


        }

    }
}