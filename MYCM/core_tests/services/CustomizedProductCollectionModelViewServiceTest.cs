using System;
using core.modelview.customizedproductcollection;
using core.domain;
using Xunit;
using System.Collections.Generic;
using static core.domain.CustomizedProduct;
using core.modelview.customizedproduct;

namespace core_tests.services
{
    /// <summary>
    /// Unit testing class for CustomizedProductCollectionModelView
    /// </summary>
    public class CustomizedProductCollectionModelViewServiceTest
    {
        [Fact]
        public void ensureFromEntityAsBasicThrowsExceptionIfEntityIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => CustomizedProductCollectionModelViewService.fromEntityAsBasic(null));
        }

        [Fact]
        public void ensureFromEntityAsBasicReturnsBasicModelViewIfEntityIsValid()
        {

            CustomizedProductCollection customizedProductCollection =
                createCustomizedProductCollection();

            GetBasicCustomizedProductCollectionModelView expectedModelView =
                new GetBasicCustomizedProductCollectionModelView();

            expectedModelView.name = customizedProductCollection.name;
            expectedModelView.hasCustomizedProducts = customizedProductCollection.collectionProducts.Count != 0;

            GetBasicCustomizedProductCollectionModelView actualModelView =
                CustomizedProductCollectionModelViewService.fromEntityAsBasic(customizedProductCollection);

            assertBasicModelView(expectedModelView, actualModelView);
        }

        [Fact]
        public void ensureFromEntityThrowsExceptionIfEntityIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => CustomizedProductCollectionModelViewService.fromEntity(null));
        }

        [Fact]
        public void ensureFromEntityReturnsModelViewIfEntityIsValid()
        {
            CustomizedProductCollection customizedProductCollection =
                createCustomizedProductCollection();

            GetCustomizedProductCollectionModelView expectedModelView =
                new GetCustomizedProductCollectionModelView();

            expectedModelView.name = customizedProductCollection.name;
            expectedModelView.customizedProducts = new List<GetBasicCustomizedProductModelView>();
            expectedModelView.customizedProducts.Add(new GetBasicCustomizedProductModelView());
            expectedModelView.customizedProducts[0].designation =
                customizedProductCollection.collectionProducts[0].customizedProduct.designation;
            expectedModelView.customizedProducts[0].reference =
                customizedProductCollection.collectionProducts[0].customizedProduct.reference;
            expectedModelView.customizedProducts[0].serialNumber =
                customizedProductCollection.collectionProducts[0].customizedProduct.serialNumber;
            expectedModelView.customizedProducts[0].productId =
                customizedProductCollection.collectionProducts[0].customizedProduct.product.Id;

            GetCustomizedProductCollectionModelView actualModelView =
                CustomizedProductCollectionModelViewService.fromEntity(customizedProductCollection);

            Assert.Equal(expectedModelView.name, actualModelView.name);
            Assert.Equal(expectedModelView.customizedProducts[0].designation,
                        actualModelView.customizedProducts[0].designation);
            Assert.Equal(expectedModelView.customizedProducts[0].reference,
                        actualModelView.customizedProducts[0].reference);
            Assert.Equal(expectedModelView.customizedProducts[0].serialNumber,
                        actualModelView.customizedProducts[0].serialNumber);
            Assert.Equal(expectedModelView.customizedProducts[0].productId,
                        actualModelView.customizedProducts[0].productId);
        }

        [Fact]
        public void ensureFromCollectionThrowsExceptionIfCollectionIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => CustomizedProductCollectionModelViewService.fromCollection(null));
        }

        [Fact]
        public void ensureFromCollectionReturnsModelViewIfCollectionIsValid()
        {
            CustomizedProductCollection customizedProductCollection =
                createCustomizedProductCollection();

            CustomizedProductCollection otherCustomizedProductCollection =
                createCustomizedProductCollection();

            GetAllCustomizedProductCollectionsModelView expectedModelView =
                new GetAllCustomizedProductCollectionsModelView();

            expectedModelView =
                new GetAllCustomizedProductCollectionsModelView(){
                    CustomizedProductCollectionModelViewService.fromEntityAsBasic(customizedProductCollection),
                    CustomizedProductCollectionModelViewService.fromEntityAsBasic(otherCustomizedProductCollection)
                };

            List<CustomizedProductCollection> collection =
                new List<CustomizedProductCollection>() { 
                    customizedProductCollection, otherCustomizedProductCollection };

            GetAllCustomizedProductCollectionsModelView actualModelView =
                CustomizedProductCollectionModelViewService.fromCollection(collection);

            assertBasicModelView(expectedModelView[0], actualModelView[0]);
            assertBasicModelView(expectedModelView[1], actualModelView[1]);
        }

        private void assertBasicModelView(GetBasicCustomizedProductCollectionModelView expectedModelView, GetBasicCustomizedProductCollectionModelView actualModelView)
        {
            Assert.Equal(expectedModelView.name, actualModelView.name);
            Assert.Equal(expectedModelView.hasCustomizedProducts, actualModelView.hasCustomizedProducts);
        }

        private CustomizedProductCollection createCustomizedProductCollection()
        {
            CustomizedProductCollection customizedProductCollection =
                new CustomizedProductCollection("hello");

            CustomizedProduct customizedProduct =
                buildCustomizedProductInstance();

            customizedProductCollection.addCustomizedProduct(customizedProduct);

            return customizedProductCollection;
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

            CustomizedProduct customizedProduct =
                 CustomizedProductBuilder.createAnonymousUserCustomizedProduct
                    (
                     "serial number 123", product, customizedDimensions
                    ).withMaterial(mat).build();
            
            customizedProduct.finalizeCustomization();

            return customizedProduct;
        }
    }
}