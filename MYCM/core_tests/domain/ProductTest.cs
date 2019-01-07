using core.domain;
using core.dto;
using core.persistence;
using System;
using System.Collections.Generic;
using Xunit;
using static core.domain.CustomizedProduct;

namespace core_tests.domain {
    /// <summary>
    /// Unit and Business rules tests for Product entity class
    /// </summary>
    public class ProductTest {
        /// <summary>
        /// Creates an instance of Material.
        /// </summary>
        /// <returns>Created instance of Material.</returns>
        private Material buildValidMaterial() {

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            return new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);
        }

        /// <summary>
        /// Creates an instance of ProductCategory.
        /// </summary>
        /// <returns>Created instance of ProductCategory.</returns>
        private ProductCategory buildValidCategory() {
            return new ProductCategory("All Products");
        }

        /// <summary>
        /// Creates an instance of Measurement.
        /// </summary>
        /// <returns>Created instance of Measurement.</returns>
        private Measurement buildValidMeasurement() {
            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            return new Measurement(heightDimension, widthDimension, depthDimension);
        }

        /// <summary>
        /// Creates a simple instance of Product (no complementary products nor slot dimensions)
        /// </summary>
        /// <returns>Created instance of Product.</returns>
        private Product buildValidSimpleProduct() {
            return new Product("#001", "Simple Product", "Simple_Product_001.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });
        }

        /// <summary>
        /// Ensures that product can't be created with a null reference
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithNullReference() {
            Action invalidNullProductReferenceCreation = () =>
                new Product(null, "Shelf", "shelf.obj", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });
            //Since the product was created with a null reference then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidNullProductReferenceCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with an empty reference
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithEmptyReference() {
            Action invalidEmptyProductReferenceCreation = () =>
                new Product("", "Shelf", "shelf.dae", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });
            //Since the product was created with an empty reference then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidEmptyProductReferenceCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with a null designation
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithNullDesignation() {
            Action invalidNullProductDesignationCreation = () =>
                new Product("#666", null, "666.gltf", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });
            //Since the product was created with a null designation then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidNullProductDesignationCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with an empty designation
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithEmptyDesignation() {
            Action invalidEmptyProductDesignationCreation = () =>
                new Product("#666", "", "666.fbx", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });
            //Since the product was created with an empty designation then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidEmptyProductDesignationCreation);
        }

