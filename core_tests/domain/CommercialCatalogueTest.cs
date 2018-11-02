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


            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);
            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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

            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);

            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);
            CommercialCatalogue comCatalogue = new CommercialCatalogue(reference, designation, list);

            Assert.Equal(comCatalogue.id(), reference, true);
        }
        /**
        <summary>
            Test to ensure that the method ChangeReference works.
         </summary>
         */
        [Fact]
        public void ensurechangeReferenceInComCatalogue()
        {
            Console.WriteLine("ensurechangeReferenceInComCatalogue");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");


            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);
            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);
            listCollection.Add(custProCollection);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);
            CommercialCatalogue comCatalogue = new CommercialCatalogue(reference, designation, list);

            comCatalogue.changeReference("Mudei referecia");
            Assert.Equal(comCatalogue.reference, "Mudei referecia", true);
        }
        /**
               <summary>
                   Test to ensure that the method ChangeReference works.
                </summary>
                */
        [Fact]
        public void ensurechangeNullReferenceInComCatalogue()
        {
            Console.WriteLine("ensurechangeNullReferenceInComCatalogue");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);
            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);

            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);


            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);
            CommercialCatalogue comCatalogue = new CommercialCatalogue(reference, designation, list);

            ///comCatalogue.changeDesignation("Mudei designation");
            Assert.Throws<ArgumentException>(() => comCatalogue.changeReference(null));

        }
        /**
       <summary>
           Test to ensure that the method ChangeReference works.
        </summary>
        */
        [Fact]
        public void ensurechangeEmpetyReferenceInComCatalogue()
        {
            Console.WriteLine("ensurechangeEmpetyReferenceInComCatalogue");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);
            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);
            listCollection.Add(custProCollection);


            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);
            CommercialCatalogue comCatalogue = new CommercialCatalogue(reference, designation, list);

            ///comCatalogue.changeDesignation("Mudei designation");
            Assert.Throws<ArgumentException>(() => comCatalogue.changeReference(""));

        }
        /**
       <summary>
           Test to ensure that the method ChangeReference works.
        </summary>
        */
        [Fact]
        public void ensurechangeDesignationInComCatalogue()
        {
            Console.WriteLine("ensurechangeDesignationInComCatalogue");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);
            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);
            CommercialCatalogue comCatalogue = new CommercialCatalogue(reference, designation, list);

            comCatalogue.changeDesignation("Mudei designation");
            Assert.Equal(comCatalogue.designation, "Mudei designation", true);
        }

        /**
               <summary>
                   Test to ensure that the method ChangeReference works.
                </summary>
                */
        [Fact]
        public void ensurechangeNullDesignationInComCatalogue()
        {
            Console.WriteLine("ensurechangeNullDesignationInComCatalogue");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);
            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);

            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);
            CommercialCatalogue comCatalogue = new CommercialCatalogue(reference, designation, list);

            ///comCatalogue.changeDesignation("Mudei designation");
            Assert.Throws<ArgumentException>(() => comCatalogue.changeDesignation(null));

        }
        /**
       <summary>
           Test to ensure that the method ChangeReference works.
        </summary>
        */
        [Fact]
        public void ensurechangeEmpetyDesignationInComCatalogue()
        {
            Console.WriteLine("ensurechangeEmpetyDesignationInComCatalogue");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);
            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);
            listCollection.Add(custProCollection);


            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);
            CommercialCatalogue comCatalogue = new CommercialCatalogue(reference, designation, list);

            ///comCatalogue.changeDesignation("Mudei designation");
            Assert.Throws<ArgumentException>(() => comCatalogue.changeDesignation(""));

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

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);
            listCollection.Add(custProCollection);
            ;

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);
            CommercialCatalogue comCatalogue = new CommercialCatalogue(reference, designation, list);



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

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);
            listCollection.Add(custProCollection);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);
            CommercialCatalogue comCatalogue = new CommercialCatalogue(reference, designation, list);



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

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);

            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);

            Assert.Throws<ArgumentException>(() => new CommercialCatalogue(null, "This doesn't work", list));
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

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);
            listCollection.Add(custProCollection);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);

            Assert.Throws<ArgumentException>(() => new CommercialCatalogue("", "Let me see...", list));
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

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);

            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);


            Assert.Throws<ArgumentException>(() => new CommercialCatalogue("Have you tried turning it off and then on again?", null, list));
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

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);
            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custProducts);
            listCollection.Add(custProCollection);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);


            Assert.Throws<ArgumentException>(() => new CommercialCatalogue("Still not working", "", list));
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

            List<CatalogueCollection> list = new List<CatalogueCollection>();



            Assert.Throws<ArgumentException>(() => new CommercialCatalogue("Goodbye", "See you later", list));
        }

        /* //addCollection tests

        /**
        <summary>
            Test to ensure that an already existent customizedProductCollections cannot be added to the CommercialCataloguev's list of customizedProduct.
        </summary>
       */
        [Fact]
        public void ensureAlreadyExistCollectionCannotBeAdded()
        {
            Console.WriteLine("ensureAlreadyExistentCustProducCannotBeAdded");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", list);



            Assert.False(comCatalogue.addCollection(catalogueCollection));
        }
        /**
        <summary>
            Test to ensure that an Collections can be added to the CommercialCataloguev's list of customizedProduct.
        </summary>
       */
        [Fact]
        public void ensureaddCollectionCanBeAdded()
        {
            Console.WriteLine("ensureaddCollectionCanBeAdded");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);


            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);


            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One");



            Assert.True(comCatalogue.addCollection(catalogueCollection));
        }

        ///<summary>
        /// Test to ensure that a null collection cannot be added to the CommercialCatalogue's.
        ///</summary>

        [Fact]
        public void ensureNullCollectionCannotBeAdded()
        {
            Console.WriteLine("ensureNullCollectionCannotBeAdded");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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

            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", list);
            Assert.False(comCatalogue.addCollection(null));
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


            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", list);

            ///Assert.False(comCatalogue.removeCollection(new CatalogueCollection(custProCollection, listCustomizedProducts)));

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


            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);
            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", list);

            Assert.False(comCatalogue.removeCollection(null));

        }

        //hasCollection tests

        /**
        <summary>
            Test to ensure that an existent customizedProduct is found in the CommercialCatalogue's list of customizedProduct.
        </summary>
        */
        [Fact]
        public void ensureValidCollectionExists()
        {
            Console.WriteLine("ensureValidCollectionExists");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");


            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);
            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", list);

            Assert.True(comCatalogue.hasCollection(custProCollection));
        }
        /**
        <summary>
            Test to ensure that an existent collection is found in the CommercialCatalogue's.
        </summary>
        */
        [Fact]
        public void ensureValidCollectionNotExists()
        {
            Console.WriteLine("ensureValidCollectionNotExists");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");


            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);

            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One");

            Assert.False(comCatalogue.hasCollection(custProCollection));
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


            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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
            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);
            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", list);

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

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);
            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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

            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue1 = new CommercialCatalogue("Another", "One", list);

            Assert.Equal(comCatalogue1.GetHashCode(), comCatalogue1.GetHashCode());
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

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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

            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);


            CommercialCatalogue comCatalogue1 = new CommercialCatalogue("Another", "One", list);
            CommercialCatalogue comCatalogue2 = new CommercialCatalogue("Equal", "One", list);


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

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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

            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue1 = new CommercialCatalogue("Another", "One", list);
            CommercialCatalogue comCatalogue2 = new CommercialCatalogue("Another", "One", list);



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

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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

            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);


            CommercialCatalogue comCatalogue1 = new CommercialCatalogue("Another", "One", list);


            Assert.False(comCatalogue1.Equals(null));
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

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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

            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);


            CommercialCatalogue comCatalogue1 = new CommercialCatalogue("Another", "One", list);

            Assert.False(comCatalogue1.Equals("stars"));
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

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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

            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);

            CommercialCatalogue comCatalogue1 = new CommercialCatalogue("Another", "One", list);
            CommercialCatalogue comCatalogue2 = new CommercialCatalogue("Another", "One", list);



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
            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);
            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
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

            CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(material, color, finish);
            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);

            custoProducts.Add(custProduct);
            List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
            CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
            listCollection.Add(custProCollection);

            List<CatalogueCollection> list = new List<CatalogueCollection>();
            List<CustomizedProduct> listCustomizedProducts = new List<CustomizedProduct>();
            listCustomizedProducts.Add(custProduct);
            CatalogueCollection catalogueCollection = new CatalogueCollection(custProCollection, listCustomizedProducts);
            list.Add(catalogueCollection);

            listCollection.Add(custProCollection);


            CommercialCatalogue comCatalogue1 = new CommercialCatalogue(reference, designation, list);
            CommercialCatalogueDTO dto = new CommercialCatalogueDTO();
            dto.reference = reference;
            dto.designation = designation;
            dto.catalogueCollectionDTOs = new List<CatalogueCollectionDTO>(DTOUtils.parseToDTOS(list));
            CommercialCatalogueDTO dto2 = comCatalogue1.toDTO();
            Assert.Equal(dto.reference, dto2.reference);
            Assert.Equal(dto.designation, dto2.designation);
            //Assert.Equal(dto.collectionList, dto2.collectionList);
        }
    }
}