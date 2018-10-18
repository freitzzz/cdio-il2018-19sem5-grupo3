// using System;
// using Xunit;
// using System.Collections.Generic;
// using core.domain;
// using core.dto;
// using support.dto;
// namespace core_tests.domain
// {
//     /**
//     <summary>
//         Tests of the class Component.
//     </summary>
//     */
//     public class ComponentTest
//     {
//         //id tests

//         /**
//         <summary>
//             Test to ensure that the method id works.
//          </summary>
//          */
//         [Fact]
//         public void ensureIdMethodWorks()
//         {
//             Console.WriteLine("ensureIdMethodWorks");
//             Color color = Color.valueOf("Azul", 1, 1, 1, 1);
//             Finish finish = Finish.valueOf("Acabamento polido");
//             ProductCategory prodCat = new ProductCategory("Category 1");
//             List<Double> values2 = new List<Double>();
//             values2.Add(500.0); //Width
//             DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
//             List<Dimension> valuest = new List<Dimension>();
//             valuest.Add(d2);
//             IEnumerable<Dimension> heightDimensions = valuest;
//             IEnumerable<Dimension> widthDimensions = valuest;
//             IEnumerable<Dimension> depthDimensions = valuest;
//             List<Color> colors = new List<Color>();
//             colors.Add(color);

//             List<Finish> finishes = new List<Finish>();
//             finishes.Add(finish);

//             Material material = new Material("1234", "Materail", colors, finishes);
//             List<Material> listMaterial = new List<Material>();
//             listMaterial.Add(material);
//             IEnumerable<Material> materials = listMaterial;
//             Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

//             Component component = new Component(product);

//             Assert.Equal(component.id(), product);
//         }

//         //sameAs tests

//         /**
//         <summary>
//             Test to ensure that the method sameAs works, for two equal identities.
//          </summary>
//          */
//         [Fact]
//         public void ensureComponentsWithEqualIdentitiesAreTheSame()
//         {
//             Console.WriteLine("ensureComponentsWithEqualIdentitiesAreTheSame");

//             Color color = Color.valueOf("Azul", 1, 1, 1, 1);
//             Finish finish = Finish.valueOf("Acabamento polido");
//             ProductCategory prodCat = new ProductCategory("Category 1");
//             List<Double> values2 = new List<Double>();
//             values2.Add(500.0); //Width
//             DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
//             List<Dimension> valuest = new List<Dimension>();
//             valuest.Add(d2);
//             IEnumerable<Dimension> heightDimensions = valuest;
//             IEnumerable<Dimension> widthDimensions = valuest;
//             IEnumerable<Dimension> depthDimensions = valuest;
//             List<Color> colors = new List<Color>();
//             colors.Add(color);

//             List<Finish> finishes = new List<Finish>();
//             finishes.Add(finish);

//             Material material = new Material("1234", "Materail", colors, finishes);
//             List<Material> listMaterial = new List<Material>();
//             listMaterial.Add(material);
//             IEnumerable<Material> materials = listMaterial;
//             Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

//             Component component = new Component(product);

//             Assert.True(component.sameAs(product));
//         }

//         /**
//         <summary>
//             Test to ensure that the method sameAs works, for two different product.
//          </summary>
//          */
//         /**[Fact]
//         public void ensureComponentsWithDifferentIdentitiesAreNotTheSame()
//         {
//             Console.WriteLine("ensureComponentsWithDifferentIdentitiesAreNotTheSame");

//             Color color = Color.valueOf("Azul", 1, 1, 1, 1);
//             Finish finish = Finish.valueOf("Acabamento polido");
//             ProductCategory prodCat = new ProductCategory("Category 1");
//             List<Double> values2 = new List<Double>();
//             values2.Add(500.0); //Width
//             DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
//             List<Dimension> valuest = new List<Dimension>();
//             valuest.Add(d2);
//             IEnumerable<Dimension> heightDimensions = valuest;
//             IEnumerable<Dimension> widthDimensions = valuest;
//             IEnumerable<Dimension> depthDimensions = valuest;
//             List<Color> colors = new List<Color>();
//             colors.Add(color);

//             List<Finish> finishes = new List<Finish>();
//             finishes.Add(finish);

