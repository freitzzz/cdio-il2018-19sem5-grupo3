using System;
using System.Collections.Generic;
using support.dto;
using core.domain;
using core.dto;
using Xunit;
using System.Linq;
using static core.domain.CustomizedProduct;

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
        public void ensureMultipleSpacesIsNotAValidName()
        {
            Assert.Throws<ArgumentException>(() => new CustomizedProductCollection("       "));
        }

        [Fact]
        public void ensureNameCanBeChangedToValidNewName()
        {
            var oldName = "Luigi";
            var newName = "Mario";
            CustomizedProductCollection instance =
                new CustomizedProductCollection(oldName);

            instance.changeName(newName);

            Assert.NotEqual(oldName, instance.name);
        }

        [Fact]
        public void ensureNameCantBeChangedToEmptyString()
        {
            CustomizedProductCollection instance =
                new CustomizedProductCollection("Shroom");

            Assert.Throws<ArgumentException>(() => instance.changeName(""));
        }

        [Fact]
        public void ensureNameCantBeChangedToNull()
        {
            CustomizedProductCollection instance =
                new CustomizedProductCollection("Peach");

            Assert.Throws<ArgumentException>(() => instance.changeName(null));
        }

        [Fact]
        public void ensureNameCantBeChangedToMultipleSpaces()
        {
            CustomizedProductCollection instance =
                new CustomizedProductCollection("Shroom");

            Assert.Throws<ArgumentException>(() => instance.changeName("             "));
        }

        [Fact]
        public void ensureSameAsWorksForEqualCustomizedProductCollections()
        {
            Assert.True(new CustomizedProductCollection("Luigi").sameAs("Luigi"));
        }

        [Fact]
        public void ensureToDtoThrowsException()
        {
            Assert.Throws<NotImplementedException>(() => new CustomizedProductCollection("hi").toDTO());
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
            CustomizedProductCollection instance = new CustomizedProductCollection("Mario");

            instance.addCustomizedProduct(customizedProduct);

            Assert.NotEmpty(instance.collectionProducts);
            Assert.Equal(customizedProduct, instance.collectionProducts[0].customizedProduct);
        }

        [Fact]
        public void ensureAddCustomizedProductFailsIfItAlreadyExists()
        {
            CustomizedProduct customizedProduct = buildCustomizedProductInstance();
            List<CustomizedProduct> list = new List<CustomizedProduct>();
            list.Add(customizedProduct);

            CustomizedProductCollection instance = new CustomizedProductCollection("Mario", list);

            Assert.Throws<ArgumentException>(() => instance.addCustomizedProduct(customizedProduct));
        }

        [Fact]
        public void ensureAddCustomizedProductFailsIfItIsNull()
        {
            CustomizedProductCollection instance = new CustomizedProductCollection("Mario");

            Assert.Throws<ArgumentException>(() => instance.addCustomizedProduct(null));
        }

        [Fact]
        public void ensureRemovedCustomizedProductWorksForExistingProduct()
        {
            CustomizedProduct cp = buildCustomizedProductInstance();
            List<CustomizedProduct> list = new List<CustomizedProduct>();
            list.Add(cp);

            CustomizedProductCollection instance = new CustomizedProductCollection("Mario", list);

            instance.removeCustomizedProduct(cp);

            Assert.Empty(instance.collectionProducts);
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

            Product product = new Product("Kinda dead", "So tired", "riperino.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            //Customized Material
            CustomizedMaterial mat = CustomizedMaterial.valueOf(material, color1, finish2);


            CustomizedProduct cp = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", product, customizedDimensions)
                .withMaterial(mat).build();
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

            Product product = new Product("Kinda dead", "So tired", "riperino.gltf", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            //Customized Material
            CustomizedMaterial mat = CustomizedMaterial.valueOf(material, color1, finish2);

            return CustomizedProductBuilder.createAnonymousUserCustomizedProduct("serial number 123", product, customizedDimensions).withMaterial(mat).build();
        }
    }
}