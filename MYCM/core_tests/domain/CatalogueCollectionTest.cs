using System;
using System.Collections.Generic;
using support.dto;
using core.domain;
using core.dto;
using Xunit;
using static core.domain.CustomizedProduct;

namespace core_tests.domain
{
    /// <summary>
    /// Tests of the class CatalogueCollection
    /// </summary>
    public class CatalogueCollectionTest
    {
        private CustomizedProduct buildCustomizedProduct(string serialNumber)
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
            Finish finish = Finish.valueOf("Amém", 12);
            finishes.Add(finish);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.glb", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color, finish);

            return CustomizedProductBuilder.createAnonymousUserCustomizedProduct(serialNumber, product, customizedDimensions).withMaterial(custMaterial1).build();
        }

        private CustomizedProductCollection buildCustomizedProductCollection()
        {
            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("Closets Spring 2019");

            CustomizedProduct customizedProduct1 = buildCustomizedProduct("1234");
            customizedProduct1.finalizeCustomization();     //!customized products added to collections need to be finished

            customizedProductCollection.addCustomizedProduct(customizedProduct1);

            CustomizedProduct customizedProduct2 = buildCustomizedProduct("1235");
            customizedProduct2.finalizeCustomization();     //!customized products added to collections need to be finished

            customizedProductCollection.addCustomizedProduct(customizedProduct2);

            return customizedProductCollection;
        }

        private CatalogueCollection buildCatalogueCollection()
        {
            return new CatalogueCollection(buildCustomizedProductCollection());
        }

        [Fact]
        public void ensureCatalogueCollectionCantBeCreatedWithNullCustomizedProductCollection()
        {
            Action createCatalogueCollection = () => new CatalogueCollection(null);

            Assert.Throws<ArgumentException>(createCatalogueCollection);
        }

        [Fact]
        public void ensureCatalogueCollectionCantBeCreatedWithNullCustomizedProducts()
        {
            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("Closets Spring 2019");

            Action createCatalogueCollection = () => new CatalogueCollection(customizedProductCollection, null);

            Assert.Throws<ArgumentException>(createCatalogueCollection);
        }

        [Fact]
        public void ensureCatalogueCollectionCantBeCreatedWithNullCustomizedProductCollectionWhenSpecifyingCustomizedProducts()
        {
            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("Closets Spring 2019");

            Action createCatalogueCollection = () => new CatalogueCollection(null, new List<CustomizedProduct>());

            Assert.Throws<ArgumentException>(createCatalogueCollection);
        }

        [Fact]
        public void ensureCatalogueCollectionCantBeCreatedWithProductsNotInCustomizedProductCollection()
        {
            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("Closets Spring 2019");

            CustomizedProduct customizedProduct1 = buildCustomizedProduct("1234");
            customizedProduct1.finalizeCustomization();     //!customized products added to collections need to be finished

            customizedProductCollection.addCustomizedProduct(customizedProduct1);

            CustomizedProduct customizedProduct2 = buildCustomizedProduct("1235");

            List<CustomizedProduct> customizedProducts = new List<CustomizedProduct>() { customizedProduct1, customizedProduct2 };

            Action createCatalogueCollection = () => new CatalogueCollection(customizedProductCollection, customizedProducts);

            Assert.Throws<ArgumentException>(createCatalogueCollection);
        }

        [Fact]
        public void ensureCatalogueCollectionCantBeCreatedWithNullCustomizedProductsInCustomizedProductEnumerable()
        {
            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("Closets Spring 2019");

            CustomizedProduct customizedProduct1 = buildCustomizedProduct("1234");
            customizedProduct1.finalizeCustomization();     //!customized products added to collections need to be finished

            customizedProductCollection.addCustomizedProduct(customizedProduct1);

            CustomizedProduct customizedProduct2 = buildCustomizedProduct("1235");

            List<CustomizedProduct> customizedProducts = new List<CustomizedProduct>() { customizedProduct1, customizedProduct2, null };

            Action createCatalogueCollection = () => new CatalogueCollection(customizedProductCollection, customizedProducts);

            Assert.Throws<ArgumentException>(createCatalogueCollection);
        }

        [Fact]
        public void ensureCatalogueCollectionCanBeCreatedIfCustomizedProductCollectionIsNotNull()
        {
            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("Closets Spring 2019");

            CatalogueCollection catalogueCollection = new CatalogueCollection(customizedProductCollection);

            Assert.NotNull(catalogueCollection);
        }