//             Material material = new Material("1234", "Materail", colors, finishes);
//             List<Material> listMaterial = new List<Material>();
//             listMaterial.Add(material);
//             IEnumerable<Material> materials = listMaterial;
//             Product product1 = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

//             Component component1 = new Component(product1);

//             Assert.False(component1.sameAs(product1));
//         }*/

//         //checkComponentProperties tests

//         /**
//         <summary>
//             Test to ensure that the instance of Component isn't built if the product is null.
//         </summary>
//          */
//         [Fact]
//         public void ensureNullProductIsNotValid()
//         {
//             Console.WriteLine("ensureNullProductIsNotValid");

//             Assert.Throws<ArgumentException>(() => new Component(null));
//         }

//         /**
//         <summary>
//             Test to ensure that the instance of Component isn't built if the list of restriction is empty.
//         </summary>
//        */
//         [Fact]
//         public void ensureEmptyRestrictionsIsNotValid()
//         {
//             Console.WriteLine("ensureEmptyRestrictionsIsNotValid");
//             Color color = Color.valueOf("Azul", 1, 1, 1, 1);
//             Finish finish = Finish.valueOf("Acabamento polido");
//             ProductCategory prodCat = new ProductCategory("Category 1");
//             List<Double> values2 = new List<Double>();
//             values2.Add(500.0); //Width
//             DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
//             List<Dimension> valuest = new List<Dimension>();
//             valuest.Add(d2);
//             IEnumerable<Dimension> heightDimensions = valuest;
//             IEnumerable<Dimension> widthDimensions = valuest;
//             IEnumerable<Dimension> depthDimensions = valuest;
//             List<Color> colors = new List<Color>();
//             colors.Add(color);

//             List<Finish> finishes = new List<Finish>();
//             finishes.Add(finish);

//             Material material = new Material("1234", "Materail", colors, finishes);
//             List<Material> listMaterial = new List<Material>();
//             listMaterial.Add(material);
//             IEnumerable<Material> materials = listMaterial;
//             Product product1 = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);
//             List<Restriction> restricions = new List<Restriction>();

//             Assert.Throws<ArgumentException>(() => new Component(product1, restricions));
//         }
//         /**
//         <summary>
//             Test to ensure that the instance of Component isn't built if the list of restriction is null.
//         </summary>
//        */
//         [Fact]
//         public void ensureNullRestrictionsIsNotValid()
//         {
//             Console.WriteLine("ensureNullRestrictionsIsNotValid");
//             Color color = Color.valueOf("Azul", 1, 1, 1, 1);
//             Finish finish = Finish.valueOf("Acabamento polido");
//             ProductCategory prodCat = new ProductCategory("Category 1");
//             List<Double> values2 = new List<Double>();
//             values2.Add(500.0); //Width
//             DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
//             List<Dimension> valuest = new List<Dimension>();
//             valuest.Add(d2);
//             IEnumerable<Dimension> heightDimensions = valuest;
//             IEnumerable<Dimension> widthDimensions = valuest;
//             IEnumerable<Dimension> depthDimensions = valuest;
//             List<Color> colors = new List<Color>();
//             colors.Add(color);

//             List<Finish> finishes = new List<Finish>();
//             finishes.Add(finish);

//             Material material = new Material("1234", "Materail", colors, finishes);
//             List<Material> listMaterial = new List<Material>();
//             listMaterial.Add(material);
//             IEnumerable<Material> materials = listMaterial;
//             Product product1 = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);


//             Assert.Throws<ArgumentException>(() => new Component(product1, null));
//         }

//         //GetHashCode tests

//         /**
//         <summary>
//            Test to ensure that the method GetHashCode works.
//         </summary>
//         */
//         [Fact]
//         public void ensureGetHashCodeWorks()
//         {
//             Console.WriteLine("ensureNullRestrictionsIsNotValid");
//             Color color = Color.valueOf("Azul", 1, 1, 1, 1);
//             Finish finish = Finish.valueOf("Acabamento polido");
//             ProductCategory prodCat = new ProductCategory("Category 1");
//             List<Double> values2 = new List<Double>();
//             values2.Add(500.0); //Width
//             DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
//             List<Dimension> valuest = new List<Dimension>();
//             valuest.Add(d2);
//             IEnumerable<Dimension> heightDimensions = valuest;
//             IEnumerable<Dimension> widthDimensions = valuest;
//             IEnumerable<Dimension> depthDimensions = valuest;
//             List<Color> colors = new List<Color>();
//             colors.Add(color);

