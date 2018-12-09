using System;
using Xunit;
using System.Collections;
using core.domain;
using System.Collections.Generic;
using core.dto;
using System.Linq;
using static core.domain.CustomizedProduct;

namespace core_tests.domain
{
    /// <summary>
    /// Unit testing class for Slot
    /// </summary>
    public class SlotTest
    {
        private ProductCategory buildCategory()
        {
            ProductCategory category = new ProductCategory("All Products");
            return category;
        }

        private Material buildMaterial()
        {
            Finish matte = Finish.valueOf("Matte", 30);
            Finish glossy = Finish.valueOf("Glossy", 60);

            Color red = Color.valueOf("Red", 255, 0, 0, 0);
            Color blue = Color.valueOf("Blue", 0, 0, 255, 0);

            List<Color> colors = new List<Color>() { red, blue };
            List<Finish> finishes = new List<Finish>() { matte, glossy };

            Material material = new Material("123", "This Is A Material", "123.jpg", colors, finishes);

            return material;
        }

        private Product buildProduct()
        {
            List<Material> materials = new List<Material>() { buildMaterial() };

            Dimension height = new ContinuousDimensionInterval(40, 300, 1);
            Dimension width = new ContinuousDimensionInterval(40, 300, 1);
            Dimension depth = new ContinuousDimensionInterval(40, 300, 1);
            Measurement measurement = new Measurement(height, width, depth);

            List<Measurement> measurements = new List<Measurement>() { measurement };

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(20, 50, 30);

            Product product = new Product("001", "This Is a Product", "model.obj", buildCategory(), materials, measurements, productSlotWidths);

            return product;
        }

        private CustomizedProduct buildCustomizedProduct(string serialNumber, CustomizedDimensions customizedDimensions)
        {
            Finish matte = Finish.valueOf("Matte", 30);
            Color red = Color.valueOf("Red", 255, 0, 0, 0);

            CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(buildMaterial(), red, matte);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createAnonymousUserCustomizedProduct(serialNumber, buildProduct(), customizedDimensions)
                .withMaterial(customizedMaterial).build();

            return customizedProduct;
        }

        [Fact]
        public void ensureInstanceIsNotCreatedIfIdentifierIsNull()
        {
            Action act = () => new Slot(null, CustomizedDimensions.valueOf(20, 30, 10));

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsNotCreatedIfIdentifierIsEmpty()
        {
            Action act = () => new Slot("", CustomizedDimensions.valueOf(20, 30, 10));

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsNotCreatedIfCustomizedDimensionsAreNull()
        {
            Action act = () => new Slot("identfier 1", null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureInstanceIsCreated()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(10, 20, 30));

            Assert.NotNull(instance);
        }

        [Fact]
        public void ensureAddingNullCustomizedProductThrowsException()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 20, 300));

            Action addNullCustomizedProductAction = () => instance.addCustomizedProduct(null);

            Assert.Throws<ArgumentException>(addNullCustomizedProductAction);
        }

