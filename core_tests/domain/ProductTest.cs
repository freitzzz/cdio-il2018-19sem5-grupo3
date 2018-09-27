using core.domain;
using core.persistence;
using System;
using System.Collections.Generic;
using Xunit;

namespace core_tests.domain{
    /// <summary>
    /// Unit and Business rules tests for Product entity class
    /// </summary>
    public class ProductTest{

        private static readonly Material PREDEFINED_MATERIAL
                    =new Material("#444"
                                    ,"Tinta Branca"
                                    ,new List<Color>(new []{Color.valueOf("Black",0,0,0,0)})
                                    ,new List<Finish>(new []{Finish.valueOf("Matte")}));

        private static readonly List<Material> PREDEFINED_MATERIALS=new List<Material>(new []{PREDEFINED_MATERIAL});

        /// <summary>
        /// Asserts that product can't be created with a null reference
        /// </summary>
        [Fact]
        public void assertProductCantBeCreatedWithNullReference(){
            Console.WriteLine("assertProductCantBeCreatedWithNullReference");
            Action invalidNullProductReferenceCreation=()=>new Product(null,"Shelf",PREDEFINED_MATERIALS);
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
            Action invalidEmptyProductReferenceCreation=()=>new Product("","Shelf",PREDEFINED_MATERIALS);
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
            Action invalidNullProductDesignationCreation=()=>new Product("#666",null,PREDEFINED_MATERIALS);
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
            Action invalidEmptyProductDesignationCreation=()=>new Product("#666","",PREDEFINED_MATERIALS);
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
            Action invalidNullComplementedProductsProductCreation=()=>new Product("#666","Shelf",PREDEFINED_MATERIALS,null);
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
            Action invalidEmptyComplementedProductsProductCreation=()=>new Product("#666","Shelf",PREDEFINED_MATERIALS,new List<Product>());
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
            List<Material> duplicatedMaterials=new List<Material>(new []{PREDEFINED_MATERIAL,PREDEFINED_MATERIAL});
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
            List<Material> materials=new List<Material>(new []{PREDEFINED_MATERIAL});
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
            Material productMaterial=PREDEFINED_MATERIAL;
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
            Material productMaterial=PREDEFINED_MATERIAL;
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
            Material productMaterial=PREDEFINED_MATERIAL;
            Product product=new Product("#666","Shelf",new List<Material>(new []{productMaterial}));
            //Since we added a valid material then it should return true
            Assert.True(product.addMaterial(new Material("#4445","Tinta Preta",new List<Color>(new []{Color.valueOf("Black",0,0,0,0)}),new List<Finish>(new []{Finish.valueOf("Matte")}))));
        }

        /// <summary>
        /// Asserts that a product cant add a null complemented product
        /// </summary>
        [Fact]
        public void assertProductCantAddNullComplementedProducts(){
            Console.WriteLine("assertProductCantAddNullComplementedProducts");
            Material productMaterial=PREDEFINED_MATERIAL;
            Product product=new Product("#666","Shelf",new List<Material>(new []{productMaterial}));
            //Since we added a null complemented product then it should return false
            Assert.False(product.addComplementedProduct(null));
        }

        /// <summary>
        /// Asserts that a product cant add a duplicated complemented product
        /// </summary>
        [Fact]
        public void assertProductCantAddDuplicatedComplementedProducts(){
            Console.WriteLine("assertProductCantAddDuplicatedComplementedProducts");
            Material productMaterial=PREDEFINED_MATERIAL;
            Product complementedProduct=new Product("#665","Shelf",new List<Material>(new []{productMaterial}));
            Product product=new Product("#666","Shelf",new List<Material>(new []{productMaterial}),new List<Product>(new[]{complementedProduct}));
            //Since we added a duplicated complemented product then it should return false
            Assert.False(product.addComplementedProduct(complementedProduct));
        }

        /// <summary>
        /// Asserts that a product cant add complemented products which are the same reference
        /// </summary>
        [Fact]
        public void assertProductCantAddComplementedProductsSameReference(){
            Console.WriteLine("assertProductCantAddComplementedProductsSameReference");
            Material productMaterial=PREDEFINED_MATERIAL;
            Product complementedProduct=new Product("#665","Shelf",new List<Material>(new []{productMaterial}));
            Product product=new Product("#666","Shelf",new List<Material>(new []{productMaterial}),new List<Product>(new[]{complementedProduct}));
            //Since we added a complemented product which is the same reference as the base product then it should return false
            Assert.False(product.addComplementedProduct(product));
        }

        /// <summary>
        /// Asserts that a product identifier is the same as its created one
        /// </summary>
        [Fact]
        public void assertProductIdentifierIsTheSame(){
            Console.WriteLine("assertProductIdentifierIsTheSame");
            string id="#666";
            Product product=new Product(id,"Shelf",new List<Material>(new []{PREDEFINED_MATERIAL}));
            //Since the product was created with the id "#666" then its id should be "#666"
            Assert.True(product.id().Equals(id));
        }

        /// <summary>
        /// Asserts that a product identifier is the same as its created one
        /// </summary>
        [Fact]
        public void assertProductIdentierSameAs(){
            Console.WriteLine("assertProductIdentierSameAs");
            string id="#666";
            Product product=new Product(id,"Shelf",new List<Material>(new []{PREDEFINED_MATERIAL}));
            //Since the product was created with the id "#666" then its id should be "#666"
            Assert.True(product.sameAs(id));
        }

    }
}