//             List<Finish> finishes = new List<Finish>();
//             finishes.Add(finish);

//             Material material = new Material("1234", "Materail", colors, finishes);
//             List<Material> listMaterial = new List<Material>();
//             listMaterial.Add(material);
//             IEnumerable<Material> materials = listMaterial;
//             Product product1 = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);
//             Product product2 = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

//             Component component1 = new Component(product1);
//             Component component2 = new Component(product2);

//             Assert.Equal(component1.GetHashCode(), component2.GetHashCode());
//         }

//         //Equals tests

//         /**
//         <summary>
//             Test to ensure that the method Equals works, for two Components with different product.
//          </summary>
//          */
//         [Fact]
//         public void ensureComponentsWithDifferentReferencesAreNotEqual()
//         {
//             Console.WriteLine("ensureComponentsWithDifferentReferencesAreNotEqual");

//             Color color = Color.valueOf("Azul", 1, 1, 1, 1);
//             Finish finish = Finish.valueOf("Acabamento polido");
//             ProductCategory prodCat = new ProductCategory("Category 1");
//             List<Double> values2 = new List<Double>();
//             values2.Add(500.0); //Width
//             DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
//             List<Dimension> valuest = new List<Dimension>();
//             valuest.Add(d2);
//             IEnumerable<Dimension> heightDimensions = valuest;
//             IEnumerable<Dimension> widthDimensions = valuest;
//             IEnumerable<Dimension> depthDimensions = valuest;
//             List<Color> colors = new List<Color>();
//             colors.Add(color);

//             List<Finish> finishes = new List<Finish>();
//             finishes.Add(finish);

//             Material material = new Material("1234", "Materail", colors, finishes);
//             List<Material> listMaterial = new List<Material>();
//             listMaterial.Add(material);
//             IEnumerable<Material> materials = listMaterial;
//             Product product1 = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);
//             Product product2 = new Product("456", "product2", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

//             Component component1 = new Component(product1);
//             Component component2 = new Component(product2);

//             Assert.False(component1.Equals(component2));
//         }

//         /**
//         <summary>
//             Test to ensure that the method Equals works, for two Components with the same product.
//          </summary>
//          */
//         [Fact]
//         public void ensureComponentsWithSameReferencesAreEqual()
//         {
//             Console.WriteLine("ensureComponentsWithSameReferencesAreEqual");

//             Color color = Color.valueOf("Azul", 1, 1, 1, 1);
//             Finish finish = Finish.valueOf("Acabamento polido");
//             ProductCategory prodCat = new ProductCategory("Category 1");
//             List<Double> values2 = new List<Double>();
//             values2.Add(500.0); //Width
//             DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
//             List<Dimension> valuest = new List<Dimension>();
//             valuest.Add(d2);
//             IEnumerable<Dimension> heightDimensions = valuest;
//             IEnumerable<Dimension> widthDimensions = valuest;
//             IEnumerable<Dimension> depthDimensions = valuest;
//             List<Color> colors = new List<Color>();
//             colors.Add(color);

//             List<Finish> finishes = new List<Finish>();
//             finishes.Add(finish);

//             Material material = new Material("1234", "Materail", colors, finishes);
//             List<Material> listMaterial = new List<Material>();
//             listMaterial.Add(material);
//             IEnumerable<Material> materials = listMaterial;
//             Product product1 = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

//             Component component1 = new Component(product1);
//             Component component2 = new Component(product1);
//             Assert.True(component1.Equals(component2));
//         }
//         /**
//         <summary>
//             Test to ensure that the method Equals works, for two Components with the diferent product.
//          </summary>
//          */
//         [Fact]
//         public void ensureComponentsWithDiferentReferencesAreEqual()
//         {
//             Console.WriteLine("ensureComponentsWithDiferentReferencesAreEqual");

