using core.domain;
using System;
using System.Collections.Generic;
using Xunit;

namespace core_tests.domain{
    /// <summary>
    /// Unit and Business rules tests for Product entity class
    /// </summary>
    public class ProductTest{

        /// <summary>
        /// Asserts that product can't be created with a null reference
        /// </summary>
        [Fact]
        public void assertProductCantBeCreatedWithNullReference(){
            Console.WriteLine("assertProductCantBeCreatedWithNullReference");
            Action invalidNullProductReferenceCreation=()=>new Product(null,"Shelf",new List<Material>(new []{new Material("#444","Tinta Branca")}));
            //Since the product was created with a null reference then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidNullProductReferenceCreation);
        }

        /// <summary>
        /// Asserts that product can't be created with an empty reference
        /// </summary>
        [Fact]
        public void assertProductCantBeCreatedWithEmptyReference(){
            Console.WriteLine("assertProductCantBeCreatedWithEmptyReference");
            Action invalidEmptyProductReferenceCreation=()=>new Product("","Shelf",new List<Material>(new []{new Material("#444","Tinta Branca")}));
            //Since the product was created with an empty reference then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidEmptyProductReferenceCreation);
        }

        /// <summary>
        /// Asserts that product can't be created with a null designation
        /// </summary>
        [Fact]
        public void assertProductCantBeCreatedWithNullDesignation(){
            Console.WriteLine("assertProductCantBeCreatedWithNullDesignation");
            Action invalidNullProductDesignationCreation=()=>new Product("#666",null,new List<Material>(new []{new Material("#444","Tinta Branca")}));
            //Since the product was created with a null designation then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidNullProductDesignationCreation);
        }

        /// <summary>
        /// Asserts that product can't be created with an empty designation
        /// </summary>
        [Fact]
        public void assertProductCantBeCreatedWithEmptyDesignation(){
            Console.WriteLine("assertProductCantBeCreatedWithEmptyDesignation");
            Action invalidEmptyProductDesignationCreation=()=>new Product("#666","",new List<Material>(new []{new Material("#444","Tinta Branca")}));
            //Since the product was created with an empty designation then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidEmptyProductDesignationCreation);
        }

        /// <summary>
        /// Asserts that product can't be created with null materials
        /// </summary>
        [Fact]
        public void assertProductCantBeCreatedWithNullMaterials(){
            Console.WriteLine("assertProductCantBeCreatedWithNullMaterials");
            Action invalidNullMaterialsProductCreation=()=>new Product("#666","Shelf",null);
            //Since the product was created with null materials then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidNullMaterialsProductCreation);
        }

        /// <summary>
        /// Asserts that product can't be created with empty materials
        /// </summary>
        [Fact]
        public void assertProductCantBeCreatedWithEmptyMaterials(){
            Console.WriteLine("assertProductCantBeCreatedWithEmptyMaterials");
            Action invalidEmptyMaterialsProductCreation=()=>new Product("#666","Shelf",new List<Material>());
            //Since the product was created with empty materials then it should throw
            //An ArgumentException
            //A product needs at least one material to be valid
            Assert.Throws<ArgumentException>(invalidEmptyMaterialsProductCreation);
        }

        /// <summary>
        /// Asserts that product can't be created with null complemented products
        /// </summary>
        [Fact]
        public void assertProductCantBeCreatedWithNullComplementedProducts(){
            Console.WriteLine("assertProductCantBeCreatedWithNullComplementedProducts");
            Action invalidNullComplementedProductsProductCreation=()=>new Product("#666","Shelf",new List<Material>(new []{new Material("#444","Tinta Branca")}),null);
            //Since the product was created with null complemented products then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidNullComplementedProductsProductCreation);
        }

        /// <summary>
        /// Asserts that product can't be created with empty complemented products
        /// </summary>
        [Fact]
        public void assertProductCantBeCreatedWithEmptyComplementedProducts(){
            Console.WriteLine("assertProductCantBeCreatedWithEmptyComplementedProducts");
            Action invalidEmptyComplementedProductsProductCreation=()=>new Product("#666","Shelf",new List<Material>(new []{new Material("#444","Tinta Branca")}),new List<Product>());
            //Since the product was created with empty complemented products then it should throw
            //An ArgumentException
            //Even though that a product may not have complemented products, if we are 
            //Creating a product with an empty enumerable of complemented products
            //Then it should throw an exception since there is already a constructor for that case
            Assert.Throws<ArgumentException>(invalidEmptyComplementedProductsProductCreation);
        }

