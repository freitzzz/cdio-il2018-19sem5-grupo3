using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.dto;
using support.dto;
using Xunit;
using static core.domain.CustomizedProduct;

namespace core_tests.domain
{
    /// <summary>
    /// Unit testing class for CustomizedProduct
    /// </summary>
    //TODO Create a method that returns a customized product to substantially reduce lines of code
    public class CustomizedProductTest
    {
        //These are all seperated into their own methods in order to allow for each property to be tested

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

        private CustomizedProduct buildValidInstance(string serialNumber)
        {
            CustomizedDimensions selectedDimensions = buildCustomizedDimensions();

            return CustomizedProductBuilder.createAnonymousUserCustomizedProduct(serialNumber, buildValidProduct(), selectedDimensions).build();
        }

        private CustomizedProduct buildValidFinishedInstance(string serialNumber)
        {
            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            CustomizedDimensions selectedDimensions = buildCustomizedDimensions();

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createAnonymousUserCustomizedProduct(serialNumber, buildValidProduct(), selectedDimensions).build();

            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            customizedProduct.finalizeCustomization();

            return customizedProduct;
        }

        private CustomizedProduct buildValidInstanceWithSubCustomizedProducts(string serialNumber)
        {
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

            Product otherComponent = new Product("This is another reference", "This is another Designation", "component.gltf", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { componentMeasurement });

            Product component = new Product("This is another reference", "This is another Designation", "component.gltf", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { componentMeasurement }, complementaryProducts: new List<Product> { otherComponent });

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, complementaryProducts: new List<Product> { component }, slotWidths: productSlotWidths);

            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", product, customizedProductDimensions).build();

            CustomizedProduct customizedComponent = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", component, customizedProductDimensions).build();

            CustomizedProduct otherCustomizedComponent = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", otherComponent, customizedProductDimensions).build();

            customizedProduct.changeCustomizedMaterial(buildCustomizedMaterial());

            customizedComponent.addCustomizedProduct(otherCustomizedComponent, customizedComponent.slots[0]);

            customizedProduct.addCustomizedProduct(customizedComponent, customizedProduct.slots[0]);

            return customizedProduct;
        }

