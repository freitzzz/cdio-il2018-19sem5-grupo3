using core.domain;
using core.dto;
using core.persistence;
using System;
using System.Collections.Generic;
using Xunit;

namespace core_tests.domain {
    /// <summary>
    /// Unit and Business rules tests for Product entity class
    /// </summary>
    public class ProductTest {
        private static readonly Material PREDEFINED_MATERIAL
                    = new Material("#444"
                                    , "Tinta Branca"
                                    , new List<Color>(new[] { Color.valueOf("Black", 0, 0, 0, 0) })
                                    , new List<Finish>(new[] { Finish.valueOf("Matte") }));

        private static readonly List<Material> PREDEFINED_MATERIALS
                    = new List<Material>(new[] { PREDEFINED_MATERIAL });

        private readonly List<Measurement> PREDEFINED_MEASUREMENTS = new List<Measurement>(){new Measurement(   height: new SingleValueDimension(21),
                                                                                                                width: new SingleValueDimension(50),
                                                                                                                depth: new SingleValueDimension(15)
                                                                                                            )
                                                                                            };

        private static readonly ProductCategory PREDEFEFINED_CATEGORY = new ProductCategory("Test");

        /// <summary>
        /// Ensures that product can't be created with a null reference
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithNullReference() {
            Console.WriteLine("ensureProductCantBeCreatedWithNullReference");
            Action invalidNullProductReferenceCreation = () => new Product(null, "Shelf", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            //Since the product was created with a null reference then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidNullProductReferenceCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with an empty reference
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithEmptyReference() {
            Console.WriteLine("ensureProductCantBeCreatedWithEmptyReference");
            Action invalidEmptyProductReferenceCreation = () => new Product("", "Shelf", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            //Since the product was created with an empty reference then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidEmptyProductReferenceCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with a null designation
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithNullDesignation() {
            Console.WriteLine("ensureProductCantBeCreatedWithNullDesignation");
            Action invalidNullProductDesignationCreation = () => new Product("#666", null, PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            //Since the product was created with a null designation then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidNullProductDesignationCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with an empty designation
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithEmptyDesignation() {
            Console.WriteLine("ensureProductCantBeCreatedWithEmptyDesignation");
            Action invalidEmptyProductDesignationCreation = () => new Product("#666", "", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            //Since the product was created with an empty designation then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidEmptyProductDesignationCreation);
        }

        [Fact]
        public void ensureProductCantBeCreatedWithNullProductCategory()
        {
            Action action = () => new Product("#666", "Shelf", null, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);

            Assert.Throws<ArgumentException>(action);
        }

        /// <summary>
        /// Ensures that product can't be created with null materials
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithNullMaterials() {
            Console.WriteLine("ensureProductCantBeCreatedWithNullMaterials");
            Action invalidNullMaterialsProductCreation = () => new Product("#666", "Shelf", PREDEFEFINED_CATEGORY, null, PREDEFINED_MEASUREMENTS);
            //Since the product was created with null materials then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidNullMaterialsProductCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with empty materials
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithEmptyMaterials() {
            Console.WriteLine("ensureProductCantBeCreatedWithEmptyMaterials");
            Action invalidEmptyMaterialsProductCreation = () => new Product("#666", "Shelf", PREDEFEFINED_CATEGORY, new List<Material>(), PREDEFINED_MEASUREMENTS);
            //Since the product was created with empty materials then it should throw
            //An ArgumentException
            //A product needs at least one material to be valid
            Assert.Throws<ArgumentException>(invalidEmptyMaterialsProductCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with null complemented products
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithNullComplementedProducts() {
            Console.WriteLine("ensureProductCantBeCreatedWithNullComplementedProducts");
            Action invalidNullComplementedProductsProductCreation = () => new Product("#666", "Shelf", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, null, PREDEFINED_MEASUREMENTS);
            //Since the product was created with null complemented products then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidNullComplementedProductsProductCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with empty complemented products
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithEmptyComplementedProducts() {
            Console.WriteLine("ensureProductCantBeCreatedWithEmptyComplementedProducts");
            Action invalidEmptyComplementedProductsProductCreation = () => new Product("#666", "Shelf", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, new List<Product>(), PREDEFINED_MEASUREMENTS);
            //Since the product was created with empty complemented products then it should throw
            //An ArgumentException
            //Even though that a product may not have complemented products, if we are 
            //Creating a product with an empty enumerable of complemented products
            //Then it should throw an exception since there is already a constructor for that case
            Assert.Throws<ArgumentException>(invalidEmptyComplementedProductsProductCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with duplicated materials
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithDuplicatedMaterials() {
            Console.WriteLine("ensureProductCantBeCreatedWithDuplicatedMaterials");
            List<Material> duplicatedMaterials = new List<Material>(new[] { PREDEFINED_MATERIAL, PREDEFINED_MATERIAL });
            Action invalidDuplicatedMaterialsProductCreation = () => new Product("#666", "Shelf", PREDEFEFINED_CATEGORY, duplicatedMaterials, PREDEFINED_MEASUREMENTS);
            //Since the product was created with duplicated materials, then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidDuplicatedMaterialsProductCreation);
        }

        /// <summary>
        /// Ensures that product can't be created with duplicated materials
        /// </summary>
        [Fact]
        public void ensureProductCantBeCreatedWithDuplicatedComplementedProduct() {
            Console.WriteLine("ensureProductCantBeCreatedWithDuplicatedComplementedProduct");
            List<Material> materials = new List<Material>(new[] { PREDEFINED_MATERIAL });

            Product complement1 = new Product("#666","Shelf", PREDEFEFINED_CATEGORY,materials, PREDEFINED_MEASUREMENTS);
            Product complement2 = new Product("#666","Shelf", PREDEFEFINED_CATEGORY,materials, PREDEFINED_MEASUREMENTS);

            List<Product> duplicatedProducts = new List<Product>(){complement1, complement2};

            Action invalidDuplicatedComplementedProductsProductCreation = () => new Product("#665", "Structure", PREDEFEFINED_CATEGORY, materials, duplicatedProducts, PREDEFINED_MEASUREMENTS);
            //Since the product was created with duplicated complemented products, then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidDuplicatedComplementedProductsProductCreation);
        }

        /// <summary>
        /// Ensures that a product can't add duplicated materials
        /// </summary>
        [Fact]
        public void ensureProductCantAddNullMaterial() {
            Console.WriteLine("ensureProductCantAddNullMaterial");
            Material productMaterial = PREDEFINED_MATERIAL;
            Product product = new Product("#666", "Shelf", PREDEFEFINED_CATEGORY, new List<Material>(new[] { productMaterial }), PREDEFINED_MEASUREMENTS);
            //Since we added a null material then it should return false
            Assert.False(product.addMaterial(null));
        }

        /// <summary>
        /// Ensures that a product can't add duplicated materials
        /// </summary>
        [Fact]
        public void ensureProductCantAddDuplicatedMaterials() {
            Console.WriteLine("ensureProductCantAddDuplicatedMaterials");
            Material productMaterial = PREDEFINED_MATERIAL;
            Product product = new Product("#666", "Shelf", PREDEFEFINED_CATEGORY, new List<Material>(new[] { productMaterial }), PREDEFINED_MEASUREMENTS);
            //Since we added a duplicated material then it should return false
            Assert.False(product.addMaterial(productMaterial));
        }

        /// <summary>
        /// Ensures that a product can add duplicated materials
        /// </summary>
        [Fact]
        public void ensureProductCanAddValidMaterials() {
            Console.WriteLine("ensureProductCanAddValidMaterials");
            Material productMaterial = PREDEFINED_MATERIAL;
            Product product = new Product("#666", "Shelf", PREDEFEFINED_CATEGORY, new List<Material>(new[] { productMaterial }), PREDEFINED_MEASUREMENTS);
            //Since we added a valid material then it should return true
            Assert.True(product.addMaterial(new Material("#4445", "Tinta Preta", new List<Color>(new[] { Color.valueOf("Black", 0, 0, 0, 0) }), new List<Finish>(new[] { Finish.valueOf("Matte") }))));
        }

        /// <summary>
        /// Ensures that a product cant add a null complemented product
        /// </summary>
        [Fact]
        public void ensureProductCantAddNullComplementedProducts() {
            Console.WriteLine("ensureProductCantAddNullComplementedProducts");
            Material productMaterial = PREDEFINED_MATERIAL;
            Product product = new Product("#666", "Shelf", PREDEFEFINED_CATEGORY, new List<Material>(new[] { productMaterial }), PREDEFINED_MEASUREMENTS);
            //Since we added a null complemented product then it should return false
            Assert.False(product.addComplementedProduct(null));
        }

        /// <summary>
        /// Ensures that a product cant add a duplicated complemented product
        /// </summary>
        [Fact]
        public void ensureProductCantAddDuplicatedComplementedProducts() {
            Console.WriteLine("ensureProductCantAddDuplicatedComplementedProducts");
            Material productMaterial = PREDEFINED_MATERIAL;
            Product complementedProduct = new Product("#665", "Shelf", PREDEFEFINED_CATEGORY, new List<Material>(new[] { productMaterial }), PREDEFINED_MEASUREMENTS);
            Product product = new Product("#666", "Shelf", PREDEFEFINED_CATEGORY, new List<Material>(){ productMaterial }, new List<Product>(){complementedProduct}, PREDEFINED_MEASUREMENTS);
            //Since we added a duplicated complemented product then it should return false
            Assert.False(product.addComplementedProduct(complementedProduct));
        }

        /// <summary>
        /// Ensures that a product cant add complemented products which are the same reference
        /// </summary>
        [Fact]
        public void ensureProductCantAddComplementedProductsSameReference() {
            Console.WriteLine("ensureProductCantAddComplementedProductsSameReference");
            Material productMaterial = PREDEFINED_MATERIAL;
            Product complementedProduct = new Product("#665", "Shelf", PREDEFEFINED_CATEGORY, new List<Material>(new[] { productMaterial }), PREDEFINED_MEASUREMENTS);
            Product product = new Product("#666", "Shelf", PREDEFEFINED_CATEGORY, new List<Material>(new[] { productMaterial }), new List<Product>(){complementedProduct}, PREDEFINED_MEASUREMENTS);
            //Since we added a complemented product which is the same reference as the base product then it should return false
            Assert.False(product.addComplementedProduct(product));
        }

        /// <summary>
        /// Ensures that a product identifier is the same as its created one
        /// </summary>
        [Fact]
        public void ensureProductIdentifierIsTheSame() {
            Console.WriteLine("ensureProductIdentifierIsTheSame");
            string id = "#666";
            Product product = new Product(id, "Shelf", PREDEFEFINED_CATEGORY, new List<Material>(new[] { PREDEFINED_MATERIAL }), PREDEFINED_MEASUREMENTS);
            //Since the product was created with the id "#666" then its id should be "#666"
            Assert.Equal(product.id(), id);
        }

        /// <summary>
        /// Ensures that a product identifier is the same as its created one
        /// </summary>
        [Fact]
        public void ensureProductIdentierSameAs() {
            Console.WriteLine("ensureProductIdentierSameAs");
            string id = "#666";
            Product product = new Product(id, "Shelf", PREDEFEFINED_CATEGORY, new List<Material>(new[] { PREDEFINED_MATERIAL }), PREDEFINED_MEASUREMENTS);
            //Since the product was created with the id "#666" then its id should be "#666"
            Assert.True(product.sameAs(id));
        }

        /// <summary>
        /// Ensures that a product with a minimum slot size larger than its maximum size is invalid
        /// </summary>
        [Fact]
        public void ensureInvalidMinToMaxSlotSizeRatioIsInvalid(){
            Console.WriteLine("ensureInvalidMinToMaxSlotSizeRatioIsInvalid");
            string id = "Test";
            ///for height
            Assert.Throws<ArgumentException>(() => new Product(id, "Shelf", true, CustomizedDimensions.valueOf(5, 6, 7),
            CustomizedDimensions.valueOf(6, 6, 7), CustomizedDimensions.valueOf(5, 6, 7),
            PREDEFEFINED_CATEGORY, new List<Material>(new[] { PREDEFINED_MATERIAL }), 
            PREDEFINED_MEASUREMENTS));
            ///for width
            Assert.Throws<ArgumentException>(() => new Product(id, "Shelf", true, CustomizedDimensions.valueOf(5, 6, 7),
            CustomizedDimensions.valueOf(4, 9, 7), CustomizedDimensions.valueOf(5, 6, 7),
            PREDEFEFINED_CATEGORY, new List<Material>(new[] { PREDEFINED_MATERIAL }), 
            PREDEFINED_MEASUREMENTS));
            ///for depth
            Assert.Throws<ArgumentException>(() => new Product(id, "Shelf", true, CustomizedDimensions.valueOf(5, 6, 7),
            CustomizedDimensions.valueOf(4, 6, 9), CustomizedDimensions.valueOf(5, 6, 7),
            PREDEFEFINED_CATEGORY, new List<Material>(new[] { PREDEFINED_MATERIAL }), 
            PREDEFINED_MEASUREMENTS));
        }

        /// <summary>
        /// Ensures that a product with a recommended slot size larger than its maximum size is invalid
        /// </summary>
        [Fact]
        public void ensureInvalidRecommendedToMaxSlotSizeRatioIsInvalid(){
            Console.WriteLine("ensureInvalidRecommendedToMaxSlotSizeRatioIsInvalid");
            string id = "Test";
            ///for height
            Assert.Throws<ArgumentException>(() => new Product(id, "Shelf", true, CustomizedDimensions.valueOf(5, 6, 7),
            CustomizedDimensions.valueOf(4, 4, 4), CustomizedDimensions.valueOf(6, 6, 7),
            PREDEFEFINED_CATEGORY, new List<Material>(new[] { PREDEFINED_MATERIAL }), 
            PREDEFINED_MEASUREMENTS));
            ///for width
            Assert.Throws<ArgumentException>(() => new Product(id, "Shelf", true, CustomizedDimensions.valueOf(5, 6, 7),
            CustomizedDimensions.valueOf(4, 4, 4), CustomizedDimensions.valueOf(5, 9, 7),
            PREDEFEFINED_CATEGORY, new List<Material>(new[] { PREDEFINED_MATERIAL }), 
            PREDEFINED_MEASUREMENTS));
            ///for depth
            Assert.Throws<ArgumentException>(() => new Product(id, "Shelf", true, CustomizedDimensions.valueOf(5, 6, 7),
            CustomizedDimensions.valueOf(4, 4, 4), CustomizedDimensions.valueOf(5, 6, 9),
            PREDEFEFINED_CATEGORY, new List<Material>(new[] { PREDEFINED_MATERIAL }), 
            PREDEFINED_MEASUREMENTS));
        }

        /// <summary>
        /// Ensures that a product with a recommended slot size smaller than its minimum size is invalid
        /// </summary>
        [Fact]
        public void ensureInvalidRecommendedToMinSlotSizeRatioIsInvalid(){
            Console.WriteLine("ensureInvalidRecommendedToMinSlotSizeRatioIsInvalid");
            string id = "Test";
            ///for height
            Assert.Throws<ArgumentException>(() => new Product(id, "Shelf", true, CustomizedDimensions.valueOf(5, 6, 7),
            CustomizedDimensions.valueOf(4, 4, 4), CustomizedDimensions.valueOf(2, 6, 7),
            PREDEFEFINED_CATEGORY, new List<Material>(new[] { PREDEFINED_MATERIAL }), 
            PREDEFINED_MEASUREMENTS));
            ///for width
            Assert.Throws<ArgumentException>(() => new Product(id, "Shelf", true, CustomizedDimensions.valueOf(5, 6, 7),
            CustomizedDimensions.valueOf(4, 4, 4), CustomizedDimensions.valueOf(2, 6, 7),
            PREDEFEFINED_CATEGORY, new List<Material>(new[] { PREDEFINED_MATERIAL }), 
            PREDEFINED_MEASUREMENTS));
            ///for depth
            Assert.Throws<ArgumentException>(() => new Product(id, "Shelf", true, CustomizedDimensions.valueOf(5, 6, 7),
            CustomizedDimensions.valueOf(4, 4, 4), CustomizedDimensions.valueOf(5, 6, 2),
            PREDEFEFINED_CATEGORY, new List<Material>(new[] { PREDEFINED_MATERIAL }), 
            PREDEFINED_MEASUREMENTS));
        }

        /// <summary>
        /// Ensures that the textual description of two equal products is the same
        /// </summary>
        [Fact]
        public void ensureToStringWorks(){
            Console.WriteLine("ensureToStringWorks");
            string id = "Test";
            Assert.Equal(new Product(id, "Shelf", true, CustomizedDimensions.valueOf(5, 6, 7),
            CustomizedDimensions.valueOf(4, 4, 4), CustomizedDimensions.valueOf(5, 6, 6),
            PREDEFEFINED_CATEGORY, new List<Material>(new[] { PREDEFINED_MATERIAL }), 
            PREDEFINED_MEASUREMENTS).ToString(),
            new Product(id, "Shelf", true, CustomizedDimensions.valueOf(5, 6, 7),
            CustomizedDimensions.valueOf(4, 4, 4), CustomizedDimensions.valueOf(5, 6, 6),
            PREDEFEFINED_CATEGORY, new List<Material>(new[] { PREDEFINED_MATERIAL }), 
            PREDEFINED_MEASUREMENTS).ToString());
        }
    }
}