        [Fact]
        public void ensureAddingNullCustomizedProductDoesNotAddCustomizedProduct()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 20, 300));

            try
            {
                instance.addCustomizedProduct(null);
            }
            catch (Exception) { }

            Assert.False(instance.hasCustomizedProducts());
        }


        [Fact]
        public void ensureAddCustomizedProductDoesNotThrowExceptionIfProductFitsAndSlotIsEmpty()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            //dimensions that match the values available to the Product
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(80, 180, 50);

            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            //Add customized product to slot
            Action addValidCustomizedProductAction = () => instance.addCustomizedProduct(customizedProduct);

            Exception exception = Record.Exception(addValidCustomizedProductAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddCustomizedProductSucceedsIfProductFitsAndSlotIsEmpty()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            //dimensions that match the values available to the Product
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(80, 180, 50);

            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            //Add customized product to slot
            instance.addCustomizedProduct(customizedProduct);

            Assert.NotEmpty(instance.customizedProducts);
            Assert.True(instance.hasCustomizedProduct(customizedProduct));
        }

        [Fact]
        public void ensureAddingCustomizedProductBiggerThanSlotThrowsException()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(50, 50, 50));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(80, 180, 50);

            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            Action addBiggerThanSlotCustomizedProduct = () => instance.addCustomizedProduct(customizedProduct);

            Assert.Throws<ArgumentException>(addBiggerThanSlotCustomizedProduct);
        }

        [Fact]
        public void ensureAddingCustomizedProductBiggerThanSlotDoesNotAddCustomizedProduct()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(50, 50, 50));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(80, 180, 50);

            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            //Add customized product to slot
            try
            {
                instance.addCustomizedProduct(customizedProduct);
            }
            catch (Exception) { }

            Assert.Empty(instance.customizedProducts);
        }

        [Fact]
        public void ensureAddCustomizedProductThrowsExceptionIfProductDoesNotFit()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 70));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(80, 180, 50);
            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            instance.addCustomizedProduct(customizedProduct);

            CustomizedDimensions otherCustomizedDimensions = CustomizedDimensions.valueOf(80, 200, 70);
            CustomizedProduct otherCustomizedProduct = buildCustomizedProduct("1235", otherCustomizedDimensions);


            Action addCustomizedProductAction = () => instance.addCustomizedProduct(otherCustomizedProduct);

            Assert.Throws<ArgumentException>(addCustomizedProductAction);
        }

        [Fact]
        public void ensureAddCustomizedProductDoesNotAddCustomizedProductIfProductDoesNotFit()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 70));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(80, 180, 50);
            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            instance.addCustomizedProduct(customizedProduct);

            CustomizedDimensions otherCustomizedDimensions = CustomizedDimensions.valueOf(80, 200, 70);
            CustomizedProduct otherCustomizedProduct = buildCustomizedProduct("1235", otherCustomizedDimensions);

            try
            {
                instance.addCustomizedProduct(otherCustomizedProduct);
            }
            catch (Exception) { }

            Assert.False(instance.hasCustomizedProduct(otherCustomizedProduct));
        }

        [Fact]
        public void ensureAddingDuplicateCustomizedProductThrowsException()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 70));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(80, 180, 50);
            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            instance.addCustomizedProduct(customizedProduct);

            CustomizedDimensions otherCustomizedDimensions = CustomizedDimensions.valueOf(40, 50, 40);
            CustomizedProduct otherCustomizedProduct = buildCustomizedProduct("1234", otherCustomizedDimensions);

            Action addDuplicateCustomizedProductAction = () => instance.addCustomizedProduct(otherCustomizedProduct);

            Assert.Throws<ArgumentException>(addDuplicateCustomizedProductAction);
        }

        [Fact]
        public void ensureAddingDuplicateCustomizedDoesNotAddCustomizedProduct()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 70));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(80, 180, 50);
            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            instance.addCustomizedProduct(customizedProduct);

            CustomizedDimensions otherCustomizedDimensions = CustomizedDimensions.valueOf(40, 50, 40);
            CustomizedProduct otherCustomizedProduct = buildCustomizedProduct("1234", otherCustomizedDimensions);

            try
            {
                instance.addCustomizedProduct(otherCustomizedProduct);
            }
            catch (Exception) { }

            Assert.Single(instance.customizedProducts);
        }

        [Fact]
        public void ensureAddingCustomizedProductThatFitsInSlotDoesNotThrowException()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(50, 200, 300);
            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            instance.addCustomizedProduct(customizedProduct);

            CustomizedDimensions otherCustomizedDimensions = CustomizedDimensions.valueOf(40, 200, 300);
            CustomizedProduct otherCustomizedProduct = buildCustomizedProduct("1235", otherCustomizedDimensions);

            Action addCustomizedProductAction = () => instance.addCustomizedProduct(otherCustomizedProduct);

            Exception exception = Record.Exception(addCustomizedProductAction);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddingCustomizedProductThatFitsInSlotAddsCustomizedProduct()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(50, 200, 300);
            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            instance.addCustomizedProduct(customizedProduct);

            CustomizedDimensions otherCustomizedDimensions = CustomizedDimensions.valueOf(40, 200, 300);
            CustomizedProduct otherCustomizedProduct = buildCustomizedProduct("1235", otherCustomizedDimensions);

            instance.addCustomizedProduct(otherCustomizedProduct);

            //Add 2nd customized product to slot
            Assert.True(instance.hasCustomizedProduct(otherCustomizedProduct));
            Assert.True(instance.customizedProducts.Count == 2);
        }

        [Fact]
        public void ensureRemovingNullCustomizedProductThrowsException()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            Action removeNullCustomProductAction = () => instance.removeCustomizedProduct(null);

            Assert.Throws<ArgumentException>(removeNullCustomProductAction);
        }

        [Fact]
        public void ensureRemovingNonExistingCustomizedProductThrowsException()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(50, 200, 300);
            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            Action removeNonExistingCustomProductAction = () => instance.removeCustomizedProduct(customizedProduct);

            Assert.Throws<ArgumentException>(removeNonExistingCustomProductAction);
        }

        [Fact]
        public void ensureRemovingAddedCustomizedProductDoesNotThrowException()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(50, 200, 300);
            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            instance.addCustomizedProduct(customizedProduct);

            Action removeAddedCustomizedProductAction = () => instance.removeCustomizedProduct(customizedProduct);

            Exception exception = Record.Exception(removeAddedCustomizedProductAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureRemovingAddedCustomizedProductRemovesCustomizedProduct()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(50, 200, 300);
            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            instance.addCustomizedProduct(customizedProduct);
            instance.removeCustomizedProduct(customizedProduct);

            Assert.False(instance.hasCustomizedProducts());
        }

        [Fact]
        public void ensureChangingToNullDimensionsThrowsException()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 20, 300));

            Action changeNullDimensionsAction = () => instance.changeDimensions(null);

            Assert.Throws<ArgumentException>(changeNullDimensionsAction);
        }

        [Fact]
        public void ensureChangingToNullDimensionsDoesNotChangeDimensions()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 20, 300));

            try
            {
                instance.changeDimensions(null);
            }
            catch (Exception) { }

            Assert.NotNull(instance.slotDimensions);
        }

        [Fact]
        public void ensureChangingDimensionsWhileSlotHasProductsThrowsException()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(50, 200, 300);
            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            instance.addCustomizedProduct(customizedProduct);

            CustomizedDimensions otherCustomizedDimensions = CustomizedDimensions.valueOf(80, 150, 250);

            Action changeDimensionsAction = () => instance.changeDimensions(otherCustomizedDimensions);

            Assert.Throws<InvalidOperationException>(changeDimensionsAction);
        }

        [Fact]
        public void ensureChangingDimensionsWhileSlotHasProductsDoesNotChangeDimensions()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(50, 200, 300);
            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            instance.addCustomizedProduct(customizedProduct);

            CustomizedDimensions otherCustomizedDimensions = CustomizedDimensions.valueOf(80, 150, 250);
            try
            {
                instance.changeDimensions(otherCustomizedDimensions);
            }
            catch (Exception) { }

            Assert.NotEqual(instance.slotDimensions, otherCustomizedDimensions);
        }

        [Fact]
        public void ensureChangingDimensionsAfterRemovingAllCustomizedProductsDoesNotThrowException()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(50, 200, 300);
            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            instance.addCustomizedProduct(customizedProduct);
            instance.removeCustomizedProduct(customizedProduct);

            CustomizedDimensions otherCustomizedDimensions = CustomizedDimensions.valueOf(80, 150, 250);

            Action changeDimensionsAction = () => instance.changeDimensions(otherCustomizedDimensions);

            Exception exception = Record.Exception(changeDimensionsAction);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureChangingDimensionsAfterRemovingAllCustomizedProductsChangesDimensions()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(50, 200, 300);
            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", customizedDimensions);

            instance.addCustomizedProduct(customizedProduct);
            instance.removeCustomizedProduct(customizedProduct);

            CustomizedDimensions otherCustomizedDimensions = CustomizedDimensions.valueOf(80, 150, 250);
            instance.changeDimensions(otherCustomizedDimensions);

            Assert.Equal(instance.slotDimensions, otherCustomizedDimensions);
        }

        [Fact]
        public void ensureIdReturnsSlotIdentifier()
        {
            string slotIdentifier = "identifier 0";

            Slot instance = new Slot(slotIdentifier, CustomizedDimensions.valueOf(100, 200, 300));

            Assert.Equal(slotIdentifier, instance.id());
        }

        [Fact]
        public void ensureSameAsSlotIdentifierReturnsTrue()
        {
            string slotIdentifier = "identifier 0";

            Slot instance = new Slot(slotIdentifier, CustomizedDimensions.valueOf(100, 200, 300));

            Assert.True(instance.sameAs(slotIdentifier));
        }

        [Fact]
        public void ensureSameAsOtherStringReturnsFalse()
        {
            string slotIdentifier = "identifier 0";

            Slot instance = new Slot(slotIdentifier, CustomizedDimensions.valueOf(100, 200, 300));

            string otherString = "this is not the slot's identifier";

            Assert.False(instance.sameAs(otherString));
        }

        [Fact]
        public void ensureInstanceIsEqualToItself()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            Assert.True(instance.Equals(instance));
        }

        [Fact]
        public void ensureInstanceIsNotEqualToNull()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            Assert.False(instance.Equals(null));
        }

        [Fact]
        public void ensureInstanceIsNotEqualToObjectOfOtherType()
        {
            string slotIdentifier = "identifier 0";

            Slot instance = new Slot(slotIdentifier, CustomizedDimensions.valueOf(100, 200, 300));

            Assert.False(instance.Equals(slotIdentifier));
        }

        [Fact]
        public void ensureInstancesWithSameIdentifierAreEqual()
        {
            string slotIdentifier = "identifier 0";

            Slot instance = new Slot(slotIdentifier, CustomizedDimensions.valueOf(100, 200, 300));

            Slot other = new Slot(slotIdentifier, CustomizedDimensions.valueOf(150, 75, 125));

            Assert.True(instance.Equals(other));
        }

        [Fact]
        public void ensureHashCodeIsDifferentIfIdentifierIsDifferent()
        {
            Slot instance = new Slot("identifier 0", CustomizedDimensions.valueOf(100, 200, 300));

            Slot other = new Slot("identifier 1", CustomizedDimensions.valueOf(150, 75, 125));

            Assert.NotEqual(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureHashCodeIsEqualIfIdentifierIsEqual()
        {
            string slotIdentifier = "identifier 0";

            Slot instance = new Slot(slotIdentifier, CustomizedDimensions.valueOf(100, 200, 300));

            Slot other = new Slot(slotIdentifier, CustomizedDimensions.valueOf(150, 75, 125));

            Assert.Equal(instance.GetHashCode(), other.GetHashCode());
        }

        [Fact]
        public void ensureToStringWorks()
        {
            string slotIdentifier = "identifier";
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(100, 200, 300);

            Slot instance = new Slot(slotIdentifier, customizedDimensions);
            Slot other = new Slot(slotIdentifier, customizedDimensions);

            CustomizedProduct customizedProduct = buildCustomizedProduct("1234", CustomizedDimensions.valueOf(50, 200, 300));

            instance.addCustomizedProduct(customizedProduct);
            other.addCustomizedProduct(customizedProduct);

            Assert.Equal(instance.ToString(), other.ToString());
        }

        // [Fact]
        // public void ensureToDTOWorks()
        // {
        //     Slot instance = new Slot(CustomizedDimensions.valueOf(1, 1, 1));
        //     var category = new ProductCategory("Drawers");
        //     //Creating Dimensions
        //     List<Double> valuesList = new List<Double> { 100, 200, 300 };
        //     DiscreteDimensionInterval discreteDimensionInterval = new DiscreteDimensionInterval(valuesList);
        //     Measurement measurement = new Measurement(discreteDimensionInterval, discreteDimensionInterval, discreteDimensionInterval);
        //     List<Measurement> measurements = new List<Measurement>() { measurement };
        //     //Creating a material
        //     string reference = "1160912";
        //     string designation = "FR E SH A VOCA DO";
        //     List<Color> colors = new List<Color>();
        //     Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
        //     Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
        //     colors.Add(color);
        //     colors.Add(color1);
        //     List<Finish> finishes = new List<Finish>();
        //     Finish finish = Finish.valueOf("Acabamento matte", 0);
        //     Finish finish2 = Finish.valueOf("Acabamento polido", 100);
        //     finishes.Add(finish);
        //     finishes.Add(finish2);
        //     Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
        //     List<Material> materials = new List<Material>();
        //     materials.Add(material);
        //     IEnumerable<Material> matsList = materials;
        //     Product product = new Product("#666", "Shelf", "shelf666.glb", category, matsList, measurements);
        //     CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(100.0, 200.0, 300.0);
        //     //Customized Material
        //     CustomizedMaterial customizedMaterial = CustomizedMaterial.valueOf(material, color1, finish2);
        //     CustomizedProduct customizedProduct = new CustomizedProduct("#666", "Shelf", customizedMaterial, customizedDimensions, product);

        //     instance.addCustomizedProduct(customizedProduct);

        //     SlotDTO instanceDTO = instance.toDTO();

        //     Assert.Equal(instance.slotDimensions.Id, instanceDTO.customizedDimensions.Id);
        //     Assert.Equal(instance.slotDimensions.depth, instanceDTO.customizedDimensions.depth);
        //     Assert.Equal(instance.slotDimensions.width, instanceDTO.customizedDimensions.width);
        //     Assert.Equal(instance.slotDimensions.height, instanceDTO.customizedDimensions.height);
        // }
    }
}