        /// <summary>
        /// Asserts that product can't be created with duplicated materials
        /// </summary>
        [Fact]
        public void assertProductCantBeCreatedWithDuplicatedMaterials(){
            Console.WriteLine("assertProductCantBeCreatedWithDuplicatedMaterials");
            List<Material> duplicatedMaterials=new List<Material>(new []{new Material("#444","Tinta Branca"),new Material("#444","Tinta Roxa")});
            Action invalidDuplicatedMaterialsProductCreation=()=>new Product("#666","Shelf",duplicatedMaterials);
            //Since the product was created with duplicated materials, then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidDuplicatedMaterialsProductCreation);
        }
        
        /// <summary>
        /// Asserts that product can't be created with duplicated materials
        /// </summary>
        [Fact]
        public void assertProductCantBeCreatedWithDuplicatedComplementedProduct(){
            Console.WriteLine("assertProductCantBeCreatedWithDuplicatedComplementedProduct");
            List<Material> materials=new List<Material>(new []{new Material("#444","Tinta Branca")});
            List<Product> duplicatedProducts=new List<Product>(new []{new Product("#666","Shelf",materials),new Product("#666","Shelf",materials)});
            Action invalidDuplicatedComplementedProductsProductCreation=()=>new Product("#666","Shelf",materials,duplicatedProducts);
            //Since the product was created with duplicated complemented products, then it should throw
            //An ArgumentException
            Assert.Throws<ArgumentException>(invalidDuplicatedComplementedProductsProductCreation);
        }

        /// <summary>
        /// Asserts that a product can't add duplicated materials
        /// </summary>
        [Fact]
        public void assertProductCantAddNullMaterial(){
            Console.WriteLine("assertProductCantAddNullMaterial");
            Material productMaterial=new Material("#444","Tinta Branca");
            Product product=new Product("#666","Shelf",new List<Material>(new []{productMaterial}));
            //Since we added a null material then it should return false
            Assert.False(product.addMaterial(null));
        }

        /// <summary>
        /// Asserts that a product can't add duplicated materials
        /// </summary>
        [Fact]
        public void assertProductCantAddDuplicatedMaterials(){
            Console.WriteLine("assertProductCantAddDuplicatedMaterials");
            Material productMaterial=new Material("#444","Tinta Branca");
            Product product=new Product("#666","Shelf",new List<Material>(new []{productMaterial}));
            //Since we added a duplicated material then it should return false
            Assert.False(product.addMaterial(productMaterial));
        }

        /// <summary>
        /// Asserts that a product can add duplicated materials
        /// </summary>
        [Fact]
        public void assertProductCanAddValidMaterials(){
            Console.WriteLine("assertProductCanAddValidMaterials");
            Material productMaterial=new Material("#444","Tinta Branca");
            Product product=new Product("#666","Shelf",new List<Material>(new []{productMaterial}));
            //Since we added a valid material then it should return true
            Assert.True(product.addMaterial(new Material("#445","Tinta Roxa")));
        }

        /// <summary>
        /// Asserts that a product cant add a null complemented product
        /// </summary>
        [Fact]
        public void assertProductCantAddNullComplementedProducts(){
            Console.WriteLine("assertProductCantAddNullComplementedProducts");
            Material productMaterial=new Material("#444","Tinta Branca");
            Product product=new Product("#666","Shelf",new List<Material>(new []{productMaterial}));
            //Since we added a null complemented product then it should return false
            Assert.False(product.addComplementedProduct(null));
        }

        /// <summary>
        /// Asserts that a product cant add a null complemented product
        /// </summary>
        [Fact]
        public void assertProductCantAddDuplicatedComplementedProducts(){
            Console.WriteLine("assertProductCantAddNullComplementedProducts");
            Material productMaterial=new Material("#444","Tinta Branca");
            Product complementedProduct=new Product("#665");
            Product product=new Product("#666","Shelf",new List<Material>(new []{productMaterial}));
            //Since we added a null complemented product then it should return false
            Assert.False(product.addComplementedProduct(null));
        }


    }
}