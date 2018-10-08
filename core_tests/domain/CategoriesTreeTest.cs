using System;
using Xunit;
using core.domain;
using System.Collections.Generic;

namespace core_tests.domain
{

    /// <summary>
    /// Tests of the class CategoriesTree.
    /// </summary>
    public class CategoriesTreeTest
    {

        /// <summary>
        /// Test to ensure that a valid ProductCategory is added.
        /// </summary>
        [Fact]
        public void ensureAddCategoryWorks()
        {
            CategoriesTree tree = new CategoriesTree();
            Assert.True(tree.addCategory(new ProductCategory("Child"), new ProductCategory("Root")));
        }

        /// <summary>
        /// Test to ensure that a null child ProductCategory is not added.
        /// </summary>
        [Fact]
        public void ensureAddCategoryFailsForNullChild()
        {
            CategoriesTree tree = new CategoriesTree();
            Assert.False(tree.addCategory(null, new ProductCategory("Root")));
        }


        /// <summary>
        /// Test to ensure that a null parent ProductCategory is not added.
        /// </summary>
        [Fact]
        public void ensureAddCategoryFailsForNullParent()
        {
            CategoriesTree tree = new CategoriesTree();
            Assert.False(tree.addCategory(new ProductCategory("Child"), null));
        }


        /// <summary>
        /// Test to ensure that a child ProductCategory is not added for a non-existent parent.
        /// </summary>
        [Fact]
        public void ensureAddCategoryFailsForNonExistentParent()
        {
            CategoriesTree tree = new CategoriesTree();
            Assert.False(tree.addCategory(new ProductCategory("Child"), new ProductCategory("Parent")));
        }

        /// <summary>
        /// Test to ensure that an already existent child ProductCategory is not added (since the children are stored in an HashSet).
        /// </summary>
        [Fact]
        public void ensureAddCategoryFailsForAlreadyExistentChild()
        {
            CategoriesTree tree = new CategoriesTree();

            ProductCategory parent = new ProductCategory("Root");
            ProductCategory child = new ProductCategory("Child");

            tree.addCategory(child, parent);
            Assert.False(tree.addCategory(child, parent));
        }

        /// <summary>
        /// Test to ensure that all the children ProductCategories are removed when its parent is removed.
        /// </summary>
        [Fact]
        public void ensureRemoveCategoryWorksForTheParentAndItsChildren()
        {
            CategoriesTree tree = new CategoriesTree();
            ProductCategory test = new ProductCategory("Anakin");
            tree.addCategory(test, new ProductCategory("Root"));
            tree.addCategory(new ProductCategory("Luke"), new ProductCategory("Anakin"));

            tree.removeCategory(test);

            Assert.True(tree.size() == 0); //Both the parent and the child Nodes were removed
        }

        /// <summary>
        /// Test to ensure that a non-existent Node cannot be removed.
        /// </summary>
        [Fact]
        public void ensureRemoveCategoryFailsForNonExistentNode()
        {
            CategoriesTree tree = new CategoriesTree();
            Assert.False(tree.removeCategory(new ProductCategory("I do not exist")));
        }

        /// <summary>
        /// Test to ensure that a null ProductCategory cannot be removed.
        /// </summary>
        [Fact]
        public void ensureRemoveCategoryFailsForNullNode()
        {
            CategoriesTree tree = new CategoriesTree();
            Assert.False(tree.removeCategory(null));
        }

        /// <summary>
        /// Test to ensure that all ProductCategories within the CategoriesTree are returned, except the Root ProductCategory.
        /// </summary>
        [Fact]
        public void ensureGetAllCategoriesWorks()
        {
            CategoriesTree tree = new CategoriesTree();

            ProductCategory parent = new ProductCategory("Parent");
            ProductCategory child = new ProductCategory("Child");

            tree.addCategory(parent, new ProductCategory("Root"));
            tree.addCategory(child, parent);

            List<ProductCategory> categories = new List<ProductCategory>();

            categories.Add(parent);
            categories.Add(child);
            //The Root ProductCategory is not added because it isn't an actual ProductCategory

            Assert.Equal(categories, tree.getAllCategories());
        }