//             Color color = Color.valueOf("Azul", 1, 1, 1, 1);
//             Finish finish = Finish.valueOf("Acabamento polido");
//             ProductCategory prodCat = new ProductCategory("Category 1");
//             List<Double> values2 = new List<Double>();
//             values2.Add(500.0); //Width
//             DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
//             List<Dimension> valuest = new List<Dimension>();
//             valuest.Add(d2);
//             IEnumerable<Dimension> heightDimensions = valuest;
//             IEnumerable<Dimension> widthDimensions = valuest;
//             IEnumerable<Dimension> depthDimensions = valuest;
//             List<Color> colors = new List<Color>();
//             colors.Add(color);

//             List<Finish> finishes = new List<Finish>();
//             finishes.Add(finish);

//             Material material = new Material("1234", "Materail", colors, finishes);
//             List<Material> listMaterial = new List<Material>();
//             listMaterial.Add(material);
//             IEnumerable<Material> materials = listMaterial;
//             Product product1 = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);
//             Product product2 = new Product("456", "product2", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

//             Component component1 = new Component(product1);
//             Component component2 = new Component(product2);
//             Assert.False(component1.Equals(component2));
//         }

//         /**
//         <summary>
//             Test to ensure that the method Equals works, for a null Component.
//          </summary>
//          */
//         [Fact]
//         public void ensureNullObjectIsNotEqual()
//         {
//             Console.WriteLine("ensureNullObjectIsNotEqual");

//             Color color = Color.valueOf("Azul", 1, 1, 1, 1);
//             Finish finish = Finish.valueOf("Acabamento polido");
//             ProductCategory prodCat = new ProductCategory("Category 1");
//             List<Double> values2 = new List<Double>();
//             values2.Add(500.0); //Width
//             DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
//             List<Dimension> valuest = new List<Dimension>();
//             valuest.Add(d2);
//             IEnumerable<Dimension> heightDimensions = valuest;
//             IEnumerable<Dimension> widthDimensions = valuest;
//             IEnumerable<Dimension> depthDimensions = valuest;
//             List<Color> colors = new List<Color>();
//             colors.Add(color);

//             List<Finish> finishes = new List<Finish>();
//             finishes.Add(finish);

//             Material material = new Material("1234", "Materail", colors, finishes);
//             List<Material> listMaterial = new List<Material>();
//             listMaterial.Add(material);
//             IEnumerable<Material> materials = listMaterial;
//             Product product1 = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

//             Component component1 = new Component(product1);

//             Assert.False(component1.Equals(null));
//         }

//         /**
//         <summary>
//             Test to ensure that the method Equals works, for a Component and an object of another type.
//          </summary>
//          */
//         [Fact]
//         public void ensureDifferentTypesAreNotEqual()
//         {
//             Console.WriteLine("ensureDifferentTypesAreNotEqual");

//             Color color = Color.valueOf("Azul", 1, 1, 1, 1);
//             Finish finish = Finish.valueOf("Acabamento polido");
//             ProductCategory prodCat = new ProductCategory("Category 1");
//             List<Double> values2 = new List<Double>();
//             values2.Add(500.0); //Width
//             DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
//             List<Dimension> valuest = new List<Dimension>();
//             valuest.Add(d2);
//             IEnumerable<Dimension> heightDimensions = valuest;
//             IEnumerable<Dimension> widthDimensions = valuest;
//             IEnumerable<Dimension> depthDimensions = valuest;
//             List<Color> colors = new List<Color>();
//             colors.Add(color);

//             List<Finish> finishes = new List<Finish>();
//             finishes.Add(finish);

//             Material material = new Material("1234", "Materail", colors, finishes);
//             List<Material> listMaterial = new List<Material>();
//             listMaterial.Add(material);
//             IEnumerable<Material> materials = listMaterial;
//             Product product1 = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

//             Component component1 = new Component(product1);
//             Assert.False(product1.Equals("stars"));
//         }

//         //ToString tests

//         /**
//         <summary>
//             Test to ensure that the method ToString works.
//          </summary>
//          */
//         [Fact]
//         public void ensureToStringWorks()
//         {
//             Console.WriteLine("ensureToStringWorks");

