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

        [Fact]
        public void ensureIdMethodWorks()
        {
            Console.WriteLine("ensureIdMethodWorks");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");


            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);
            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);

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

        [Fact]
        public void ensurechangeReferenceInComCatalogue()
        {
            Console.WriteLine("ensurechangeReferenceInComCatalogue");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");


            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);
            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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
            Assert.Equal("Mudei referecia", comCatalogue.reference, true);
        }

        [Fact]
        public void ensurechangeNullReferenceInComCatalogue()
        {
            Console.WriteLine("ensurechangeNullReferenceInComCatalogue");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);
            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

            Assert.Throws<ArgumentException>(() => comCatalogue.changeReference(null));

        }

        [Fact]
        public void ensurechangeEmpetyReferenceInComCatalogue()
        {
            Console.WriteLine("ensurechangeEmpetyReferenceInComCatalogue");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);
            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

            Assert.Throws<ArgumentException>(() => comCatalogue.changeReference(""));
        }

        [Fact]
        public void ensurechangeDesignationInComCatalogue()
        {
            Console.WriteLine("ensurechangeDesignationInComCatalogue");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);
            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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
            Assert.Equal("Mudei designation", comCatalogue.designation, true);
        }

        [Fact]
        public void ensurechangeNullDesignationInComCatalogue()
        {
            Console.WriteLine("ensurechangeNullDesignationInComCatalogue");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);
            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

        [Fact]
        public void ensurechangeEmpetyDesignationInComCatalogue()
        {
            Console.WriteLine("ensurechangeEmpetyDesignationInComCatalogue");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);
            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

            Assert.Throws<ArgumentException>(() => comCatalogue.changeDesignation(""));
        }

        [Fact]
        public void ensureComCataloguesWithEqualIdentitiesAreTheSame()
        {
            Console.WriteLine("ensureComCataloguesWithEqualIdentitiesAreTheSame");

            string reference = "123456789";
            string designation = "Commercial Catalogue 2019";

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1",  "product123.glb", prodCat, materials, measurements);
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

        [Fact]
        public void ensureNullReferenceIsNotValid()
        {
            Console.WriteLine("ensureNullReferenceIsNotValid");

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

        [Fact]
        public void ensureEmptyReferenceIsNotValid()
        {
            Console.WriteLine("ensureEmptyReferenceIsNotValid");

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

        [Fact]
        public void ensureNullDesignationIsNotValid()
        {
            Console.WriteLine("ensureNullDesignationIsNotValid");

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

        [Fact]
        public void ensureEmptyDesignationIsNotValid()
        {
            Console.WriteLine("ensureEmptyDesignationIsNotValid");

            List<CustomizedProduct> custProducts = new List<CustomizedProduct>();

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);
            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

        [Fact]
        public void ensureNullCustProductListIsNotValid()
        {
            Console.WriteLine("ensureNullCustProductListIsNotValid");

            Assert.Throws<ArgumentException>(() => new CommercialCatalogue("Hello", "It's me, Mario", null));
        }

        [Fact]
        public void ensureEmptyCustProductListIsNotValid()
        {
            Console.WriteLine("ensureEmptyCustProductListIsNotValid");

            List<CatalogueCollection> list = new List<CatalogueCollection>();

            Assert.Throws<ArgumentException>(() => new CommercialCatalogue("Goodbye", "See you later", list));
        }

        [Fact]
        public void ensureAlreadyExistCollectionCannotBeAdded()
        {
            Console.WriteLine("ensureAlreadyExistentCustProducCannotBeAdded");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");

            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

        [Fact]
        public void ensureaddCollectionCanBeAdded()
        {
            Console.WriteLine("ensureaddCollectionCanBeAdded");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");

            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

        [Fact]
        public void ensureNullCollectionCannotBeAdded()
        {
            Console.WriteLine("ensureNullCollectionCannotBeAdded");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");

            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);

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

        [Fact]
        public void ensureNonExistentCollectionCannotBeRemoved()
        {
            Console.WriteLine("ensureNonExistentCollectionCannotBeRemoved");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");


            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");

            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

        [Fact]
        public void ensureExistentCollectionCannotBeRemoved()
        {
            Console.WriteLine("ensureNonExistentCollectionCannotBeRemoved");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");


            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");

            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

        [Fact]
        public void ensureValidCollectionExists()
        {
            Console.WriteLine("ensureValidCollectionExists");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");


            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");

            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

        [Fact]
        public void ensureValidCollectionNotExists()
        {
            Console.WriteLine("ensureValidCollectionNotExists");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");

            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

        [Fact]
        public void ensureNullComCatalogueDoesNotExist()
        {
            Console.WriteLine("ensureNullComCatalogueDoesNotExist");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");


            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");

            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);
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

        [Fact]
        public void ensureGetHashCodeWorks()
        {
            Console.WriteLine("ensureGetHashCodeWorks");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);
            ProductCategory prodCat = new ProductCategory("Category 1");

            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);

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

        [Fact]
        public void ensureComCatalogueWithDifferentReferencesAreNotEqual()
        {
            Console.WriteLine("ensureComCatalogueWithDifferentReferencesAreNotEqual");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);

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

        [Fact]
        public void ensureCommercialCataloguesWithSameReferencesAreEqual()
        {
            Console.WriteLine("ensureMaterialsWithSameReferencesAreEqual");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");

            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);

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

        [Fact]
        public void ensureNullObjectIsNotEqual()
        {
            Console.WriteLine("ensureNullObjectIsNotEqual");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");

            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);

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

        [Fact]
        public void ensureDifferentTypesAreNotEqual()
        {
            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");

            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);

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

        [Fact]
        public void ensureToStringWorks()
        {
            Console.WriteLine("ensureToStringWorks");

            List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");

            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);

            ProductCategory prodCat = new ProductCategory("Category 1");

            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);

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
            CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(21, 30, 17);
            ProductCategory prodCat = new ProductCategory("Category 1");
            Dimension heightDimension = new SingleValueDimension(21);
            Dimension widthDimension = new SingleValueDimension(30);
            Dimension depthDimension = new SingleValueDimension(17);

            Measurement measurement = new Measurement(heightDimension, widthDimension, depthDimension);
            List<Measurement> measurements = new List<Measurement>() { measurement };
            List<Color> colors = new List<Color>();
            colors.Add(color);
            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);
            Material material = new Material("1234", "Material", "ola.jpg", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", "product123.glb", prodCat, materials, measurements);

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
        }
    }
}