using System;
using System.Collections.Generic;
using support.dto;
using core.domain;
using core.dto;
using Xunit;
namespace core_tests.domain
{
    public class CatalogueCollectionTest
    {
        [Fact]
        public void ensureCatalogueCollectionContructorIsInvalid()
        {
            var category = new ProductCategory("Drawers");



            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            finishes.Add(finish);

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);



            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);
            List<CustomizedProduct> list = new List<CustomizedProduct>();
            list.Add(cp);

           

            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("Mario");
            CatalogueCollection CatalogueCollection = new CatalogueCollection(list,customizedProductCollection);
            Assert.Throws<ArgumentException>(() =>  new CatalogueCollection(list, null)); 
        }

        
        [Fact]
        public void ensureCatalogueCollectionContructorIsValid()
        {
            var category = new ProductCategory("Drawers");



            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            finishes.Add(finish);

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);



            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);
            List<CustomizedProduct> list = new List<CustomizedProduct>();
            list.Add(cp);

           

            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("Mario");
            CatalogueCollection CatalogueCollection = new CatalogueCollection(list,customizedProductCollection);
            Assert.True(CatalogueCollection.Equals( new CatalogueCollection(list,customizedProductCollection)));
        }

         [Fact]
        public void ensureHashCodeIsEqual()
        {
            var category = new ProductCategory("Drawers");



            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);

            IEnumerable<Dimension> heightValues = valuest;
            IEnumerable<Dimension> widthValues = valuest;
            IEnumerable<Dimension> depthValues = valuest;

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém");
            finishes.Add(finish);

            Material material = new Material(reference, designation, colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", category, matsList, heightValues, widthValues, depthValues);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(1.2, 1.5, 20.3);

            //Customized Material
            Color color1 = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish2 = Finish.valueOf("Acabamento polido");
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(color1, finish2);



            CustomizedProduct cp = new CustomizedProduct("#666", "Shelf", custMaterial1, customizedDimensions, product);
            List<CustomizedProduct> list = new List<CustomizedProduct>();
            list.Add(cp);

           

            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection("Mario");
            CatalogueCollection CatalogueCollection = new CatalogueCollection(list,customizedProductCollection);
            Assert.True(CatalogueCollection.Equals( new CatalogueCollection(list,customizedProductCollection)));

        }

    //      /**
    //     <summary>
    //         Test to ensure that a null collection cannot be added to the CommercialCatalogue's.
    //     </summary>
    //    */
    //     [Fact]
    //     public void ensureCollectionCannotBeAdded()
    //     {
    //         Console.WriteLine("ensureCollectionCannotBeAdded");

    //         List<CustomizedProduct> custoProducts = new List<CustomizedProduct>();
    //         Color color = Color.valueOf("Azul", 1, 1, 1, 1);
    //         Finish finish = Finish.valueOf("Acabamento polido");
    //         CustomizedMaterial custMaterial = CustomizedMaterial.valueOf(color, finish);
    //         CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(15.5, 10, 4.3);

    //         ProductCategory prodCat = new ProductCategory("Category 1");
    //         List<Double> values2 = new List<Double>();
    //         values2.Add(500.0); //Width
    //         DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
    //         List<Dimension> valuest = new List<Dimension>();
    //         valuest.Add(d2);
    //         IEnumerable<Dimension> heightDimensions = valuest;
    //         IEnumerable<Dimension> widthDimensions = valuest;
    //         IEnumerable<Dimension> depthDimensions = valuest;
    //         List<Color> colors = new List<Color>();
    //         colors.Add(color);

    //         List<Finish> finishes = new List<Finish>();
    //         finishes.Add(finish);

    //         Material material = new Material("1234", "Material", colors, finishes);
    //         List<Material> listMaterial = new List<Material>();
    //         listMaterial.Add(material);
    //         IEnumerable<Material> materials = listMaterial;
    //         Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

    //         CustomizedProduct custProduct = new CustomizedProduct("123", "CustomizedProduct1", custMaterial, custDimensions, product);
    //         custoProducts.Add(custProduct);
    //         List<CustomizedProductCollection> listCollection = new List<CustomizedProductCollection>();
    //         CustomizedProductCollection custProCollection = new CustomizedProductCollection("Collection", custoProducts);
    //         listCollection.Add(custProCollection);
    //         CustomizedProductCollection collection2 = new CustomizedProductCollection("Collection2", custoProducts);

    //         CommercialCatalogue comCatalogue = new CommercialCatalogue("Another", "One", listCollection);
    //         Assert.True(comCatalogue.addCollection(collection2));
    //     }
    }
}