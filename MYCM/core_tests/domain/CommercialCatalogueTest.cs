using System;
using Xunit;
using System.Collections.Generic;
using core.domain;
using core.dto;
using support.dto;
using static core.domain.CustomizedProduct;
using System.Linq;

namespace core_tests.domain
{

    /// <summary>
    /// Tests of the class CommercialCatalogue
    /// </summary>
    public class CommercialCatalogueTest
    {

        private CustomizedProduct buildCustomizedProduct(string serialNumber)
        {
            var category = new ProductCategory("Drawers");
            //Creating Dimensions
            List<Double> values2 = new List<Double>();

            values2.Add(500.0); //Width

            DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);

            Measurement measurement = new Measurement(d2, d2, d2);
            List<Measurement> measurements = new List<Measurement>() { measurement };

            //Creating a material
            string reference = "1160912";
            string designation = "FR E SH A VOCA DO";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém", 12);
            finishes.Add(finish);

            Material material = new Material(reference, designation, "ola.jpg", colors, finishes);
            List<Material> materials = new List<Material>();
            materials.Add(material);

            IEnumerable<Material> matsList = materials;

            Product product = new Product("#666", "Shelf", "shelf666.glb", category, matsList, measurements);
            CustomizedDimensions customizedDimensions = CustomizedDimensions.valueOf(500.0, 500.0, 500.0);

            //Customized Material
            CustomizedMaterial custMaterial1 = CustomizedMaterial.valueOf(material, color, finish);

            return CustomizedProductBuilder.createAnonymousUserCustomizedProduct(serialNumber, product, customizedDimensions).withMaterial(custMaterial1).build();
        }

        private CustomizedProductCollection buildCustomizedProductCollection(string collectionName)
        {
            CustomizedProductCollection customizedProductCollection = new CustomizedProductCollection(collectionName);

            CustomizedProduct customizedProduct1 = buildCustomizedProduct("1234");
            customizedProduct1.finalizeCustomization();     //!customized products added to collections need to be finished

            customizedProductCollection.addCustomizedProduct(customizedProduct1);

            CustomizedProduct customizedProduct2 = buildCustomizedProduct("1235");
            customizedProduct2.finalizeCustomization();     //!customized products added to collections need to be finished

            customizedProductCollection.addCustomizedProduct(customizedProduct2);

            return customizedProductCollection;
        }

        private CatalogueCollection buildCatalogueCollection(string collectionName)
        {
            return new CatalogueCollection(buildCustomizedProductCollection(collectionName));
        }

        [Fact]
        public void ensureCommercialCatalogueCantBeCreatedWithNullReference()
        {
            Action createCommercialCatalogue = () => new CommercialCatalogue(null, "catalogue's designation");

            Assert.Throws<ArgumentException>(createCommercialCatalogue);
        }

        [Fact]
        public void ensureCommercialCatalogueCantBeCreatedWithEmptyReference()
        {
            Action createCommercialCatalogue = () => new CommercialCatalogue("", "catalogue's designation");

            Assert.Throws<ArgumentException>(createCommercialCatalogue);
        }

        [Fact]
        public void ensureCommercialCatalogueCantBeCreatedWithNullDesignation()
        {
            Action createCommercialCatalogue = () => new CommercialCatalogue("reference", null);

            Assert.Throws<ArgumentException>(createCommercialCatalogue);
        }

        [Fact]
        public void ensureCommercialCatalogueCantBeCreatedWithEmptyDesignation()
        {
            Action createCommercialCatalogue = () => new CommercialCatalogue("reference", "");

            Assert.Throws<ArgumentException>(createCommercialCatalogue);
        }

        [Fact]
        public void ensureCommercialCatalogueCantBeCreatedWithNullCatalogueCollectionsEnumerable()
        {
            Action createCommercialCatalogue = () => new CommercialCatalogue("reference", "designation", null);

            Assert.Throws<ArgumentException>(createCommercialCatalogue);
        }

        [Fact]
        public void ensureCommercialCatalogueCantBeCreatedWithNullCatalogueCollections()
        {
            Action createCommercialCatalogue = () => new CommercialCatalogue("reference", "designation", new List<CatalogueCollection>() { null });

            Assert.Throws<ArgumentException>(createCommercialCatalogue);
        }

