using System;
using System.Linq;
using System.Collections.Generic;
using support.utils;

namespace core.domain
{
    /// <summary>
    /// Represents a Tree with ProductCategories as Nodes.
    /// </summary>
    public class CategoriesTree
    {

        /// <summary>
        /// Long that represents the database ID.
        /// </summary>
        private long id;

        /// <summary>
        /// Node that contains the root of the Tree.
        /// </summary>
        private Node root;

        /// <summary>
        /// Size of the Tree (number of Nodes).
        /// </summary>
        private int nodes;

        /// <summary>
        /// Builds as instance of CategoriesTree.
        /// </summary>
        public CategoriesTree()
        {
            root = new Node(new ProductCategory("Root"));
            nodes = 0; //Number of nodes is set to 0 to ignore the Root ProductCategory
        }

        /// <summary>
        /// Returns the size of the Tree (number of Nodes excluding the Root Node).
        /// </summary>
        /// <returns>the number of Categories in the Tree</returns>
        public int size()
        {
            return this.nodes;
        }

        /// <summary>
        /// Finds all Nodes within the Tree.
        /// </summary>
        /// <returns>List with all Categories</returns>
        public List<ProductCategory> getAllCategories()
        {
            List<Node> nodes = new List<Node>();
            getAllNodes(nodes, root);
            nodes.Remove(root);

            List<ProductCategory> categories = new List<ProductCategory>();
            foreach (Node node in nodes)
            {
                categories.Add(node.getElement());
            }

            return categories;
        }

        /// <summary>
        /// Auxiliar method that finds all Nodes within the Tree.
        /// Recursively adds the Nodes to the List received as a parameter.
        /// </summary>
        /// <param name="nodes">List to fill with the Nodes</param>
        /// <param name="node">Node to search</param>
        private void getAllNodes(List<Node> nodes, Node node)
        {
            nodes.Add(node);

            foreach (Node child in node.getChildren())
            {
                getAllNodes(nodes, child);
            }
        }

        /// <summary>
        /// Adds a new Node to the Tree.
        /// </summary>
        /// <param name="childElement">ProductCategory to add</param>
        /// <param name="parent">ProductCategory parent of the one to be added</param>
        /// <returns>true if the ProductCategory is successfully added, false if not</returns>
        public bool addCategory(ProductCategory childElement, ProductCategory parent)
        {
            if (childElement == null || parent == null) return false;

            Node parentNode = findCategoryNode(parent);

            if (parentNode == null) return false;

            if (!parentNode.addChild(new Node(childElement))) return false;

            nodes++;
            return true;
        }

        /// <summary>
        /// Deactivates a Node from the Tree and all its children.
        /// </summary>
        /// <param name="category"></param>
        /// <returns>true if the Category is successfully removed, false if not</returns>
        public bool removeCategory(ProductCategory category)
        {
            if (category == null) return false;

            Node node = findCategoryNode(category);
            if (node == null) return false;

            removeCategory(node);
            return true;
        }

        /// <summary>
        /// Auxiliar method that deactivates a Node from the Tree and all its children.
        /// Recursively deactivates the Node received as a parameter.
        /// </summary>
        /// <param name="node">Node to deactivate</param>
        private void removeCategory(Node node)
        {
            node.getElement().deactivate();
            nodes--;
            
            if (!node.isLeaf())
            {
                foreach (Node child in node.getChildren())
                {
                    removeCategory(child);
                }
            }
        }

