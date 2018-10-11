using System;
using Xunit;
using System.Collections.Generic;
using System.Text;
using core.domain;
using core.dto;

namespace core_tests.domain {
    public class ProductMaterialTest {
        private static readonly Material PREDEFINED_MATERIAL = new Material("#444", "Tinta Branca", new List<Color>(new[] { Color.valueOf("White", 0, 0, 0, 0) }), new List<Finish>(new[] { Finish.valueOf("Matte") }));
        private static readonly Material PREDEFINED_MATERIAL2 = new Material("#445", "Tinta Preta", new List<Color>(new[] { Color.valueOf("Black", 255, 255, 255, 255) }), new List<Finish>(new[] { Finish.valueOf("Matte") }));
        private static readonly List<Material> PREDEFINED_MATERIALS = new List<Material>(new[] { PREDEFINED_MATERIAL });
        private static readonly List<Dimension> PREDEFINED_RESTRICTIONS = new List<Dimension>(new[] { SingleValueDimension.valueOf(20f) });
        private static readonly ProductCategory PREDEFEFINED_CATEGORY = new ProductCategory("Test");
        /// <summary>
        /// Ensures addRestriction returns false when argument is null
        /// </summary>
        [Fact]
        public void ensureAddedRestrictionIsNotNull() {
            Console.WriteLine("ensureAddedRestrictionIsNotNull");
            Product p = new Product("#666", "der alte würfelt nicht", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Assert.False(pm.addRestriction(null));
        }
        /// <summary>
        /// Ensures addRestriction returns true when argument is valid
        /// </summary>
        [Fact]
        public void ensureAddRestrictionSucceeds() {
            Console.WriteLine("ensureAddRestrictionSucceeds");
            Product p = new Product("#666", "der alte würfelt nicht", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Restriction rest = new Restriction("restriction");
            Assert.True(pm.addRestriction(rest));
            Assert.True(pm.restrictionExists(rest));
        }
        /// <summary>
        /// Ensures addRestriction returns false when restriction already exists
        /// </summary>
        [Fact]
        public void ensureAddedRestrictionIsUnique() {
            Console.WriteLine("ensureAddedRestrictionIsUnique");
            Product p = new Product("#666", "der alte würfelt nicht", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Restriction rest = new Restriction("restriction");
            pm.addRestriction(rest);
            Assert.False(pm.addRestriction(rest));
        }
        /// <summary>
        /// Ensures restrictionExists returns true when restriction was previously added
        /// </summary>
        [Fact]
        public void ensureRestrictionExistsSucceeds() {
            Console.WriteLine("ensureRestrictionExistsSucceeds");
            Product p = new Product("#666", "der alte würfelt nicht", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Restriction rest = new Restriction("restriction");
            pm.addRestriction(rest);
            Assert.True(pm.restrictionExists(rest));
        }
        /// <summary>
        /// Ensures restrictionExists returns false when argument is null
        /// </summary>
        [Fact]
        public void ensureRestrictionExistsFailsWithNullArgument() {
            Console.WriteLine("ensureRestrictionExistsFailsWithNullArgument");
            Product p = new Product("#666", "der alte würfelt nicht", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Assert.False(pm.restrictionExists(null));
        }
        /// <summary>
        /// Ensures restrictionExists returns false when restriction wasn't previously added
        /// </summary>
        [Fact]
        public void ensureRestrictionExistsFails() {
            Console.WriteLine("ensureRestrictionExistsSucceeds");
            Product p = new Product("#666", "der alte würfelt nicht", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Restriction rest = new Restriction("restriction");
            Assert.False(pm.restrictionExists(rest));
        }
        /// <summary>
        /// Ensures removeRestriction returns false when argument is null
        /// </summary>
        [Fact]
        public void ensureRemoveRestrictionArgumentIsNotNull() {
            Console.WriteLine("ensureRemoveRestrictionArgumentIsNotNull");
            Product p = new Product("#666", "der alte würfelt nicht", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Assert.False(pm.removeRestriction(null));
        }
        /// <summary>
        /// Ensures removeRestriction returns false when restriction wasn't previously added
        /// </summary>
        [Fact]
        public void ensureRemoveRestrictionArgumentExists() {
            Console.WriteLine("ensureRemoveRestrictionArgumentExists");
            Product p = new Product("#666", "der alte würfelt nicht", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Restriction rest = new Restriction("restriction");
            Assert.False(pm.removeRestriction(rest));
        }
        /// <summary>
        /// Ensures removeRestriction returns true when restriction was previously added
        /// </summary>
        [Fact]
        public void ensureRemoveRestrictionSucceeds() {
            Console.WriteLine("ensureRemoveRestrictionSucceeds");
            Product p = new Product("#666", "der alte würfelt nicht", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Restriction rest = new Restriction("restriction");
            pm.addRestriction(rest);
            Assert.True(pm.removeRestriction(rest));
        }
        /// <summary>
        /// Ensures hasMaterial returns false with null argument
        /// </summary>
        [Fact]
        public void ensureHasMaterialFailsWithNullArgument() {
            Console.WriteLine("ensureHasMaterialFailsWithNullArgument");
            Product p = new Product("#666", "der alte würfelt nicht", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Assert.False(pm.hasMaterial(null));
        }
        /// <summary>
        /// Ensures hasMaterial returns false if productmaterial does not contain material
        /// </summary>
        [Fact]
        public void ensureHasMaterialFails() {
            Console.WriteLine("ensureHasMaterialFails");
            Product p = new Product("#666", "der alte würfelt nicht", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2);
            Assert.False(pm.hasMaterial(PREDEFINED_MATERIAL));
        }
        /// <summary>
        /// Ensures hasMaterial returns true if productmaterial contains material
        /// </summary>
        [Fact]
        public void ensureHasMaterialSucceeds() {
            Console.WriteLine("ensureHasMaterialSucceeds");
            Product p = new Product("#666", "der alte würfelt nicht", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2, new List<Restriction>());
            Assert.True(pm.hasMaterial(PREDEFINED_MATERIAL2));
        }
        /// <summary>
        /// Ensures default get method for product attribute works
        /// </summary>
        [Fact]
        public void ensureGetProductWorks() {
            Console.WriteLine("ensureGetProductWorks");
            Product p = new Product("#666", "der alte würfelt nicht", PREDEFEFINED_CATEGORY, PREDEFINED_MATERIALS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS, PREDEFINED_RESTRICTIONS);
            ProductMaterial pm = new ProductMaterial(p, PREDEFINED_MATERIAL2, new List<Restriction>());
            Assert.True(pm.product.Equals(p));
        }
    }
}