        [Fact]
        public void ensureProductCantBeCreatedWithEmptyModelFilename() {
            Action emptyModelFilenameProductCreation = () => new Product("#666", "Shelf", "",
                buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Assert.Throws<ArgumentException>(emptyModelFilenameProductCreation);
        }

        [Fact]
        public void ensureProductCantBeCreatedWithNullModelFilename() {
            Action nullModelFilenameProductCreation = () => new Product("#666", "Shelf", null,
                buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Assert.Throws<ArgumentException>(nullModelFilenameProductCreation);
        }

        [Fact]
        public void ensureProductCantBeCreatedWithInvalidModelFileExtension() {
            Action invalidExtesionFilenameProductCreation = () => new Product("#666", "Shelf", "shelf666.jpg",
               buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Assert.Throws<ArgumentException>(invalidExtesionFilenameProductCreation);
        }

        [Fact]
        public void ensureProductCantBeCreatedWithNullProductCategory() {
            Action action = () => new Product("#666", "Shelf", "shelf.glb", null, new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Assert.Throws<ArgumentNullException>(action);
        }

        /// <summary>
        /// Ensures that product can't be created with null materials
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithNullMaterials() {
            Action invalidNullMaterialsProductCreation = () =>
                new Product("#666", "Shelf", "shelf.glb", buildValidCategory(), null, new List<Measurement>() { buildValidMeasurement() });
            //Since the product was created with null materials then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidNullMaterialsProductCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with empty materials
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithEmptyMaterials() {
            Action invalidEmptyMaterialsProductCreation = () =>
                new Product("#666", "Shelf", "shelf.glb", buildValidCategory(), new List<Material>(), new List<Measurement>() { buildValidMeasurement() });
            //Since the product was created with empty materials then it should throw
            //An ArgumentException
            //A product needs at least one material to be valid
            Assert.Throws<ArgumentException>(invalidEmptyMaterialsProductCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with duplicated materials
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithDuplicatedMaterials() {
            List<Material> duplicatedMaterials = new List<Material>() { buildValidMaterial(), buildValidMaterial() };
            Action invalidDuplicatedMaterialsProductCreation = () =>
                new Product("#666", "Shelf", "shelf.glb", buildValidCategory(), duplicatedMaterials, new List<Measurement>() { buildValidMeasurement() });
            //Since the product was created with duplicated materials, then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidDuplicatedMaterialsProductCreation);
        }

        [Fact]
        public void ensureProductCantBeCreatedWithNullMeasurements() {
            Action nullMeasurementsProductCreation = () =>
                new Product("#666", "Shelf", "shelf.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, null);

            Assert.Throws<ArgumentException>(nullMeasurementsProductCreation);
        }

        [Fact]
        public void ensureProductCantBeCreatedWithEmptyMeasurements() {
            Action emptyMeasurementsProductCreation = () =>
                new Product("#666", "Shelf", "shelf.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>());

            Assert.Throws<ArgumentException>(emptyMeasurementsProductCreation);
        }

        [Fact]
        public void ensureProductCantBeCreatedWithDuplicatedMeasurements() {
            List<Measurement> measurements = new List<Measurement>() { buildValidMeasurement(), buildValidMeasurement() };

            Action duplicatedMeasurementsProductCreation = () => new Product("#666", "Shelf", "shelf.glb", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, measurements);

            Assert.Throws<ArgumentException>(duplicatedMeasurementsProductCreation);
        }


        /// <summary>
        /// Ensures that product can't be created with null complementary products
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithNullComplementaryProducts() {
            IEnumerable<Product> complementaryProducts = null;
            Action invalidNullComplementaryProductsProductCreation = () =>
                new Product("#666", "Shelf", "shelf.glb", buildValidCategory(),
                    new List<Material>() { buildValidMaterial() },
                    new List<Measurement>() { buildValidMeasurement() }, complementaryProducts);
            //Since the product was created with null complementary products then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidNullComplementaryProductsProductCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with empty complementary products
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithEmptyComplementaryProducts() {
            Action invalidEmptyComplementaryProductsProductCreation = () =>
                new Product("#666", "Shelf", "shelf.glb", buildValidCategory(),
                    new List<Material>() { buildValidMaterial() },
                    new List<Measurement>() { buildValidMeasurement() }, new List<Product>());
            //Since the product was created with empty complementary products then it should throw
            //An ArgumentException
            //Even though that a product may not have complementary products, if we are 
            //Creating a product with an empty enumerable of complementary products
            //Then it should throw an exception since there is already a constructor for that case
            Assert.Throws<ArgumentException>(invalidEmptyComplementaryProductsProductCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with duplicated materials
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithDuplicatedComplementaryProducts() {
            List<Material> materials = new List<Material>() { buildValidMaterial() };
            List<Measurement> measurements = new List<Measurement>() { buildValidMeasurement() };

            Product complement1 = buildValidSimpleProduct();
            Product complement2 = buildValidSimpleProduct();

            List<Product> duplicatedProducts = new List<Product>() { complement1, complement2 };

            Action invalidDuplicatedComplementaryProductsProductCreation = () => new Product("#665", "Structure", "structure.glb",
                buildValidCategory(), materials, measurements, duplicatedProducts);
            //Since the product was created with duplicated complementary products, then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidDuplicatedComplementaryProductsProductCreation);
        }

        [Fact]
        public void ensureProductCantBeCreatedWithNullProductSlotWidths() {
            ProductSlotWidths slotWidths = null;

            Action nullMinSlotDimensionsProductCreation = () =>
                new Product("#500", "Invalid Product", "invalid_product.glb",
                    buildValidCategory(), new List<Material>() { buildValidMaterial() },
                    new List<Measurement>() { buildValidMeasurement() }, slotWidths);

            Assert.Throws<ArgumentNullException>(nullMinSlotDimensionsProductCreation);
        }

        [Fact]
        public void ensureProductWithAllValidArgumentsIsCreated() {
            string reference = "#success";
            string designation = "it just works!";
            string modelFilename = "valid_product.glb";
            ProductCategory category = buildValidCategory();
            List<Material> materials = new List<Material>() { buildValidMaterial() };
            List<Measurement> measurements = new List<Measurement>() { buildValidMeasurement() };
            Product childProduct = buildValidSimpleProduct();
            List<Product> complementaryProducts = new List<Product>() { childProduct };
            ProductSlotWidths productSlotWidths = ProductSlotWidths.valueOf(10, 25, 20);

            Product product = new Product(reference, designation, modelFilename, category, materials, measurements, complementaryProducts, productSlotWidths);

            Assert.NotNull(product);
        }

        [Fact]
        public void ensureAddingNullMaterialToProductThrowsException() {
            Product product = buildValidSimpleProduct();
            //Since we added a null material then an ArgumentNullException should be thrown
            Action nullAddAction = () => product.addMaterial(null);
            Assert.Throws<ArgumentNullException>(nullAddAction);
        }

        [Fact]
        public void ensureAddingNullMaterialToProductDoesNotAddMaterial() {
            Product product = buildValidSimpleProduct();

            Material nullMaterial = null;

            try {
                product.addMaterial(nullMaterial);
            } catch (ArgumentNullException) { }

            Assert.False(product.containsMaterial(nullMaterial));
            Assert.Single(product.productMaterials);
        }


        [Fact]
        public void ensureAddingDuplicatedMaterialToProductThrowsException() {
            Material productMaterial = buildValidMaterial();
            Product product = buildValidSimpleProduct();
            //Since we added a duplicated material then an ArgumentException should be thrown
            Action duplicateAddAction = () => product.addMaterial(productMaterial);
            Assert.Throws<ArgumentException>(duplicateAddAction);
        }

        [Fact]
        public void ensureAddingDuplicatedMaterialToProductDoesNotAddMaterial() {
            Material productMaterial = buildValidMaterial();
            Product product = buildValidSimpleProduct();

            try {
                product.addMaterial(productMaterial);
            } catch (ArgumentException) { }

            Assert.Single(product.productMaterials);
        }


        [Fact]
        public void ensureAddingValidMaterialToProductDoesNotThrowException() {
            Product product = buildValidSimpleProduct();

            Color red = Color.valueOf("Red", 255, 0, 0, 0);
            Finish matte = Finish.valueOf("Matte", 0);
            Material productMaterial = new Material("#002", "Different Material", "ola.jpg", new List<Color>() { red }, new List<Finish>() { matte });

            Action validMaterialAddAction = () => product.addMaterial(productMaterial);
            //Since we added a valid material then no exception should be thrown
            Exception exception = Record.Exception(validMaterialAddAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddingValidMaterialToProductAddsMaterial() {
            Product product = buildValidSimpleProduct();

            Color red = Color.valueOf("Red", 255, 0, 0, 0);
            Finish matte = Finish.valueOf("Matte", 0);
            Material productMaterial = new Material("#002", "Different Material", "ola.jpg", new List<Color>() { red }, new List<Finish>() { matte });

            product.addMaterial(productMaterial);

            Assert.Equal(2, product.productMaterials.Count);
            Assert.True(product.containsMaterial(productMaterial));
        }


        [Fact]
        public void ensureAddingNullComplementaryProductToProductThrowsException() {
            Product product = buildValidSimpleProduct();
            //Since we added a null complementary product then an ArgumentNullException
            Action addNullComplementAction = () => product.addComplementaryProduct(null);
            Assert.Throws<ArgumentNullException>(addNullComplementAction);
        }

        [Fact]
        public void ensureAddingNullComplementaryProductToProductDoesNotAddComplementaryProduct() {
            Product product = buildValidSimpleProduct();
            Product nullComplementaryProduct = null;

            try {
                product.addComplementaryProduct(nullComplementaryProduct);
            } catch (ArgumentNullException) { }

            Assert.False(product.containsComplementaryProduct(nullComplementaryProduct));
            Assert.Empty(product.components);
        }

        [Fact]
        public void ensureAddingProductToItselfThrowsException() {
            Product product = buildValidSimpleProduct();
            Action addRecursiveProductAction = () => product.addComplementaryProduct(product);
            Assert.Throws<ArgumentException>(addRecursiveProductAction);
        }

        [Fact]
        public void ensureAddingProductToItselfDoesNotAddProduct() {
            Product product = buildValidSimpleProduct();
            try {
                product.addComplementaryProduct(product);
            } catch (ArgumentException) { }

            Assert.False(product.containsComplementaryProduct(product));
            Assert.Empty(product.components);
        }

        [Fact]
        public void ensureAddingDuplicatedComplementaryProductThrowsException() {
            Product child = new Product("#123", "Child Product", "child123.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Product product = new Product("#001", "Simple Product", "simpleproduct.glb", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() }, new List<Product>() { child });

            Action addDuplicateProductAction = () => product.addComplementaryProduct(child);
            Assert.Throws<ArgumentException>(addDuplicateProductAction);
        }

        [Fact]
        public void ensureAddingDuplicatedComplementaryProductDoesNotAddProduct() {
            Product child = new Product("#123", "Child Product", "child123.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Product product = new Product("#001", "Simple Product", "simpleproduct.glb", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() }, new List<Product>() { child });

            try {
                product.addComplementaryProduct(child);
            } catch (ArgumentException) { }

            Assert.Single(product.components);
        }

        [Fact]
        public void ensureAddingValidComplementaryProductDoesNotThrowException() {
            Product product = buildValidSimpleProduct();
            Product child = new Product("#123", "Child Product", "child123.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Action addValidComplementaryProductAction = () => product.addComplementaryProduct(child);
            Exception exception = Record.Exception(addValidComplementaryProductAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddingValidComplementaryProductAddsComplementaryProduct() {
            Product product = buildValidSimpleProduct();
            Product child = new Product("#123", "Child Product", "child123.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });
            product.addComplementaryProduct(child);

            Assert.Single(product.components);
            Assert.True(product.containsComplementaryProduct(child));
        }

        [Fact]
        public void ensureAddingNullMandatoryComplementaryProductToProductThrowsException() {
            Product product = buildValidSimpleProduct();
            //Since we added a null complementary product then an ArgumentNullException
            Action addNullComplementAction = () => product.addMandatoryComplementaryProduct(null);
            Assert.Throws<ArgumentNullException>(addNullComplementAction);
        }

        [Fact]
        public void ensureAddingNullMandatoryComplementaryProductToProductDoesNotAddComplementaryProduct() {
            Product product = buildValidSimpleProduct();
            Product nullComplementaryProduct = null;

            try {
                product.addMandatoryComplementaryProduct(nullComplementaryProduct);
            } catch (ArgumentNullException) { }

            Assert.False(product.containsComplementaryProduct(nullComplementaryProduct));
            Assert.Empty(product.components);
        }

        [Fact]
        public void ensureAddingMandatoryComplementaryProductEqualToProductThrowsException() {
            Product product = buildValidSimpleProduct();
            Action addRecursiveProductAction = () => product.addMandatoryComplementaryProduct(product);
            Assert.Throws<ArgumentException>(addRecursiveProductAction);
        }

        [Fact]
        public void ensureAddingMandatoryComplementaryProductEqualToProductDoesNotAddProduct() {
            Product product = buildValidSimpleProduct();
            try {
                product.addMandatoryComplementaryProduct(product);
            } catch (ArgumentException) { }

            Assert.False(product.containsComplementaryProduct(product));
            Assert.Empty(product.components);
        }

        [Fact]
        public void ensureAddingDuplicatedMandatoryComplementaryProductThrowsException() {
            Product child = new Product("#123", "Child Product", "child123.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Product product = new Product("#001", "Simple Product", "simpleproduct.glb", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() }, new List<Product>() { child });

            Action addDuplicateProductAction = () => product.addMandatoryComplementaryProduct(child);
            Assert.Throws<ArgumentException>(addDuplicateProductAction);
        }

        [Fact]
        public void ensureAddingDuplicatedMandatoryComplementaryProductDoesNotAddProduct() {
            Product child = new Product("#123", "Child Product", "child123.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Product product = new Product("#001", "Simple Product", "simpleproduct.glb", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() }, new List<Product>() { child });

            try {
                product.addMandatoryComplementaryProduct(child);
            } catch (ArgumentException) { }

            Assert.Single(product.components);
        }

        [Fact]
        public void ensureAddingValidMandatoryComplementaryProductDoesNotThrowException() {
            Product product = buildValidSimpleProduct();
            Product child = new Product("#123", "Child Product", "child123.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Action addValidComplementaryProductAction = () => product.addMandatoryComplementaryProduct(child);
            Exception exception = Record.Exception(addValidComplementaryProductAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddingValidMandatoryComplementaryProductAddsComplementaryProduct() {
            Product product = buildValidSimpleProduct();
            Product child = new Product("#123", "Child Product", "child123.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });
            product.addMandatoryComplementaryProduct(child);

            Assert.Single(product.components);
            Assert.True(product.containsComplementaryProduct(child));
        }

        [Fact]
        public void ensureAddingNullMeasurementToProductThrowsException() {
            Product product = buildValidSimpleProduct();
            Measurement nullMeasurement = null;
            Action addNullMeasurementAction = () => product.addMeasurement(nullMeasurement);
            Assert.Throws<ArgumentNullException>(addNullMeasurementAction);
        }

        [Fact]
        public void ensureAddingNullMeasurementToProductDoesNotAddMeasurement() {
            Product product = buildValidSimpleProduct();
            Measurement nullMeasurement = null;
            try {
                product.addMeasurement(nullMeasurement);
            } catch (ArgumentNullException) { }

            Assert.Single(product.productMeasurements);
            Assert.False(product.containsMeasurement(nullMeasurement));
        }

        [Fact]
        public void ensureAddingDuplicateMeasurementToProductThrowsException() {
            Product product = buildValidSimpleProduct();

            Measurement duplicateMeasurement = buildValidMeasurement();

            Action addDuplicateMeasurementAction = () => product.addMeasurement(duplicateMeasurement);
            Assert.Throws<ArgumentException>(addDuplicateMeasurementAction);
        }

        [Fact]
        public void ensureAddingDuplicateMeasurementToProductDoesNotAddMeasurement() {
            Product product = buildValidSimpleProduct();
            Measurement duplicateMeasurement = buildValidMeasurement();
            try {
                product.addMeasurement(duplicateMeasurement);
            } catch (ArgumentException) { }

            Assert.Single(product.productMeasurements);
        }

        [Fact]
        public void ensureAddingValidMeasurementToProductDoesNotThrowException() {
            Product product = buildValidSimpleProduct();

            Dimension heightDimension = new DiscreteDimensionInterval(new List<double>() { 40, 45, 50, 55, 60, 65 });
            Dimension widthDimension = new ContinuousDimensionInterval(70, 90, 2);
            Dimension depthDimension = new SingleValueDimension(30);

            Measurement validMeasurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Action addValidMeasurementAction = () => product.addMeasurement(validMeasurement);
            Exception exception = Record.Exception(addValidMeasurementAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddingValidMeasurementToProductAddsMeasurement() {
            Product product = buildValidSimpleProduct();

            Dimension heightDimension = new DiscreteDimensionInterval(new List<double>() { 40, 45, 50, 55, 60, 65 });
            Dimension widthDimension = new ContinuousDimensionInterval(70, 90, 2);
            Dimension depthDimension = new SingleValueDimension(30);

            Measurement validMeasurement = new Measurement(heightDimension, widthDimension, depthDimension);
            product.addMeasurement(validMeasurement);

            Assert.Equal(2, product.productMeasurements.Count);
            Assert.True(product.containsMeasurement(validMeasurement));
        }

        [Fact]
        public void ensureAddingRestrictionToNullComplementaryThrowsException() {
            Product product = buildValidSimpleProduct();

            Product nullComplementaryProduct = null;
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action addRestrictionToNullProductAction = () => product.addComplementaryProductRestriction(nullComplementaryProduct, restriction);
            Assert.Throws<ArgumentNullException>(addRestrictionToNullProductAction);
        }

        [Fact]
        public void ensureAddingNullRestrictionToComplementaryProductThrowsException() {

            Product product = buildValidSimpleProduct();

            Product complementaryProduct = new Product("#172", "Complementary Product", "complementaryproduct.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });
            Restriction restriction = null;

            product.addComplementaryProduct(complementaryProduct);

            Action addNullRestrictionToProductAction = () => product.addComplementaryProductRestriction(product, restriction);

            Assert.Throws<ArgumentNullException>(addNullRestrictionToProductAction);
        }

        [Fact]
        public void ensureAddingRestrictionToNotAddedComplementaryProductThrowsException() {
            Product product = buildValidSimpleProduct();

            Product complementaryProduct = new Product("#172", "Complementary Product", "complementaryproduct.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });
            Restriction restriction = new Restriction("I'm restricting things, here!", new SameMaterialAndFinishAlgorithm());

            Action addNullRestrictionToProductAction = () => product.addComplementaryProductRestriction(product, restriction);

            Assert.Throws<ArgumentException>(addNullRestrictionToProductAction);
        }

        [Fact]
        public void ensureAddingRestrictionToValidComplementaryProductDoesNotThrowException() {
            Product product = buildValidSimpleProduct();

            Product complementaryProduct = new Product("#172", "Complementary Product", "complementaryproduct.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });
            Restriction restriction = new Restriction("I'm restricting things, here!", new SameMaterialAndFinishAlgorithm());

            product.addComplementaryProduct(complementaryProduct);

            Action addNullRestrictionToProductAction = () => product.addComplementaryProductRestriction(complementaryProduct, restriction);

            Exception exception = Record.Exception(addNullRestrictionToProductAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddingRestrictionToNullMeasurementThrowsException() {
            Product product = buildValidSimpleProduct();

            Measurement measurement = null;
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action addRestrictionToNullMeasurement = () => product.addMeasurementRestriction(measurement, restriction);

            Assert.Throws<ArgumentNullException>(addRestrictionToNullMeasurement);
        }

        [Fact]
        public void ensureAddingNullRestrictionToMeasurementThrowsException() {
            Product product = buildValidSimpleProduct();

            Measurement measurement = buildValidMeasurement();
            Restriction restriction = null;

            Action addNullRestrictionToMeasurementAction = () => product.addMeasurementRestriction(measurement, restriction);

            Assert.Throws<ArgumentNullException>(addNullRestrictionToMeasurementAction);
        }

        [Fact]
        public void ensureAddingRestrictionToNotAddedMeasurementThrowsException() {
            Product product = buildValidSimpleProduct();

            Dimension heightDimension = new ContinuousDimensionInterval(50, 60, 2);
            Dimension widthDimension = new SingleValueDimension(120);
            Dimension depthDimension = new DiscreteDimensionInterval(new List<double>() { 20, 30, 35 });

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action addRestrictionToNotAddedMeasurementAction = () => product.addMeasurementRestriction(measurement, restriction);

            Assert.Throws<ArgumentException>(addRestrictionToNotAddedMeasurementAction);
        }

        [Fact]
        public void ensureAddingRestrictionToValidMeasurementDoesNotThrowException() {
            Product product = buildValidSimpleProduct();

            Measurement measurement = buildValidMeasurement();
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action addRestrictionToMeasurementAction = () => product.addMeasurementRestriction(measurement, restriction);
            Exception exception = Record.Exception(addRestrictionToMeasurementAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddingRestrictionToNullMaterialThrowsException() {
            Product product = buildValidSimpleProduct();

            Material material = null;
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action addRestrictionToNullMaterialAction = () => product.addMaterialRestriction(material, restriction);

            Assert.Throws<ArgumentNullException>(addRestrictionToNullMaterialAction);
        }

        [Fact]
        public void ensureAddingNullRestrictionToMaterialThrowsException() {
            Product product = buildValidSimpleProduct();

            Material material = buildValidMaterial();
            Restriction restriction = null;

            Action addNullRestrictionToMaterialAction = () => product.addMaterialRestriction(material, restriction);

            Assert.Throws<ArgumentNullException>(addNullRestrictionToMaterialAction);
        }

        [Fact]
        public void ensureAddingRestrictionToNotAddedMaterialThrowsException() {
            Product product = buildValidSimpleProduct();

            Finish finish = Finish.valueOf("Clear Coating", 4);
            Color color = Color.valueOf("Really Really Red", 255, 0, 0, 0);
            Material material = new Material("#154", "Stained Wood", "stainedwood.jpg", new List<Color>() { color }, new List<Finish>() { finish });

            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action addRestrictionToNotAddedMaterial = () => product.addMaterialRestriction(material, restriction);
            Assert.Throws<ArgumentException>(addRestrictionToNotAddedMaterial);
        }

        [Fact]
        public void ensureAddingRestrictionToValidMaterialDoesNotThrowException() {
            Product product = buildValidSimpleProduct();

            Material material = buildValidMaterial();
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action addRestrictionToMaterialAction = () => product.addMaterialRestriction(material, restriction);

            Exception exception = Record.Exception(addRestrictionToMaterialAction);
            Assert.Null(exception);
        }


        [Fact]
        public void ensureChangingToNullReferenceThrowsException() {
            Product product = buildValidSimpleProduct();

            Action changeToNullReferenceAction = () => product.changeProductReference(null);

            Assert.Throws<ArgumentException>(changeToNullReferenceAction);
        }

        [Fact]
        public void ensureChangingToNullReferenceDoesNotChangeReference() {
            Product product = buildValidSimpleProduct();
            try {
                product.changeProductReference(null);
            } catch (ArgumentException) { }

            Assert.NotNull(product.reference);
        }

        [Fact]
        public void ensureChangingToEmptyReferenceThrowsException() {
            Product product = buildValidSimpleProduct();

            Action changeToEmptyReferenceAction = () => product.changeProductReference("");

            Assert.Throws<ArgumentException>(changeToEmptyReferenceAction);
        }

        [Fact]
        public void ensureChangingToEmptyReferenceDoesNotChangeReference() {
            Product product = buildValidSimpleProduct();
            try {
                product.changeProductReference(null);
            } catch (ArgumentException) { }

            Assert.NotEqual("", product.reference);
        }

        [Fact]
        public void ensureChangingToSameReferenceThrowsException() {
            Product product = buildValidSimpleProduct();

            Action changeToSameReferenceAction = () => product.changeProductReference(product.reference);

            Assert.Throws<ArgumentException>(changeToSameReferenceAction);
        }

        [Fact]
        public void ensureChangingToValidReferenceDoesNotThrowException() {
            Product product = buildValidSimpleProduct();
            string newReference = "This is a brand new reference!";

            Action changeToValidReferenceAction = () => product.changeProductReference(newReference);
            Exception exception = Record.Exception(changeToValidReferenceAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureChangingToValidReferenceChangesReference() {
            Product product = buildValidSimpleProduct();
            string newReference = "This is a brand new reference!";

            product.changeProductReference(newReference);
            Assert.Equal(newReference, product.reference);
        }

        [Fact]
        public void ensureChangingToNullDesignationThrowsException() {
            Product product = buildValidSimpleProduct();

            Action changeToNullDesignationAction = () => product.changeProductDesignation(null);

            Assert.Throws<ArgumentException>(changeToNullDesignationAction);
        }

        [Fact]
        public void ensureChangingToNullDesignationDoesNotChangeDesignation() {
            Product product = buildValidSimpleProduct();
            try {
                product.changeProductDesignation(null);
            } catch (ArgumentException) { }

            Assert.NotNull(product.designation);
        }

        [Fact]
        public void ensureChangingToEmptyDesignationThrowsException() {
            Product product = buildValidSimpleProduct();

            Action changeToEmptyDesignationAction = () => product.changeProductDesignation("");

            Assert.Throws<ArgumentException>(changeToEmptyDesignationAction);
        }

        [Fact]
        public void ensureChangingToEmptyDesignationDoesNotChangeDesignation() {
            Product product = buildValidSimpleProduct();
            try {
                product.changeProductDesignation("");
            } catch (ArgumentException) { }

            Assert.NotEqual("", product.designation);
        }

        [Fact]
        public void ensureChangingToSameDesignationThrowsException() {
            Product product = buildValidSimpleProduct();

            Action changeToSameDesignationAction = () => product.changeProductDesignation(product.designation);
            Assert.Throws<ArgumentException>(changeToSameDesignationAction);
        }

        [Fact]
        public void ensureChangingToValidDesignationDoesNotThrowException() {
            Product product = buildValidSimpleProduct();
            string newDesignation = "This is an updated designation";

            Action changeToValidDesignationAction = () => product.changeProductDesignation(newDesignation);
            Exception exception = Record.Exception(changeToValidDesignationAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureChangingToValidDesignationChangesDesignation() {
            Product product = buildValidSimpleProduct();
            string newDesignation = "This is an updated designation";

            product.changeProductDesignation(newDesignation);

            Assert.Equal(newDesignation, product.designation);
        }

        [Fact]
        public void ensureChangingToInvalidModelFilenameThrowsException() {
            Product product = buildValidSimpleProduct();

            string modelFilename = "newmodel.xpto";
            Action changeToInvalidFilenameAction = () => product.changeModelFilename(modelFilename);

            Assert.Throws<ArgumentException>(changeToInvalidFilenameAction);
        }

        [Fact]
        public void ensureChangingToValidModelFilenameDoesNotThrowException() {
            Product product = buildValidSimpleProduct();

            string modelFilename = "updatedmodelfile.glb";
            Action changeToValidFilenameAction = () => product.changeModelFilename(modelFilename);

            Exception exception = Record.Exception(changeToValidFilenameAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureChangingToNullCategoryThrowsException() {
            Product product = buildValidSimpleProduct();

            Action changeToNullCategoryAction = () => product.changeProductCategory(null);

            Assert.Throws<ArgumentNullException>(changeToNullCategoryAction);
        }

        [Fact]
        public void ensureChangingToNullCategoryDoesNotChangeCategory() {
            Product product = buildValidSimpleProduct();

            try {
                product.changeProductCategory(null);
            } catch (ArgumentNullException) { }

            Assert.NotNull(product.productCategory);
        }

        [Fact]
        public void ensureChangingToSameCategoryThrowsException() {
            Product product = buildValidSimpleProduct();
            ProductCategory category = buildValidCategory();

            Action changeToSameCategoryAction = () => product.changeProductCategory(category);

            Assert.Throws<ArgumentException>(changeToSameCategoryAction);
        }

        [Fact]
        public void ensureChangingToValidCategoryDoesNotThrowException() {
            Product product = buildValidSimpleProduct();
            ProductCategory category = new ProductCategory("This is not the same category as before");

            Action changeToValidCategoryAction = () => product.changeProductCategory(category);

            Exception exception = Record.Exception(changeToValidCategoryAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureChangingToValidCategoryChangesCategory() {
            Product product = buildValidSimpleProduct();
            ProductCategory category = new ProductCategory("This is not the same category as before");

            product.changeProductCategory(category);

            Assert.Equal(category, product.productCategory);
        }

        [Fact]
        public void ensureRemovingTheLastMeasurementThrowsException() {
            Product product = buildValidSimpleProduct();
            Measurement measurementToRemove = product.productMeasurements[0].measurement;

            Action removeLastMeasurementAction = () => product.removeMeasurement(measurementToRemove);
            Assert.Throws<InvalidOperationException>(removeLastMeasurementAction);
        }

        [Fact]
        public void ensureRemovingTheLastMeasurementDoesNotRemoveLastMeasurement() {
            Product product = buildValidSimpleProduct();
            Measurement measurementToRemove = product.productMeasurements[0].measurement;

            try {
                product.removeMeasurement(measurementToRemove);
            } catch (InvalidOperationException) { }

            Assert.NotEmpty(product.productMeasurements);
        }

        [Fact]
        public void ensureRemovingNullMeasurementThrowsException() {
            Product product = buildValidSimpleProduct();

            Action removeNullMeasurementAction = () => product.removeMeasurement(null);

            Assert.Throws<ArgumentException>(removeNullMeasurementAction);
        }

        [Fact]
        public void ensureRemovingNotAddedMeasurementThrowsException() {
            Product product = buildValidSimpleProduct();

            Dimension heightDimension = new DiscreteDimensionInterval(new List<double>() { 40, 45, 50, 55, 60, 65 });
            Dimension widthDimension = new ContinuousDimensionInterval(70, 90, 2);
            Dimension depthDimension = new SingleValueDimension(30);

            Measurement foreignMeasurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Action removeForeignMeasurementAction = () => product.removeMeasurement(foreignMeasurement);
            Assert.Throws<ArgumentException>(removeForeignMeasurementAction);
        }

        [Fact]
        public void ensureRemovingValidMeasurementDoesNotThrowException() {
            Dimension heightDimension = new DiscreteDimensionInterval(new List<double>() { 40, 45, 50, 55, 60, 65 });
            Dimension widthDimension = new ContinuousDimensionInterval(70, 90, 2);
            Dimension depthDimension = new SingleValueDimension(30);

            Measurement measurement1 = new Measurement(heightDimension, widthDimension, depthDimension);
            Measurement measurement2 = buildValidMeasurement();

            List<Measurement> measurements = new List<Measurement>() { measurement1, measurement2 };

            Product product = new Product("#123", "Modern Closet", "closet123.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, measurements);

            Action removeValidMeasurementAction = () => product.removeMeasurement(measurement1);

            Exception exception = Record.Exception(removeValidMeasurementAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureRemovingValidMeasurementRemovesMeasurement() {
            Dimension heightDimension = new DiscreteDimensionInterval(new List<double>() { 40, 45, 50, 55, 60, 65 });
            Dimension widthDimension = new ContinuousDimensionInterval(70, 90, 2);
            Dimension depthDimension = new SingleValueDimension(30);

            Measurement measurement1 = new Measurement(heightDimension, widthDimension, depthDimension);
            Measurement measurement2 = buildValidMeasurement();

            List<Measurement> measurements = new List<Measurement>() { measurement1, measurement2 };

            Product product = new Product("#123", "Modern Closet", "closet123.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, measurements);

            product.removeMeasurement(measurement1);

            Assert.Single(product.productMeasurements);
        }


        [Fact]
        public void ensureRemovingLastMaterialThrowsException() {
            Product product = buildValidSimpleProduct();
            Material lastMaterial = product.productMaterials[0].material;

            Action removeLastMaterialAction = () => product.removeMaterial(lastMaterial);
            Assert.Throws<InvalidOperationException>(removeLastMaterialAction);
        }

        [Fact]
        public void ensureRemovingLastMaterialDoesNotRemoveMaterial() {
            Product product = buildValidSimpleProduct();
            Material lastMaterial = product.productMaterials[0].material;

            try {
                product.removeMaterial(lastMaterial);
            } catch (InvalidOperationException) { }

            Assert.True(product.containsMaterial(lastMaterial));
            Assert.Single(product.productMaterials);
        }

        [Fact]
        public void ensureRemovingNullMaterialThrowsException() {
            Product product = buildValidSimpleProduct();

            Action removeNullMaterialAction = () => product.removeMaterial(null);
            Assert.Throws<ArgumentException>(removeNullMaterialAction);
        }

        [Fact]
        public void ensureRemovingNotAddedMaterialThrowsException() {
            Product product = buildValidSimpleProduct();

            Color red = Color.valueOf("Red", 255, 0, 0, 0);
            Finish matte = Finish.valueOf("Matte", 0);
            Material foreignMaterial = new Material("#002", "Different Material", "ola.jpg", new List<Color>() { red }, new List<Finish>() { matte });

            Action removeForeignMaterialAction = () => product.removeMaterial(foreignMaterial);
            Assert.Throws<ArgumentException>(removeForeignMaterialAction);
        }

        [Fact]
        public void ensureRemovingValidMaterialDoesNotThrowException() {
            Color red = Color.valueOf("Red", 255, 0, 0, 0);
            Finish varnish = Finish.valueOf("Varnish", 50);
            Material material1 = new Material("#002", "Different Material", "ola.jpg", new List<Color>() { red }, new List<Finish>() { varnish });

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };
            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };
            Material material2 = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);

            Product product = new Product("#123", "Amazing Product", "amazing123.glb", buildValidCategory(),
                new List<Material>() { material1, material2 }, new List<Measurement>() { buildValidMeasurement() });

            Action removeValidMaterialAction = () => product.removeMaterial(material2);
            Exception exception = Record.Exception(removeValidMaterialAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureRemovingValidMaterialRemovesMaterial() {
            Color red = Color.valueOf("Red", 255, 0, 0, 0);
            Finish varnish = Finish.valueOf("Varnish", 50);
            Material material1 = new Material("#002", "Different Material", "ola.jpg", new List<Color>() { red }, new List<Finish>() { varnish });

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };
            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };
            Material material2 = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);

            Product product = new Product("#123", "Amazing Product", "amazing123.glb", buildValidCategory(),
                new List<Material>() { material1, material2 }, new List<Measurement>() { buildValidMeasurement() });

            product.removeMaterial(material1);
            Assert.False(product.containsMaterial(material1));
            Assert.Single(product.productMaterials);
        }

        [Fact]
        public void ensureRemovingNullComplementaryProductThrowsException() {
            Product product = buildValidSimpleProduct();

            Action removeNullProductAction = () => product.removeComplementaryProduct(null);
            Assert.Throws<ArgumentException>(removeNullProductAction);
        }

        [Fact]
        public void ensureRemovingNotAddedComplementaryProductThrowsException() {
            Product child = buildValidSimpleProduct();
            //the product does not own the child
            Product product = new Product("#003", "Super Stylish Product", "stylish003.glb", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Action removeForeignProductAction = () => product.removeComplementaryProduct(child);
            Assert.Throws<ArgumentException>(removeForeignProductAction);
        }

        [Fact]
        public void ensureRemovingValidComplementaryProductDoesNotThrowException() {
            Product child = buildValidSimpleProduct();
            Product product = new Product("#003", "Super Stylish Product", "stylish003.glb", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() }, new List<Product>() { child });

            Action removeValidProductAction = () => product.removeComplementaryProduct(child);
            Exception exception = Record.Exception(removeValidProductAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureRemovingValidComplementaryProductRemovesComplementaryProduct() {
            Product child = buildValidSimpleProduct();
            Product product = new Product("#003", "Super Stylish Product", "stylish003.glb", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() }, new List<Product>() { child });

            product.removeComplementaryProduct(child);
            Assert.False(product.containsComplementaryProduct(child));
            Assert.Empty(product.components);
        }

        [Fact]
        public void ensureRemovingRestrictionFromNullMeasurementThrowsException() {
            Product product = buildValidSimpleProduct();

            Measurement measurement = null;
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action removeRestrictionFromNullMeasurementAction = () => product.removeMeasurementRestriction(measurement, restriction);

            Assert.Throws<ArgumentNullException>(removeRestrictionFromNullMeasurementAction);
        }

        [Fact]
        public void ensureRemovingNullRestrictionFromMeasurementThrowsException() {
            Product product = buildValidSimpleProduct();

            Measurement measurement = buildValidMeasurement();
            Restriction restriction = null;

            Action removeNullRestrictionFromMeasurementAction = () => product.removeMeasurementRestriction(measurement, restriction);

            Assert.Throws<ArgumentNullException>(removeNullRestrictionFromMeasurementAction);
        }

        [Fact]
        public void ensureRemovingRestrictionFromNotAddedMeasurementThrowsException() {
            Product product = buildValidSimpleProduct();

            Dimension heightDimension = new ContinuousDimensionInterval(50, 60, 2);
            Dimension widthDimension = new SingleValueDimension(120);
            Dimension depthDimension = new DiscreteDimensionInterval(new List<double>() { 20, 30, 35 });

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action removeRestrictionFromNotAddedMeasurementAction = () => product.removeMeasurementRestriction(measurement, restriction);

            Assert.Throws<ArgumentException>(removeRestrictionFromNotAddedMeasurementAction);
        }

        [Fact]
        public void ensureRemoveNotAddedMeasurementRestrictionThrowsException() {
            Product product = buildValidSimpleProduct();

            Measurement measurement = buildValidMeasurement();
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action removeRestrictionFromValidMeasurementAction = () => product.removeMeasurementRestriction(measurement, restriction);

            Assert.Throws<ArgumentException>(removeRestrictionFromValidMeasurementAction);
        }

        [Fact]
        public void ensureRemoveMeasurementRestrictionDoesNotThrowException() {
            Product product = buildValidSimpleProduct();

            Measurement measurement = buildValidMeasurement();
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            product.addMeasurementRestriction(measurement, restriction);

            Action removeRestrictionFromValidMeasurementAction = () => product.removeMeasurementRestriction(measurement, restriction);

            Exception exception = Record.Exception(removeRestrictionFromValidMeasurementAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureRemovingRestrictionFromNullMaterialThrowsException() {
            Product product = buildValidSimpleProduct();

            Material material = null;
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action removeRestrictionFromNullMaterialAction = () => product.removeMaterialRestriction(material, restriction);

            Assert.Throws<ArgumentNullException>(removeRestrictionFromNullMaterialAction);
        }

        [Fact]
        public void ensureRemovingNullRestrictionFromMaterialRestriction() {
            Product product = buildValidSimpleProduct();

            Material material = buildValidMaterial();
            Restriction restriction = null;

            Action removeNullRestrictionFromMaterialAction = () => product.removeMaterialRestriction(material, restriction);

            Assert.Throws<ArgumentNullException>(removeNullRestrictionFromMaterialAction);
        }

        [Fact]
        public void ensureRemovingRestrictionFromNotAddedMaterialThrowsException() {
            Product product = buildValidSimpleProduct();

            Finish finish = Finish.valueOf("Clear Coating", 5);
            Color color = Color.valueOf("Really Really Red", 255, 0, 0, 0);
            Material material = new Material("#154", "Stained Wood", "stainedwood.jpg", new List<Color>() { color }, new List<Finish>() { finish });

            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action removeRestrictionFromNotAddedMaterialAction = () => product.removeMaterialRestriction(material, restriction);

            Assert.Throws<ArgumentException>(removeRestrictionFromNotAddedMaterialAction);
        }

        [Fact]
        public void ensureRemovingNotAddedMaterialRestrictionThrowsException() {
            Product product = buildValidSimpleProduct();

            Material material = buildValidMaterial();
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action removeNotAddedRestrictionAction = () => product.removeMaterialRestriction(material, restriction);

            Assert.Throws<ArgumentException>(removeNotAddedRestrictionAction);
        }

        [Fact]
        public void ensureRemoveMaterialRestrictionDoesNotThrowException() {
            Product product = buildValidSimpleProduct();

            Material material = buildValidMaterial();
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            product.addMaterialRestriction(material, restriction);

            Action removeMaterialRestrictionAction = () => product.removeMaterialRestriction(material, restriction);

            Exception exception = Record.Exception(removeMaterialRestrictionAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureRemovingRestrictionFromNullComplementaryProductThrowsException() {
            Product product = buildValidSimpleProduct();

            Product complementaryProduct = null;
            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action removeRestrictionFromNullComplementaryProductAction = () => product.removeComplementaryProductRestriction(complementaryProduct, restriction);

            Assert.Throws<ArgumentNullException>(removeRestrictionFromNullComplementaryProductAction);
        }

        [Fact]
        public void ensureRemovingNullRestrictionFromComplementaryProductThrowsException() {
            Product product = buildValidSimpleProduct();

            Product complementaryProduct = new Product("#172", "Complementary Product", "complementaryproduct.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() }); ;

            product.addComplementaryProduct(complementaryProduct);

            Restriction restriction = null;

            Action removeNullRestrictionFromComplementaryProductAction = () => product.removeComplementaryProductRestriction(complementaryProduct, restriction);

            Assert.Throws<ArgumentNullException>(removeNullRestrictionFromComplementaryProductAction);
        }

        [Fact]
        public void ensureRemovingRestrictionFromNotAddedComplementaryProductThrowsException() {
            Product product = buildValidSimpleProduct();

            Product complementaryProduct = new Product("#172", "Complementary Product", "complementaryproduct.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() }); ;

            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action removeRestrictionFromNotAddedComplementaryProductAction = () => product.removeComplementaryProductRestriction(complementaryProduct, restriction);

            Assert.Throws<ArgumentException>(removeRestrictionFromNotAddedComplementaryProductAction);
        }

        [Fact]
        public void ensureRemovingNotAddedComplementaryProductRestrictionThrowsException() {
            Product product = buildValidSimpleProduct();

            Product complementaryProduct = new Product("#172", "Complementary Product", "complementaryproduct.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() }); ;

            product.addComplementaryProduct(complementaryProduct);

            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            Action removeNotAddedRestrictionAction = () => product.removeComplementaryProductRestriction(complementaryProduct, restriction);

            Assert.Throws<ArgumentException>(removeNotAddedRestrictionAction);
        }

        [Fact]
        public void ensureRemovingComplementaryProductRestrictionDoesNotThrowException() {
            Product product = buildValidSimpleProduct();

            Product complementaryProduct = new Product("#172", "Complementary Product", "complementaryproduct.obj", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() }); ;

            product.addComplementaryProduct(complementaryProduct);

            Restriction restriction = new Restriction("This is a restriction", new SameMaterialAndFinishAlgorithm());

            product.addComplementaryProductRestriction(complementaryProduct, restriction);

            Action removeComplementaryRestrictionAction = () => product.removeComplementaryProductRestriction(complementaryProduct, restriction);

            Exception exception = Record.Exception(removeComplementaryRestrictionAction);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureContainsMaterialReturnsFalseIfMaterialIsNull() {
            Product product = buildValidSimpleProduct();
            Assert.False(product.containsMaterial(null));
        }

        [Fact]
        public void ensureContainsMaterialReturnsFalseIfMaterialWasNotAdded() {
            Product product = buildValidSimpleProduct();

            Color red = Color.valueOf("Red", 255, 0, 0, 0);
            Finish varnish = Finish.valueOf("Varnish", 50);
            Material material1 = new Material("#002", "Different Material", "ola.jpg", new List<Color>() { red }, new List<Finish>() { varnish });

            Assert.False(product.containsMaterial(material1));
        }

        [Fact]
        public void ensureContainsMaterialReturnsTrueIfMaterialWasAdded() {
            Product product = buildValidSimpleProduct();
            Material material = buildValidMaterial();

            Assert.True(product.containsMaterial(material));
        }

        [Fact]
        public void ensureContainsComplementaryProductReturnsFalseIfComplementaryProductIsNull() {
            Product product = buildValidSimpleProduct();

            Assert.False(product.containsComplementaryProduct(null));
        }

        [Fact]
        public void ensureContainsComplementaryProductReturnsFalseIfComplementaryProductWasNotAdded() {
            Product child = buildValidSimpleProduct();

            Product product = new Product("#003", "Super Stylish Product", "stylish003.glb", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Assert.False(product.containsComplementaryProduct(child));
        }

        [Fact]
        public void ensureContainsComplementaryProductReturnsTrueIfComplementaryProductWasAdded() {
            Product child = buildValidSimpleProduct();

            Product product = new Product("#003", "Super Stylish Product", "stylish003.glb", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() }, new List<Product>() { child });

            Assert.True(product.containsComplementaryProduct(child));
        }

        [Fact]
        public void ensureContainsMeasurementReturnsFalseIfMeasurementIsNull() {
            Product product = buildValidSimpleProduct();

            Assert.False(product.containsMeasurement(null));
        }

        [Fact]
        public void ensureContainsMeasurementReturnsFalseIfMeasurementWasNotAdded() {
            Product product = buildValidSimpleProduct();

            Dimension heightDimension = new DiscreteDimensionInterval(new List<double>() { 40, 45, 50, 55, 60, 65 });
            Dimension widthDimension = new ContinuousDimensionInterval(70, 90, 2);
            Dimension depthDimension = new SingleValueDimension(30);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Assert.False(product.containsMeasurement(measurement));
        }

        [Fact]
        public void ensureContaintsMeasurementReturnsTrueIfMeasurementWasAdded() {
            Product product = buildValidSimpleProduct();

            Dimension heightDimension = new DiscreteDimensionInterval(new List<double>() { 40, 45, 50, 55, 60, 65 });
            Dimension widthDimension = new ContinuousDimensionInterval(70, 90, 2);
            Dimension depthDimension = new SingleValueDimension(30);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            product.addMeasurement(measurement);

            Assert.True(product.containsMeasurement(measurement));
        }

        /// <summary>
        /// Ensures that a product identifier is the same as its created one
        /// </summary>
        [Fact]
        public void ensureProductIdentifierIsTheSame() {
            string id = "#666";
            Product product = new Product(id, "Shelf", "shelf666.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });
            //Since the product was created with the id "#666" then its id should be "#666"
            Assert.Equal(product.id(), id);
        }

        /// <summary>
        /// Ensures that a product identifier is the same as its created one
        /// </summary>
        [Fact]
        public void ensureProductIdentierSameAs() {
            string id = "#666";
            Product product = new Product(id, "Shelf", "shelf666.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });
            //Since the product was created with the id "#666" then its id should be "#666"
            Assert.True(product.sameAs(id));
        }


        [Fact]
        public void ensureSameInstanceIsEqual() {
            Product product = buildValidSimpleProduct();
            Assert.True(product.Equals(product));
        }

        [Fact]
        public void ensureNullObjectIsNotEqual() {
            Product product = buildValidSimpleProduct();
            Assert.False(product.Equals(null));
        }

        [Fact]
        public void ensureDifferentTypeObjectIsNotEqual() {
            Product product = buildValidSimpleProduct();
            Assert.False(product.Equals("product"));
        }

        [Fact]
        public void ensureDifferentReferenceInstanceIsNotEqual() {
            Product product = buildValidSimpleProduct();
            Product otherProduct = new Product("#725", "This is another product", "anotherproduct.glb", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Assert.NotEqual(product, otherProduct);
        }

        [Fact]
        public void ensureSameReferenceInstanceIsEqual() {
            Product product = buildValidSimpleProduct();
            Product otherProduct = new Product("#001", "This is another product", "anotherproduct.glb", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Assert.Equal(product, otherProduct);
        }

        [Fact]
        public void ensureDifferentReferenceProducesDifferentHashCode() {
            Product product = buildValidSimpleProduct();
            Product otherProduct = new Product("#725", "This is another product", "anotherproduct.glb", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Assert.NotEqual(product.GetHashCode(), otherProduct.GetHashCode());
        }

        [Fact]
        public void ensureSameReferenceProducesSameHashCode() {
            Product product = buildValidSimpleProduct();
            Product otherProduct = new Product("#001", "This is another product", "anotherproduct.glb", buildValidCategory(),
                new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() });

            Assert.Equal(product.GetHashCode(), otherProduct.GetHashCode());
        }

        /// <summary>
        /// Ensures that the textual description of two equal products is the same
        /// </summary>
        [Fact]
        public void ensureToStringWorks() {
            string id = "Test";
            Assert.Equal(new Product(id, "Shelf", "shelf.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() },
            ProductSlotWidths.valueOf(4, 4, 4)).ToString(),
            new Product(id, "Shelf", "shelf.glb", buildValidCategory(), new List<Material>() { buildValidMaterial() }, new List<Measurement>() { buildValidMeasurement() },
            ProductSlotWidths.valueOf(4, 4, 4)).ToString());
        }

        [Fact]
        public void ensureGetRestrictedComponentsReturnsEmptyListIfArgumentIsNull() {
            ProductCategory cat = new ProductCategory("All Products");

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            Material material = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);
            Material material2 = new Material("#002", "Expensive Wood", "ola.jpg", colors, finishes);

            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Product product = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            Product product2 = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));

            CustomizedDimensions customDimension = CustomizedDimensions.valueOf(50, 80, 25);
            CustomizedMaterial customMaterial = CustomizedMaterial.valueOf(material, white, matte);
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", product, customDimension).build();

            customizedProduct.changeCustomizedMaterial(customMaterial);
            customizedProduct.finalizeCustomization();

            Assert.Empty(product.getRestrictedComponents(null));
        }
        [Fact]
        public void ensureGetRestrictedComponentsAppliesProductMeasurementRestriction() {
            ProductCategory cat = new ProductCategory("All Products");

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            Material material = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);
            Material material2 = new Material("#002", "Expensive Wood", "ola.jpg", colors, finishes);

            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Product product = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            Product product2 = new Product("Test2", "Shelf2", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));

            product.addComplementaryProduct(product2);

            CustomizedDimensions customDimension = CustomizedDimensions.valueOf(50, 80, 25);
            CustomizedMaterial customMaterial = CustomizedMaterial.valueOf(material, white, matte);
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", product, customDimension).build();

            customizedProduct.changeCustomizedMaterial(customMaterial);

            customizedProduct.finalizeCustomization();
            product.productMeasurements[0].measurement.addRestriction(new Restriction("same material", new SameMaterialAndFinishAlgorithm()));
            List<Product> returnedList = (List<Product>)product.getRestrictedComponents(customizedProduct);
            Product returned = returnedList[0];
            Assert.True(returned.productMaterials.Count == 1);
            Assert.True(returned.productMaterials[0].material.Equals(material));
        }
        [Fact]
        public void ensureGetRestrictedComponentsAppliesProductMaterialRestriction() {
            ProductCategory cat = new ProductCategory("All Products");

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            Material material = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);
            Material material2 = new Material("#002", "Expensive Wood", "ola.jpg", colors, finishes);

            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Product product = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            Product product2 = new Product("Test2", "Shelf2", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));

            product.addComplementaryProduct(product2);

            CustomizedDimensions customDimension = CustomizedDimensions.valueOf(50, 80, 25);
            CustomizedMaterial customMaterial = CustomizedMaterial.valueOf(material, white, matte);
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", product, customDimension).build();

            customizedProduct.changeCustomizedMaterial(customMaterial);

            customizedProduct.finalizeCustomization();
            product.productMaterials[0].addRestriction(new Restriction("same material", new SameMaterialAndFinishAlgorithm()));
            List<Product> returnedList = (List<Product>)product.getRestrictedComponents(customizedProduct);
            Product returned = returnedList[0];
            Assert.True(returned.productMaterials.Count == 1);
            Assert.True(returned.productMaterials[0].material.Equals(material));
        }
        [Fact]
        public void ensureGetRestrictedComponentsAppliesComponentRestrictions() {
            ProductCategory cat = new ProductCategory("All Products");

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            Material material = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);
            Material material2 = new Material("#002", "Expensive Wood", "ola.jpg", colors, finishes);

            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Product product = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            Product product2 = new Product("Test2", "Shelf2", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));

            product.addComplementaryProduct(product2);

            CustomizedDimensions customDimension = CustomizedDimensions.valueOf(50, 80, 25);
            CustomizedMaterial customMaterial = CustomizedMaterial.valueOf(material, white, matte);
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", product, customDimension).build();

            customizedProduct.changeCustomizedMaterial(customMaterial);

            customizedProduct.finalizeCustomization();
            product.components[0].addRestriction(new Restriction("same material", new SameMaterialAndFinishAlgorithm()));
            List<Product> returnedList = (List<Product>)product.getRestrictedComponents(customizedProduct);
            Product returned = returnedList[0];
            Assert.True(returned.productMaterials.Count == 1);
            Assert.True(returned.productMaterials[0].material.Equals(material));
        }
        [Fact]
        public void ensureGetRestrictedComponentsDoesNotApplyRestrictionsOfDifferentComponents() {
            ProductCategory cat = new ProductCategory("All Products");

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            Material material = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);
            Material material2 = new Material("#002", "Expensive Wood", "ola.jpg", colors, finishes);

            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Product product = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            Product product2 = new Product("Test2", "Shelf2", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            Product product3 = new Product("Test3", "Shelf3", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));


            product.addComplementaryProduct(product2);
            product.addComplementaryProduct(product3);

            CustomizedDimensions customDimension = CustomizedDimensions.valueOf(50, 80, 25);
            CustomizedMaterial customMaterial = CustomizedMaterial.valueOf(material, white, matte);
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", product, customDimension).build();

            customizedProduct.changeCustomizedMaterial(customMaterial);

            customizedProduct.finalizeCustomization();
            product.components[0].addRestriction(new Restriction("same material", new SameMaterialAndFinishAlgorithm()));
            List<Product> returnedList = (List<Product>)product.getRestrictedComponents(customizedProduct);
            Product firstComponent = returnedList[0];
            Product secondComponent = returnedList[1];
            Assert.True(firstComponent.productMaterials.Count == 1);
            Assert.True(firstComponent.productMaterials[0].material.Equals(material));
            Assert.True(secondComponent.productMaterials.Count == 2);
        }
        [Fact]
        public void ensureGetRestrictedComponentsDoesNotReturnsProductsThatDontObeyMeasurementRestrictions() {
            ProductCategory cat = new ProductCategory("All Products");

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            Material material = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);
            Material material2 = new Material("#002", "Expensive Wood", "ola.jpg", colors, finishes);

            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Product product = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            Product product2 = new Product("Test2", "Shelf2", "shelf.glb", cat, new List<Material>() { material }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            product.addComplementaryProduct(product2);
            CustomizedDimensions customDimension = CustomizedDimensions.valueOf(50, 80, 25);
            CustomizedMaterial customMaterial = CustomizedMaterial.valueOf(material2, white, matte);
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", product, customDimension).build();

            customizedProduct.changeCustomizedMaterial(customMaterial);

            customizedProduct.finalizeCustomization();
            product.productMeasurements[0].measurement.addRestriction(new Restriction("same material", new SameMaterialAndFinishAlgorithm()));
            Assert.Empty(product.getRestrictedComponents(customizedProduct));
        }
        [Fact]
        public void ensureGetRestrictedComponentsDoesNotReturnsProductsThatDontObeyMaterialRestrictions() {
            ProductCategory cat = new ProductCategory("All Products");

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            Material material = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);
            Material material2 = new Material("#002", "Expensive Wood", "ola.jpg", colors, finishes);

            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Product product = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            Product product2 = new Product("Test2", "Shelf2", "shelf.glb", cat, new List<Material>() { material }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            product.addComplementaryProduct(product2);
            CustomizedDimensions customDimension = CustomizedDimensions.valueOf(50, 80, 25);
            CustomizedMaterial customMaterial = CustomizedMaterial.valueOf(material2, white, matte);
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", product, customDimension).build();

            customizedProduct.changeCustomizedMaterial(customMaterial);

            customizedProduct.finalizeCustomization();
            product.productMaterials[0].addRestriction(new Restriction("same material", new SameMaterialAndFinishAlgorithm()));
            Assert.Empty(product.getRestrictedComponents(customizedProduct));
        }
        [Fact]
        public void ensureGetRestrictedComponentsDoesNotReturnsProductsThatDontObeyComponentRestrictions() {
            ProductCategory cat = new ProductCategory("All Products");

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            Material material = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);
            Material material2 = new Material("#002", "Expensive Wood", "ola.jpg", colors, finishes);

            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Product product = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            Product product2 = new Product("Test2", "Shelf2", "shelf.glb", cat, new List<Material>() { material }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            product.addComplementaryProduct(product2);

            CustomizedDimensions customDimension = CustomizedDimensions.valueOf(50, 80, 25);
            CustomizedMaterial customMaterial = CustomizedMaterial.valueOf(material2, white, matte);
            CustomizedProduct customizedProduct = CustomizedProductBuilder.createCustomizedProduct("reference", product, customDimension).build();

            customizedProduct.changeCustomizedMaterial(customMaterial);

            customizedProduct.finalizeCustomization();
            product.components[0].addRestriction(new Restriction("same material", new SameMaterialAndFinishAlgorithm()));
            Assert.Empty(product.getRestrictedComponents(customizedProduct));
        }
        [Fact]
        public void ensureGetAllComponentsAsProductsReturnsEmptyIfProductDoesNotHaveComponents() {
            ProductCategory cat = new ProductCategory("All Products");

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            Material material = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);
            Material material2 = new Material("#002", "Expensive Wood", "ola.jpg", colors, finishes);

            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Product product = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            Assert.Empty(product.getAllComponentsAsProducts());
        }
        [Fact]
        public void ensureGetAllComponentsAsProductsWorks() {
            ProductCategory cat = new ProductCategory("All Products");

            Color black = Color.valueOf("Deep Black", 0, 0, 0, 0);
            Color white = Color.valueOf("Blinding White", 255, 255, 255, 0);
            List<Color> colors = new List<Color>() { black, white };

            Finish glossy = Finish.valueOf("Glossy", 100);
            Finish matte = Finish.valueOf("Matte", 0);
            List<Finish> finishes = new List<Finish>() { glossy, matte };

            Material material = new Material("#001", "Really Expensive Wood", "ola.jpg", colors, finishes);
            Material material2 = new Material("#002", "Expensive Wood", "ola.jpg", colors, finishes);

            Dimension heightDimension = new SingleValueDimension(50);
            Dimension widthDimension = new DiscreteDimensionInterval(new List<double>() { 60, 65, 70, 80, 90, 105 });
            Dimension depthDimension = new ContinuousDimensionInterval(10, 25, 5);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);

            Product product = new Product("Test", "Shelf", "shelf.glb", cat, new List<Material>() { material, material2 }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            Product product2 = new Product("Test2", "Shelf2", "shelf.glb", cat, new List<Material>() { material }, new List<Measurement>() { measurement }, ProductSlotWidths.valueOf(4, 4, 4));
            product.addComplementaryProduct(product2);
            Assert.True(product.getAllComponentsAsProducts().Count == 1);
        }
    }
}