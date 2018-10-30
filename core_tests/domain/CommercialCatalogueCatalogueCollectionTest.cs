using System;
using Xunit;
using core.domain;
using System.Collections.Generic;

namespace support_tests.domain
{
    /// <summary>
    /// Unit testing for class CommercialCatalogueCatalogueCollection.
    /// </summary>
    public class CommercialCatalogueCatalogueCollectionTest
    {

        [Fact]
        public void ensureObjectIsNotCreatedForNullCommercialCatalogue()
        {
            Color color = Color.valueOf("Somebody", 1, 2, 3, 4);
            List<Color> colors = new List<Color>();
            colors.Add(color);

            Finish finish = Finish.valueOf("once");
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            List<Double> values = new List<Double>();
            values.Add(500.0);

            DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(values);
            List<Dimension> dimensionList = new List<Dimension>();
            dimensionList.Add(discreteDimensionInterval);

            Material material = new Material("told", "me", colors, finishes);
            List<Material> materialList = new List<Material>();
            materialList.Add(material);

            ProductCategory productCategory = new ProductCategory("the");

            IEnumerable<Dimension> heightDimensionsEnumerable = dimensionList;
            IEnumerable<Dimension> widthDimensionsEnumerable = dimensionList;
            IEnumerable<Dimension> depthDimensionsEnumerable = dimensionList;
            IEnumerable<Material> materialEnumerable = materialList;

            Product product = new Product("world", "is", productCategory, materialEnumerable,
            heightDimensionsEnumerable, widthDimensionsEnumerable, depthDimensionsEnumerable);

            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);
            CustomizedProduct customizedProduct = new CustomizedProduct("gonna", "roll", customizedMaterial,
            customizedDimensions, product);

            List<CustomizedProduct> customizedProductList = new List<CustomizedProduct>();
            customizedProductList.Add(customizedProduct);

            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("me",
            customizedProductList);
            List<CustomizedProductCollection> collectionList = new List<CustomizedProductCollection>();
            collectionList.Add(customizedProductCollection);

            CatalogueCollection catalogueCollection = new CatalogueCollection(customizedProductCollection,
            customizedProductList);

            Assert.Throws<ArgumentException>(() => new CommercialCatalogueCatalogueCollection(null,
            catalogueCollection));
        }

        [Fact]
        public void ensureObjectIsNotCreatedForNullCatalogueCollection()
        {
            Color color = Color.valueOf("Somebody", 1, 2, 3, 4);
            List<Color> colors = new List<Color>();
            colors.Add(color);

            Finish finish = Finish.valueOf("once");
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            List<Double> values = new List<Double>();
            values.Add(500.0);

            DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(values);
            List<Dimension> dimensionList = new List<Dimension>();
            dimensionList.Add(discreteDimensionInterval);

            Material material = new Material("told", "me", colors, finishes);
            List<Material> materialList = new List<Material>();
            materialList.Add(material);

            ProductCategory productCategory = new ProductCategory("the");

            IEnumerable<Dimension> heightDimensionsEnumerable = dimensionList;
            IEnumerable<Dimension> widthDimensionsEnumerable = dimensionList;
            IEnumerable<Dimension> depthDimensionsEnumerable = dimensionList;
            IEnumerable<Material> materialEnumerable = materialList;

            Product product = new Product("world", "is", productCategory, materialEnumerable,
            heightDimensionsEnumerable, widthDimensionsEnumerable, depthDimensionsEnumerable);

            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);
            CustomizedProduct customizedProduct = new CustomizedProduct("gonna", "roll", customizedMaterial,
            customizedDimensions, product);

            List<CustomizedProduct> customizedProductList = new List<CustomizedProduct>();
            customizedProductList.Add(customizedProduct);

            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("me",
            customizedProductList);
            List<CustomizedProductCollection> collectionList = new List<CustomizedProductCollection>();
            collectionList.Add(customizedProductCollection);

            CatalogueCollection catalogueCollection = new CatalogueCollection(customizedProductCollection,
            customizedProductList);
            List<CatalogueCollection> catalogueCollectionList = new List<CatalogueCollection>();
            catalogueCollectionList.Add(catalogueCollection);

            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("I", "ain't", catalogueCollectionList);

            Assert.Throws<ArgumentException>(() => new CommercialCatalogueCatalogueCollection(commercialCatalogue,
            null));
        }

        [Fact]
        public void ensureObjectIsCreatedWithValidParameters()
        {
            Color color = Color.valueOf("Somebody", 1, 2, 3, 4);
            List<Color> colors = new List<Color>();
            colors.Add(color);

            Finish finish = Finish.valueOf("once");
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            List<Double> values = new List<Double>();
            values.Add(500.0);

            DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(values);
            List<Dimension> dimensionList = new List<Dimension>();
            dimensionList.Add(discreteDimensionInterval);

            Material material = new Material("told", "me", colors, finishes);
            List<Material> materialList = new List<Material>();
            materialList.Add(material);

            ProductCategory productCategory = new ProductCategory("the");

            IEnumerable<Dimension> heightDimensionsEnumerable = dimensionList;
            IEnumerable<Dimension> widthDimensionsEnumerable = dimensionList;
            IEnumerable<Dimension> depthDimensionsEnumerable = dimensionList;
            IEnumerable<Material> materialEnumerable = materialList;

            Product product = new Product("world", "is", productCategory, materialEnumerable,
            heightDimensionsEnumerable, widthDimensionsEnumerable, depthDimensionsEnumerable);

            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);
            CustomizedProduct customizedProduct = new CustomizedProduct("gonna", "roll", customizedMaterial,
            customizedDimensions, product);

            List<CustomizedProduct> customizedProductList = new List<CustomizedProduct>();
            customizedProductList.Add(customizedProduct);

            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("me",
            customizedProductList);
            List<CustomizedProductCollection> collectionList = new List<CustomizedProductCollection>();
            collectionList.Add(customizedProductCollection);

            CatalogueCollection catalogueCollection = new CatalogueCollection(customizedProductCollection,
            customizedProductList);
            List<CatalogueCollection> catalogueCollectionList = new List<CatalogueCollection>();
            catalogueCollectionList.Add(catalogueCollection);

            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("I", "ain't", catalogueCollectionList);

            Assert.NotNull(new CommercialCatalogueCatalogueCollection(commercialCatalogue, catalogueCollection));
        }
    }
}