//             Color color = Color.valueOf("Azul", 1, 1, 1, 1);
//             Finish finish = Finish.valueOf("Acabamento polido");
//             ProductCategory prodCat = new ProductCategory("Category 1");
//             List<Double> values2 = new List<Double>();
//             values2.Add(500.0); //Width
//             DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
//             List<Dimension> valuest = new List<Dimension>();
//             valuest.Add(d2);
//             IEnumerable<Dimension> heightDimensions = valuest;
//             IEnumerable<Dimension> widthDimensions = valuest;
//             IEnumerable<Dimension> depthDimensions = valuest;
//             List<Color> colors = new List<Color>();
//             colors.Add(color);

//             List<Finish> finishes = new List<Finish>();
//             finishes.Add(finish);

//             Material material = new Material("1234", "Materail", colors, finishes);
//             List<Material> listMaterial = new List<Material>();
//             listMaterial.Add(material);
//             IEnumerable<Material> materials = listMaterial;
//             Product product1 = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

//             Component component1 = new Component(product1);
//             Component component2 = new Component(product1);

//             Assert.Equal(component1.ToString(), component2.ToString());
//         }
//         [Fact]
//         public void testToDTO()
//         {
//             Console.WriteLine("toDTO");
//             Color color = Color.valueOf("Azul", 1, 1, 1, 1);
//             Finish finish = Finish.valueOf("Acabamento polido");
//             ProductCategory prodCat = new ProductCategory("Category 1");
//             List<Double> values2 = new List<Double>();
//             values2.Add(500.0); //Width
//             DiscreteDimensionInterval d2 = new DiscreteDimensionInterval(values2);
//             List<Dimension> valuest = new List<Dimension>();
//             valuest.Add(d2);
//             IEnumerable<Dimension> heightDimensions = valuest;
//             IEnumerable<Dimension> widthDimensions = valuest;
//             IEnumerable<Dimension> depthDimensions = valuest;
//             List<Color> colors = new List<Color>();
//             colors.Add(color);

//             List<Finish> finishes = new List<Finish>();
//             finishes.Add(finish);

//             Material material = new Material("1234", "Materail", colors, finishes);
//             List<Material> listMaterial = new List<Material>();
//             listMaterial.Add(material);
//             IEnumerable<Material> materials = listMaterial;
//             Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

//             Component component = new Component(product);

//             ComponentDTO expected = new ComponentDTO();
//             expected.product = product.toDTO();

//             ComponentDTO actual = component.toDTO();
//             Assert.Equal(expected.product.complements, actual.product.complements);
//             Assert.Equal(expected.product.designation, actual.product.designation);
//             Assert.Equal(expected.product.dimensions.depthDimensionDTOs.Capacity, actual.product.dimensions.depthDimensionDTOs.Capacity);
//             Assert.Equal(expected.product.dimensions.depthDimensionDTOs.Count, actual.product.dimensions.depthDimensionDTOs.Count);
//             Assert.Equal(expected.product.dimensions.heightDimensionDTOs.Count, actual.product.dimensions.heightDimensionDTOs.Count);
//             Assert.Equal(expected.product.dimensions.heightDimensionDTOs.Capacity, actual.product.dimensions.heightDimensionDTOs.Capacity);
//             Assert.Equal(expected.product.dimensions.widthDimensionDTOs.Capacity, actual.product.dimensions.widthDimensionDTOs.Capacity);
//             Assert.Equal(expected.product.dimensions.widthDimensionDTOs.Count, actual.product.dimensions.widthDimensionDTOs.Count);
//             Assert.Equal(expected.product.productCategory.name, actual.product.productCategory.name);
//             Assert.Equal(expected.product.productCategory.parentId, actual.product.productCategory.parentId);
//             Assert.Equal(expected.product.productMaterials.Count, actual.product.productMaterials.Count);
//             Assert.Equal(expected.product.productMaterials.Capacity, actual.product.productMaterials.Capacity);
//             Assert.Equal(expected.product.reference, actual.product.reference);
//             Assert.Equal(expected.product.id, actual.product.id);
//         }
//     }
// }