        /// <summary>
        /// Test to ensure that a null ProductCategory's parent is null.
        /// </summary>
        [Fact]
        public void ensureFindCategoryParentIsNullForNullCategory()
        {
            CategoriesTree tree = new CategoriesTree();
            Assert.Null(tree.findCategoryParent(null));
        }

        /// <summary>
        /// Test to ensure that the Root ProductCategory's parent is null.
        /// </summary>
        [Fact]
        public void ensureFindCategoryParentIsNullForRootNode()
        {
            CategoriesTree tree = new CategoriesTree();
            Assert.Null(tree.findCategoryParent(new ProductCategory("Root")));
        }

        /// <summary>
        /// Test to ensure that a non-existent ProductCategory's parent is null.
        /// </summary>
        [Fact]
        public void ensureFindCategoryParentFailsForNonExistentNode()
        {
            CategoriesTree tree = new CategoriesTree();
            Assert.Null(tree.findCategoryParent(new ProductCategory("I am not real")));
        }

        /// <summary>
        /// Test to ensure that the parent of a valid ProductCategory is found.
        /// </summary>
        [Fact]
        public void ensureFindCategoryParentWorksForValidNode()
        {
            CategoriesTree tree = new CategoriesTree();
            ProductCategory parent = new ProductCategory("Parent");
            ProductCategory child = new ProductCategory("Child");

            tree.addCategory(parent, new ProductCategory("Root"));
            tree.addCategory(child, parent);

            Assert.Equal(parent, tree.findCategoryParent(child));
        }

        /// <summary>
        /// Test to ensure that the children of a null ProductCategory are null.
        /// </summary>
        [Fact]
        public void ensureFindCategoryChildrenIsNullForNullNode()
        {
            CategoriesTree tree = new CategoriesTree();
            Assert.Null(tree.findCategoryChildren(null));
        }

        /// <summary>
        /// Test to ensure that the children of a non-existent ProductCategory are null.
        /// </summary>
        [Fact]
        public void ensureFindCategoryChildrenIsNullForNonExistentNode()
        {
            CategoriesTree tree = new CategoriesTree();
            Assert.Null(tree.findCategoryChildren(new ProductCategory("Fake news")));
        }

        /// <summary>
        /// Test to ensure that the children of a leaf ProductCategory are an empty collection.
        /// </summary>
        [Fact]
        public void ensureFindCategoryChildrenIsEmptyForLeafNode()
        {
            CategoriesTree tree = new CategoriesTree();
            ProductCategory leaf = new ProductCategory("I am a leaf");
            tree.addCategory(leaf, new ProductCategory("Root"));

            Assert.Empty(tree.findCategoryChildren(leaf));
        }

        /// <summary>
        /// Test to ensure that the children of a valid ProductCategory are found.
        /// </summary>
        [Fact]
        public void ensureFindCategoryChildrenContainsExpectedChildren()
        {
            CategoriesTree tree = new CategoriesTree();
            ProductCategory parent = new ProductCategory("Parent");
            ProductCategory child1 = new ProductCategory("Kiddo");
            ProductCategory child2 = new ProductCategory("Second Kiddo");

            tree.addCategory(parent, new ProductCategory("Root"));
            tree.addCategory(child1, parent);
            tree.addCategory(child2, parent);

            List<ProductCategory> categories = new List<ProductCategory>();
            categories.Add(child1);
            categories.Add(child2);

            Assert.Equal(categories, tree.findCategoryChildren(parent));
        }

        /// <summary>
        /// Test to ensure that all leaves are found.
        /// </summary>
        [Fact]
        public void ensureFindAllLeavesWorks()
        {
            CategoriesTree tree = new CategoriesTree();
            ProductCategory parent = new ProductCategory("I'm not on the list");
            ProductCategory leaf1 = new ProductCategory("Imma leaf");
            ProductCategory leaf2 = new ProductCategory("Omagod I'm also a leaf");

            tree.addCategory(parent, new ProductCategory("Root"));
            tree.addCategory(leaf1, parent);
            tree.addCategory(leaf2, parent);

            List<ProductCategory> leaves = new List<ProductCategory>();
            leaves.Add(leaf1);
            leaves.Add(leaf2);

            Assert.Equal(leaves, tree.findAllLeaves());
        }
    }
}