        [Fact]
        public void ensureCatalogueCollectionCanBeCreatedIfAllCustomizedProductsWereAddedToCustomizedProductCollection()
        {
            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("Closets Spring 2019");

            CustomizedProduct customizedProduct1 = buildCustomizedProduct("1234");
            customizedProduct1.finalizeCustomization();     //!customized products added to collections need to be finished

            customizedProductCollection.addCustomizedProduct(customizedProduct1);

            CustomizedProduct customizedProduct2 = buildCustomizedProduct("1235");
            customizedProduct2.finalizeCustomization();     //!customized products added to collections need to be finished

            customizedProductCollection.addCustomizedProduct(customizedProduct2);

            List<CustomizedProduct> customizedProducts = new List<CustomizedProduct>() { customizedProduct1, customizedProduct2 };

            CatalogueCollection catalogueCollection = new CatalogueCollection(customizedProductCollection, customizedProducts);

            Assert.NotNull(catalogueCollection);
        }

        [Fact]
        public void ensureCreatingCatalogueCollectionWithCustomizedProductCollectionAddsAllCustomizedProducts()
        {
            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("Closets Spring 2019");

            CustomizedProduct customizedProduct1 = buildCustomizedProduct("1234");
            customizedProduct1.finalizeCustomization();     //!customized products added to collections need to be finished

            customizedProductCollection.addCustomizedProduct(customizedProduct1);

            CustomizedProduct customizedProduct2 = buildCustomizedProduct("1235");
            customizedProduct2.finalizeCustomization();     //!customized products added to collections need to be finished

            customizedProductCollection.addCustomizedProduct(customizedProduct2);

            CatalogueCollection catalogueCollection = new CatalogueCollection(customizedProductCollection);

            Assert.True(catalogueCollection.hasCustomizedProduct(customizedProduct1));
            Assert.True(catalogueCollection.hasCustomizedProduct(customizedProduct2));
        }


        [Fact]
        public void ensureAddingNullCustomizedProductThrowsException()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            Action addNullCustomizedProduct = () => catalogueCollection.addCustomizedProduct(null);

