using System;
using Xunit;
using System.Collections.Generic;
using core.domain;
using core.dto;
using support.dto;
namespace core_tests.domain
{
    /**
    <summary>
        Tests of the class CommercialCatalogue.
    </summary>
    */
    public class CommercialCatalogueTest
    {
        //id tests

        /**
        <summary>
            Test to ensure that the method id works.
         </summary>
         */
        [Fact]
        public void ensureIdMethodWorks()
        {
            Console.WriteLine("ensureIdMethodWorks");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);
            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);

            CommercialCatalogue comCatalogue = CommercialCatalogue.valueOf(reference, designation, custProducts);

            Assert.Equal(comCatalogue.id(), reference, true);
        }

        //sameAs tests

        /**
        <summary>
            Test to ensure that the method sameAs works, for two equal identities.
         </summary>
         */
        [Fact]
        public void ensureComCataloguesWithEqualIdentitiesAreTheSame()
        {
            Console.WriteLine("ensureComCataloguesWithEqualIdentitiesAreTheSame");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);

            CommercialCatalogue comCatalogue = CommercialCatalogue.valueOf(reference, designation, custProducts);


            Assert.True(comCatalogue.sameAs(reference));
        }

        /**
        <summary>
            Test to ensure that the method sameAs works, for two different identities.
         </summary>
         */
        [Fact]
        public void ensureComCataloguesWithDifferentIdentitiesAreNotTheSame()
        {
            Console.WriteLine("ensureComCataloguesWithDifferentIdentitiesAreNotTheSame");

            string anotherReference = "1160907";

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct =new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);

            CommercialCatalogue comCatalogue = CommercialCatalogue.valueOf(reference, designation, custProducts);



            Assert.False(comCatalogue.sameAs(anotherReference));
        }

        //checkCommercialCatalogueProperties tests

        /**
        <summary>
            Test to ensure that the instance of CommercialCatalogue isn't built if the reference is null.
        </summary>
         */
        [Fact]
        public void ensureNullReferenceIsNotValid()
        {
            Console.WriteLine("ensureNullReferenceIsNotValid");

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);

            Assert.Throws<ArgumentException>(() => CommercialCatalogue.valueOf(null, "This doesn't work", custProducts));
        }

        /**
        <summary>
            Test to ensure that the instance of CommercialCatalogue isn't built if the reference is empty.
        </summary>
       */
        [Fact]
        public void ensureEmptyReferenceIsNotValid()
        {
            Console.WriteLine("ensureEmptyReferenceIsNotValid");

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);

            Assert.Throws<ArgumentException>(() => CommercialCatalogue.valueOf("", "Let me see...", custProducts));
        }

        /**
        <summary>
            Test to ensure that the instance of CommercialCatalogue isn't built if the designation is null.
        </summary>
       */
        [Fact]
        public void ensureNullDesignationIsNotValid()
        {
            Console.WriteLine("ensureNullDesignationIsNotValid");

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);
            Assert.Throws<ArgumentException>(() => CommercialCatalogue.valueOf("Have you tried turning it off and then on again?", null, custProducts));
        }

        /**
        <summary>
            Test to ensure that the instance of CommercialCatalogue isn't built if the designation is empty.
        </summary>
       */
        [Fact]
        public void ensureEmptyDesignationIsNotValid()
        {
            Console.WriteLine("ensureEmptyDesignationIsNotValid");

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);

            Assert.Throws<ArgumentException>(() => CommercialCatalogue.valueOf("Still not working", "", custProducts));
        }

        /**
        <summary>
            Test to ensure that the instance of CommercialCatalogue isn't built if the list of costumizedProducts is null.
        </summary>
       */
        [Fact]
        public void ensureNullCustProductListIsNotValid()
        {
            Console.WriteLine("ensureNullCustProductListIsNotValid");

            Assert.Throws<ArgumentException>(() => CommercialCatalogue.valueOf("Hello", "It's me, Mario", null));
        }

        /**
        <summary>
            Test to ensure that the instance of CommercialCatalogue isn't built if the list of costumizedProducts is empty.
        </summary>
       */
        [Fact]
        public void ensureEmptyCustProductListIsNotValid()
        {
            Console.WriteLine("ensureEmptyCustProductListIsNotValid");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();

            Assert.Throws<ArgumentException>(() => CommercialCatalogue.valueOf("Goodbye", "See you later", custoProducts));
        }

        //addCustomizedProduct tests

        /**
        <summary>
            Test to ensure that an already existent customizedProduct cannot be added to the CommercialCataloguev's list of customizedProduct.
        </summary>
       */
        [Fact]
        public void ensureAlreadyExistentCustProductCannotBeAdded()
        {
            Console.WriteLine("ensureAlreadyExistentCustProducCannotBeAdded");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct =new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custoProducts.Add(custProduct);

            CommercialCatalogue comCatalogue = CommercialCatalogue.valueOf("Another", "One", custoProducts);

            Assert.False(comCatalogue.addCustomizedProduct(custProduct));
        }

        /**
        <summary>
            Test to ensure that a null customizedProduct cannot be added to the CommercialCatalogue's list of customizedProduct.
        </summary>
       */
        [Fact]
        public void ensureNullCustProducCannotBeAdded()
        {
            Console.WriteLine("ensureNullCustProducCannotBeAdded");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);

            CommercialCatalogue comCatalogue = CommercialCatalogue.valueOf("Another", "One", custoProducts);
            Assert.False(comCatalogue.addCustomizedProduct(null));
        }

        /**
        <summary>
            Test to ensure that a valid customizedProduct can be added to the CommercialCatalogue's list of customizedProduct.
        </summary>
       */
        [Fact]
        public void ensureValidCustProducCanBeAdded()
        {
            Console.WriteLine("ensureValidCustProducCanBeAdded");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);

            CommercialCatalogue comCatalogue = CommercialCatalogue.valueOf("Another", "One", custoProducts);

            Assert.True(comCatalogue.addCustomizedProduct(new CustomizedProduct("566", "CustomizedProduct2", custMaterial, custDimensions, product)));
        }

        //removeCustomizedProduct tests

        /**
        <summary>
            Test to ensure that a non-existent customizedProduct cannot be removed from the CommercialCatalogue's list of customizedProduct.
        </summary>
       */
        [Fact]
        public void ensureNonExistentCustProducCannotBeRemoved()
        {
            Console.WriteLine("ensureNonExistentCustProducCannotBeRemoved");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);

            CommercialCatalogue comCatalogue = CommercialCatalogue.valueOf("Another", "One", custoProducts);

            Assert.False(comCatalogue.removeCustomizedProduct(new CustomizedProduct("Tira", "CustomizedProduct3", custMaterial, custDimensions, product)));
        }

        /**
        <summary>
            Test to ensure that a null customizedProduct cannot be removed from the CommercialCatalogue's list of customizedProduct.
        </summary>
       */
        [Fact]
        public void ensureNullCustProducCannotBeRemoved()
        {
            Console.WriteLine("ensureNullCustProducCannotBeRemoved");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);

            CommercialCatalogue comCatalogue = CommercialCatalogue.valueOf("Another", "One", custoProducts);

            Assert.False(comCatalogue.removeCustomizedProduct(null));
        }

        /**
         <summary>
             Test to ensure that a valid customizedProduct can be removed from the CommercialCatalogue's list of customizedProduct.
         </summary>
        */
        [Fact]
        public void ensureValidCustProducCanBeRemoved()
        {
            Console.WriteLine("ensureValidCustProducCanBeRemoved");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);

            CommercialCatalogue comCatalogue = CommercialCatalogue.valueOf("Another", "One", custoProducts);

            Assert.True(comCatalogue.removeCustomizedProduct(custProduct));
        }

        //hasCustomizedProduct tests

        /**
        <summary>
            Test to ensure that an existent customizedProduct is found in the CommercialCatalogue's list of customizedProduct.
        </summary>
        */
        [Fact]
        public void ensureValidCustoProductExists()
        {
            Console.WriteLine("ensureValidCustoProductExists");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);

            CommercialCatalogue comCatalogue = CommercialCatalogue.valueOf("Another", "One", custoProducts);

            Assert.True(comCatalogue.hasCustomizedProduct(custProduct));
        }

        /**
        <summary>
            Test to ensure that a null customizedProduct is not found in the CommercialCatalogue's list of customizedProduct.
        </summary>
        */
        [Fact]
        public void ensureNullComCatalogueDoesNotExist()
        {
            Console.WriteLine("ensureNullComCatalogueDoesNotExist");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);

            CommercialCatalogue comCatalogue = CommercialCatalogue.valueOf("Another", "One", custoProducts);

            Assert.False(comCatalogue.hasCustomizedProduct(null));
        }

        //GetHashCode tests

        /**
        <summary>
           Test to ensure that the method GetHashCode works.
        </summary>
        */
        [Fact]
        public void ensureGetHashCodeWorks()
        {
            Console.WriteLine("ensureGetHashCodeWorks");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);

            CommercialCatalogue comCatalogue1 = CommercialCatalogue.valueOf("Another", "One", custoProducts);
            CommercialCatalogue comCatalogue2 = CommercialCatalogue.valueOf("Another", "One", custoProducts);

            Assert.Equal(comCatalogue1.GetHashCode(), comCatalogue2.GetHashCode());
        }

        //Equals tests

        /**
        <summary>
            Test to ensure that the method Equals works, for two CommercialCatalogue with different references.
         </summary>
         */
        [Fact]
        public void ensureComCatalogueWithDifferentReferencesAreNotEqual()
        {
            Console.WriteLine("ensureComCatalogueWithDifferentReferencesAreNotEqual");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);

            CommercialCatalogue comCatalogue1 = CommercialCatalogue.valueOf("Another", "One", custoProducts);
            CommercialCatalogue comCatalogue2 = CommercialCatalogue.valueOf("Equal", "One", custoProducts);


            Assert.False(comCatalogue1.Equals(comCatalogue2));
        }

        /**
        <summary>
            Test to ensure that the method Equals works, for two CommercialCatalogue with the same reference.
         </summary>
         */
        [Fact]
        public void ensureCommercialCataloguesWithSameReferencesAreEqual()
        {
            Console.WriteLine("ensureMaterialsWithSameReferencesAreEqual");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);

            CommercialCatalogue comCatalogue1 = CommercialCatalogue.valueOf("Another", "One", custoProducts);
            CommercialCatalogue comCatalogue2 = CommercialCatalogue.valueOf("Another", "One", custoProducts);



            Assert.True(comCatalogue1.Equals(comCatalogue2));
        }

        /**
        <summary>
            Test to ensure that the method Equals works, for a null CommercialCatalogue.
         </summary>
         */
        [Fact]
        public void ensureNullObjectIsNotEqual()
        {
            Console.WriteLine("ensureNullObjectIsNotEqual");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);

            CommercialCatalogue comCatalogue = CommercialCatalogue.valueOf("Another", "One", custoProducts);


            Assert.False(comCatalogue.Equals(null));
        }

        /**
        <summary>
            Test to ensure that the method Equals works, for a CommercialCatalogue and an object of another type.
         </summary>
         */
        [Fact]
        public void ensureDifferentTypesAreNotEqual()
        {
            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);

            CommercialCatalogue comCatalogue = CommercialCatalogue.valueOf("Another", "One", custoProducts);


            Assert.False(comCatalogue.Equals("stars"));
        }

        //ToString tests

        /**
        <summary>
            Test to ensure that the method ToString works.
         </summary>
         */
        [Fact]
        public void ensureToStringWorks()
        {
            Console.WriteLine("ensureToStringWorks");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);

            CommercialCatalogue comCatalogue1 = CommercialCatalogue.valueOf("Another", "One", custoProducts);
            CommercialCatalogue comCatalogue2 = CommercialCatalogue.valueOf("Another", "One", custoProducts);



            Assert.Equal(comCatalogue1.ToString(), comCatalogue2.ToString());
        }
        /** [Fact]
         public void testToDTO()
         {
             Console.WriteLine("toDTO");
             string reference = "123456789.";
             string designation = "Commercial Catalogue 2019";
             List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
             CommercialCatalogue comCatalogue = new CommercialCatalogue(reference, designation, custoProducts);
             CommercialCatalogueDTO dto = new CommercialCatalogueDTO();
             dto.reference = reference;
             dto.designation = designation;
             dto.custProducts = new List<CustomizedProductDTO>(DTOUtils.parseToDTOS(custoProducts));
             CommercialCatalogueDTO dto2 = comCatalogue.toDTO();
             Assert.Equal(dto.reference, dto2.reference);
             //Assert.Equal(dto.designation, dto2.designation);
            // Assert.Equal(dto.custProducts, dto2.custProducts);
         }*/
    }
}