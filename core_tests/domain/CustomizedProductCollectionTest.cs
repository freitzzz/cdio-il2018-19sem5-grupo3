using System;
using System.Collections.Generic;
using support.dto;
using core.domain;
using core.dto;
using Xunit;
using System.Linq;

namespace core_tests.domain
{
    /// <summary>
    /// Tests of the class CustomizedProductCollection.
    /// </summary>
    public class CustomizedProductCollectionTest
    {
        /// <summary>
        /// Test to ensure that a CustomizedProductCollection can't be created with a null list of CustomizedProducts.
        /// </summary>
        [Fact]
        public void ensureNullCustomizedProductListIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => new CustomizedProductCollection("It's-a-me, Mario", null));
        }

        /// <summary>
        /// Test to ensure that a CustomizedProductCollection can't be created with an empty list of CustomizedProducts.
        /// </summary>
        [Fact]
        public void ensureEmptyCustomizedProductListIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => new CustomizedProductCollection("It's-a-me, Mario", new List<CustomizedProduct>()));
        }

        /// <summary>
        /// Test to ensure that a CustomizedProductCollection can't be created with duplicated elements in the list of CustomizedProducts.
        /// </summary>
        [Fact]
        public void ensureCustomizedProductsListWithDuplicatesIsNotValid()
        {
            CustomizedProduct cp = buildCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);
            products.Add(cp);

            Assert.Throws<ArgumentException>(() => new CustomizedProductCollection("Mario", products));
        }


        /// <summary>
        /// Test to ensure that a CustomizedProductCollection can't be created with a null name.
        /// </summary>
        [Fact]
        public void ensureNullNameIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => new CustomizedProductCollection(null));
        }

        /// <summary>
        /// Test to ensure that a CustomizedProductCollection can't be created with an empty name.
        /// </summary>
        [Fact]
        public void ensureEmptyNameIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => new CustomizedProductCollection(""));
        }

        /// <summary>
        /// Test to ensure that a CustomizedProductCollection's name can be changed if the string is valid.
        /// </summary>
        [Fact]
        public void ensureValidNameCanBeChanged()
        {
            Assert.True(new CustomizedProductCollection("Luigi").changeName("Mario"));
        }

        /// <summary>
        /// Test to ensure that a CustomizedProductCollection's name can't be changed if the string is empty.
        /// </summary>
        [Fact]
        public void ensureEmptyNameCantBeChanged()
        {
            Assert.False(new CustomizedProductCollection("'Shroom").changeName(""));
        }

        /// <summary>
        /// Test to ensure that a CustomizedProductCollection's name can't be changed if the string is null.
        /// </summary>
        [Fact]
        public void ensureNullNameCantBeChanged()
        {
            Assert.False(new CustomizedProductCollection("Peach").changeName(null));
        }

        /// <summary>
        /// Test to ensure that a CustomizedProductCollection is the same as another entity's identity if it is the same.
        /// </summary>
        [Fact]
        public void ensureSameAsWorksForEqualCustomizedProductCollections()
        {
            Assert.True(new CustomizedProductCollection("Luigi").sameAs("Luigi"));
        }


        /// <summary>
        /// Test to ensure that the DTO is the expected.
        /// </summary>
        [Fact]
        public void ensureToDtoIsTheExpected()
        {
            var collection = new CustomizedProductCollection("Mario");
            var collectionDTO = new CustomizedProductCollectionDTO();
            collectionDTO.name = "Mario";
            collectionDTO.customizedProducts = new List<CustomizedProductDTO>(DTOUtils.parseToDTOS(collection.collectionProducts.Select(cp => cp.customizedProduct)));

            Assert.Equal(collectionDTO.name, collection.toDTO().name);
            Assert.Equal(collectionDTO.id, collection.toDTO().id);
            Assert.Equal(collectionDTO.customizedProducts, collection.toDTO().customizedProducts);
        }

        /// <summary>
        /// Test to ensure that a CustomizedProductCollection is not the same as another entity's identity if it is different.
        /// </summary>
        [Fact]
        public void ensureSameAsFailsForDifferentCustomizedProductCollections()
        {
            Assert.False(new CustomizedProductCollection("Luigi").sameAs("Mario"));
        }

        /// <summary>
        /// Test to ensure that a valid CustomizedProduct can be added to the list.
        /// </summary>
        [Fact]
        public void ensureAddCustomizedProductWorksForValidProduct()
        {
            CustomizedProduct customizedProduct = buildCustomizedProductInstance();

            Assert.True(new CustomizedProductCollection("Mario").addCustomizedProduct(customizedProduct));
        }

        /// <summary>
        /// Test to ensure that a CustomizedProduct can't be added to the list if it already exists.
        /// </summary>
        [Fact]
        public void ensureAddCustomizedProductFailsIfItAlreadyExists()
        {
            CustomizedProduct customizedProduct = buildCustomizedProductInstance();
            List<CustomizedProduct> list = new List<CustomizedProduct>();
            list.Add(customizedProduct);

            Assert.False(new CustomizedProductCollection("Mario", list).addCustomizedProduct(customizedProduct));
        }

        /// <summary>
        /// Test to ensure that a null CustomizedProduct can't be added to the list.
        /// </summary>
        [Fact]
        public void ensureAddCustomizedProductFailsIfItIsNull()
        {
            Assert.False(new CustomizedProductCollection("Mario").addCustomizedProduct(null));
        }


        /// <summary>
        /// Test to ensure that a CustomizedProduct can be removed from the list.
        /// </summary>
        [Fact]
        public void ensureRemovedCustomizedProductWorksForAlreadyExistentProduct()
        {
            CustomizedProduct cp = buildCustomizedProductInstance();
            List<CustomizedProduct> list = new List<CustomizedProduct>();
            list.Add(cp);

            Assert.True(new CustomizedProductCollection("Mario", list).removeCustomizedProduct(cp));
        }

        /// <summary>
        /// Test to ensure that the id of the CustomizedProductCollection is the expected.
        /// </summary>
        [Fact]
        public void ensureIdMethodWorks()
        {
            Assert.Equal("Mario", new CustomizedProductCollection("Mario").id());
        }

        /// <summary>
        /// Test to ensure that an already disabled CustomizedProductCollection can't be disabled.
        /// </summary>
        [Fact]
        public void ensureDisabledCustomizedProductCollectionCantBeDisabled()
        {
            CustomizedProductCollection collection = new CustomizedProductCollection("Mario");
            collection.deactivate();

            Assert.False(collection.deactivate());
        }
        /// <summary>
        /// Test to ensure that an enabled CustomizedProductCollection can be disabled.
        /// </summary>
        [Fact]
        public void ensureEnabledCustomizedProductCollectionCanBeDisabled()
        {
            Assert.True(new CustomizedProductCollection("Mario").deactivate());
        }

        /// <summary>
        /// Test to ensure that different CustomizedProductCollections are not equal.
        /// </summary>
        [Fact]
        public void ensureNotEqualCustomizedProductCollectionsAreNotEqual()
        {
            var category = new ProductCategory("It's-a-me again");

            //Creating Dimensions
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() {measurement};

            //Creating a material
            string reference = "Just referencing";
            string designation = "Doin' my thing";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Goin' to church", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Burro quando foge", 1, 2, 3, 4);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Prayin'");
            Finish finish2 = Finish.valueOf("Estragado");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("Kinda dead", "So tired", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial mat = CustomizedMaterial.valueOf(material,color1, finish2);


            CustomizedProduct cp = new CustomizedProduct("Mushrooms", "Are deadly", mat, customizedDimensions, product);
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.NotEqual(new CustomizedProductCollection("Mario", products), new CustomizedProductCollection("Luigi", products));
        }


        /// <summary>
        /// Test to ensure that two equal CustomizedProductCollections are equal.
        /// </summary>
        [Fact]
        public void ensureEqualCustomizedProductCollectionsAreEqual()
        {
            CustomizedProduct cp = buildCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.Equal(new CustomizedProductCollection("Mario", products), new CustomizedProductCollection("Mario", products));
        }

        /// <summary>
        /// Test to ensure that a different type object is not the same as a CustomizedProductCollection.
        /// </summary>
        [Fact]
        public void ensureDifferentTypeObjectIsNotEqualToCustomizedProductCollection()
        {
            CustomizedProduct cp = buildCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.False(new CustomizedProductCollection("Mario", products).Equals("Something"));
        }

        /// <summary>
        /// Test to ensure that a null object is not the same as a CustomizedProductCollection.
        /// </summary>
        [Fact]
        public void ensureNullObjectIsNotEqualToCustomizedProductCollection()
        {
            CustomizedProduct cp = buildCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.False(new CustomizedProductCollection("Mario", products).Equals(null));
        }

        /// <summary>
        /// Test to ensure that the generated hash code is the same for equal CustomizedProductCollections.
        /// </summary>
        [Fact]
        public void ensureHashCodeWorks()
        {
            CustomizedProduct cp = buildCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.Equal(new CustomizedProductCollection("Mario", products).GetHashCode(),
            new CustomizedProductCollection("Mario", products).GetHashCode());
        }

        /// <summary>
        /// Test to ensure that the string that describes the CustomizedProductCollection is the same for equal CustomizedProductCollections.
        /// </summary>
        [Fact]
        public void ensureToStringWorks()
        {

            CustomizedProduct cp = buildCustomizedProductInstance();

            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.Equal(new CustomizedProductCollection("Mario", products).ToString(),
            new CustomizedProductCollection("Mario", products).ToString());
        }

        [Fact]
        public void ensureHasCustomizedProductReturnsFalseIfCustomizedProductIsNull()
        {
            CustomizedProductCollection collection = new CustomizedProductCollection("Collection");

            Assert.False(collection.hasCustomizedProduct(null));
        }

        [Fact]
        public void ensureHasCustomizedProductReturnsFalseIfCustomizedProductWasNotAdded()
        {
            CustomizedProductCollection collection = new CustomizedProductCollection("Collection");

            CustomizedProduct customizedProduct = buildCustomizedProductInstance();

            Assert.False(collection.hasCustomizedProduct(customizedProduct));
        }

        [Fact]
        public void ensureHasCustomizedProductReturnsTrueIfCustomizedProductWasAdded()
        {
            CustomizedProductCollection collection = new CustomizedProductCollection("Collection");

            CustomizedProduct customizedProduct = buildCustomizedProductInstance();

            collection.addCustomizedProduct(customizedProduct);

            Assert.True(collection.hasCustomizedProduct(customizedProduct));
        }

        private CustomizedProduct buildCustomizedProductInstance()
        {
            var category = new ProductCategory("It's-a-me again");

            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() {measurement};

            //Creating a material
            string reference = "Just referencing";
            string designation = "Doin' my thing";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Goin' to church", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Burro quando foge", 1, 2, 3, 4);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Prayin'");
            Finish finish2 = Finish.valueOf("Estragado");
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("Kinda dead", "So tired", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            CustomizedMaterial mat = CustomizedMaterial.valueOf(material,color1, finish2);



            return new CustomizedProduct("Peach", "Luigi", mat, customizedDimensions, product);
        }
    }
}