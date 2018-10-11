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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);
            listCollection.Add(custProCollection);
            CommercialCatalogue comCatalogue = new CommercialCatalogue(reference, designation, listCollection);

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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);
            listCollection.Add(custProCollection);


            CommercialCatalogue comCatalogue = new CommercialCatalogue(reference, designation, listCollection);


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

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue(reference, designation, listCollection);



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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);
            listCollection.Add(custProCollection);

            Assert.Throws<ArgumentException>(() => new CommercialCatalogue(null, "This doesn't work", listCollection));
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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);
            listCollection.Add(custProCollection);

            Assert.Throws<ArgumentException>(() => new CommercialCatalogue("", "Let me see...", listCollection));
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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);
            listCollection.Add(custProCollection);
            Assert.Throws<ArgumentException>(() => new CommercialCatalogue("Have you tried turning it off and then on again?", null, listCollection));
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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);
            listCollection.Add(custProCollection);

            Assert.Throws<ArgumentException>(() => new CommercialCatalogue("Still not working", "", listCollection));
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

            Assert.Throws<ArgumentException>(() => new CommercialCatalogue("Hello", "It's me, Mario", null));
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

            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();

            Assert.Throws<ArgumentException>(() => new CommercialCatalogue("Goodbye", "See you later", listCollection));
        }

        /* //addCustomizedProduct tests

        /**
        <summary>
            Test to ensure that an already existent customizedProductCollections cannot be added to the CommercialCataloguev's list of customizedProduct.
        </summary>
       */
        [Fact]
        public void ensureAlreadyExistentCustProductCollectionCannotBeAdded()
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

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);

            Assert.False(comCatalogue.addCustomizedProductToCollection(custProCollection, custProduct));
        }

        /**
        <summary>
            Test to ensure that a null collection cannot be added to the CommercialCatalogue's list of customizedProduct.
        </summary>
       */
        [Fact]
        public void ensureNullCollectionCannotBeAddedProducts()
        {
            Console.WriteLine("ensureNullCollectionCannotBeAddedProducts");

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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);
            Assert.False(comCatalogue.addCustomizedProductToCollection(null, custProduct));
        }
        /**
        <summary>
            Test to ensure that a null collection cannot be added to the CommercialCatalogue's.
        </summary>
       */
        [Fact]
        public void ensureNullCollectionCannotBeAdded()
        {
            Console.WriteLine("ensureNullCollectionCannotBeAdded");

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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);
            Assert.False(comCatalogue.addCollection(null));
        }
        /**
        <summary>
            Test to ensure that a null collection cannot be added to the CommercialCatalogue's.
        </summary>
       */
        [Fact]
        public void ensureCollectionCannotBeAdded()
        {
            Console.WriteLine("ensureCollectionCannotBeAdded");

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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);
            CustomizedProductCollection collection2 = new CustomizedProductCollection("Collection2", custoProducts);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);
            Assert.True(comCatalogue.addCollection(collection2));
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
            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);

            Assert.True(comCatalogue.addCustomizedProductToCollection(custProCollection, new CustomizedProduct("566", "CustomizedProduct2", custMaterial, custDimensions, product)));
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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);

            Assert.False(comCatalogue.removeCustomizedProductFromCollection(custProCollection, new CustomizedProduct("Tira", "CustomizedProduct3", custMaterial, custDimensions, product)));
        }
/**
        <summary>
            Test to ensure that a non-existent collestion cannot be removed from the CommercialCatalogue's.
        </summary>
       */
        [Fact]
        public void ensureNonExistentCollectionCannotBeRemoved()
        {
            Console.WriteLine("ensureNonExistentCollectionCannotBeRemoved");

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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);

            Assert.False(comCatalogue.removeCollection(new CustomizedProductCollection("Collection2", custoProducts)));
            
        }
        /**
        <summary>
            Test to ensure that a existent collestion cannot be removed from the CommercialCatalogue's.
        </summary>
       */
        [Fact]
        public void ensureExistentCollectionCannotBeRemoved()
        {
            Console.WriteLine("ensureNonExistentCollectionCannotBeRemoved");

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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);

            Assert.False(comCatalogue.removeCollection(null));
            
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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);
            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);

            Assert.False(comCatalogue.removeCustomizedProductFromCollection(null, custProduct));
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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);
            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);

            Assert.True(comCatalogue.removeCustomizedProductFromCollection(custProCollection, custProduct));
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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);

            Assert.True(comCatalogue.hasCollection(custProCollection));
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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);

            Assert.False(comCatalogue.hasCollection(null));
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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue1 = new CommercialCatalogue("Another", "One", listCollection);
            CommercialCatalogue comCatalogue2 = new CommercialCatalogue("Another", "One", listCollection);

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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue1 = new CommercialCatalogue("Another", "One", listCollection);
            CommercialCatalogue comCatalogue2 = new CommercialCatalogue("Equal", "One", listCollection);


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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue1 = new CommercialCatalogue("Another", "One", listCollection);
            CommercialCatalogue comCatalogue2 = new CommercialCatalogue("Another", "One", listCollection);



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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);


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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);


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
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue1 = new CommercialCatalogue("Another", "One", listCollection);
            CommercialCatalogue comCatalogue2 = new CommercialCatalogue("Another", "One", listCollection);



            Assert.Equal(comCatalogue1.ToString(), comCatalogue2.ToString());
        }
        [Fact]
        public void testToDTO()
        {
            Console.WriteLine("toDTO");
            string reference = "123456789.";
            string designation = "Commercial Catalogue 2019";
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
            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);
            CommercialCatalogue comCatalogue = new CommercialCatalogue(reference, designation, listCollection);
            CommercialCatalogueDTO dto = new CommercialCatalogueDTO();
            dto.reference = reference;
            dto.designation = designation;
            dto.collectionList = new List<CustomizedProductCollectionDTO>(DTOUtils.parseToDTOS(listCollection));
            CommercialCatalogueDTO dto2 = comCatalogue.toDTO();
            Assert.Equal(dto.reference, dto2.reference);
            Assert.Equal(dto.designation, dto2.designation);
            //Assert.Equal(dto.collectionList, dto2.collectionList);
        }
    }
}