            Assert.Throws<ArgumentException>(addNullCustomizedProduct);
        }

        [Fact]
        public void ensureAddingNullCustomizedProductDoesNotAddCustomizedProduct()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            try
            {
                catalogueCollection.addCustomizedProduct(null);
            }
            catch (Exception) { }

            Assert.Equal(2, catalogueCollection.catalogueCollectionProducts.Count);
        }

        [Fact]
        public void ensureAddingCustomizedProductNotAddedToCustomizedProductCollectionThrowsException()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            CustomizedProduct customizedProduct = buildCustomizedProduct("12345");

            Action addCustomizedProduct = () => catalogueCollection.addCustomizedProduct(customizedProduct);

            Assert.Throws<ArgumentException>(addCustomizedProduct);
        }

        [Fact]
        public void ensureAddingCustomizedProductNotAddedTocustomizedProductCollectionDoesNotAddCustomizedProduct()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            CustomizedProduct customizedProduct = buildCustomizedProduct("12345");

            try
            {
                catalogueCollection.addCustomizedProduct(customizedProduct);
            }
            catch (Exception) { }

            Assert.Equal(2, catalogueCollection.catalogueCollectionProducts.Count);
            Assert.False(catalogueCollection.hasCustomizedProduct(customizedProduct));
        }

        [Fact]
        public void ensureAddingDuplicateCustomizedProductThrowsException()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            CustomizedProduct customizedProduct = buildCustomizedProduct("1234");

            Action addCustomizedProduct = () => catalogueCollection.addCustomizedProduct(customizedProduct);

            Assert.Throws<ArgumentException>(addCustomizedProduct);
        }

        [Fact]
        public void ensureAddingDuplicateCustomizedProductDoesNotAddCustomizedProduct()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            CustomizedProduct customizedProduct = buildCustomizedProduct("1234");

            try
            {
                catalogueCollection.addCustomizedProduct(customizedProduct);
            }
            catch (Exception) { }

            Assert.Equal(2, catalogueCollection.catalogueCollectionProducts.Count);
        }

        [Fact]
        public void ensureAddingValidCustomizedProductDoesNotThrowException()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            CustomizedProductCollection customizedProductCollection = catalogueCollection.customizedProductCollection;

            CustomizedProduct customizedProduct = buildCustomizedProduct("123545");
            customizedProduct.finalizeCustomization();

            customizedProductCollection.addCustomizedProduct(customizedProduct);

            Action addValidCustomizedProduct = () => catalogueCollection.addCustomizedProduct(customizedProduct);

            Exception exception = Record.Exception(addValidCustomizedProduct);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddingValidCustomizedProductAddsCustomizedProduct()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            CustomizedProductCollection customizedProductCollection = catalogueCollection.customizedProductCollection;

            CustomizedProduct customizedProduct = buildCustomizedProduct("123545");
            customizedProduct.finalizeCustomization();

            customizedProductCollection.addCustomizedProduct(customizedProduct);

            catalogueCollection.addCustomizedProduct(customizedProduct);

            Assert.Equal(3, catalogueCollection.catalogueCollectionProducts.Count);
            Assert.True(catalogueCollection.hasCustomizedProduct(customizedProduct));
        }

        [Fact]
        public void ensureRemovingNullCustomizedProductThrowsException()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            Action removeCustomizedProduct = () => catalogueCollection.removeCustomizedProduct(null);

            Assert.Throws<ArgumentException>(removeCustomizedProduct);
        }

        [Fact]
        public void ensureRemovingNullCustomizedProductDoesNotRemoveCustomizedProduct()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            try
            {
                catalogueCollection.removeCustomizedProduct(null);
            }
            catch (Exception) { }

            Assert.Equal(2, catalogueCollection.catalogueCollectionProducts.Count);
        }

        [Fact]
        public void ensureRemovingNotAddedCustomizedProductThrowsException()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            CustomizedProduct customizedProduct = buildCustomizedProduct("12354112");

            Action removeCustomizedProduct = () => catalogueCollection.removeCustomizedProduct(customizedProduct);

            Assert.Throws<ArgumentException>(removeCustomizedProduct);
        }

        [Fact]
        public void ensureRemovingNotAddedCustomizedProductDoesNotRemoveCustomizedProduct()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            CustomizedProduct customizedProduct = buildCustomizedProduct("12354112");

            try
            {
                catalogueCollection.removeCustomizedProduct(customizedProduct);
            }
            catch (Exception) { }

            Assert.Equal(2, catalogueCollection.catalogueCollectionProducts.Count);
        }

        [Fact]
        public void ensureRemovingValidCustomizedProductDoesNotThrowException()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            CustomizedProduct customizedProduct = buildCustomizedProduct("1234");

            Action removeCustomizedProduct = () => catalogueCollection.removeCustomizedProduct(customizedProduct);

            Exception exception = Record.Exception(removeCustomizedProduct);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureRemovingValidCustomizedProductRemovesCustomizedProduct()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            CustomizedProduct customizedProduct = buildCustomizedProduct("1234");

            catalogueCollection.removeCustomizedProduct(customizedProduct);

            Assert.False(catalogueCollection.hasCustomizedProduct(customizedProduct));
            Assert.Single(catalogueCollection.catalogueCollectionProducts);
        }

        [Fact]
        public void ensureIdReturnsCustomizedProductCollectionBusinessIdentifier()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            Assert.Equal(catalogueCollection.id(), catalogueCollection.customizedProductCollection.id());
        }

        [Fact]
        public void ensureSameAsReturnsTrueIfArgumentIsEqualToTheBusinessIdentifier()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            string name = catalogueCollection.customizedProductCollection.name;

            Assert.True(catalogueCollection.sameAs(name));
        }

        [Fact]
        public void ensureSameAsReturnsFalseIfArgumentIsNotEqualToTheBusinessIdentifier()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            string name = "some other name";

            Assert.False(catalogueCollection.sameAs(name));
        }


        [Fact]
        public void ensureInstancesHaveSameHashCode()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();
            CatalogueCollection catalogueCollection2 = buildCatalogueCollection();

            Assert.Equal(catalogueCollection.GetHashCode(), catalogueCollection2.GetHashCode());
        }

        [Fact]
        public void ensureInstancesHaveDifferentHashCode()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("Some other collection");
            CatalogueCollection otherCatalogueCollection = new CatalogueCollection(customizedProductCollection);

            Assert.NotEqual(catalogueCollection.GetHashCode(), otherCatalogueCollection.GetHashCode());
        }

        [Fact]
        public void ensureInstanceIsEqualToItself()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            Assert.True(catalogueCollection.Equals(catalogueCollection));
        }

        [Fact]
        public void ensureNullIsNotEqual()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            Assert.False(catalogueCollection.Equals(null));
        }

        [Fact]
        public void ensureDifferentObjectTypeIsNotEqual()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            Assert.False(catalogueCollection.Equals("this is a string"));
        }

        [Fact]
        public void ensureCatalogueCollectionWithDifferentCustomizedProductCollectionIsNotEqual()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("Some other collection");
            CatalogueCollection otherCatalogueCollection = new CatalogueCollection(customizedProductCollection);

            Assert.False(catalogueCollection.Equals(otherCatalogueCollection));
        }

        [Fact]
        public void ensureCatalogueCollectionWithEqualCustomizedProductCollectionIsEqual()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            CatalogueCollection otherCatalogueCollection = buildCatalogueCollection();

            Assert.True(catalogueCollection.Equals(otherCatalogueCollection));
        }

        [Fact]
        public void ensureToStringIsEqualIfInstancesAreEqual()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection();

            CatalogueCollection otherCatalogueCollection = buildCatalogueCollection();

            Assert.Equal(catalogueCollection.ToString(), otherCatalogueCollection.ToString());
        }

    }
}