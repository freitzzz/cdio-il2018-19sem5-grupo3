using System;
using Xunit;
using System.Collections.Generic;
using System.Text;
using core.domain;
using core.dto;

namespace core_tests.domain {
    public class ProductMaterialTest {
        private static readonly Material PREDEFINED_MATERIAL = new Material("#444", "Tinta Branca", "ola.jpg", new List<Color>(new[] { Color.valueOf("White", 0, 0, 0, 0) }), new List<Finish>(new[] { Finish.valueOf("Matte",0) }));
        private static readonly Material PREDEFINED_MATERIAL2 = new Material("#445", "Tinta Preta", "ola.jpg", new List<Color>(new[] { Color.valueOf("Black", 255, 255, 255, 255) }), new List<Finish>(new[] { Finish.valueOf("Matte",0) }));
        private static readonly List<Material> PREDEFINED_MATERIALS = new List<Material>(new[] { PREDEFINED_MATERIAL });
         private readonly List<Measurement> PREDEFINED_MEASUREMENTS = new List<Measurement>(){new Measurement(   height: new SingleValueDimension(21),
                                                                                                                width: new SingleValueDimension(50),
                                                                                                                depth: new SingleValueDimension(15)
                                                                                                            )
                                                                                            };
        private static readonly ProductCategory PREDEFEFINED_CATEGORY = new ProductCategory("Test");


        [Fact]
        public void ensureAddRestrictionThrowsExceptionIfRestrictionIsNull() {
            Product p = new Product("#666", "der alte würfelt nicht", "product666.glb", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Action addNullRestrictionAction = () => pm.addRestriction(null);

            Assert.Throws<ArgumentException>(addNullRestrictionAction);
        }

        [Fact]
        public void ensureAddRestrictionSucceeds() {
            Product p = new Product("#666", "der alte würfelt nicht", "product666.glb", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Restriction rest = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());

            Action addValidRestrictionAction = () => pm.addRestriction(rest);

            Exception exception = Record.Exception(addValidRestrictionAction);
            Assert.Null(exception);
            Assert.True(pm.hasRestriction(rest));
        }

        [Fact]
        public void ensureAddingDuplicateRestrictionThrowsException() {
            Product p = new Product("#666", "der alte würfelt nicht", "product666.glb", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Restriction rest = new Restriction("restriction", new WidthPercentageAlgorithm());
            pm.addRestriction(rest);

            Action addDuplicateRestrictionAction = () => pm.addRestriction(rest);
            Assert.Throws<ArgumentException>(addDuplicateRestrictionAction);
        }


        [Fact]
        public void ensureHasRestrictionReturnsTrueIfRestrictionWasAdded() {
            Product p = new Product("#666", "der alte würfelt nicht", "product666.glb", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Restriction rest = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());
            pm.addRestriction(rest);
            Assert.True(pm.hasRestriction(rest));
        }


        [Fact]
        public void ensureHasRestrictionReturnsFalseIfRestrictionIsNull() {
            Product p = new Product("#666", "der alte würfelt nicht", "product666.glb", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Assert.False(pm.hasRestriction(null));
        }

        [Fact]
        public void ensureHasRestrictionrReturnsFalseIfRestrictionWasNotAdded() {
            Product p = new Product("#666", "der alte würfelt nicht", "product666.glb", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Restriction rest = new Restriction("restriction", new WidthPercentageAlgorithm());
            Assert.False(pm.hasRestriction(rest));
        }

        [Fact]
        public void ensureRemovingNullRestrictionThrowsException() {
            Product p = new Product("#666", "der alte würfelt nicht", "product666.glb", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);

            Action removeNullRestrictionAction = () => pm.removeRestriction(null);

            Assert.Throws<ArgumentException>(removeNullRestrictionAction);
        }

        [Fact]
        public void ensureRemovingDuplicateRestrictionThrowsException() {
            Product p = new Product("#666", "der alte würfelt nicht", "product666.glb", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Restriction rest = new Restriction("restriction", new SameMaterialAndFinishAlgorithm());
            
            Action addNullRestrictionCreation = () =>  pm.removeRestriction(rest);
            Assert.Throws<ArgumentException>(addNullRestrictionCreation);
        }
    
        [Fact]
        public void ensureRemovePreviouslyAddedRestrictionDoesNotThrowException() {
            Product p = new Product("#666", "der alte würfelt nicht", "product666.glb", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Restriction rest = new Restriction("restriction", new WidthPercentageAlgorithm());
            pm.addRestriction(rest);

            Action removeValidRestrictionAction = () => pm.removeRestriction(rest);

            Exception exception = Record.Exception(removeValidRestrictionAction);

            Assert.Null(exception);
        }
        /// <summary>
        /// Ensures hasMaterial returns false with null argument
        /// </summary>
        [Fact]
        public void ensureHasMaterialFailsWithNullArgument() {
            Product p = new Product("#666", "der alte würfelt nicht", "product666.glb", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Assert.False(pm.hasMaterial(null));
        }
        /// <summary>
        /// Ensures hasMaterial returns false if productmaterial does not contain material
        /// </summary>
        [Fact]
        public void ensureHasMaterialFails() {
            Product p = new Product("#666", "der alte würfelt nicht", "product666.glb", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Assert.False(pm.hasMaterial(PREDEFINED_MATERIAL));
        }
        /// <summary>
        /// Ensures hasMaterial returns true if productmaterial contains material
        /// </summary>
        [Fact]
        public void ensureHasMaterialSucceeds() {
            Product p = new Product("#666", "der alte würfelt nicht", "product666.glb", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2, new List<Restriction>());
            Assert.True(pm.hasMaterial(PREDEFINED_MATERIAL2));
        }
        /// <summary>
        /// Ensures default get method for product attribute works
        /// </summary>
        [Fact]
        public void ensureGetProductWorks() {
            Product p = new Product("#666", "der alte würfelt nicht", "product666.glb", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_MEASUREMENTS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2, new List<Restriction>());
            Assert.True(pm.product.Equals(p));
        }
    }
}