        /// <summary>
        /// Finds a Node within the Tree.
        /// </summary>
        /// <param name="category">ProductCategory in question</param>
        /// <returns>The Node which element is the received ProductCategory, null if it isn't found</returns>
        private Node findCategoryNode(ProductCategory category)
        {
            if (category.Equals(root.getElement())) return root; //If the ProductCategory is the root, then its Node is the root itself

            List<Node> nodes = new List<Node>();
            getAllNodes(nodes, root);

            foreach (Node node in nodes)
            {
                if (node.getElement().Equals(category))
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a Node's parent within the Tree.
        /// </summary>
        /// <param name="category">ProductCategory in question</param>
        /// <returns>the element (ProductCategory) of the parent Node</returns>
        public ProductCategory findCategoryParent(ProductCategory category)
        {
            if(category == null) return null;

            if (category.Equals(root.getElement())) return null; //If the ProductCategory is the root, then there is no parent

            Node catNode = findCategoryNode(category);
            if(catNode == null) return null;

            List<Node> nodes = new List<Node>();
            getAllNodes(nodes, root);

            foreach (Node node in nodes)
            {
                if (node.getChildren().Contains(catNode))
                {
                    return node.getElement();
                }
            }
            return null;
        }

        /// <summary>
        /// Finds a Node's children within the Tree.
        /// </summary>
        /// <param name="category">ProductCategory in question</param>
        /// <returns>List of elements (ProductCategory) of the Node's children</returns>
        public List<ProductCategory> findCategoryChildren(ProductCategory category)
        {
            if (category == null) return null;

            Node node = findCategoryNode(category);
            if (node == null) return null;

            List<ProductCategory> children = new List<ProductCategory>();
            if (node.isLeaf()) return children;

            foreach (Node child in node.getChildren())
            {
                children.Add(child.getElement());
            }
            return children;
        }

        /// <summary>
        /// Finds all leaves within the Tree.
        /// </summary>
        /// <returns>all the Categories that are leaves in the Tree</returns>
        public List<ProductCategory> findAllLeaves()
        {
            List<ProductCategory> leaves = new List<ProductCategory>();
            findAllLeaves(leaves, root);
            return leaves;
        }

        /// <summary>
        /// Auxiliar method that finds all Nodes that are leaves (have no children).
        /// </summary>
        /// <param name="leaves">List to fill with all the leaves of the Tree</param>
        /// <param name="node">Node to search</param>
        private void findAllLeaves(List<ProductCategory> leaves, Node node)
        {
            if (node.isLeaf())
            {
                leaves.Add(node.getElement());
                return;
            }

            foreach (Node child in node.getChildren())
            {
                findAllLeaves(leaves, child);
            }
        }

        /// <summary>
        /// Inner class that represents a Node of the Tree.
        /// </summary>
        public class Node
        {
            /// <summary>
            /// Parent Node of the current Node.
            /// </summary>
            private Node parent;

            /// <summary>
            /// Current Node's children.
            /// </summary>
            private HashSet<Node> children = new HashSet<Node>();

            /// <summary>
            /// Element of the current Node.
            /// </summary>
            private ProductCategory element;

            /// <summary>
            /// Builds a new instance of ProductCategory receiving its element.
            /// </summary>
            /// <param name="element">Element of the new Node</param>
            public Node(ProductCategory element)
            {
                this.element = element;
            }

            /// <summary>
            /// Returns the Node's element.
            /// </summary>
            /// <returns>the element of the Node</returns>
            public ProductCategory getElement()
            {
                return this.element;
            }

            /// <summary>
            /// Returns the Node's parent.
            /// </summary>
            /// <returns>the parent Node of the current Node</returns>
            public Node getParent()
            {
                return this.parent;
            }

            /// <summary>
            /// Returns the Node's children.
            /// </summary>
            /// <returns>the children of the current Node</returns>
            public HashSet<Node> getChildren()
            {
                return this.children;
            }

            /// <summary>
            /// Checks if a Node is a leaf of the Tree.
            /// </summary>
            /// <returns>true if the Node is a leaf, false if not</returns>
            public bool isLeaf()
            {
                return this.children.Count == 0;
            }

            /// <summary>
            /// Adds a new child Node to the list of children of the current Node.
            /// </summary>
            /// <param name="child">Node to add</param>
            /// <returns>true if the child is successfully added, false if not</returns>
            public bool addChild(Node child)
            {
                if (child == null)
                {
                    return false;
                }

                child.parent = this;
                return children.Add(child);
            }

            /// <summary>
            /// Returns a textual description of the Node.
            /// </summary>
            /// <returns>String with the Node's element (ProductCategory info)</returns>
            public override string ToString()
            {
                return string.Format("Node info (element): {0}", element.ToString());
            }

            /// <summary>
            /// Returns the generated hash code of the Node.
            /// </summary>
            /// <returns>the Node's hash code</returns>
            public override int GetHashCode()
            {
                return element.GetHashCode();
            }

            /// <summary>
            /// Checks if a certain Node is the same as a received object.
            /// </summary>
            /// <param name="obj">Object to compare</param>
            /// <returns>true if both are equal, false if not</returns>
            public override bool Equals(object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    Node node = (Node)obj;
                    return element.Equals(node.element);
                }
            }
        }
    }
}