using Xunit;
using core.domain;
using System;

namespace core_tests.domain
{
    public class ProductCategoryTest
    {

        [Fact]
        public void ensureInstanceCantBeCreateWithNullName()
        {
            Action action = () => new ProductCategory(null);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ensureInstanceCantBeCreatedWithEmptyName()
        {
            Action action = () => new ProductCategory("");

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ensureInstanceCantBeCreatedWithWhitespaces()
        {
            Action action = () => new ProductCategory("         ");

            Assert.Throws<ArgumentException>(action);
        }


        [Fact]
        public void ensureInstanceIsCreated()
        {
            var category = new ProductCategory("Drawers");

            Assert.NotNull(category);
        }

        [Fact]
        public void ensureInstanceCanNotBeCreatedWithANullParent()
        {
            Action action = () => new ProductCategory("Drawers", null);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ensureInstanceCanNotBeCreatedIfParentHasTheSameName()
        {

            var parent = new ProductCategory("Drawers");

            Action action = () => new ProductCategory("Drawers", parent);

            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void ensureInstanceIsCreatedIfParentHasDifferentName()
        {
            var parent = new ProductCategory("Drawers");

            var category = new ProductCategory("Wooden Drawers", parent);

            Assert.NotNull(category);
        }

        [Fact]
        public void ensureChangeNameWithNullNameReturnsFalse()
        {
            var category = new ProductCategory("Drawers");

            bool changed = category.changeName(null);

            Assert.False(changed);
        }

        [Fact]
        public void ensureChangeNameWithNullNameDoesNotChangeName()
        {

            string name = "Drawers";

            var category = new ProductCategory(name);

            category.changeName(null);

            string currentName = category.name;

            Assert.Equal(name, currentName);
        }

        [Fact]
        public void ensureChangeNameWithEmptyNameReturnsFalse()
        {
            var category = new ProductCategory("Drawers");

            bool changed = category.changeName("");

            Assert.False(changed);
        }

        [Fact]
        public void ensureChangeNameWithEmptyNameDoesNotChangeName()
        {
            string name = "Drawers";

            var category = new ProductCategory(name);

            category.changeName("");

            string currentName = category.name;

            Assert.Equal(name, currentName);
        }

        [Fact]
        public void ensureChangeNameWithWhitespacesReturnsFalse()
        {
            var category = new ProductCategory("Drawers");

            Assert.False(category.changeName("         "));
        }

        [Fact]
        public void ensureChangeNameWithWhitespacesDoesNotChangeName()
        {
            string name = "Drawers";

            var category = new ProductCategory(name);

            category.changeName("           ");

            string currentName = category.name;

            Assert.Equal(name, currentName);
        }

        [Fact]
        public void ensureChangeNameWithValidNameReturnsTrue()
        {
            var category = new ProductCategory("Hangers");

            Assert.True(category.changeName("Shelves"));
        }

        [Fact]
        public void ensureChangeNameWithValidNameChangesName()
        {
            var category = new ProductCategory("Drawers");

            string newName = "Coat-hangers";

            category.changeName(newName);

            string currentName = category.name;

            Assert.Equal(newName, currentName);
        }


        [Fact]
        public void ensureActivateWorks()
        {
            var category = new ProductCategory("Pritchett's Closets");
            category.activate();

            Assert.True(category.isActive());
        }

        [Fact]
        public void ensureDeactivateWorks()
        {
            var category = new ProductCategory("Closets Closets Closets");
            category.deactivate();

            Assert.False(category.isActive());

        }

        [Fact]
        public void ensureEqualsReturnsFalseIfObjectIsNull()
        {
            ProductCategory category = new ProductCategory("Drawers");

            ProductCategory invalidCategory = null;

            Assert.False(category.Equals(invalidCategory));
        }


        [Fact]
        public void ensureEqualsReturnsFalseIfObjectIsNotCategory()
        {

            ProductCategory category = new ProductCategory("Drawers");

            Assert.False(category.Equals("product"));

        }


        [Fact]
        public void ensureEqualsReturnsTrueIfSameCategoryIsUsed()
        {
            ProductCategory category = new ProductCategory("Drawers");

            Assert.True(category.Equals(category));
        }

        [Fact]
        public void ensureEqualsReturnsTrueIfCategoriesHaveSameName()
        {
            ProductCategory category = new ProductCategory("Drawers");

            ProductCategory otherCategory = new ProductCategory("drawers");

            Assert.Equal(category, otherCategory);
        }


        [Fact]
        public void ensureCategoriesWithSameNameHaveSameHashCode()
        {
            ProductCategory category = new ProductCategory("Drawers");

            int hashCode = category.GetHashCode();

            ProductCategory otherCategory = new ProductCategory("drawers");

            int otherHashCode = otherCategory.GetHashCode();

            Assert.Equal(hashCode, otherHashCode);
        }

        [Fact]
        public void ensureCategoriesWithDifferentNamesHaveDifferentHashCodes()
        {
            ProductCategory category = new ProductCategory("Drawers");

            int hashCode = category.GetHashCode();

            ProductCategory otherCategory = new ProductCategory("Shelves");

            int otherHashCode = otherCategory.GetHashCode();

            Assert.NotEqual(hashCode, otherHashCode);
        }

        [Fact]
        public void ensureIdReturnsTheCategoryName()
        {
            ProductCategory category = new ProductCategory("Drawers");

            string id = category.id();

            string expected = "Drawers";

            Assert.Equal(expected, id);
        }

        [Fact]
        public void ensureToStringWorks()
        {
            ProductCategory category = new ProductCategory("Drawers");

            string actual = category.ToString();

            string expected = "Name: " + category.id();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ensureToDTOWorks()
        {
            var category = new ProductCategory("Shelves");

            var categoryDTO = category.toDTO().ToString();

            var otherCategory = new ProductCategory("Shelves");

            var otherCategoryDTO = otherCategory.toDTO().ToString();

            Assert.Equal(otherCategoryDTO, categoryDTO);
        }

        [Fact]
        public void ensureSameAsReturnsTrueIfIdentityIsEqual()
        {

            string identity = "Shelves";

            var category = new ProductCategory(identity);

            Assert.True(category.sameAs(identity));
        }

        [Fact]
        public void ensureSameAsReturnsFalseIfIdentityIsNotEqual()
        {
            string identity = "Shelves";

            var category = new ProductCategory(identity);

            string otherIdentity = "Drawer";

            Assert.False(category.sameAs(otherIdentity));
        }

    }
}