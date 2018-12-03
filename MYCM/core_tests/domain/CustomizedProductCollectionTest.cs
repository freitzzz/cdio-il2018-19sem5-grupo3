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

        [Fact]
        public void ensureNullCustomizedProductListIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => new CustomizedProductCollection("It's-a-me, Mario", null));
        }

        [Fact]
        public void ensureEmptyCustomizedProductListIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => new CustomizedProductCollection("It's-a-me, Mario", new List<CustomizedProduct>()));
        }

        [Fact]
        public void ensureCustomizedProductsListWithDuplicatesIsNotValid()
        {
            CustomizedProduct cp = buildCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);
            products.Add(cp);

            Assert.Throws<ArgumentException>(() => new CustomizedProductCollection("Mario", products));
        }

        [Fact]
        public void ensureNullNameIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => new CustomizedProductCollection(null));
        }

        [Fact]
        public void ensureEmptyNameIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => new CustomizedProductCollection(""));
        }

        [Fact]
        public void ensureValidNameCanBeChanged()
        {
            Assert.True(new CustomizedProductCollection("Luigi").changeName("Mario"));
        }

        [Fact]
        public void ensureEmptyNameCantBeChanged()
        {
            Assert.False(new CustomizedProductCollection("'Shroom").changeName(""));
        }

        [Fact]
        public void ensureNullNameCantBeChanged()
        {
            Assert.False(new CustomizedProductCollection("Peach").changeName(null));
        }

        [Fact]
        public void ensureSameAsWorksForEqualCustomizedProductCollections()
        {
            Assert.True(new CustomizedProductCollection("Luigi").sameAs("Luigi"));
        }

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

        [Fact]
        public void ensureSameAsFailsForDifferentCustomizedProductCollections()
        {
            Assert.False(new CustomizedProductCollection("Luigi").sameAs("Mario"));
        }

        [Fact]
        public void ensureAddCustomizedProductWorksForValidProduct()
        {
            CustomizedProduct customizedProduct = buildCustomizedProductInstance();
            Assert.True(new CustomizedProductCollection("Mario").addCustomizedProduct(customizedProduct));
        }

        [Fact]
        public void ensureAddCustomizedProductFailsIfItAlreadyExists()
        {
            CustomizedProduct customizedProduct = buildCustomizedProductInstance();
            List<CustomizedProduct> list = new List<CustomizedProduct>();
            list.Add(customizedProduct);

            Assert.False(new CustomizedProductCollection("Mario", list).addCustomizedProduct(customizedProduct));
        }

        [Fact]
        public void ensureAddCustomizedProductFailsIfItIsNull()
        {
            Assert.False(new CustomizedProductCollection("Mario").addCustomizedProduct(null));
        }

        [Fact]
        public void ensureRemovedCustomizedProductWorksForAlreadyExistentProduct()
        {
            CustomizedProduct cp = buildCustomizedProductInstance();
            List<CustomizedProduct> list = new List<CustomizedProduct>();
            list.Add(cp);

            Assert.True(new CustomizedProductCollection("Mario", list).removeCustomizedProduct(cp));
        }

        [Fact]
        public void ensureIdMethodWorks()
        {
            Assert.Equal("Mario", new CustomizedProductCollection("Mario").id());
        }

        [Fact]
        public void ensureDisabledCustomizedProductCollectionCantBeDisabled()
        {
            CustomizedProductCollection collection = new CustomizedProductCollection("Mario");
            collection.deactivate();

            Assert.False(collection.deactivate());
        }

        [Fact]
        public void ensureEnabledCustomizedProductCollectionCanBeDisabled()
        {
            Assert.True(new CustomizedProductCollection("Mario").deactivate());
        }

        [Fact]
        public void ensureNotEqualCustomizedProductCollectionsAreNotEqual()
        {
            var category = new ProductCategory("It's-a-me again");

            //Creating Dimensions
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "Just referencing";
            string designation = "Doin' my thing";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Goin' to church", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Burro quando foge", 1, 2, 3, 4);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Prayin'", 3);
            Finish finish2 = Finish.valueOf("Estragado", 9);
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("Kinda dead", "So tired", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            //Customized Material
            CustomizedMaterial mat = CustomizedMaterial.valueOf(material, color1, finish2);


            CustomizedProduct cp = new CustomizedProduct("Mushrooms", "Are deadly", mat, customizedDimensions, product);
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.NotEqual(new CustomizedProductCollection("Mario", products), new CustomizedProductCollection("Luigi", products));
        }

        [Fact]
        public void ensureEqualCustomizedProductCollectionsAreEqual()
        {
            CustomizedProduct cp = buildCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.Equal(new CustomizedProductCollection("Mario", products), new CustomizedProductCollection("Mario", products));
        }

        [Fact]
        public void ensureDifferentTypeObjectIsNotEqualToCustomizedProductCollection()
        {
            CustomizedProduct cp = buildCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.False(new CustomizedProductCollection("Mario", products).Equals("Something"));
        }

        [Fact]
        public void ensureNullObjectIsNotEqualToCustomizedProductCollection()
        {
            CustomizedProduct cp = buildCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.False(new CustomizedProductCollection("Mario", products).Equals(null));
        }

        [Fact]
        public void ensureHashCodeWorks()
        {
            CustomizedProduct cp = buildCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.Equal(new CustomizedProductCollection("Mario", products).GetHashCode(),
            new CustomizedProductCollection("Mario", products).GetHashCode());
        }

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
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "Just referencing";
            string designation = "Doin' my thing";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Goin' to church", 1, 2, 3, 0);
            Color color1 = Color.valueOf("Burro quando foge", 1, 2, 3, 4);
            colors.Add(color);
            colors.Add(color1);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Prayin'", 12);
            Finish finish2 = Finish.valueOf("Estragado", 13);
            finishes.Add(finish);
            finishes.Add(finish2);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("Kinda dead", "So tired", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            //Customized Material
            CustomizedMaterial mat = CustomizedMaterial.valueOf(material, color1, finish2);



            return new CustomizedProduct("Peach", "Luigi", mat, customizedDimensions, product);
        }
    }
}