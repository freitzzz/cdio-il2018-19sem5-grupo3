using System;
using Xunit;
using System.Collections;
using core.domain;
using System.Collections.Generic;
using core.dto;
using System.Linq;

namespace core_tests.domain
{
    /// <summary>
    /// Unit testing class for Slot
    /// </summary>
    public class SlotTest
    {
        [Fact]
        public void ensureConstructorDetectsNullDimensions()
        {
            Action act = () => new Slot(null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsCreated()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(10, 20, 30));

            Assert.NotNull(instance);
        }

        [Fact]
        public void ensureAddCustomizedProductDoesNotAddNull()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(100, 20, 300));

            Assert.False(instance.addCustomizedProduct(null));
        }

        [Fact]
        public void ensureAddCustomizedProductWorks()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(100, 200, 300));
            var category = new ProductCategory("Drawers");
            //Creating Dimensions
            List<Double> valuesList = new List<Double>();
            valuesList.Add(500.0); //Width
            DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(valuesList);
            List<Dimension> dimensionList = new List<Dimension>();
            dimensionList.Add(discreteDimensionInterval);
            IEnumerable<Dimension> heightValues = dimensionList;
            IEnumerable<Dimension> widthValues = dimensionList;
            IEnumerable<Dimension> depthValues = dimensionList;
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
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material,color1, finish2);
            CustomizedProduct customizedProduct = new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product);

            //Add customized product to slot
            instance.addCustomizedProduct(customizedProduct);

            Assert.NotEmpty(instance.customizedProducts);
        }

        [Fact]
        public void ensureRemoveCustomizedProductDoesNotRemoveNull()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(100, 200, 300));

            Assert.False(instance.removeCustomizedProduct(null));
        }

        [Fact]
        public void ensureRemoveCustomizedProductDoesNotRemoveNonExistingProduct()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(100, 200, 300));
            var category = new ProductCategory("Drawers");
            //Creating Dimensions
            List<Double> valuesList = new List<Double>();
            valuesList.Add(500.0); //Width
            DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(valuesList);
            List<Dimension> dimensionList = new List<Dimension>();
            dimensionList.Add(discreteDimensionInterval);
            IEnumerable<Dimension> heightValues = dimensionList;
            IEnumerable<Dimension> widthValues = dimensionList;
            IEnumerable<Dimension> depthValues = dimensionList;
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
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material,color1, finish2);
            CustomizedProduct customizedProduct = new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product);

            Assert.False(instance.removeCustomizedProduct(customizedProduct));
        }

        [Fact]
        public void ensureRemoveCustomizedProductWorks()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(100, 200, 300));
            var category = new ProductCategory("Drawers");
            //Creating Dimensions
            List<Double> valuesList = new List<Double>();
            valuesList.Add(500.0); //Width
            DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(valuesList);
            List<Dimension> dimensionList = new List<Dimension>();
            dimensionList.Add(discreteDimensionInterval);
            IEnumerable<Dimension> heightValues = dimensionList;
            IEnumerable<Dimension> widthValues = dimensionList;
            IEnumerable<Dimension> depthValues = dimensionList;
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
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material,color1, finish2);
            CustomizedProduct customizedProduct = new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product);

            instance.addCustomizedProduct(customizedProduct);

            Assert.True(instance.removeCustomizedProduct(customizedProduct));
        }

        [Fact]
        public void ensureEqualsReturnsTrueForSameInstanceComparison()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(1, 2, 3));

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void ensureEqualsReturnsFalseForNullComparison()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(10, 20, 30));

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureEqualsReturnsFalseForInstancesOfDifferentTypes()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(40, 40, 40));

            Assert.False(instance.Equals("bananas"));
        }

        [Fact]
        public void ensureEqualsReturnsFalseForSlotsWithDifferentDimensions()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(1, 2, 3));
            Slot other = new Slot(CustomizedDimensions.valueOf(5, 5, 5));

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureEqualsReturnsFalseForSlotsWithDifferentCustomizedProductsList()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(1, 1, 1));
            Slot other = new Slot(CustomizedDimensions.valueOf(1, 1, 1));
            var category = new ProductCategory("Drawers");
            //Creating Dimensions
            List<Double> valuesList = new List<Double>();
            valuesList.Add(500.0); //Width
            DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(valuesList);
            List<Dimension> dimensionList = new List<Dimension>();
            dimensionList.Add(discreteDimensionInterval);
            IEnumerable<Dimension> heightValues = dimensionList;
            IEnumerable<Dimension> widthValues = dimensionList;
            IEnumerable<Dimension> depthValues = dimensionList;
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
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material,color1, finish2);
            CustomizedProduct customizedProduct = new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product);

            instance.addCustomizedProduct(customizedProduct);

            Assert.False(instance.Equals(other));
        }

        [Fact]
        public void ensureEqualsReturnsTrueForSlotsWithSameDimensionsAndCustomizedProductsList()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(1, 1, 1));
            Slot other = new Slot(CustomizedDimensions.valueOf(1, 1, 1));
            var category = new ProductCategory("Drawers");
            //Creating Dimensions
            List<Double> valuesList = new List<Double>();
            valuesList.Add(500.0); //Width
            DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(valuesList);
            List<Dimension> dimensionList = new List<Dimension>();
            dimensionList.Add(discreteDimensionInterval);
            IEnumerable<Dimension> heightValues = dimensionList;
            IEnumerable<Dimension> widthValues = dimensionList;
            IEnumerable<Dimension> depthValues = dimensionList;
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
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material,color1, finish2);
            CustomizedProduct customizedProduct = new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product);

            instance.addCustomizedProduct(customizedProduct);
            other.addCustomizedProduct(customizedProduct);

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureGetHashCodeIsTheSameForEqualSlots()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(1, 1, 1));
            Slot other = new Slot(CustomizedDimensions.valueOf(1, 1, 1));
            var category = new ProductCategory("Drawers");
            //Creating Dimensions
            List<Double> valuesList = new List<Double>();
            valuesList.Add(500.0); //Width
            DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(valuesList);
            List<Dimension> dimensionList = new List<Dimension>();
            dimensionList.Add(discreteDimensionInterval);
            IEnumerable<Dimension> heightValues = dimensionList;
            IEnumerable<Dimension> widthValues = dimensionList;
            IEnumerable<Dimension> depthValues = dimensionList;
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
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material,color1, finish2);
            CustomizedProduct customizedProduct = new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product);

            instance.addCustomizedProduct(customizedProduct);
            other.addCustomizedProduct(customizedProduct);

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureGetHashCodeIsDifferentForSlotsWithDifferentDimensions()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(1, 1, 1));
            Slot other = new Slot(CustomizedDimensions.valueOf(1, 2, 1));
            var category = new ProductCategory("Drawers");
            //Creating Dimensions
            List<Double> valuesList = new List<Double>();
            valuesList.Add(500.0); //Width
            DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(valuesList);
            List<Dimension> dimensionList = new List<Dimension>();
            dimensionList.Add(discreteDimensionInterval);
            IEnumerable<Dimension> heightValues = dimensionList;
            IEnumerable<Dimension> widthValues = dimensionList;
            IEnumerable<Dimension> depthValues = dimensionList;
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
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material,color1, finish2);
            CustomizedProduct customizedProduct = new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product);

            instance.addCustomizedProduct(customizedProduct);
            other.addCustomizedProduct(customizedProduct);

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureGetHashCodeIsDifferentForSlotsWithDifferentCustomizedProductsList()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(1, 1, 1));
            Slot other = new Slot(CustomizedDimensions.valueOf(1, 1, 1));
            var category = new ProductCategory("Drawers");
            //Creating Dimensions
            List<Double> valuesList = new List<Double>();
            valuesList.Add(500.0); //Width
            DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(valuesList);
            List<Dimension> dimensionList = new List<Dimension>();
            dimensionList.Add(discreteDimensionInterval);
            IEnumerable<Dimension> heightValues = dimensionList;
            IEnumerable<Dimension> widthValues = dimensionList;
            IEnumerable<Dimension> depthValues = dimensionList;
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
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material,color1, finish2);
            CustomizedProduct customizedProduct = new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product);

            instance.addCustomizedProduct(customizedProduct);

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureGetHashCodeIsDifferentForSlotsWithDifferentDimensionsAndDifferentCustomizedProductsList()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(1, 1, 1));
            Slot other = new Slot(CustomizedDimensions.valueOf(1, 2, 1));
            var category = new ProductCategory("Drawers");
            //Creating Dimensions
            List<Double> valuesList = new List<Double>();
            valuesList.Add(500.0); //Width
            DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(valuesList);
            List<Dimension> dimensionList = new List<Dimension>();
            dimensionList.Add(discreteDimensionInterval);
            IEnumerable<Dimension> heightValues = dimensionList;
            IEnumerable<Dimension> widthValues = dimensionList;
            IEnumerable<Dimension> depthValues = dimensionList;
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
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material,color1, finish2);
            CustomizedProduct customizedProduct = new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product);

            instance.addCustomizedProduct(customizedProduct);

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureToStringWorks()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(1, 1, 1));
            Slot other = new Slot(CustomizedDimensions.valueOf(1, 1, 1));
            var category = new ProductCategory("Drawers");
            //Creating Dimensions
            List<Double> valuesList = new List<Double>();
            valuesList.Add(500.0); //Width
            DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(valuesList);
            List<Dimension> dimensionList = new List<Dimension>();
            dimensionList.Add(discreteDimensionInterval);
            IEnumerable<Dimension> heightValues = dimensionList;
            IEnumerable<Dimension> widthValues = dimensionList;
            IEnumerable<Dimension> depthValues = dimensionList;
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
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material,color1, finish2);
            CustomizedProduct customizedProduct = new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product);

            instance.addCustomizedProduct(customizedProduct);
            other.addCustomizedProduct(customizedProduct);

            Assert.Equal(instance.ToString(), other.ToString());
        }

        [Fact]
        public void ensureToDTOWorks()
        {
            Slot instance = new Slot(CustomizedDimensions.valueOf(1, 1, 1));
            var category = new ProductCategory("Drawers");
            //Creating Dimensions
            List<Double> valuesList = new List<Double>();
            valuesList.Add(500.0); //Width
            DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(valuesList);
            List<Dimension> dimensionList = new List<Dimension>();
            dimensionList.Add(discreteDimensionInterval);
            IEnumerable<Dimension> heightValues = dimensionList;
            IEnumerable<Dimension> widthValues = dimensionList;
            IEnumerable<Dimension> depthValues = dimensionList;
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
            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material,color1, finish2);
            CustomizedProduct customizedProduct = new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product);

            instance.addCustomizedProduct(customizedProduct);

            SlotDTO instanceDTO = instance.toDTO();

            Assert.Equal(instance.slotDimensions.Id, instanceDTO.customizedDimensions.Id);
            Assert.Equal(instance.slotDimensions.depth, instanceDTO.customizedDimensions.depth);
            Assert.Equal(instance.slotDimensions.width, instanceDTO.customizedDimensions.width);
            Assert.Equal(instance.slotDimensions.height, instanceDTO.customizedDimensions.height);
        }
    }
}