        private CustomizedProduct buildValidFinishedInstanceWithSubCustomizedProducts(string serialNumber)
        {
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
                .createAnonymousUserCustomizedProduct("serial number", product, customizedProductDimensions).build();

            CustomizedProduct customizedComponent = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", component, customizedProductDimensions).build();

            customizedProduct.changeCustomizedMaterial(buildCustomizedMaterial());
            customizedComponent.changeCustomizedMaterial(buildCustomizedMaterial());

            customizedProduct.addCustomizedProduct(customizedComponent, customizedProduct.slots[0]);

            customizedProduct.finalizeCustomization();

            return customizedProduct;
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateAnonymousUserCustomizedProductIfSerialNumberIsNull()
        {
            Action buildAction = () => CustomizedProductBuilder.createAnonymousUserCustomizedProduct(null, buildValidProduct(), buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateAnonymousUserCustomizedProductIfSerialNumberIsEmpty()
        {
            Action buildAction = () => CustomizedProductBuilder.createAnonymousUserCustomizedProduct("", buildValidProduct(), buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateAnonymousUserCustomizedProductIfProductIsNull()
        {
            Action buildAction = () => CustomizedProductBuilder.createAnonymousUserCustomizedProduct("1", null, buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateAnonymousUserCustomizedProductIfCustomizedDimensionsDimensionsIsNull()
        {
            Action buildAction = () => CustomizedProductBuilder.createAnonymousUserCustomizedProduct("1", buildValidProduct(), null);

            Assert.Throws<ArgumentException>(buildAction);
        }


        [Fact]
        public void ensureCustomizedProductBuilderCantCreateRegisteredUserCustomizedProductIfSerialNumberIsNull()
        {
            Action buildAction = () => CustomizedProductBuilder.createRegisteredUserCustomizedProduct(null, "user auth token", buildValidProduct(), buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateRegisteredUserCustomizedProductIfSerialNumberIsEmpty()
        {
            Action buildAction = () => CustomizedProductBuilder.createRegisteredUserCustomizedProduct("", "user auth token", buildValidProduct(), buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateRegisteredUserCustomizedProductIfAuthenticationTokenIsNull()
        {
            Action buildAction = () => CustomizedProductBuilder.createRegisteredUserCustomizedProduct("1234", null, buildValidProduct(), buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateRegisteredUserCustomizedProductIfAuthenticationTokenIsEmpty()
        {
            Action buildAction = () => CustomizedProductBuilder.createRegisteredUserCustomizedProduct("1234", "", buildValidProduct(), buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateRegisteredUserCustomizedProductIfProductIsNull()
        {
            Action buildAction = () => CustomizedProductBuilder.createRegisteredUserCustomizedProduct("1234", "user auth token", null, buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateRegisteredUserCustomizedProductIfCustomizedDimensionsIsNull()
        {
            Action buildAction = () => CustomizedProductBuilder.createRegisteredUserCustomizedProduct("1234", "user auth token", buildValidProduct(), null);

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateManagerCustomizedProductIfReferenceIsNull()
        {
            Action buildAction = () => CustomizedProductBuilder.createManagerCustomizedProduct(null, "manager auth token", buildValidProduct(), buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateManagerCustomizedProductIfReferenceIsEmpty()
        {
            Action buildAction = () => CustomizedProductBuilder.createManagerCustomizedProduct("", "manager auth token", buildValidProduct(), buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateManagerCustomizedProductIfAuthenticationTokenIsNull()
        {
            Action buildAction = () => CustomizedProductBuilder.createManagerCustomizedProduct("1234", null, buildValidProduct(), buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateManagerCustomizedProductIfAuthenticationTokenIsEmpty()
        {
            Action buildAction = () => CustomizedProductBuilder.createManagerCustomizedProduct("1234", "", buildValidProduct(), buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateManagerCustomizedProductIfProductIsNull()
        {
            Action buildAction = () => CustomizedProductBuilder.createManagerCustomizedProduct("1234", "manager auth token", null, buildCustomizedDimensions());

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantCreateManagerCustomizedProductIfCustomizedDimensionsIsNull()
        {
            Action buildAction = () => CustomizedProductBuilder.createManagerCustomizedProduct("1234", "manager auth token", buildValidProduct(), null);

            Assert.Throws<ArgumentException>(buildAction);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantBuildCustomizedProductWithDesignationIfDesignationIsNull()
        {
            CustomizedProductBuilder builder = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("1234", buildValidProduct(), buildCustomizedDimensions());

            Action buildWithNullDesignation = () => builder.withDesignation(null);

            Assert.Throws<ArgumentException>(buildWithNullDesignation);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantBuildCustomizedProductWithDesignationIfDesignationIsEmpty()
        {
            CustomizedProductBuilder builder = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("1234", buildValidProduct(), buildCustomizedDimensions());

            Action buildWithNullDesignation = () => builder.withDesignation("");

            Assert.Throws<ArgumentException>(buildWithNullDesignation);
        }

        [Fact]
        public void ensureCustomizedProductBuilderCantBuildCustomizedProductWithCustomizedMaterialIfCustomizedMaterialIsNull()
        {
            CustomizedProductBuilder builder = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("1234", buildValidProduct(), buildCustomizedDimensions());

            Action buildWithNullMaterial = () => builder.withMaterial(null);

            Assert.Throws<ArgumentException>(buildWithNullMaterial);
        }

        [Fact]
        public void ensureSlotMatchingCustomizedProductDimensionsIsCreatedWhenCustomizedProductIsBuilt()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.slots.First().slotDimensions, customizedProduct.customizedDimensions);
        }

        [Fact]
        public void ensureChangingReferenceIfCustomizedProductHasSerialNumberThrowsException()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Action changeReference = () => customizedProduct.changeReference("this is a reference");

            Assert.Throws<InvalidOperationException>(changeReference);
        }

        [Fact]
        public void ensureChangingReferenceIfCustomizedProductHasSerialNumberDoesNotChangeReference()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            try
            {
                customizedProduct.changeReference("this is a reference");
            }
            catch (Exception) { }

            Assert.Null(customizedProduct.reference);
        }

        [Fact]
        public void ensureChangingReferenceIfCustomizationIsFinishedThrowsException()
        {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createManagerCustomizedProduct("this is a reference", "manager auth token",
                buildValidProduct(), buildCustomizedDimensions()).build();

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            //the CustomizedProduct needs a CustomizedMaterial prior to finishing customization
            customizedProduct.finalizeCustomization();

            Action changeReference = () => customizedProduct.changeReference("this is another reference");

            Assert.Throws<InvalidOperationException>(changeReference);
        }

        [Fact]
        public void ensureChangingReferenceIfCustomizationIsFinishedDoesNotChangeReference()
        {
            string reference = "this is a reference";

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createManagerCustomizedProduct(reference, "manager auth token",
                buildValidProduct(), buildCustomizedDimensions()).build();

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            //the CustomizedProduct needs a CustomizedMaterial prior to finishing customization
            customizedProduct.finalizeCustomization();

            try
            {
                customizedProduct.changeReference("this is another reference");
            }
            catch (Exception) { }

            Assert.Equal(reference, customizedProduct.reference);
        }

        [Fact]
        public void ensureChangingReferenceToNullReferenceThrowsException()
        {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createManagerCustomizedProduct("this is a reference", "manager auth token",
                buildValidProduct(), buildCustomizedDimensions()).build();

            Action changeReference = () => customizedProduct.changeReference(null);

            Assert.Throws<ArgumentException>(changeReference);
        }

        [Fact]
        public void ensureChangingReferenceToNullReferenceDoesNotChangeReference()
        {
            string reference = "this is a reference";

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createManagerCustomizedProduct(reference, "manager auth token",
                buildValidProduct(), buildCustomizedDimensions()).build();

            try
            {
                customizedProduct.changeReference(null);
            }
            catch (Exception) { }

            Assert.Equal(reference, customizedProduct.reference);
        }

        [Fact]
        public void ensureChangingReferenceToEmptyReferenceThrowsException()
        {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createManagerCustomizedProduct("this is a reference", "manager auth token",
                buildValidProduct(), buildCustomizedDimensions()).build();

            Action changeReference = () => customizedProduct.changeReference("");

            Assert.Throws<ArgumentException>(changeReference);
        }

        [Fact]
        public void ensureChangingReferenceToEmptyReferenceDoesNotChangeReference()
        {
            string reference = "this is a reference";

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createManagerCustomizedProduct(reference, "manager auth token",
                buildValidProduct(), buildCustomizedDimensions()).build();

            try
            {
                customizedProduct.changeReference("");
            }
            catch (Exception) { }

            Assert.Equal(reference, customizedProduct.reference);
        }

        [Fact]
        public void ensureChangingReferenceToValidReferenceDoesNotThrowException()
        {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createManagerCustomizedProduct("this is a reference", "manager auth token",
                buildValidProduct(), buildCustomizedDimensions()).build();


            Action changeReference = () => customizedProduct.changeReference("this is another reference");

            Exception exception = Record.Exception(changeReference);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureChangingReferenceToValidReferenceChangesValue()
        {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createManagerCustomizedProduct("this is a reference", "manager auth token",
                buildValidProduct(), buildCustomizedDimensions()).build();

            string newReference = "this is another reference";

            customizedProduct.changeReference(newReference);

            Assert.Equal(newReference, customizedProduct.reference);
        }

        [Fact]
        public void ensureChangingDesignationIfCustomizationIsFinishedThrowsException()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            //the CustomizedProduct needs a CustomizedMaterial prior to finishing customization
            customizedProduct.finalizeCustomization();

            Action changeDesignation = () => customizedProduct.changeDesignation("this is a designation");

            Assert.Throws<InvalidOperationException>(changeDesignation);
        }

        [Fact]
        public void ensureChangingDesignationIfCustomizationIsFinishedDoesNotChangeDesignation()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            //the CustomizedProduct needs a CustomizedMaterial prior to finishing customization
            customizedProduct.finalizeCustomization();

            try
            {
                customizedProduct.changeDesignation("this is a designation");
            }
            catch (Exception) { }

            Assert.Null(customizedProduct.designation);
        }

        [Fact]
        public void ensureChangingDesignationToNullDesignationThrowsException()
        {
            string designation = "this is a designation";

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createManagerCustomizedProduct("this is a reference", "manager auth token",
                buildValidProduct(), buildCustomizedDimensions()).withDesignation(designation).build();

            Action changeDesignation = () => customizedProduct.changeDesignation(null);

            Assert.Throws<ArgumentException>(changeDesignation);
        }

        [Fact]
        public void ensureChangingDesignationToNullDesignationDoesNotChangeDesignation()
        {
            string designation = "this is a designation";

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createManagerCustomizedProduct("this is a reference", "manager auth token",
                buildValidProduct(), buildCustomizedDimensions()).withDesignation(designation).build();

            try
            {
                customizedProduct.changeDesignation(designation);
            }
            catch (Exception) { }

            Assert.Equal(designation, customizedProduct.designation);
        }

        [Fact]
        public void ensureChangingDesignationToEmptyDesignationThrowsException()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Action changeDesignation = () => customizedProduct.changeDesignation("");

            Assert.Throws<ArgumentException>(changeDesignation);
        }

        [Fact]
        public void ensureChangingDesignationToEmptyDesignationDoesNotChangeDesignation()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            try
            {
                customizedProduct.changeDesignation("");
            }
            catch (Exception) { }

            Assert.Null(customizedProduct.designation);
        }

        [Fact]
        public void ensureChangingDesignationToValidDesignationDoesNotThrowException()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Action changeDesignation = () => customizedProduct.changeDesignation("this is a designation");

            Exception exception = Record.Exception(changeDesignation);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureChangingDesignationToValidDesignationChangesDesignation()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            string designation = "this is a designation";

            customizedProduct.changeDesignation(designation);

            Assert.Equal(designation, customizedProduct.designation);
        }

        [Fact]
        public void ensureChangingMaterialIfCustomizationIsFinishedThrowsException()
        {
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
        public void ensureChangingMaterialIfCustomizationIsFinishedDoesNotChangeMaterial()
        {
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

            try
            {
                customizedProduct.changeCustomizedMaterial(customizedMaterial);
            }
            catch (Exception) { }

            Assert.Equal(customizedMaterial, customizedProduct.customizedMaterial);
            Assert.NotEqual(otherCustomizedMaterial, customizedProduct.customizedMaterial);
        }

        [Fact]
        public void ensureChangingMaterialToNullCustomizedMaterialThrowsException()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Action changeMaterial = () => customizedProduct.changeCustomizedMaterial(null);

            Assert.Throws<ArgumentException>(changeMaterial);
        }

        [Fact]
        public void ensureChangingMaterialToNullCustomizedMaterialDoesNotChangeMaterial()
        {
            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createRegisteredUserCustomizedProduct("serial number", "user auth token",
               buildValidProduct(), buildCustomizedDimensions()).withMaterial(customizedMaterial).build();

            try
            {
                customizedProduct.changeCustomizedMaterial(null);
            }
            catch (Exception) { }

            Assert.Equal(customizedMaterial, customizedProduct.customizedMaterial);
        }

        [Fact]
        public void ensureChangingFinishIfCustomizationIsFinishedThrowsException()
        {
            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createRegisteredUserCustomizedProduct("serial number", "user auth token",
               buildValidProduct(), buildCustomizedDimensions()).withMaterial(customizedMaterial).build();

            customizedProduct.finalizeCustomization();

            Action changeFinish = () => customizedProduct.changeFinish(buildGlossyFinish());

            Assert.Throws<InvalidOperationException>(changeFinish);
        }

        [Fact]
        public void ensureChangingFinishIfCustomizationIsFinishedDoesNotChangeFinish()
        {
            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createRegisteredUserCustomizedProduct("serial number", "user auth token",
               buildValidProduct(), buildCustomizedDimensions()).withMaterial(customizedMaterial).build();

            customizedProduct.finalizeCustomization();

            try
            {
                customizedProduct.changeFinish(buildGlossyFinish());
            }
            catch (Exception) { }

            Assert.Equal(buildMatteFinish(), customizedProduct.customizedMaterial.finish);
        }

        [Fact]
        public void ensureChangingFinishIfCustomizedMaterialIsNullThrowsException()
        {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createRegisteredUserCustomizedProduct("serial number", "user auth token",
               buildValidProduct(), buildCustomizedDimensions()).build();

            Action changeFinish = () => customizedProduct.changeFinish(buildGlossyFinish());

            Assert.Throws<InvalidOperationException>(changeFinish);
        }

        [Fact]
        public void ensureChangingFinishIfCustomizedMaterialIsNullDoesNotChangeFinish()
        {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createRegisteredUserCustomizedProduct("serial number", "user auth token",
                buildValidProduct(), buildCustomizedDimensions()).build();

            try
            {
                customizedProduct.changeFinish(buildGlossyFinish());
            }
            catch (Exception) { }

            Assert.Null(customizedProduct.customizedMaterial);
        }

        [Fact]
        public void ensureChangingFinishIfCustomizedMaterialIsDefinedDoesNotThrowException()
        {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createRegisteredUserCustomizedProduct("serial number", "user auth token",
                buildValidProduct(), buildCustomizedDimensions()).withMaterial(buildCustomizedMaterial()).build();

            Action changeFinish = () => customizedProduct.changeFinish(buildGlossyFinish());

            Exception exception = Record.Exception(changeFinish);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureChangingFinishIfCustomizedMaterialIsDefinedChangesFinish()
        {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createRegisteredUserCustomizedProduct("serial number", "user auth token",
                 buildValidProduct(), buildCustomizedDimensions()).withMaterial(buildCustomizedMaterial()).build();

            Finish finish = buildGlossyFinish();

            customizedProduct.changeFinish(finish);

            Assert.Equal(finish, customizedProduct.customizedMaterial.finish);
        }

        [Fact]
        public void ensureChangingColorIfCustomizationIsFinishedThrowsException()
        {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createRegisteredUserCustomizedProduct("serial number", "user auth token",
                buildValidProduct(), buildCustomizedDimensions()).withMaterial(buildCustomizedMaterial()).build();

            customizedProduct.finalizeCustomization();

            Action changeColor = () => customizedProduct.changeColor(buildGreenColor());

            Assert.Throws<InvalidOperationException>(changeColor);
        }

        [Fact]
        public void ensureChangingColorIfCustomizationIsFinishedDoesNotChangeFinish()
        {
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createRegisteredUserCustomizedProduct("serial number", "user auth token",
                buildValidProduct(), buildCustomizedDimensions()).withMaterial(buildCustomizedMaterial()).build();

            customizedProduct.finalizeCustomization();

            try
            {
                customizedProduct.changeColor(buildGreenColor());
            }
            catch (Exception) { }

            Assert.Equal(buildRedColor(), customizedProduct.customizedMaterial.color);
        }

        [Fact]
        public void ensureChangingColorIfCustomizedMaterialIsNotDefinedThrowsException()
        {
            //TODO: implement this
        }

        [Fact]
        public void ensureChangingColorIfCustomizedMaterialIsNotDefinedDoesNotChangeColor()
        {
            //TODO: implement this
        }

        [Fact]
        public void ensureAddingSlotAfterCustomizationIsFinishedThrowsException()
        {
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
        public void ensureAddingSlotAfterCustomizationIsFinishedDoesNotAddSlot()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            //the CustomizedProduct needs a CustomizedMaterial prior to finishing customization
            customizedProduct.finalizeCustomization();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(76, 35, 35);

            try
            {
                customizedProduct.addSlot(slotDimensions);
            }
            catch (Exception) { }

            //Make sure that there's only one slot
            //And that that slot is the one matching the customized product's dimensions
            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.customizedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotIfProductDoesNotSupportSlotsThrowsException()
        {
            Dimension firstHeightDimension = new ContinuousDimensionInterval(50, 100, 2);
            Dimension firstWidthDimension = new DiscreteDimensionInterval(new List<double>() { 75, 80, 85, 90, 95, 120 });
            Dimension firstDepthDimension = new SingleValueDimension(25);

            Measurement firstMeasurement = new Measurement(firstHeightDimension, firstWidthDimension, firstDepthDimension);

            Dimension sideDimension = new SingleValueDimension(60);
            Measurement secondMeasurement = new Measurement(sideDimension, sideDimension, sideDimension);

            Product productWithoutSlotSupport = new Product("#429", "Fabulous Closet", "fabcloset.glb",
                buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { firstMeasurement, secondMeasurement });

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", productWithoutSlotSupport, buildCustomizedDimensions()).build();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(76, 35, 35);

            Action addSlot = () => customizedProduct.addSlot(slotDimensions);

            Assert.Throws<InvalidOperationException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotIfProductDoesNotSupportSlotsDoesNotAddSlot()
        {
            Dimension firstHeightDimension = new ContinuousDimensionInterval(50, 100, 2);
            Dimension firstWidthDimension = new DiscreteDimensionInterval(new List<double>() { 75, 80, 85, 90, 95, 120 });
            Dimension firstDepthDimension = new SingleValueDimension(25);

            Measurement firstMeasurement = new Measurement(firstHeightDimension, firstWidthDimension, firstDepthDimension);

            Dimension sideDimension = new SingleValueDimension(60);
            Measurement secondMeasurement = new Measurement(sideDimension, sideDimension, sideDimension);

            Product productWithoutSlotSupport = new Product("#429", "Fabulous Closet", "fabcloset.glb",
                buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { firstMeasurement, secondMeasurement });

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", productWithoutSlotSupport, buildCustomizedDimensions()).build();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(76, 35, 35);

            try
            {
                customizedProduct.addSlot(slotDimensions);
            }
            catch (Exception) { }

            //Make sure that there's only one slot
            //And that that slot is the one matching the customized product's dimensions
            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.customizedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotWithNullDimensionsThrowsException()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            Action addSlotWithNullDimensions = () => customizedProduct.addSlot(null);

            Assert.Throws<ArgumentException>(addSlotWithNullDimensions);
        }

        [Fact]
        public void ensureAddingSlotWithNullDimensionsDoesNotAddSlot()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            try
            {
                customizedProduct.addSlot(null);
            }
            catch (Exception) { }

            //Make sure that there's only one slot
            //And that that slot is the one matching the customized product's dimensions
            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.customizedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotWithDimensionsNotFollowingSpecificationThrowsException()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedDimensions invalidDimensions = CustomizedDimensions.valueOf(76, 23, 25);

            Action addSlot = () => customizedProduct.addSlot(invalidDimensions);

            Assert.Throws<ArgumentException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotWithDimensionsNotFollowingSpecificationDoesNotAddSlot()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            CustomizedDimensions invalidDimensions = CustomizedDimensions.valueOf(76, 23, 25);

            try
            {
                customizedProduct.addSlot(invalidDimensions);
            }
            catch (Exception) { }

            //Make sure that there's only one slot
            //And that that slot is the one matching the customized product's dimensions
            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.customizedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotHigherThanCustomizedProductThrowsException()
        {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("serial number", buildValidProduct(), customizedProductDimensions).build();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(70, 40, 60);

            Action addSlot = () => customizedProduct.addSlot(slotDimensions);

            Assert.Throws<ArgumentException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotHigherThanCustomizedProductDoesNotAddCustomizedProduct()
        {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("serial number", buildValidProduct(), customizedProductDimensions).build();

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(70, 40, 60);

            try
            {
                customizedProduct.addSlot(slotDimensions);
            }
            catch (Exception) { }

            //Make sure that there's only one slot
            //And that that slot is the one matching the customized product's dimensions
            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.customizedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotWiderThanCustomizedProductThrowsException()
        {
            //TODO: implement this
        }

        [Fact]
        public void ensureAddingSlotWiderThanCustomizedProductDoesNotAddSlot()
        {
            //TODO: implement this
        }

        [Fact]
        public void ensureAddingSlotDeeperThanCustomizedProductThrowsException()
        {
            //TODO: implement this
        }

        [Fact]
        public void ensureAddingSlotDeeperThanCustomizedProductDoesNotAddSlot()
        {
            //TODO: implement this
        }

        [Fact]
        public void ensureAddingSlotWhenCustomizedProductsHaveBeenAddedThrowsException()
        {
            Dimension heightDimension = new SingleValueDimension(60);
            Dimension widthDimension = new ContinuousDimensionInterval(70, 200, 1);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 100, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);

            Dimension childDimension = new SingleValueDimension(30);

            Measurement childMeasurement = new Measurement(childDimension, childDimension, childDimension);

            Product childProduct = new Product("This is a Child", "child product", "child.dae", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { childMeasurement });

            product.addComplementaryProduct(childProduct);

            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", product, customizedProductDimensions).build();

            CustomizedDimensions childCustomizedDimensions = CustomizedDimensions.valueOf(30, 30, 30);

            CustomizedProduct childCustomizedProduct = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number 1", childProduct, childCustomizedDimensions).build();

            customizedProduct.addCustomizedProduct(childCustomizedProduct, customizedProduct.slots.First());

            CustomizedDimensions otherSlotDimensions = CustomizedDimensions.valueOf(60, 100, 60);

            Action addSlot = () => customizedProduct.addSlot(otherSlotDimensions);

            Assert.Throws<InvalidOperationException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotThatInvalidatesMainSlotThrowsException()
        {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("serial number", buildValidProduct(), customizedProductDimensions).build();

            //The product's specifications allows for the slot widths to vary between 25 and 50
            //Adding a slot with a width of 40 will cause the other slot to not follow specification, generating an error

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(60, 40, 60);

            Action addSlot = () => customizedProduct.addSlot(slotDimensions);

            Assert.Throws<ArgumentException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotThatInvalidatesMainSlotDoesNotAddSlot()
        {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("serial number", buildValidProduct(), customizedProductDimensions).build();

            //The product's specifications allows for the slot widths to vary between 25 and 50
            //Adding a slot with a width of 40 will cause the other slot to not follow specification, generating an error

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(60, 40, 60);

            try
            {
                customizedProduct.addSlot(slotDimensions);
            }
            catch (Exception) { }

            //Make sure that there's only one slot
            //And that that slot is the one matching the customized product's dimensions
            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProduct.customizedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotThatDoesNotInvalidateMainSlotDoesNotThrowException()
        {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("serial number", buildValidProduct(), customizedProductDimensions).build();

            //The product's specifications allows for the slot widths to vary between 25 and 50
            //Adding a slot width of 35 will make the other slot have a width of 25, respecting specification

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(60, 35, 60);

            Action addSlot = () => customizedProduct.addSlot(slotDimensions);

            Exception exception = Record.Exception(addSlot);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddingSlotThatDoesNotInvalidateMainSlotAddsSlot()
        {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("serial number", buildValidProduct(), customizedProductDimensions).build();

            //The product's specifications allows for the slot widths to vary between 25 and 50
            //Adding a slot width of 35 will make the other slot have a width of 25, respecting specification

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(60, 35, 60);

            customizedProduct.addSlot(slotDimensions);

            Assert.Equal(2, customizedProduct.slots.Count);
        }

        [Fact]
        public void ensureAddingSlotThatDoesNotInvalidateMainSlotResizesMainSlot()
        {
            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 60, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder.createAnonymousUserCustomizedProduct("serial number", buildValidProduct(), customizedProductDimensions).build();

            //The product's specifications allows for the slot widths to vary between 25 and 50
            //Adding a slot width of 35 will make the other slot have a width of 25, respecting specification

            CustomizedDimensions slotDimensions = CustomizedDimensions.valueOf(60, 35, 60);

            customizedProduct.addSlot(slotDimensions);

            CustomizedDimensions expectedDimensions = CustomizedDimensions.valueOf(60, 25, 60);

            Assert.Equal(expectedDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotThatMakesTheMainSlotNotFollowTheSlotSpecificationsThrowsException()
        {
            Dimension heightDimension = new SingleValueDimension(60);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 100, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);


            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", product, customizedProductDimensions).build();

            //Adding a slot with a width of 60 makes the main slot have a width of 140, which exceeds the maximum
            Action addSlot = () => customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));

            Assert.Throws<ArgumentException>(addSlot);
        }

        [Fact]
        public void ensureAddingSlotThatMakesTheMainSlotNotFollowTheSlotSpecificationsDoesNotAddSlot()
        {
            Dimension heightDimension = new SingleValueDimension(60);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 100, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);


            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", product, customizedProductDimensions).build();

            //Adding a slot with a width of 60 makes the main slot have a width of 140, which exceeds the maximum
            try
            {
                customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));
            }
            catch (Exception) { }

            Assert.Single(customizedProduct.slots);
            Assert.Equal(customizedProductDimensions, customizedProduct.slots.First().slotDimensions);
        }

        [Fact]
        public void ensureAddingSlotThatHasValidWidthWithPreviouslyAddedSlotsDoesNotThrowException()
        {
            Dimension heightDimension = new SingleValueDimension(60);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 140, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);


            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", product, customizedProductDimensions).build();

            customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));
            customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 50, 60));

            //adding both of these slots will create a slot with a width of 90

            Action addSlot = () => customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));

            Exception exception = Record.Exception(addSlot);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddingSlotThatHasValidWidthWithPreviouslyAddedSlotsAddsSlot()
        {
            Dimension heightDimension = new SingleValueDimension(60);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 140, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);


            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", product, customizedProductDimensions).build();

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
        public void ensureAddingRecommendedNumberOfSlotsDoesNotThrowException()
        {
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
                .createAnonymousUserCustomizedProduct("serial number", product, customizedProductDimensions).build();

            int recommendedNumberOfSlots = (int)(customizedProductDimensions.width / productSlotWidths.recommendedWidth);

            CustomizedDimensions slotDimensions =
                CustomizedDimensions.valueOf(60, productSlotWidths.recommendedWidth, 60);


            Action addSlots = () =>
            {
                customizedProduct.addSlot(CustomizedDimensions.valueOf(60, productSlotWidths.maxWidth, 60));
                customizedProduct.addSlot(slotDimensions);
                customizedProduct.addSlot(slotDimensions);
            };

            Exception exception = Record.Exception(addSlots);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureChangingDimensionsAfterCustomizationIsFinishedThrowsException()
        {
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
        public void ensureChangingDimensionsAfterCustomizationIsFinishedDoesNotChangeDimensions()
        {
            CustomizedProduct customizedProduct = buildValidInstance("1234");

            //specify a material before finalizing customization
            CustomizedMaterial customizedMaterial = buildCustomizedMaterial();
            customizedProduct.changeCustomizedMaterial(customizedMaterial);

            customizedProduct.finalizeCustomization();

            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(81, 90, 25);

            try
            {
                customizedProduct.changeDimensions(customizedDimensions);
            }
            catch (Exception) { }

            Assert.NotEqual(customizedDimensions, customizedProduct.customizedDimensions);
            Assert.Equal(buildCustomizedDimensions(), customizedProduct.customizedDimensions);
        }

        [Fact]
        public void ensureChangingDimensionsIfSlotsHaveBeenAddedThrowsException()
        {
            Dimension heightDimension = new ContinuousDimensionInterval(60, 80, 2);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 140, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);


            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", product, customizedProductDimensions).build();

            customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));

            Action changeDimensions = () => customizedProduct.changeDimensions(CustomizedDimensions.valueOf(72, 200, 60));

            Assert.Throws<InvalidOperationException>(changeDimensions);
        }

        [Fact]
        public void ensureChangingDimensionsIfSlotsHaveBeenAddedDoesNotChangeDimensions()
        {
            Dimension heightDimension = new ContinuousDimensionInterval(60, 80, 2);
            Dimension widthDimension = new SingleValueDimension(200);
            Dimension depthDimension = new SingleValueDimension(60);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(40, 140, 60);

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, productSlotWidths);


            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(60, 200, 60);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", product, customizedProductDimensions).build();

            customizedProduct.addSlot(CustomizedDimensions.valueOf(60, 60, 60));

            try
            {
                customizedProduct.changeDimensions(CustomizedDimensions.valueOf(72, 200, 60));
            }
            catch (Exception) { }

            Assert.Equal(customizedProductDimensions, customizedProduct.customizedDimensions);
        }

        [Fact]
        public void ensureIdMatchesReferenceIfCustomizedProductIsCreatedWithAReference()
        {
            string reference = "this is a reference";

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createManagerCustomizedProduct(reference, "Manager Auth Token", buildValidProduct(), buildCustomizedDimensions()).build();

            Assert.Equal(reference, customizedProduct.id());
        }

        [Fact]
        public void ensureIdMatchesSerialNumberIfCustomizedProductIsCreatedWithASerialNumber()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstance(serialNumber);

            Assert.Equal(serialNumber, customizedProduct.id());
        }

        [Fact]
        public void ensureCustomizedProductSameAsItsReference()
        {
            string reference = "this is a reference";

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createManagerCustomizedProduct(reference, "Manager Auth Token", buildValidProduct(), buildCustomizedDimensions()).build();

            Assert.True(customizedProduct.sameAs(reference));
        }

        [Fact]
        public void ensureCustomizedProductSameAsItsSerialNumber()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstance(serialNumber);

            Assert.True(customizedProduct.sameAs(serialNumber));
        }

        [Fact]
        public void ensureActivatingAnActivatedCustomizedProductReturnsFalse()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstance(serialNumber);

            customizedProduct.activate();

            Assert.False(customizedProduct.activate());
        }

        [Fact]
        public void ensureActivatingAnActivatedCustomizedProductDoesntActivateItAndItsChildren()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts(serialNumber);

            customizedProduct.activate();

            Assert.False(customizedProduct.activate());
            Assert.True(customizedProduct.activated);
            foreach (CustomizedProduct child in customizedProduct.slots[0].customizedProducts)
            {
                Assert.True(child.activated);
            }
        }

        [Fact]
        public void ensureActivatingADeactivatedCustomizedProductReturnsTrue()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstance(serialNumber);
            customizedProduct.deactivate();

            Assert.True(customizedProduct.activate());
            Assert.True(customizedProduct.activated);
        }

        [Fact]
        public void ensureActivatingADeactivatedCustomizedProductActivatesItAndItsChildren()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts(serialNumber);
            customizedProduct.deactivate();
            Assert.True(customizedProduct.activate());
            Assert.True(customizedProduct.activated);
            foreach (CustomizedProduct child in customizedProduct.slots[0].customizedProducts)
            {
                Assert.True(child.activated);
            }
        }

        [Fact]
        public void ensureDeactivatingADeactivatedCustomizedProductReturnsFalse()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstance(serialNumber);
            customizedProduct.deactivate();
            Assert.False(customizedProduct.deactivate());
            Assert.False(customizedProduct.activated);
        }

        [Fact]
        public void ensureDeactivatingAnActivatedCustomizedProductReturnsTrue()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstance(serialNumber);

            customizedProduct.activate();

            Assert.True(customizedProduct.deactivate());
            Assert.False(customizedProduct.activated);
        }

        [Fact]
        public void ensureDeactivatingAnActivatedCustomizedProductDeactivatesItAndItsChildren()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts(serialNumber);

            customizedProduct.activate();

            Assert.True(customizedProduct.deactivate());
            Assert.False(customizedProduct.activated);
            foreach (CustomizedProduct child in customizedProduct.slots[0].customizedProducts)
            {
                Assert.False(child.activated);
            }
        }

        [Fact]
        public void ensureDeactivatingADeactivatedCustomizedProductDoesntDeactivateItAndItsChildren()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts(serialNumber);
            customizedProduct.deactivate();

            Assert.False(customizedProduct.deactivate());
            Assert.False(customizedProduct.activated);
            foreach (CustomizedProduct child in customizedProduct.slots[0].customizedProducts)
            {
                Assert.False(child.activated);
            }
        }

        [Fact]
        public void ensureAddingCustomizedProductToFinishedCustomizedProductThrowsException()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidFinishedInstance(serialNumber);

            Action act = () => customizedProduct.addCustomizedProduct(buildValidInstance(serialNumber), customizedProduct.slots[0]);

            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void ensureAddingNullCustomizedProductThrowsException()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstance(serialNumber);

            Action act = () => customizedProduct.addCustomizedProduct(null, customizedProduct.slots[0]);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureAddingCustomizedProductToNullSlotThrowsException()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstance(serialNumber);

            Action act = () => customizedProduct.addCustomizedProduct(buildValidInstance(serialNumber), null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureAddingCustomizedProductToNonMatchingSlotThrowsException()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstance(serialNumber);

            Action act = () => customizedProduct.addCustomizedProduct(buildValidInstance(serialNumber),
                                 new Slot("hey i'm a slot identifier", CustomizedDimensions.valueOf(100, 100, 100)));

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureAddingCustomizedProductThatIsntAPossibleComponentThrowsException()
        {
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
                .createAnonymousUserCustomizedProduct("serial number", product, customizedProductDimensions).build();

            CustomizedProduct customizedComponent = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", component, customizedProductDimensions).build();

            customizedProduct.changeCustomizedMaterial(buildCustomizedMaterial());

            Action act = () => customizedProduct.addCustomizedProduct(customizedComponent, customizedProduct.slots[0]);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureAddingValidCustomizedProductAddsCustomizedProductToASlot()
        {
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
                .createAnonymousUserCustomizedProduct("serial number", product, customizedProductDimensions).build();

            CustomizedProduct customizedComponent = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", component, customizedProductDimensions).build();

            customizedProduct.changeCustomizedMaterial(buildCustomizedMaterial());

            customizedProduct.addCustomizedProduct(customizedComponent, customizedProduct.slots[0]);

            Assert.Equal(customizedProduct.slots[0].customizedProducts[0], customizedComponent);
        }

        [Fact]
        public void ensureRemovingCustomizedProductFromFinishedCustomizedProductThrowsException()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidFinishedInstanceWithSubCustomizedProducts(serialNumber);

            Action act = () => customizedProduct.removeCustomizedProduct(customizedProduct.slots[0].customizedProducts[0], customizedProduct.slots[0]);

            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void ensureRemovingNullCustomizedProductThrowsException()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts(serialNumber);

            Action act = () => customizedProduct.removeCustomizedProduct(null, customizedProduct.slots[0]);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureRemovingCustomizedProductFromNullSlotThrowsException()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts(serialNumber);

            Action act = () => customizedProduct.removeCustomizedProduct(customizedProduct.slots[0].customizedProducts[0], null);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureRemovingCustomizedProductFromSlotThatDoesntHaveItThrowsException()
        {
            Dimension heightDimension = new SingleValueDimension(30);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(30);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(10, 30, 20);

            Dimension componentHeightDimension = new SingleValueDimension(5);
            Dimension componentWidthDimension = new SingleValueDimension(5);
            Dimension componentDepthDimension = new SingleValueDimension(5);

            Measurement componentMeasurement = new Measurement(componentHeightDimension, componentWidthDimension, componentDepthDimension);

            Product component = new Product("This is another reference", "This is another Designation", "component.gltf", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { componentMeasurement });

            Product product = new Product("This is A Reference", "This is A Designation", "model.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { measurement }, complementaryProducts: new List<Product> { component }, slotWidths: productSlotWidths);

            CustomizedDimensions customizedProductDimensions = CustomizedDimensions.valueOf(30, 30, 30);

            CustomizedProduct customizedProduct = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", product, customizedProductDimensions).build();

            CustomizedProduct customizedComponent = CustomizedProductBuilder
                .createAnonymousUserCustomizedProduct("serial number", component, CustomizedDimensions.valueOf(5, 5, 5)).build();

            customizedProduct.changeCustomizedMaterial(buildCustomizedMaterial());

            customizedProduct.addSlot(CustomizedDimensions.valueOf(10, 10, 10));
            customizedProduct.addSlot(CustomizedDimensions.valueOf(10, 10, 10));

            customizedProduct.addCustomizedProduct(customizedComponent, customizedProduct.slots[0]);

            Action act = () => customizedProduct.removeCustomizedProduct(customizedProduct.slots[0].customizedProducts[0], customizedProduct.slots[1]);

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureRemovingCustomizedProductFromSlotRemovesIt()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts(serialNumber);

            customizedProduct.removeCustomizedProduct(customizedProduct.slots[0].customizedProducts[0], customizedProduct.slots[0]);

            Assert.Empty(customizedProduct.slots[0].customizedProducts);
        }

        [Fact]
        public void ensureSubCustomizedProductsFinalizingTheCustomizationProcessThrowsException()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts(serialNumber);

            Action act = () => customizedProduct.slots[0].customizedProducts[0].finalizeCustomization();

            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void ensureFinalizingCustomizationOfCustomizedProductWithoutCustomizedMaterialThrowsException()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstance(serialNumber);

            Action act = () => customizedProduct.finalizeCustomization();

            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ensureFinalizingCustomizationOfAValidCustomizedProductSetsStatusToFinished()
        {
            string serialNumber = "serial number";

            CustomizedProduct customizedProduct = buildValidInstanceWithSubCustomizedProducts(serialNumber);
            customizedProduct.changeCustomizedMaterial(buildCustomizedMaterial());
            customizedProduct.slots[0].customizedProducts[0].changeCustomizedMaterial(buildCustomizedMaterial());
            customizedProduct.slots[0].customizedProducts[0].slots[0].customizedProducts[0].changeCustomizedMaterial(buildCustomizedMaterial());

            customizedProduct.finalizeCustomization();

            Assert.Equal(CustomizationStatus.FINISHED, customizedProduct.status);
        }
    }
}