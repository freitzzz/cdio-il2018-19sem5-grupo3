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
            CustomizedProduct cp = buildFinishedCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);
            products.Add(cp);

            Assert.Throws<ArgumentException>(() => new CustomizedProductCollection("Mario", products));
        }

        [Fact]
        public void ensureCollectionCantBeInstantiatedWithUnfinishedCustomizedProducts()
        {
            Assert.Throws<ArgumentException>(
                () => new CustomizedProductCollection(
                        "Mario",
                        new List<CustomizedProduct>(){
                            buildFinishedCustomizedProductInstance(),
                            buildUnfinishedCustomizedProductInstance()
                        }
                    )
            );
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
            CustomizedProduct customizedProduct = buildFinishedCustomizedProductInstance();
            CustomizedProductCollection instance = new CustomizedProductCollection("Mario");

            instance.addCustomizedProduct(customizedProduct);

            Assert.NotEmpty(instance.collectionProducts);
            Assert.Equal(customizedProduct, instance.collectionProducts[0].customizedProduct);
        }

        [Fact]
        public void ensureAddCustomizedProductFailsIfItAlreadyExists()
        {
            CustomizedProduct customizedProduct = buildFinishedCustomizedProductInstance();
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
        public void ensureAddingPendingCustomizedProductThrowsException()
        {
            Product product = buildValidProduct();

            CustomizedProductCollection instance = new CustomizedProductCollection("Mario");

            Assert.Throws<ArgumentException>(() => instance.addCustomizedProduct(
                buildUnfinishedCustomizedProductInstance()
            ));
        }

        [Fact]
        public void ensureRemovedCustomizedProductWorksForExistingProduct()
        {
            CustomizedProduct cp = buildFinishedCustomizedProductInstance();
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
            cp.finalizeCustomization();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.NotEqual(new CustomizedProductCollection("Mario", products), new CustomizedProductCollection("Luigi", products));
        }

        [Fact]
        public void ensureEqualCustomizedProductCollectionsAreEqual()
        {
            CustomizedProduct cp = buildFinishedCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.Equal(new CustomizedProductCollection("Mario", products), new CustomizedProductCollection("Mario", products));
        }

        [Fact]
        public void ensureDifferentTypeObjectIsNotEqualToCustomizedProductCollection()
        {
            CustomizedProduct cp = buildFinishedCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.False(new CustomizedProductCollection("Mario", products).Equals("Something"));
        }

        [Fact]
        public void ensureNullObjectIsNotEqualToCustomizedProductCollection()
        {
            CustomizedProduct cp = buildFinishedCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.False(new CustomizedProductCollection("Mario", products).Equals(null));
        }

        [Fact]
        public void ensureHashCodeWorks()
        {
            CustomizedProduct cp = buildFinishedCustomizedProductInstance();
            List<CustomizedProduct> products = new List<CustomizedProduct>();
            products.Add(cp);

            Assert.Equal(new CustomizedProductCollection("Mario", products).GetHashCode(),
            new CustomizedProductCollection("Mario", products).GetHashCode());
        }

        [Fact]
        public void ensureToStringWorks()
        {

            CustomizedProduct cp = buildFinishedCustomizedProductInstance();

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

            CustomizedProduct customizedProduct = buildFinishedCustomizedProductInstance();

            Assert.False(collection.hasCustomizedProduct(customizedProduct));
        }

        [Fact]
        public void ensureHasCustomizedProductReturnsTrueIfCustomizedProductWasAdded()
        {
            CustomizedProductCollection collection = new CustomizedProductCollection("Collection");

            CustomizedProduct customizedProduct = buildFinishedCustomizedProductInstance();

            collection.addCustomizedProduct(customizedProduct);

            Assert.True(collection.hasCustomizedProduct(customizedProduct));
        }

        private ProductCategory buildValidCategory()
        {
            return new ProductCategory("Closets");
        }

        private Finish buildGlossyFinish()
        {
            return Finish.valueOf("Glossy", 90);
        }

        private Finish buildMatteFinish()
        {
            return Finish.valueOf("Matte", 2);
        }

        private Color buildRedColor()
        {
            return Color.valueOf("Deep Red", 255, 0, 0, 0);
        }

        private Color buildGreenColor()
        {
            return Color.valueOf("Totally Green", 0, 255, 0, 0);
        }

        private Material buildValidMaterial()
        {

            Finish glossy = buildGlossyFinish();
            Finish matte = buildMatteFinish();

            Color red = buildRedColor();
            Color green = buildGreenColor();


            return new Material("#123", "MDF", "ola.jpg", new List<Color>() { red, green }, new List<Finish>() { glossy, matte });
        }

        private Product buildValidProduct()
        {
            Dimension firstHeightDimension = new ContinuousDimensionInterval(50, 100, 2);
            Dimension firstWidthDimension = new DiscreteDimensionInterval(new List<double>() { 75, 80, 85, 90, 95, 120 });
            Dimension firstDepthDimension = new SingleValueDimension(25);

            Measurement firstMeasurement = new Measurement(firstHeightDimension, firstWidthDimension, firstDepthDimension);

            Dimension sideDimension = new SingleValueDimension(60);
            Measurement secondMeasurement = new Measurement(sideDimension, sideDimension, sideDimension);

            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(25, 50, 35);

            return new Product("#429", "Fabulous Closet", "fabcloset.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { firstMeasurement, secondMeasurement }, slotWidths);
        }

        private CustomizedDimensions buildCustomizedDimensions()
        {
            return CustomizedDimensions.valueOf(76, 80, 25);
        }

        private CustomizedMaterial buildCustomizedMaterial()
        {
            Material material = buildValidMaterial();
            Finish selectedFinish = buildMatteFinish();
            Color selectedColor = buildRedColor();
            return CustomizedMaterial.valueOf(material, selectedColor, selectedFinish);
        }

        private CustomizedProduct buildFinishedCustomizedProductInstance()
        {
            string serialNumber = "123";

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            CustomizedDimensions selectedDimensions = buildCustomizedDimensions();

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createAnonymousUserCustomizedProduct(serialNumber, buildValidProduct(), selectedDimensions).build();

            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            customizedProduct.finalizeCustomization();

            return customizedProduct;
        }

        private CustomizedProduct buildUnfinishedCustomizedProductInstance()
        {
            string serialNumber = "123";

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            CustomizedDimensions selectedDimensions = buildCustomizedDimensions();

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createAnonymousUserCustomizedProduct(serialNumber, buildValidProduct(), selectedDimensions).build();

            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            return customizedProduct;
        }
    }
}