        [Fact]
        public void ensureCommercialCatalogueCantBeCreatedWithEmptyCatalogueCollectionsEnumerable()
        {
            Action createCommercialCatalogue = () => new CommercialCatalogue("reference", "designation", new List<CatalogueCollection>());

            Assert.Throws<ArgumentException>(createCommercialCatalogue);
        }

        [Fact]
        public void ensureCommercialCatalogueCanBeCreatedWithValidCatalogueCollectionEnumerable()
        {
            CatalogueCollection catalogueCollection = buildCatalogueCollection("Christmas 2018");
            CatalogueCollection catalogueCollection1 = buildCatalogueCollection("Happy New 2019");

            IEnumerable<CatalogueCollection> catalogueCollections = new List<CatalogueCollection>() { catalogueCollection, catalogueCollection1 };

            Action createCommercialCatalogue = () => new CommercialCatalogue("reference", "designation", catalogueCollections);

            Exception exception = Record.Exception(createCommercialCatalogue);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureChangingToNullReferenceThrowsException()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            Action changeReference = () => commercialCatalogue.changeReference(null);

            Assert.Throws<ArgumentException>(changeReference);
        }

        [Fact]
        public void ensureChangingtoNullReferenceDoesNotChangeReference()
        {
            string reference = "reference";

            CommercialCatalogue commercialCatalogue = new CommercialCatalogue(reference, "designation");

            try
            {
                commercialCatalogue.changeReference(null);
            }
            catch (Exception) { }

            Assert.Equal(reference, commercialCatalogue.reference);
        }

        [Fact]
        public void ensureChangingToEmptyReferenceThrowsException()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            Action changeReference = () => commercialCatalogue.changeReference("");

