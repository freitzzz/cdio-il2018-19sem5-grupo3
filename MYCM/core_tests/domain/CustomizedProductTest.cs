using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.dto;
using support.dto;
using Xunit;
using static core.domain.CustomizedProduct;

namespace core_tests.domain {
    /// <summary>
    /// Unit testing class for CustomizedProduct
    /// </summary>
    public class CustomizedProductTest {
        //These are all seperated into their own methods in order to allow for each property to be tested

        private ProductCategory buildValidCategory() {
            return new ProductCategory("Closets");
        }

        private Finish buildGlossyFinish() {
            return Finish.valueOf("Glossy", 90);
        }

        private Finish buildMatteFinish() {
            return Finish.valueOf("Matte", 2);
        }

        private Color buildRedColor() {
            return Color.valueOf("Deep Red", 255, 0, 0, 0);
        }

        private Color buildGreenColor() {
            return Color.valueOf("Totally Green", 0, 255, 0, 0);
        }

        private Material buildValidMaterial() {

            Finish glossy = buildGlossyFinish();
            Finish matte = buildMatteFinish();

            Color red = buildRedColor();
            Color green = buildGreenColor();


            return new Material("#123", "MDF", "ola.jpg", new List<Color>() { red, green }, new List<Finish>() { glossy, matte });
        }

        private Product buildValidProduct() {
            Dimension firstHeightDimension = new ContinuousDimensionInterval(50, 100, 2);
            Dimension firstWidthDimension = new DiscreteDimensionInterval(new List<double>() { 75, 80, 85, 90, 95, 120 });
            Dimension firstDepthDimension = new SingleValueDimension(25);

            Measurement firstMeasurement = new Measurement(firstHeightDimension, firstWidthDimension, firstDepthDimension);

            Dimension sideDimension = new SingleValueDimension(60);
            Measurement secondMeasurement = new Measurement(sideDimension, sideDimension, sideDimension);

            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(25, 50, 35);

            return new Product("#429", "Fabulous Closet", "fabcloset.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { firstMeasurement, secondMeasurement }, slotWidths);
        }

        private CustomizedDimensions buildCustomizedDimensions() {
            return CustomizedDimensions.valueOf(76, 80, 25);
        }

        private CustomizedMaterial buildCustomizedMaterial() {
            Material material = buildValidMaterial();
            Finish selectedFinish = buildMatteFinish();
            Color selectedColor = buildRedColor();
            return CustomizedMaterial.valueOf(material, selectedColor, selectedFinish);
        }
        private CustomizedProduct buildValidInstance(string reference) {
            CustomizedDimensions selectedDimensions = buildCustomizedDimensions();

            return CustomizedProductBuilder.createCustomizedProduct(reference, buildValidProduct(), selectedDimensions).build();
        }
        private CustomizedProduct buildValidFinishedInstance(string reference) {
            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            CustomizedDimensions selectedDimensions = buildCustomizedDimensions();

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct(reference, buildValidProduct(), selectedDimensions).build();

            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            customizedProduct.finalizeCustomization();

            return customizedProduct;
        }

        private CustomizedProduct buildValidInstanceWithSubCustomizedProducts() {
            Dimension heightDimension = new ContinuousDimensionInterval(60, 80, 2);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 140, 60);

            Dimension componentHeightDimension = new SingleValueDimension(60);
            Dimension componentWidthDimension = new SingleValueDimension(200);
            Dimension componentDepthDimension = new SingleValueDimension(60);

            Measurement componentMeasurement = new Measurement(componentHeightDimension, componentWidthDimension, componentDepthDimension);

            Dimension otherComponentHeightDimension = new SingleValueDimension(50);
            Dimension otherComponentWidthDimension = new SingleValueDimension(190);
            Dimension otherComponentDepthDimension = new SingleValueDimension(50);

            Measurement otherComponentMeasurement = new Measurement(otherComponentHeightDimension, otherComponentWidthDimension, otherComponentDepthDimension);

            Product otherComponent = new Product("This is another another reference", "This is another Designation", "component.gltf", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { componentMeasurement });

            Product component = new Product("This is another reference", "This is another Designation", "component.gltf", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { componentMeasurement }, complementaryProducts: new List<Product> { otherComponent });

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, complementaryProducts: new List<Product> { component }, slotWidths: productSlotWidths);

            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();
            customizedProduct.changeCustomizedMaterial(buildCustomizedMaterial());

            CustomizedProduct customizedComponent = CustomizedProductBuilder
                .createCustomizedProduct(component, customizedProductDimensions, customizedProduct, customizedProduct.slots[0]).build();
            customizedComponent.changeCustomizedMaterial(buildCustomizedMaterial());

            CustomizedProduct otherCustomizedComponent = CustomizedProductBuilder
                .createCustomizedProduct(otherComponent, customizedProductDimensions, customizedComponent, customizedComponent.slots[0]).build();
            otherCustomizedComponent.changeCustomizedMaterial(buildCustomizedMaterial());

            return customizedProduct;
        }

        private CustomizedProduct buildValidFinishedInstanceWithSubCustomizedProducts(string reference) {
            Dimension heightDimension = new ContinuousDimensionInterval(60, 80, 2);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 140, 60);

            Dimension componentHeightDimension = new SingleValueDimension(60);
            Dimension componentWidthDimension = new SingleValueDimension(200);
            Dimension componentDepthDimension = new SingleValueDimension(60);

            Measurement componentMeasurement = new Measurement(componentHeightDimension, componentWidthDimension, componentDepthDimension);

            Product component = new Product("This is another reference", "This is another Designation", "component.gltf", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { componentMeasurement });

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, complementaryProducts: new List<Product> { component }, slotWidths: productSlotWidths);

            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();

            CustomizedProduct customizedComponent = CustomizedProductBuilder.createCustomizedProduct(component, customizedProductDimensions, customizedProduct, customizedProduct.slots[0]).build();

            customizedProduct.changeCustomizedMaterial(buildCustomizedMaterial());
            customizedComponent.changeCustomizedMaterial(buildCustomizedMaterial());

            customizedProduct.finalizeCustomization();

            return customizedProduct;
        }

        private CustomizedProduct buildValidInstanceWithSlotsAndSubCustomizedProducts() {
            Dimension heightDimension = new ContinuousDimensionInterval(60, 80, 2);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 140, 60);

            Dimension componentHeightDimension = new SingleValueDimension(60);
            Dimension componentWidthDimension = new SingleValueDimension(60);
            Dimension componentDepthDimension = new SingleValueDimension(60);

            Measurement componentMeasurement = new Measurement(componentHeightDimension, componentWidthDimension, componentDepthDimension);

            Product component = new Product("This is another reference", "This is another Designation", "component.gltf", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { componentMeasurement });

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, complementaryProducts: new List<Product> { component }, slotWidths: productSlotWidths);

            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();

            customizedProduct.changeCustomizedMaterial(buildCustomizedMaterial());
            customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));

            CustomizedDimensions customizedComponentDimensions = CustomizedDimensions.valueOf(60, 60, 60);
            CustomizedProduct customizedComponent = CustomizedProductBuilder
                .createCustomizedProduct(component, customizedComponentDimensions, customizedProduct, customizedProduct.slots[1]).build();
            customizedComponent.changeCustomizedMaterial(buildCustomizedMaterial());

            return customizedProduct;
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateCustomizedProductIfReferenceIsNull() {
            Action buildAction = () => CustomizedProductBuilder.createCustomizedProduct(null, buildValidProduct(), buildCustomizedDimensions());
            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateCustomizedProductIfReferenceIsEmpty() {
            Action buildAction = () => CustomizedProductBuilder.createCustomizedProduct("", buildValidProduct(), buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateCustomizedProductIfProductIsNull() {
            Action buildAction = () => CustomizedProductBuilder.createCustomizedProduct("1", null, buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateCustomizedProductIfCustomizedDimensionsDimensionsIsNull() {
            Action buildAction = () => CustomizedProductBuilder.createCustomizedProduct("1", buildValidProduct(), null);

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateCustomizedProductIfAuthTokenIsNull() {
            Action buildAction = () => CustomizedProductBuilder.createCustomizedProduct("123", null, buildValidProduct(), buildCustomizedDimensions());
            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateCustomizedProductIfAuthTokenIsEmpty() {
            Action buildAction = () => CustomizedProductBuilder.createCustomizedProduct("123", "   ", buildValidProduct(), buildCustomizedDimensions());
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantBuildCustomizedProductWithDesignationIfDesignationIsNull() {
            CustomizedProductBuilder builder = CustomizedProductBuilder.createCustomizedProduct("1234", buildValidProduct(), buildCustomizedDimensions());

            Action buildWithNullDesignation = () => builder.withDesignation(null);

            Assert.Throws<ArgumentException>(buildWithNullDesignation);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantBuildCustomizedProductWithDesignationIfDesignationIsEmpty() {
            CustomizedProductBuilder builder = CustomizedProductBuilder.createCustomizedProduct("1234", buildValidProduct(), buildCustomizedDimensions());

            Action buildWithNullDesignation = () => builder.withDesignation("");

            Assert.Throws<ArgumentException>(buildWithNullDesignation);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantBuildCustomizedProductWithCustomizedMaterialIfCustomizedMaterialIsNull() {
            CustomizedProductBuilder builder = CustomizedProductBuilder.createCustomizedProduct("1234", buildValidProduct(), buildCustomizedDimensions());

            Action buildWithNullMaterial = () => builder.withMaterial(null);

            Assert.Throws<ArgumentException>(buildWithNullMaterial);
        }

        [Fact]
        public void ensureSlotMatchingCustomizedProductDimensionsIsCreatedWhenCustomizedProductIsBuilt() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.slots.First().slotDimensions, customizedProduct.customizedDimensions);
        }

        [Fact]
        public void ensureChangingReferenceIfCustomizationIsFinishedThrowsException() {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("auth token", "this is a reference",
                buildValidProduct(), buildCustomizedDimensions()).build();

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            //the CustomizedProduct needs a CustomizedMaterial prior to finishing customization
            customizedProduct.finalizeCustomization();

            Action changeReference = () => customizedProduct.changeReference("this is another reference");

            Assert.Throws<InvalidOperationException>(changeReference);
        }

        [Fact]
        public void ensureChangingReferenceIfCustomizationIsFinishedDoesNotChangeReference() {
            string reference = "this is a reference";

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("auth token", reference,
                buildValidProduct(), buildCustomizedDimensions()).build();

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            //the CustomizedProduct needs a CustomizedMaterial prior to finishing customization
            customizedProduct.finalizeCustomization();

            try {
                customizedProduct.changeReference("this is another reference");
            } catch (Exception) { }

            Assert.Equal(reference, customizedProduct.reference);
        }

        [Fact]
        public void ensureChangingReferenceToNullReferenceThrowsException() {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("auth token", "this is a reference",
                buildValidProduct(), buildCustomizedDimensions()).build();

            Action changeReference = () => customizedProduct.changeReference(null);

            Assert.Throws<ArgumentException>(changeReference);
        }

        [Fact]
        public void ensureChangingReferenceToNullReferenceDoesNotChangeReference() {
            string reference = "this is a reference";

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("auth token",
                reference, buildValidProduct(), buildCustomizedDimensions()).build();

            try {
                customizedProduct.changeReference(null);
            } catch (Exception) { }

            Assert.Equal(reference, customizedProduct.reference);
        }

        [Fact]
        public void ensureChangingReferenceToEmptyReferenceThrowsException() {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("auth token",
            "this is a reference", buildValidProduct(), buildCustomizedDimensions()).build();

            Action changeReference = () => customizedProduct.changeReference("");

            Assert.Throws<ArgumentException>(changeReference);
        }

        [Fact]
        public void ensureChangingReferenceToEmptyReferenceDoesNotChangeReference() {
            string reference = "this is a reference";

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("auth token",
            reference, buildValidProduct(), buildCustomizedDimensions()).build();

            try {
                customizedProduct.changeReference("");
            } catch (Exception) { }

            Assert.Equal(reference, customizedProduct.reference);
        }

        [Fact]
        public void ensureChangingSubCustomizedProductReferenceThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();

            CustomizedProduct subCustomizedProduct = customizedProduct.slots[0].customizedProducts[0];

            Action changeReference = () => subCustomizedProduct.changeReference("New reference");

            Assert.Throws<InvalidOperationException>(changeReference);
        }

        [Fact]
        public void ensureChangingSubCustomizedProductReferenceDoesNotChangeReference() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();

            string expectedReference = customizedProduct.reference + "-CP1";

            CustomizedProduct subCustomizedProduct = customizedProduct.slots[0].customizedProducts[0];

            try {
                subCustomizedProduct.changeReference("a new reference");
            } catch (Exception) { }

            Assert.Equal(expectedReference, subCustomizedProduct.reference);
        }

        [Fact]
        public void ensureChangingReferenceToValidReferenceDoesNotThrowException() {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("auth token", "this is a reference",
                buildValidProduct(), buildCustomizedDimensions()).build();


            Action changeReference = () => customizedProduct.changeReference("this is another reference");

            Exception exception = Record.Exception(changeReference);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureChangingReferenceToValidReferenceChangesValue() {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("auth token", "this is a reference",
                buildValidProduct(), buildCustomizedDimensions()).build();

            string newReference = "this is another reference";

            customizedProduct.changeReference(newReference);

            Assert.Equal(newReference, customizedProduct.reference);
        }

        [Fact]
        public void ensureChangingReferenceChangesSubCustomizedProductsReference() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();

            //Building a component similar to one alredy added in order to check if reference is generated correctly
            CustomizedProduct childCustomizedProduct = customizedProduct.slots[0].customizedProducts[0];
            CustomizedProduct grandChildCustomizedProduct = childCustomizedProduct.slots[0].customizedProducts[0];

            customizedProduct.changeReference("new reference");

            string expectedChildReference = "new reference-CP1";
            string expectedGrandChildReference = "new reference-CP1-CP1";

            Assert.Equal(expectedChildReference, childCustomizedProduct.reference);
            Assert.Equal(expectedGrandChildReference, grandChildCustomizedProduct.reference);
        }

        [Fact]
        public void ensureChangingReferenceChangesSlotIdentifiers() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSlotsAndSubCustomizedProducts();

            customizedProduct.changeReference("new reference");

            string expectedSlotIdentifier = "new reference-S1";
            string expectedOtherSlotIdentifier = "new reference-S2";
            string expectedChildSlotIdentifier = "new reference-CP1-S1";

            Slot slot = customizedProduct.slots[0];
            Slot otherSlot = customizedProduct.slots[1];
            Slot childSlot = otherSlot.customizedProducts[0].slots[0];


            Assert.Equal(expectedSlotIdentifier, slot.identifier);
            Assert.Equal(expectedOtherSlotIdentifier, otherSlot.identifier);
            Assert.Equal(expectedChildSlotIdentifier, childSlot.identifier);
        }

        [Fact]
        public void ensureChangingDesignationIfCustomizationIsFinishedThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            //the CustomizedProduct needs a CustomizedMaterial prior to finishing customization
            customizedProduct.finalizeCustomization();

            Action changeDesignation = () => customizedProduct.changeDesignation("this is a designation");

            Assert.Throws<InvalidOperationException>(changeDesignation);
        }

        [Fact]
        public void ensureChangingDesignationIfCustomizationIsFinishedDoesNotChangeDesignation() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            //the CustomizedProduct needs a CustomizedMaterial prior to finishing customization
            customizedProduct.finalizeCustomization();

            try {
                customizedProduct.changeDesignation("this is a designation");
            } catch (Exception) { }

            Assert.Null(customizedProduct.designation);
        }

        [Fact]
        public void ensureChangingDesignationToNullDesignationThrowsException() {
            string designation = "this is a designation";

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("auth token", "this is a reference",
                buildValidProduct(), buildCustomizedDimensions()).withDesignation(designation).build();

            Action changeDesignation = () => customizedProduct.changeDesignation(null);

            Assert.Throws<ArgumentException>(changeDesignation);
        }

        [Fact]
        public void ensureChangingDesignationToNullDesignationDoesNotChangeDesignation() {
            string designation = "this is a designation";

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("auth token", "this is a reference",
                buildValidProduct(), buildCustomizedDimensions()).withDesignation(designation).build();

            try {
                customizedProduct.changeDesignation(designation);
            } catch (Exception) { }

            Assert.Equal(designation, customizedProduct.designation);
        }

        [Fact]
        public void ensureChangingDesignationToEmptyDesignationThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Action changeDesignation = () => customizedProduct.changeDesignation("");

            Assert.Throws<ArgumentException>(changeDesignation);
        }

        [Fact]
        public void ensureChangingDesignationToEmptyDesignationDoesNotChangeDesignation() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            try {
                customizedProduct.changeDesignation("");
            } catch (Exception) { }

            Assert.Null(customizedProduct.designation);
        }

        [Fact]
        public void ensureChangingSubCustomizedProductDesignationThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();

            CustomizedProduct subCustomizedProduct = customizedProduct.slots[0].customizedProducts[0];

            Action changeDesignation = () => subCustomizedProduct.changeDesignation("my magnificent creation");

            Assert.Throws<InvalidOperationException>(changeDesignation);
        }

        [Fact]
        public void ensureChangingSubCustomizedProductDesignationDoesNotChangeDesignation() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();

            CustomizedProduct subCustomizedProduct = customizedProduct.slots[0].customizedProducts[0];

            string expectedDesignation = customizedProduct.designation;

            try {
                subCustomizedProduct.changeDesignation("new designation");
            } catch (Exception) { }

            //sub customized products inherit their designation from the parent
            Assert.Equal(expectedDesignation, subCustomizedProduct.designation);
        }


        [Fact]
        public void ensureChangingDesignationToValidDesignationDoesNotThrowException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Action changeDesignation = () => customizedProduct.changeDesignation("this is a designation");

            Exception exception = Record.Exception(changeDesignation);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureChangingDesignationToValidDesignationChangesDesignation() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            string designation = "this is a designation";

            customizedProduct.changeDesignation(designation);

            Assert.Equal(designation, customizedProduct.designation);
        }


        [Fact]
        public void ensureChangingDesignationChangesSubCustomizedProductDesignation() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();

            string designation = "new designation";

            customizedProduct.changeDesignation(designation);

            CustomizedProduct childProduct = customizedProduct.slots[0].customizedProducts[0];
            CustomizedProduct grandChildProduct = childProduct.slots[0].customizedProducts[0];

            Assert.Equal(designation, customizedProduct.designation);
            Assert.Equal(designation, childProduct.designation);
            Assert.Equal(designation, grandChildProduct.designation);
        }


        [Fact]
        public void ensureChangingMaterialIfCustomizationIsFinishedThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            //the CustomizedProduct needs a CustomizedMaterial prior to finishing customization
            customizedProduct.finalizeCustomization();

            //a valid, but different customized material
            Material material = buildValidMaterial();
            Finish selectedFinish = buildGlossyFinish();
            Color selectedColor = buildGreenColor();
            CustomizedMaterial otherCustomizedMaterial = CustomizedMaterial.valueOf(material, selectedColor, selectedFinish);

            Action changeCustomizedMaterial = () => customizedProduct.changeCustomizedMaterial(otherCustomizedMaterial);

            Assert.Throws<InvalidOperationException>(changeCustomizedMaterial);
        }

        [Fact]
        public void ensureChangingMaterialIfCustomizationIsFinishedDoesNotChangeMaterial() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            //the CustomizedProduct needs a CustomizedMaterial prior to finishing customization
            customizedProduct.finalizeCustomization();


            //a valid, but different customized material
            Material material = buildValidMaterial();
            Finish selectedFinish = buildGlossyFinish();
            Color selectedColor = buildGreenColor();

            CustomizedMaterial otherCustomizedMaterial = CustomizedMaterial.valueOf(material, selectedColor, selectedFinish);

            try {
                customizedProduct.changeCustomizedMaterial(customizedMaterial);
            } catch (Exception) { }

            Assert.Equal(customizedMaterial, customizedProduct.customizedMaterial);
            Assert.NotEqual(otherCustomizedMaterial, customizedProduct.customizedMaterial);
        }

        [Fact]
        public void ensureChangingMaterialToNullCustomizedMaterialThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Action changeMaterial = () => customizedProduct.changeCustomizedMaterial(null);

            Assert.Throws<ArgumentException>(changeMaterial);
        }

        [Fact]
        public void ensureChangingMaterialToNullCustomizedMaterialDoesNotChangeMaterial() {
            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("user auth token", "reference",
               buildValidProduct(), buildCustomizedDimensions()).withMaterial(customizedMaterial).build();

            try {
                customizedProduct.changeCustomizedMaterial(null);
            } catch (Exception) { }

            Assert.Equal(customizedMaterial, customizedProduct.customizedMaterial);
        }

        [Fact]
        public void ensureChangingFinishIfCustomizationIsFinishedThrowsException() {
            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("user auth token", "reference",
               buildValidProduct(), buildCustomizedDimensions()).withMaterial(customizedMaterial).build();

            customizedProduct.finalizeCustomization();

            Action changeFinish = () => customizedProduct.changeFinish(buildGlossyFinish());

            Assert.Throws<InvalidOperationException>(changeFinish);
        }

        [Fact]
        public void ensureChangingFinishIfCustomizationIsFinishedDoesNotChangeFinish() {
            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("user auth token", "reference",
               buildValidProduct(), buildCustomizedDimensions()).withMaterial(customizedMaterial).build();

            customizedProduct.finalizeCustomization();

            try {
                customizedProduct.changeFinish(buildGlossyFinish());
            } catch (Exception) { }

            Assert.Equal(buildMatteFinish(), customizedProduct.customizedMaterial.finish);
        }

        [Fact]
        public void ensureChangingFinishIfCustomizedMaterialIsNullThrowsException() {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("user auth token", "reference",
               buildValidProduct(), buildCustomizedDimensions()).build();

            Action changeFinish = () => customizedProduct.changeFinish(buildGlossyFinish());

            Assert.Throws<InvalidOperationException>(changeFinish);
        }

        [Fact]
        public void ensureChangingFinishIfCustomizedMaterialIsNullDoesNotChangeFinish() {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("user auth token", "reference",
                buildValidProduct(), buildCustomizedDimensions()).build();

            try {
                customizedProduct.changeFinish(buildGlossyFinish());
            } catch (Exception) { }

            Assert.Null(customizedProduct.customizedMaterial);
        }

        [Fact]
        public void ensureChangingFinishIfCustomizedMaterialIsDefinedDoesNotThrowException() {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("user auth token", "reference",
                buildValidProduct(), buildCustomizedDimensions()).withMaterial(buildCustomizedMaterial()).build();

            Action changeFinish = () => customizedProduct.changeFinish(buildGlossyFinish());

            Exception exception = Record.Exception(changeFinish);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureChangingFinishIfCustomizedMaterialIsDefinedChangesFinish() {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("user auth token", "reference",
                 buildValidProduct(), buildCustomizedDimensions()).withMaterial(buildCustomizedMaterial()).build();

            Finish finish = buildGlossyFinish();

            customizedProduct.changeFinish(finish);

            Assert.Equal(finish, customizedProduct.customizedMaterial.finish);
        }

        [Fact]
        public void ensureChangingColorIfCustomizationIsFinishedThrowsException() {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("user auth token", "reference",
                buildValidProduct(), buildCustomizedDimensions()).withMaterial(buildCustomizedMaterial()).build();

            customizedProduct.finalizeCustomization();

            Action changeColor = () => customizedProduct.changeColor(buildGreenColor());

            Assert.Throws<InvalidOperationException>(changeColor);
        }

        [Fact]
        public void ensureChangingColorIfCustomizationIsFinishedDoesNotChangeFinish() {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("user auth token", "reference",
                buildValidProduct(), buildCustomizedDimensions()).withMaterial(buildCustomizedMaterial()).build();

            customizedProduct.finalizeCustomization();

            try {
                customizedProduct.changeColor(buildGreenColor());
            } catch (Exception) { }

            Assert.Equal(buildRedColor(), customizedProduct.customizedMaterial.color);
        }

        [Fact]
        public void ensureChangingColorIfCustomizedMaterialIsNotDefinedThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Color green = buildGreenColor();

            Action changeColor = () => customizedProduct.changeColor(green);

            Assert.Throws<InvalidOperationException>(changeColor);
        }

        [Fact]
        public void ensureChangingColorIfCustomizedMaterialIsNotDefinedDoesNotChangeColor() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Color green = buildGreenColor();

            try {
                customizedProduct.changeColor(green);
            } catch (Exception) { }

            Assert.Null(customizedProduct.customizedMaterial);
        }

        [Fact]
        public void ensureAddingSlotAfterCustomizationIsFinishedThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            //the CustomizedProduct needs a CustomizedMaterial prior to finishing customization
            customizedProduct.finalizeCustomization();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(76, 35, 35);

            Action addSlot = () => customizedProduct.addSlot(slotDimensions);

            Assert.Throws<InvalidOperationException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotAfterCustomizationIsFinishedDoesNotAddSlot() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            //the CustomizedProduct needs a CustomizedMaterial prior to finishing customization
            customizedProduct.finalizeCustomization();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(76, 35, 35);

            try {
                customizedProduct.addSlot(slotDimensions);
            } catch (Exception) { }

            //Make sure that there's only one slot
            //And that that slot is the one matching the customized product's dimensions
            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.customizedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotIfProductDoesNotSupportSlotsThrowsException() {
            Dimension firstHeightDimension = new ContinuousDimensionInterval(50, 100, 2);
            Dimension firstWidthDimension = new DiscreteDimensionInterval(new List<double>() { 75, 80, 85, 90, 95, 120 });
            Dimension firstDepthDimension = new SingleValueDimension(25);

            Measurement firstMeasurement = new Measurement(firstHeightDimension, firstWidthDimension, firstDepthDimension);

            Dimension sideDimension = new SingleValueDimension(60);
            Measurement secondMeasurement = new Measurement(sideDimension, sideDimension, sideDimension);

            Product productWithoutSlotSupport = new Product("#429", "Fabulous Closet", "fabcloset.glb",
                buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { firstMeasurement, secondMeasurement });

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", productWithoutSlotSupport, buildCustomizedDimensions()).build();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(76, 35, 35);

            Action addSlot = () => customizedProduct.addSlot(slotDimensions);

            Assert.Throws<InvalidOperationException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotIfProductDoesNotSupportSlotsDoesNotAddSlot() {
            Dimension firstHeightDimension = new ContinuousDimensionInterval(50, 100, 2);
            Dimension firstWidthDimension = new DiscreteDimensionInterval(new List<double>() { 75, 80, 85, 90, 95, 120 });
            Dimension firstDepthDimension = new SingleValueDimension(25);

            Measurement firstMeasurement = new Measurement(firstHeightDimension, firstWidthDimension, firstDepthDimension);

            Dimension sideDimension = new SingleValueDimension(60);
            Measurement secondMeasurement = new Measurement(sideDimension, sideDimension, sideDimension);

            Product productWithoutSlotSupport = new Product("#429", "Fabulous Closet", "fabcloset.glb",
                buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { firstMeasurement, secondMeasurement });

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", productWithoutSlotSupport, buildCustomizedDimensions()).build();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(76, 35, 35);

            try {
                customizedProduct.addSlot(slotDimensions);
            } catch (Exception) { }

            //Make sure that there's only one slot
            //And that that slot is the one matching the customized product's dimensions
            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.customizedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotWithNullDimensionsThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Action addSlotWithNullDimensions = () => customizedProduct.addSlot(null);

            Assert.Throws<ArgumentException>(addSlotWithNullDimensions);
        }

        [Fact]
        public void ensureAddingSlotWithNullDimensionsDoesNotAddSlot() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            try {
                customizedProduct.addSlot(null);
            } catch (Exception) { }

            //Make sure that there's only one slot
            //And that that slot is the one matching the customized product's dimensions
            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.customizedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotWithDimensionsNotFollowingSpecificationThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            //the width is smaller than the minimum of 25
            CustomizedDimensions invalidDimensions = CustomizedDimensions.valueOf(76, 23, 25);

            Action addSlot = () => customizedProduct.addSlot(invalidDimensions);

            Assert.Throws<ArgumentException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotWithDimensionsNotFollowingSpecificationDoesNotAddSlot() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            //the width is smaller than the minimum of 25
            CustomizedDimensions invalidDimensions = CustomizedDimensions.valueOf(76, 23, 25);

            try {
                customizedProduct.addSlot(invalidDimensions);
            } catch (Exception) { }

            //Make sure that there's only one slot
            //And that that slot is the one matching the customized product's dimensions
            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.customizedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotHigherThanCustomizedProductThrowsException() {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", buildValidProduct(), customizedProductDimensions).build();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(70, 40, 60);

            Action addSlot = () => customizedProduct.addSlot(slotDimensions);

            Assert.Throws<ArgumentException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotHigherThanCustomizedProductDoesNotAddCustomizedProduct() {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", buildValidProduct(), customizedProductDimensions).build();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(70, 40, 60);

            try {
                customizedProduct.addSlot(slotDimensions);
            } catch (Exception) { }

            //Make sure that there's only one slot
            //And that that slot is the one matching the customized product's dimensions
            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.customizedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotWiderThanCustomizedProductThrowsException() {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", buildValidProduct(), customizedProductDimensions).build();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(60, 70, 60);

            Action addSlot = () => customizedProduct.addSlot(slotDimensions);

            Assert.Throws<ArgumentException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotWiderThanCustomizedProductDoesNotAddSlot() {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", buildValidProduct(), customizedProductDimensions).build();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(60, 70, 60);

            try {
                customizedProduct.addSlot(slotDimensions);
            } catch (Exception) { }

            //Make sure that there's only one slot
            //And that that slot is the one matching the customized product's dimensions
            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.customizedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotDeeperThanCustomizedProductThrowsException() {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", buildValidProduct(), customizedProductDimensions).build();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(60, 60, 70);

            Action addSlot = () => customizedProduct.addSlot(slotDimensions);

            Assert.Throws<ArgumentException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotDeeperThanCustomizedProductDoesNotAddSlot() {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", buildValidProduct(), customizedProductDimensions).build();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(60, 60, 70);

            try {
                customizedProduct.addSlot(slotDimensions);
            } catch (Exception) { }

            //Make sure that there's only one slot
            //And that that slot is the one matching the customized product's dimensions
            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.customizedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotWhenCustomizedProductsHaveBeenAddedThrowsException() {
            Dimension heightDimension = new SingleValueDimension(60);
            Dimension widthDimension = new ContinuousDimensionInterval(70, 200, 1);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 100, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);

            Dimension childWidthDimension = new SingleValueDimension(200);
            Dimension childDimension = new SingleValueDimension(30);

            Measurement childMeasurement = new Measurement(childDimension, childWidthDimension, childDimension);

            Product childProduct = new Product("This is a Child", "child product", "child.dae", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { childMeasurement });

            product.addComplementaryProduct(childProduct);

            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();

            CustomizedDimensions childCustomizedDimensions = CustomizedDimensions.valueOf(30, 200, 30);

            CustomizedProduct childCustomizedProduct = CustomizedProductBuilder
                .createCustomizedProduct(childProduct, childCustomizedDimensions, customizedProduct, customizedProduct.slots.First())
                .build();

            CustomizedDimensions otherSlotDimensions = CustomizedDimensions.valueOf(60, 100, 60);

            Action addSlot = () => customizedProduct.addSlot(otherSlotDimensions);

            Assert.Throws<InvalidOperationException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotThatInvalidatesMainSlotThrowsException() {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", buildValidProduct(), customizedProductDimensions).build();

            //The product's specifications allows for the slot widths to vary between 25 and 50
            //Adding a slot with a width of 40 will cause the other slot to not follow specification, generating an error

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(60, 40, 60);

            Action addSlot = () => customizedProduct.addSlot(slotDimensions);

            Assert.Throws<ArgumentException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotThatInvalidatesMainSlotDoesNotAddSlot() {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", buildValidProduct(), customizedProductDimensions).build();

            //The product's specifications allows for the slot widths to vary between 25 and 50
            //Adding a slot with a width of 40 will cause the other slot to not follow specification, generating an error

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(60, 40, 60);

            try {
                customizedProduct.addSlot(slotDimensions);
            } catch (Exception) { }

            //Make sure that there's only one slot
            //And that that slot is the one matching the customized product's dimensions
            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.customizedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotThatDoesNotInvalidateMainSlotDoesNotThrowException() {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", buildValidProduct(), customizedProductDimensions).build();

            //The product's specifications allows for the slot widths to vary between 25 and 50
            //Adding a slot width of 35 will make the other slot have a width of 25, respecting specification

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(60, 35, 60);

            Action addSlot = () => customizedProduct.addSlot(slotDimensions);

            Exception exception = Record.Exception(addSlot);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddingSlotThatDoesNotInvalidateMainSlotAddsSlot() {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", buildValidProduct(), customizedProductDimensions).build();

            //The product's specifications allows for the slot widths to vary between 25 and 50
            //Adding a slot width of 35 will make the other slot have a width of 25, respecting specification

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(60, 35, 60);

            customizedProduct.addSlot(slotDimensions);

            Assert.Equal(2, customizedProduct.slots.Count);
        }

        [Fact]
        public void ensureAddingSlotThatDoesNotInvalidateMainSlotResizesMainSlot() {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", buildValidProduct(), customizedProductDimensions).build();

            //The product's specifications allows for the slot widths to vary between 25 and 50
            //Adding a slot width of 35 will make the other slot have a width of 25, respecting specification

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(60, 35, 60);

            customizedProduct.addSlot(slotDimensions);

            CustomizedDimensions expectedDimensions = CustomizedDimensions.valueOf(60, 25, 60);

            Assert.Equal(expectedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotThatMakesTheMainSlotNotFollowTheSlotSpecificationsThrowsException() {
            Dimension heightDimension = new SingleValueDimension(60);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 100, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);


            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();

            //Adding a slot with a width of 60 makes the main slot have a width of 140, which exceeds the maximum
            Action addSlot = () => customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));

            Assert.Throws<ArgumentException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotThatMakesTheMainSlotNotFollowTheSlotSpecificationsDoesNotAddSlot() {
            Dimension heightDimension = new SingleValueDimension(60);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 100, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);


            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();

            //Adding a slot with a width of 60 makes the main slot have a width of 140, which exceeds the maximum
            try {
                customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));
            } catch (Exception) { }

            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProductDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotThatHasValidWidthWithPreviouslyAddedSlotsDoesNotThrowException() {
            Dimension heightDimension = new SingleValueDimension(60);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 140, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);


            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();

            customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));
            customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 50, 60));

            //adding both of these slots will create a slot with a width of 90

            Action addSlot = () => customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));

            Exception exception = Record.Exception(addSlot);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddingSlotThatHasValidWidthWithPreviouslyAddedSlotsAddsSlot() {
            Dimension heightDimension = new SingleValueDimension(60);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 140, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);


            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();

            customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));
            customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 50, 60));
            customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));

            CustomizedDimensions firstSlotDimension = CustomizedDimensions.valueOf(60, 50, 60);
            CustomizedDimensions secondSlotDimension = CustomizedDimensions.valueOf(60, 40, 60);
            CustomizedDimensions thirdSlotDimension = CustomizedDimensions.valueOf(60, 50, 60);
            CustomizedDimensions fourthSlotDimension = CustomizedDimensions.valueOf(60, 60, 60);

            List<CustomizedDimensions> expectedSlotDimensions = new List<CustomizedDimensions>() { firstSlotDimension, secondSlotDimension, thirdSlotDimension, fourthSlotDimension };

            Assert.Equal(expectedSlotDimensions, customizedProduct.slots.Select(s => s.slotDimensions).ToList());
        }

        [Fact]
        public void ensureAddingRecommendedNumberOfSlotsDoesNotThrowException() {
            Dimension heightDimension = new SingleValueDimension(60);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 100, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);


            double customizedProductWidth = 200;

            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, customizedProductWidth, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();

            CustomizedDimensions slotDimensions =
                CustomizedDimensions.valueOf(60, productSlotWidths.recommendedWidth, 60);


            Action addSlots = () => {
                customizedProduct.addSlot(CustomizedDimensions.valueOf(60, productSlotWidths.maxWidth, 60));
                customizedProduct.addSlot(slotDimensions);
                customizedProduct.addSlot(slotDimensions);
            };

            Exception exception = Record.Exception(addSlots);

            Assert.Null(exception);
        }


        [Fact]
        public void ensureResizingSlotAfterCustomizationIsFinishedThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(76, 40, 25);

            customizedProduct.addSlot(slotDimensions);

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            customizedProduct.finalizeCustomization();

            Slot slot = customizedProduct.slots.LastOrDefault();

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 50, 25);

            Action resizeSlot = () => customizedProduct.resizeSlot(slot, newSlotDimensions);

            Assert.Throws<InvalidOperationException>(resizeSlot);
        }

        [Fact]
        public void ensureResizingSlotAfterCustomizationIsFinishedDoesNotResizeSlot() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(76, 40, 25);

            customizedProduct.addSlot(slotDimensions);

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            customizedProduct.finalizeCustomization();

            Slot slot = customizedProduct.slots.LastOrDefault();

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 50, 25);

            try {
                customizedProduct.resizeSlot(slot, newSlotDimensions);
            } catch (Exception) { }

            Assert.Equal(2, customizedProduct.slots.Count);
            Assert.Equal(slotDimensions, customizedProduct.slots.LastOrDefault().slotDimensions);
        }

        [Fact]
        public void ensureResizingNullSlotThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(76, 40, 25);

            customizedProduct.addSlot(slotDimensions);

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 50, 25);

            Action resizeSlot = () => customizedProduct.resizeSlot(null, newSlotDimensions);

            Assert.Throws<ArgumentException>(resizeSlot);
        }

        [Fact]
        public void ensureResizingNullSlotDoesNotResizeOtherSlots() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(76, 40, 25);

            customizedProduct.addSlot(slotDimensions);

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 50, 25);

            try {
                customizedProduct.resizeSlot(null, newSlotDimensions);
            } catch (Exception) { }

            Assert.Equal(slotDimensions, customizedProduct.slots.FirstOrDefault().slotDimensions);
            Assert.Equal(slotDimensions, customizedProduct.slots.LastOrDefault().slotDimensions);
        }

        [Fact]
        public void ensureResizingOnlyExistingSlotThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Slot slot = customizedProduct.slots.SingleOrDefault();

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 50, 25);

            Action resizeSlot = () => customizedProduct.resizeSlot(slot, newSlotDimensions);

            Assert.Throws<InvalidOperationException>(resizeSlot);
        }

        [Fact]
        public void ensureResizingOnlyExistingSlotDoesNotResizeSlot() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Slot slot = customizedProduct.slots.SingleOrDefault();

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 50, 25);

            try {
                customizedProduct.resizeSlot(slot, newSlotDimensions);
            } catch (Exception) { }

            CustomizedDimensions expectedDimensions = buildCustomizedDimensions();

            Assert.Equal(expectedDimensions, slot.slotDimensions);
        }

        [Fact]
        public void ensureResizingSlotWithWidthLessThanMinimumThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 40, 25));

            Slot slot = customizedProduct.slots.LastOrDefault();

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 20, 25);

            Action resizeSlot = () => customizedProduct.resizeSlot(slot, newSlotDimensions);

            Assert.Throws<ArgumentException>(resizeSlot);
        }

        [Fact]
        public void ensureResizingSlotWithWidthLessThanMinimumDoesNotResizeSlot() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(76, 40, 25);

            customizedProduct.addSlot(slotDimensions);

            Slot slot = customizedProduct.slots.LastOrDefault();

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 20, 25);

            try {
                customizedProduct.resizeSlot(slot, newSlotDimensions);
            } catch (Exception) { }

            Assert.Equal(slotDimensions, slot.slotDimensions);
        }

        [Fact]
        public void ensureResizingSlotWithWidthGreaterThanMaximumThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 40, 25));

            Slot slot = customizedProduct.slots.LastOrDefault();

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 60, 25);

            Action resizeSlot = () => customizedProduct.resizeSlot(slot, newSlotDimensions);

            Assert.Throws<ArgumentException>(resizeSlot);
        }

        [Fact]
        public void ensureResizingSlotWithWidthGreaterThanMaximumDoesNotResizeSlot() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(76, 40, 25);

            customizedProduct.addSlot(slotDimensions);

            Slot slot = customizedProduct.slots.LastOrDefault();

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 60, 25);

            try {
                customizedProduct.resizeSlot(slot, newSlotDimensions);
            } catch (Exception) { }

            Assert.Equal(slotDimensions, slot.slotDimensions);
        }

        [Fact]
        public void ensureResizingNotAddedSlotThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 40, 25));

            Slot slot = new Slot("not added slot", CustomizedDimensions.valueOf(76, 25, 25));

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 30, 25);

            Action resizeSlot = () => customizedProduct.resizeSlot(slot, newSlotDimensions);

            Assert.Throws<ArgumentException>(resizeSlot);
        }

        [Fact]
        public void ensureResizingNotAddedSlotDoesNotResizeOtherSlots() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(76, 40, 25);

            customizedProduct.addSlot(slotDimensions);

            Slot slot = new Slot("not added slot", CustomizedDimensions.valueOf(76, 25, 25));

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 30, 25);

            try {
                customizedProduct.resizeSlot(slot, newSlotDimensions);
            } catch (Exception) { }

            Assert.Equal(slotDimensions, customizedProduct.slots.FirstOrDefault().slotDimensions);
            Assert.Equal(slotDimensions, customizedProduct.slots.LastOrDefault().slotDimensions);
        }

        [Fact]
        public void ensureResizingValidSlotDoesNotThrowException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 30, 25));

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 30, 25));

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 25, 25);

            Action resizeSlot = () => customizedProduct.resizeSlot(customizedProduct.slots.LastOrDefault(), newSlotDimensions);

            Exception exception = Record.Exception(resizeSlot);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureResizingValidSlotResizesSlot() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 30, 25));

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 30, 25));

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 25, 25);

            customizedProduct.resizeSlot(customizedProduct.slots.LastOrDefault(), newSlotDimensions);

            List<CustomizedDimensions> expectedSlotDimensions = new List<CustomizedDimensions>(){

                CustomizedDimensions.valueOf(76,30,25),
                CustomizedDimensions.valueOf(76,25,25),
                CustomizedDimensions.valueOf(76,25,25)
            };

            Assert.Equal(expectedSlotDimensions, customizedProduct.slots.Select(s => s.slotDimensions));
        }


        [Fact]
        public void ensureResizingSlotAffectsOtherSlots() {
            Dimension heightDimension = new ContinuousDimensionInterval(50, 100, 2);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 75, 80, 85, 90, 95, 120 });
            Dimension depthDimension = new SingleValueDimension(25);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(25, 60, 35);

            Product product = new Product("#429", "Fabulous Closet", "fabcloset.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, slotWidths);

            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(76, 120, 25);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("1234", product, customizedProductDimensions).build();

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 60, 25));    // <-60-> | <-60->
            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 28, 25));    // <-46-> | <-46-> | <-28->
            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 28, 25));    // <-27,(3)-> | <-36,(6)-> | <-28-> | <-28->

            Slot slotBeingResized = customizedProduct.slots[1];

            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 43.6, 25);

            customizedProduct.resizeSlot(slotBeingResized, newSlotDimensions);

            List<CustomizedDimensions> expectedDimensions = new List<CustomizedDimensions>(){
                CustomizedDimensions.valueOf(76, 26.4, 25),
                CustomizedDimensions.valueOf(76, 43.6, 25),
                CustomizedDimensions.valueOf(76, 25, 25),
                CustomizedDimensions.valueOf(76, 25, 25)
            };

            for (int i = 0; i < 4; i++) {
                Assert.Equal(expectedDimensions[i].width, customizedProduct.slots[i].slotDimensions.width, 1);
            }
        }


        [Fact]
        public void ensureIncreasingSlotThrowsExceptionIfUnableToDecreaseOtherSlots() {

            Dimension heightDimension = new ContinuousDimensionInterval(50, 100, 2);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 75, 80, 85, 90, 95, 120 });
            Dimension depthDimension = new SingleValueDimension(25);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(25, 60, 35);

            Product product = new Product("#429", "Fabulous Closet", "fabcloset.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, slotWidths);

            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(76, 120, 25);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("1234", product, customizedProductDimensions).build();

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 60, 25));    // <-60-> | <-60->
            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 60, 25));    // <-30-> | <-30-> | <-60->
            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 45, 25));    // <-25-> | <-25-> | <-25-> | <-45->

            Slot slotBeingResized = customizedProduct.slots.LastOrDefault();

            //Even though 60 is within specification, it will be impossible, because the other slots are already at their minimum
            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 60, 25);

            Action resizeSlot = () => customizedProduct.resizeSlot(slotBeingResized, newSlotDimensions);

            Assert.Throws<ArgumentException>(resizeSlot);
        }

        [Fact]
        public void ensureDecreasingSlotThrowsExceptionIfUnableToIncreaseOtherSlots() {
            Dimension heightDimension = new ContinuousDimensionInterval(50, 100, 2);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 75, 80, 85, 90, 95, 120 });
            Dimension depthDimension = new SingleValueDimension(25);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths slotWidths = ProductSlotWidths.valueOf(25, 60, 35);

            Product product = new Product("#429", "Fabulous Closet", "fabcloset.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, slotWidths);

            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(76, 120, 25);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("1234", product, customizedProductDimensions).build();

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 60, 25));    // <-60-> | <-60->

            Slot slotBeingResized = customizedProduct.slots.LastOrDefault();

            //Resizing the slot to 45, would cause the other slot to be resized to 75 which is out of range
            CustomizedDimensions newSlotDimensions = CustomizedDimensions.valueOf(76, 45, 25);

            Action resizeSlot = () => customizedProduct.resizeSlot(slotBeingResized, newSlotDimensions);

            Assert.Throws<ArgumentException>(resizeSlot);
        }


        [Fact]
        public void ensureRemovingSlotWhenCustomizationIsFinishedThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 50, 25)); //<-30-> | <-50->

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            customizedProduct.finalizeCustomization();

            Action removeSlot = () => customizedProduct.removeSlot(customizedProduct.slots.LastOrDefault());

            Assert.Throws<InvalidOperationException>(removeSlot);
        }

        [Fact]
        public void ensureRemovingSlotWhenCustomizationIsFinishedDoesNotRemoveSlot() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 50, 25)); //<-30-> | <-50->

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            customizedProduct.finalizeCustomization();

            try {
                customizedProduct.removeSlot(customizedProduct.slots.LastOrDefault());
            } catch (Exception) { }

            Assert.Equal(2, customizedProduct.slots.Count);
        }

        [Fact]
        public void ensureRemovingNullSlotThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 50, 25)); //<-30-> | <-50->

            Action removeSlot = () => customizedProduct.removeSlot(null);

            Assert.Throws<ArgumentException>(removeSlot);
        }

        [Fact]
        public void ensureRemovingNullSlotDoesNotRemoveOtherSlots() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 50, 25)); //<-30-> | <-50->

            try {
                customizedProduct.removeSlot(null);
            } catch (Exception) { }

            Assert.Equal(2, customizedProduct.slots.Count);
        }

        [Fact]
        public void ensureRemovingNotAddedSlotThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 50, 25)); //<-30-> | <-50->

            Slot unknownSlot = new Slot("I'm not in the customized product", CustomizedDimensions.valueOf(76, 30, 25));

            Action removeSlot = () => customizedProduct.removeSlot(unknownSlot);

            Assert.Throws<ArgumentException>(removeSlot);
        }

        [Fact]
        public void ensureRemovingNotAddedSlotDoesNotRemoveOtherSlots() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 50, 25)); //<-30-> | <-50->

            Slot unknownSlot = new Slot("I'm not in the customized product", CustomizedDimensions.valueOf(76, 30, 25));

            try {
                customizedProduct.removeSlot(unknownSlot);
            } catch (Exception) { }

            Assert.Equal(2, customizedProduct.slots.Count);
        }

        [Fact]
        public void ensureRemovingSingleSlotThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Action removeSlot = () => customizedProduct.removeSlot(customizedProduct.slots.SingleOrDefault());

            Assert.Throws<InvalidOperationException>(removeSlot);
        }

        [Fact]
        public void ensureRemovingSingleSlotDoesNotRemoveSlot() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Slot slot = customizedProduct.slots.SingleOrDefault();

            try {
                customizedProduct.removeSlot(slot);
            } catch (Exception) { }

            Assert.Single(customizedProduct.slots);
        }

        [Fact]
        public void ensureRemovingSlotIfCustomizedProductHasSubCustomizedProductsThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSlotsAndSubCustomizedProducts();

            Action removeSlot = () => customizedProduct.removeSlot(customizedProduct.slots.LastOrDefault());

            Assert.Throws<InvalidOperationException>(removeSlot);
        }


        [Fact]
        public void ensureRemovingSlotIfCustomizedProductHasSubCustomizedProductsDoesNotRemoveSlot() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSlotsAndSubCustomizedProducts();

            try {
                customizedProduct.removeSlot(customizedProduct.slots.LastOrDefault());
            } catch (Exception) { }

            Assert.Equal(2, customizedProduct.slots.Count);
        }


        [Fact]
        public void ensureRemovingPenultimateSlotResizesRemainingSlotToMatchCustomizedProduct() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 50, 25)); //<-30-> | <-50->

            customizedProduct.removeSlot(customizedProduct.slots.LastOrDefault());

            Slot mainSlot = customizedProduct.slots.SingleOrDefault();

            Assert.Equal(mainSlot.slotDimensions, customizedProduct.customizedDimensions);
        }

        [Fact]
        public void ensureRemovingValidSlotDoesNotThrowException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 50, 25)); //<-30-> | <-50->

            Action removeSlot = () => customizedProduct.removeSlot(customizedProduct.slots.LastOrDefault());

            Exception exception = Record.Exception(removeSlot);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureRemovingValidSlotRemovesSlot() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 50, 25)); //<-30-> | <-50->

            customizedProduct.removeSlot(customizedProduct.slots.LastOrDefault());

            Assert.Single(customizedProduct.slots);
        }

        [Fact]
        public void ensureRemovingSlotResizesOtherSlots() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 50, 25)); //<-30-> | <-50->
            customizedProduct.addSlot(CustomizedDimensions.valueOf(76, 30, 25)); //<-25-> | <-25-> | <-30->

            Slot middleSlot = customizedProduct.slots[1];

            customizedProduct.removeSlot(middleSlot);

            //Expected layout: <-30-> | <-50->

            List<CustomizedDimensions> expectedDimensions = new List<CustomizedDimensions>(){
                CustomizedDimensions.valueOf(76,30,25),
                CustomizedDimensions.valueOf(76,50,25)
            };

            Assert.Equal(expectedDimensions, customizedProduct.slots.Select(s => s.slotDimensions));
        }

        [Fact]
        public void ensureChangingDimensionsAfterCustomizationIsFinishedThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            //specify a material before finalizing customization
            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            customizedProduct.finalizeCustomization();

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(81, 90, 25);

            Action changeDimensions = () => customizedProduct.changeDimensions(customizedDimensions);

            Assert.Throws<InvalidOperationException>(changeDimensions);
        }

        [Fact]
        public void ensureChangingDimensionsAfterCustomizationIsFinishedDoesNotChangeDimensions() {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            //specify a material before finalizing customization
            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            customizedProduct.finalizeCustomization();

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(81, 90, 25);

            try {
                customizedProduct.changeDimensions(customizedDimensions);
            } catch (Exception) { }

            Assert.NotEqual(customizedDimensions, customizedProduct.customizedDimensions);
            Assert.Equal(buildCustomizedDimensions(), customizedProduct.customizedDimensions);
        }

        [Fact]
        public void ensureChangingDimensionsIfSlotsHaveBeenAddedThrowsException() {
            Dimension heightDimension = new ContinuousDimensionInterval(60, 80, 2);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 140, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);


            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();

            customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));

            Action changeDimensions = () => customizedProduct.changeDimensions(CustomizedDimensions.valueOf(72, 200, 60));

            Assert.Throws<InvalidOperationException>(changeDimensions);
        }

        [Fact]
        public void ensureChangingDimensionsIfSlotsHaveBeenAddedDoesNotChangeDimensions() {
            Dimension heightDimension = new ContinuousDimensionInterval(60, 80, 2);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 140, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);


            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();

            customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));

            try {
                customizedProduct.changeDimensions(CustomizedDimensions.valueOf(72, 200, 60));
            } catch (Exception) { }

            Assert.Equal(customizedProductDimensions, customizedProduct.customizedDimensions);
        }

        [Fact]
        public void ensureIdMatchesReference() {
            string reference = "this is a reference";

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("Auth Token", reference, buildValidProduct(), buildCustomizedDimensions()).build();

            Assert.Equal(reference, customizedProduct.id());
        }

        [Fact]
        public void ensureCustomizedProductSameAsItsReference() {
            string reference = "this is a reference";

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("Auth Token", reference, buildValidProduct(), buildCustomizedDimensions()).build();

            Assert.True(customizedProduct.sameAs(reference));
        }

        [Fact]
        public void ensureActivatingAnActivatedCustomizedProductReturnsFalse() {
            string reference = "reference";

            CustomizedProduct customizedProduct = buildValidInstance(reference);

            customizedProduct.activate();

            Assert.False(customizedProduct.activate());
        }

        [Fact]
        public void ensureActivatingAnActivatedCustomizedProductDoesntActivateItAndItsChildren() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();

            customizedProduct.activate();

            Assert.False(customizedProduct.activate());
            Assert.True(customizedProduct.activated);
            foreach (CustomizedProduct child in customizedProduct.slots[0].customizedProducts) {
                Assert.True(child.activated);
            }
        }

        [Fact]
        public void ensureActivatingADeactivatedCustomizedProductReturnsTrue() {
            string reference = "reference";

            CustomizedProduct customizedProduct = buildValidInstance(reference);
            customizedProduct.deactivate();

            Assert.True(customizedProduct.activate());
            Assert.True(customizedProduct.activated);
        }

        [Fact]
        public void ensureActivatingADeactivatedCustomizedProductActivatesItAndItsChildren() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();
            customizedProduct.deactivate();
            Assert.True(customizedProduct.activate());
            Assert.True(customizedProduct.activated);
            foreach (CustomizedProduct child in customizedProduct.slots[0].customizedProducts) {
                Assert.True(child.activated);
            }
        }

        [Fact]
        public void ensureDeactivatingADeactivatedCustomizedProductReturnsFalse() {
            string reference = "reference";

            CustomizedProduct customizedProduct = buildValidInstance(reference);
            customizedProduct.deactivate();
            Assert.False(customizedProduct.deactivate());
            Assert.False(customizedProduct.activated);
        }

        [Fact]
        public void ensureDeactivatingAnActivatedCustomizedProductReturnsTrue() {
            string reference = "reference";

            CustomizedProduct customizedProduct = buildValidInstance(reference);

            customizedProduct.activate();

            Assert.True(customizedProduct.deactivate());
            Assert.False(customizedProduct.activated);
        }

        [Fact]
        public void ensureDeactivatingAnActivatedCustomizedProductDeactivatesItAndItsChildren() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();

            customizedProduct.activate();

            Assert.True(customizedProduct.deactivate());
            Assert.False(customizedProduct.activated);
            foreach (CustomizedProduct child in customizedProduct.slots[0].customizedProducts) {
                Assert.False(child.activated);
            }
        }

        [Fact]
        public void ensureDeactivatingADeactivatedCustomizedProductDoesntDeactivateItAndItsChildren() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();
            customizedProduct.deactivate();

            Assert.False(customizedProduct.deactivate());
            Assert.False(customizedProduct.activated);
            foreach (CustomizedProduct child in customizedProduct.slots[0].customizedProducts) {
                Assert.False(child.activated);
            }
        }

        [Fact]
        public void ensureAddingCustomizedProductToFinishedCustomizedProductThrowsException() {
            string reference = "reference";

            CustomizedProduct customizedProduct = buildValidFinishedInstanceWithSubCustomizedProducts(reference);

            //Build a customized product similar to the one already added
            Slot slot = customizedProduct.slots[0];
            CustomizedProduct child = slot.customizedProducts[0];
            Product product = child.product;
            CustomizedDimensions dimensions = child.customizedDimensions;

            Action act = () => CustomizedProductBuilder.createCustomizedProduct(product, dimensions, customizedProduct, slot);

            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void ensureAddingCustomizedProductToNullSlotThrowsException() {
            string reference = "reference";

            CustomizedProduct customizedProduct = buildValidInstance(reference);

            Action act = () => CustomizedProductBuilder.createCustomizedProduct(customizedProduct.product, customizedProduct.customizedDimensions, customizedProduct, null).build();

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureAddingCustomizedProductToNonMatchingSlotThrowsException() {
            string reference = "reference";

            CustomizedProduct customizedProduct = buildValidInstance(reference);

            CustomizedDimensions customizedDimensions = buildCustomizedDimensions();
            Product product = buildValidProduct();

            Slot nonMatchingSlot = new Slot("hey i'm a slot identifier", CustomizedDimensions.valueOf(100, 100, 100));

            Action act = () => CustomizedProductBuilder.createCustomizedProduct(product, customizedDimensions, customizedProduct, nonMatchingSlot).build();

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureAddingCustomizedProductThatIsntAPossibleComponentThrowsException() {
            Dimension heightDimension = new ContinuousDimensionInterval(60, 80, 2);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 140, 60);

            Dimension componentHeightDimension = new SingleValueDimension(60);
            Dimension componentWidthDimension = new SingleValueDimension(200);
            Dimension componentDepthDimension = new SingleValueDimension(60);

            Measurement componentMeasurement = new Measurement(componentHeightDimension, componentWidthDimension, componentDepthDimension);

            Product component = new Product("This is another reference", "This is another Designation", "component.gltf", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { componentMeasurement });

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);


            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();

            customizedProduct.changeCustomizedMaterial(buildCustomizedMaterial());

            Action act = () => CustomizedProductBuilder.createCustomizedProduct(component, customizedProductDimensions, customizedProduct, customizedProduct.slots[0]).build();

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureAddingCustomizedProductWithNoCustomizedMaterialThrowsException() {
            Dimension heightDimension = new ContinuousDimensionInterval(60, 80, 2);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 140, 60);

            Dimension componentHeightDimension = new SingleValueDimension(60);
            Dimension componentWidthDimension = new SingleValueDimension(200);
            Dimension componentDepthDimension = new SingleValueDimension(60);

            Measurement componentMeasurement = new Measurement(componentHeightDimension, componentWidthDimension, componentDepthDimension);

            Product component = new Product("This is another reference", "This is another Designation", "component.gltf", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { componentMeasurement });

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, complementaryProducts: new List<Product> { component }, slotWidths: productSlotWidths);

            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();
            CustomizedProduct customizedComponent = CustomizedProductBuilder
               .createCustomizedProduct(component, customizedProductDimensions, customizedProduct, customizedProduct.slots[0]).build();


            customizedProduct.changeCustomizedMaterial(buildCustomizedMaterial());
            Action act = () => customizedProduct.addCustomizedProduct(customizedComponent, customizedProduct.slots[0]);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureAddingValidCustomizedProductAddsCustomizedProductToASlot() {
            Dimension heightDimension = new ContinuousDimensionInterval(60, 80, 2);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 140, 60);

            Dimension componentHeightDimension = new SingleValueDimension(60);
            Dimension componentWidthDimension = new SingleValueDimension(200);
            Dimension componentDepthDimension = new SingleValueDimension(60);

            Measurement componentMeasurement = new Measurement(componentHeightDimension, componentWidthDimension, componentDepthDimension);

            Product component = new Product("This is another reference", "This is another Designation", "component.gltf", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { componentMeasurement });

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, complementaryProducts: new List<Product> { component }, slotWidths: productSlotWidths);

            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();

            customizedProduct.changeCustomizedMaterial(buildCustomizedMaterial());

            CustomizedProduct customizedComponent = CustomizedProductBuilder
                .createCustomizedProduct(component, customizedProductDimensions, customizedProduct, customizedProduct.slots[0]).build();

            Assert.Equal(customizedProduct.slots[0].customizedProducts[0], customizedComponent);
        }

        [Fact]
        public void ensureRemovingCustomizedProductFromFinishedCustomizedProductThrowsException() {
            string reference = "reference";

            CustomizedProduct customizedProduct = buildValidFinishedInstanceWithSubCustomizedProducts(reference);

            Action act = () => customizedProduct.removeCustomizedProduct(customizedProduct.slots[0].customizedProducts[0], customizedProduct.slots[0]);

            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void ensureRemovingNullCustomizedProductThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();

            Action act = () => customizedProduct.removeCustomizedProduct(null, customizedProduct.slots[0]);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureRemovingCustomizedProductFromNullSlotThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();

            Action act = () => customizedProduct.removeCustomizedProduct(customizedProduct.slots[0].customizedProducts[0], null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureRemovingCustomizedProductFromSlotThatDoesntHaveItThrowsException() {
            Dimension heightDimension = new SingleValueDimension(30);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(30);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(10, 30, 20);

            Dimension componentHeightDimension = new SingleValueDimension(5);
            Dimension componentWidthDimension = new SingleValueDimension(10);
            Dimension componentDepthDimension = new SingleValueDimension(5);

            Measurement componentMeasurement = new Measurement(componentHeightDimension, componentWidthDimension, componentDepthDimension);

            Product component = new Product("This is another reference", "This is another Designation", "component.gltf", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { componentMeasurement });

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, complementaryProducts: new List<Product> { component }, slotWidths: productSlotWidths);

            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(30, 30, 30);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createCustomizedProduct("reference", product, customizedProductDimensions).build();

            customizedProduct.changeCustomizedMaterial(buildCustomizedMaterial());

            customizedProduct.addSlot(CustomizedDimensions.valueOf(10, 10, 10));
            customizedProduct.addSlot(CustomizedDimensions.valueOf(10, 10, 10));

            CustomizedProduct customizedComponent = CustomizedProductBuilder
                .createCustomizedProduct(component, CustomizedDimensions.valueOf(5, 10, 5), customizedProduct, customizedProduct.slots[0]).build();
            customizedComponent.changeCustomizedMaterial(buildCustomizedMaterial());
            Action act = () => customizedProduct.removeCustomizedProduct(customizedProduct.slots[0].customizedProducts[0], customizedProduct.slots[1]);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureRemovingCustomizedProductFromSlotRemovesIt() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();

            customizedProduct.removeCustomizedProduct(customizedProduct.slots[0].customizedProducts[0], customizedProduct.slots[0]);

            Assert.Empty(customizedProduct.slots[0].customizedProducts);
        }

        [Fact]
        public void ensureSubCustomizedProductsFinalizingTheCustomizationProcessThrowsException() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();

            Action act = () => customizedProduct.slots[0].customizedProducts[0].finalizeCustomization();

            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void ensureFinalizingCustomizationOfCustomizedProductWithoutCustomizedMaterialThrowsException() {
            string reference = "reference";

            CustomizedProduct customizedProduct = buildValidInstance(reference);

            Action act = () => customizedProduct.finalizeCustomization();

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureFinalizingCustomizationOfAValidCustomizedProductSetsStatusToFinished() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();
            customizedProduct.changeCustomizedMaterial(buildCustomizedMaterial());
            customizedProduct.slots[0].customizedProducts[0].changeCustomizedMaterial(buildCustomizedMaterial());
            customizedProduct.slots[0].customizedProducts[0].slots[0].customizedProducts[0].changeCustomizedMaterial(buildCustomizedMaterial());

            customizedProduct.finalizeCustomization();

            Assert.Equal(CustomizationStatus.FINISHED, customizedProduct.status);
        }

        [Fact]
        public void ensureNumberOfSlotsWorks() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSlotsAndSubCustomizedProducts();
            int expectedNumSlots = 2;
            int actualNumSlots = customizedProduct.numberOfSlots();

            Assert.Equal(expectedNumSlots, actualNumSlots);
        }

        [Fact]
        public void ensureNumberOfSubCustomizedProductsWorks() {
            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts();

            int expectedNumSubCustomizedProducts = 1;
            int actualNumSubCustomizedProducts = customizedProduct.numberOfSubCustomizedProducts();

            Assert.Equal(expectedNumSubCustomizedProducts, actualNumSubCustomizedProducts);
        }
    }
}