            Assert.Throws<ArgumentException>(changeReference);
        }

        [Fact]
        public void ensureChangingToEmptyReferenceDoesNotChangeReference()
        {
            string reference = "reference";

            CommercialCatalogue commercialCatalogue = new CommercialCatalogue(reference, "designation");

            try
            {
                commercialCatalogue.changeReference("");
            }
            catch (Exception) { }

            Assert.Equal(reference, commercialCatalogue.reference);
        }


        [Fact]
        public void ensureChangingReferenceToValidReferenceDoesNotThrowException()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            Action changeReference = () => commercialCatalogue.changeReference("New reference");

            Exception exception = Record.Exception(changeReference);

            Assert.Null(exception);
        }


        [Fact]
        public void ensureChangingReferenceToValidReferenceChangesReference()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            string newReference = "New reference";

            commercialCatalogue.changeReference(newReference);

            Assert.Equal(newReference, commercialCatalogue.reference);
        }


        [Fact]
        public void ensureChangingToNullDesignationThrowsException()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            Action changeDesignation = () => commercialCatalogue.changeDesignation(null);

            Assert.Throws<ArgumentException>(changeDesignation);
        }

        [Fact]
        public void ensureChangingToNullDesignationDoesNotChangeDesignation()
        {
            string designation = "designation";

            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", designation);

            try
            {
                commercialCatalogue.changeDesignation(null);
            }
            catch (Exception) { }

            Assert.Equal(designation, commercialCatalogue.designation);
        }


        [Fact]
        public void ensureChangingToEmptyDesignationThrowsException()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            Action changeDesignation = () => commercialCatalogue.changeDesignation("");

            Assert.Throws<ArgumentException>(changeDesignation);
        }

        [Fact]
        public void ensureChangingToEmptyDesignationDoesNotChangeDesignation()
        {
            string designation = "designation";

            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", designation);

            try
            {
                commercialCatalogue.changeDesignation("");
            }
            catch (Exception) { }

            Assert.Equal(designation, commercialCatalogue.designation);
        }

        [Fact]
        public void ensureChangingToValidDesignationDoesNotThrowException()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            Action changeDesignation = () => commercialCatalogue.changeDesignation("a different designation");

            Exception exception = Record.Exception(changeDesignation);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureChangingToValidDesignationChangesDesignation()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            string newDesignation = "a different designation";

            commercialCatalogue.changeDesignation(newDesignation);

            Assert.Equal(newDesignation, commercialCatalogue.designation);
        }

        [Fact]
        public void ensureAddingNullCatalogueCollectionThrowsException()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            Action addCollection = () => commercialCatalogue.addCollection(null);

            Assert.Throws<ArgumentException>(addCollection);
        }

        [Fact]
        public void ensureAddingNullCatalogueCollectionDoesNotAddCollection()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            try
            {
                commercialCatalogue.addCollection(null);
            }
            catch (Exception) { }

            Assert.Empty(commercialCatalogue.catalogueCollectionList);
        }


        [Fact]
        public void ensureAddingDuplicateCatalogueCollectionThrowsException()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            string collectionName = "Winter 2018";

            CatalogueCollection catalogueCollection = buildCatalogueCollection(collectionName);
            commercialCatalogue.addCollection(catalogueCollection);

            CatalogueCollection otherCatalogueCollection = buildCatalogueCollection(collectionName);

            Action addCatalogueCollection = () => commercialCatalogue.addCollection(otherCatalogueCollection);

            Assert.Throws<ArgumentException>(addCatalogueCollection);
        }

        [Fact]
        public void ensureAddingDuplicateCatalogueCollectionDoesNotAddCollection()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            string collectionName = "Winter 2018";

            CatalogueCollection catalogueCollection = buildCatalogueCollection(collectionName);
            commercialCatalogue.addCollection(catalogueCollection);

            CatalogueCollection otherCatalogueCollection = buildCatalogueCollection(collectionName);

            try
            {
                commercialCatalogue.addCollection(otherCatalogueCollection);
            }
            catch (Exception) { }

            Assert.Single(commercialCatalogue.catalogueCollectionList);
        }

        [Fact]
        public void ensureAddingValidCatalogueCollectionDoesNotThrowException()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            CatalogueCollection catalogueCollection = buildCatalogueCollection("Winter 2018");
            commercialCatalogue.addCollection(catalogueCollection);

            CatalogueCollection otherCatalogueCollection = buildCatalogueCollection("Spring 2019");

            Action addCatalogueCollection = () => commercialCatalogue.addCollection(otherCatalogueCollection);

            Exception exception = Record.Exception(addCatalogueCollection);

            Assert.Null(exception);
        }

        [Fact]
        public void ensureAddingValidCatalogueCollectionAddsCollection()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            CatalogueCollection catalogueCollection = buildCatalogueCollection("Winter 2018");
            commercialCatalogue.addCollection(catalogueCollection);

            CatalogueCollection otherCatalogueCollection = buildCatalogueCollection("Spring 2019");
            commercialCatalogue.addCollection(otherCatalogueCollection);

            Assert.Equal(2, commercialCatalogue.catalogueCollectionList.Count);
            Assert.True(commercialCatalogue.hasCollection(otherCatalogueCollection));
        }

        [Fact]
        public void ensureRemovingNullCatalogueCollectionThrowsException()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            Action removeCollection = () => commercialCatalogue.removeCollection(null);

            Assert.Throws<ArgumentException>(removeCollection);
        }

        [Fact]
        public void ensureRemovingNullCatalogueCollectionDoesNotRemoveCatalogueCollection()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            CatalogueCollection catalogueCollection = buildCatalogueCollection("Winter 2018");
            commercialCatalogue.addCollection(catalogueCollection);


            try
            {
                commercialCatalogue.removeCollection(null);
            }
            catch (Exception) { }

            Assert.Single(commercialCatalogue.catalogueCollectionList);
        }

        [Fact]
        public void ensureRemovingNotAddedCatalogueCollectionThrowsException()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            CatalogueCollection catalogueCollection = buildCatalogueCollection("Winter 2018");

            Action removeCollection = () => commercialCatalogue.removeCollection(catalogueCollection);

            Assert.Throws<ArgumentException>(removeCollection);
        }

        [Fact]
        public void ensureRemovingNotAddedCatalogueCollectionDoesNotRemoveCatalogueCollection()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            CatalogueCollection catalogueCollection = buildCatalogueCollection("Winter 2018");
            commercialCatalogue.addCollection(catalogueCollection);

            CatalogueCollection otherCatalogueCollection = buildCatalogueCollection("Spring 2019");

            try
            {
                commercialCatalogue.removeCollection(otherCatalogueCollection);
            }
            catch (Exception) { }

            Assert.True(commercialCatalogue.hasCollection(catalogueCollection));
            Assert.Single(commercialCatalogue.catalogueCollectionList);
        }

        [Fact]
        public void ensureRemovingAddedCatalogueCollectionDoesNotThrowException()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            CatalogueCollection catalogueCollection = buildCatalogueCollection("Winter 2018");
            commercialCatalogue.addCollection(catalogueCollection);

            CatalogueCollection otherCatalogueCollection = buildCatalogueCollection("Spring 2019");
            commercialCatalogue.addCollection(otherCatalogueCollection);

            Action removeCollection = () => commercialCatalogue.removeCollection(otherCatalogueCollection);

            Exception exception = Record.Exception(removeCollection);
            Assert.Null(exception);
        }

        [Fact]
        public void ensureRemovingAddedCatalogueCollectionRemovesCollection()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            CatalogueCollection catalogueCollection = buildCatalogueCollection("Winter 2018");
            commercialCatalogue.addCollection(catalogueCollection);

            CatalogueCollection otherCatalogueCollection = buildCatalogueCollection("Spring 2019");
            commercialCatalogue.addCollection(otherCatalogueCollection);

            commercialCatalogue.removeCollection(otherCatalogueCollection);

            Assert.False(commercialCatalogue.hasCollection(otherCatalogueCollection));
        }

        [Fact]
        public void ensureIdReturnsCommercialCatalogueBusinessIdentifier()
        {
            string reference = "reference";

            CommercialCatalogue commercialCatalogue = new CommercialCatalogue(reference, "designation");

            Assert.Equal(reference, commercialCatalogue.id());
        }

        [Fact]
        public void ensureSameAsReturnsTrueIfArgumentIsEqualToTheBusinessIdentifier()
        {
            string reference = "reference";

            CommercialCatalogue commercialCatalogue = new CommercialCatalogue(reference, "designation");

            Assert.True(commercialCatalogue.sameAs(reference));
        }

        [Fact]
        public void ensureSameAsReturnsFalseIfArgumentIsNotEqualToTheBusinessIdentifier()
        {
            string reference = "other reference";

            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            Assert.False(commercialCatalogue.sameAs(reference));
        }

        [Fact]
        public void ensureInstancesHaveSameHashCode()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            CommercialCatalogue otherCommercialCatalogue = new CommercialCatalogue("reference", "designation");

            Assert.Equal(commercialCatalogue.GetHashCode(), otherCommercialCatalogue.GetHashCode());
        }

        [Fact]
        public void ensureInstancesHaveDifferentHashCode()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            CommercialCatalogue otherCommercialCatalogue = new CommercialCatalogue("other reference", "designation");

            Assert.NotEqual(commercialCatalogue.GetHashCode(), otherCommercialCatalogue.GetHashCode());
        }

        [Fact]
        public void ensureInstanceIsEqualToItself()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            Assert.True(commercialCatalogue.Equals(commercialCatalogue));
        }

        [Fact]
        public void ensureNullIsNotEqual()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            Assert.False(commercialCatalogue.Equals(null));
        }

        [Fact]
        public void ensureDifferentObjectTypeIsNotEqual()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            Assert.False(commercialCatalogue.Equals("reference"));
        }

        [Fact]
        public void ensureCommercialCatalogueWithDifferentReferenceIsNotEqual()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            CommercialCatalogue otherCommercialCatalogue = new CommercialCatalogue("other reference", "designation");

            Assert.NotEqual(commercialCatalogue, otherCommercialCatalogue);
        }

        [Fact]
        public void ensureCommercialCatalogueWithEqualReferenceIsEqual()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            CommercialCatalogue otherCommercialCatalogue = new CommercialCatalogue("reference", "designation");

            Assert.Equal(commercialCatalogue, otherCommercialCatalogue);
        }

        [Fact]
        public void ensureToStringIsEqualIfInstancesAreEqual()
        {
            CommercialCatalogue commercialCatalogue = new CommercialCatalogue("reference", "designation");

            CommercialCatalogue otherCommercialCatalogue = new CommercialCatalogue("reference", "designation");

            Assert.Equal(commercialCatalogue.ToString(), otherCommercialCatalogue.ToString());
        }

    }
}