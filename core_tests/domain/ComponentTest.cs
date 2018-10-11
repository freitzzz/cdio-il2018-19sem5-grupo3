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
        Tests of the class Component.
    </summary>
    */
    public class ComponentTest
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
            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Materail", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            Component component = new Component(product);

            Assert.Equal(component.id(), product);
        }

        //sameAs tests

        /**
        <summary>
            Test to ensure that the method sameAs works, for two equal identities.
         </summary>
         */
        [Fact]
        public void ensureComponentsWithEqualIdentitiesAreTheSame()
        {
            Console.WriteLine("ensureComponentsWithEqualIdentitiesAreTheSame");

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Materail", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            Component component = new Component(product);

            Assert.True(component.sameAs(product));
        }

        /**
        <summary>
            Test to ensure that the method sameAs works, for two different identities.
         </summary>
         */
        [Fact]
        public void ensureMaterialsWithDifferentIdentitiesAreNotTheSame()
        {
            Console.WriteLine("ensureMaterialsWithDifferentIdentitiesAreNotTheSame");

            Color color = Color.valueOf("Azul", 1, 1, 1, 1);
            Finish finish = Finish.valueOf("Acabamento polido");
            ProductCategory prodCat = new ProductCategory("Category 1");
            List<Double> values2 = new List<Double>();
            values2.Add(500.0); //Width
            DiscreteDimensionInterval d2 = DiscreteDimensionInterval.valueOf(values2);
            List<Dimension> valuest = new List<Dimension>();
            valuest.Add(d2);
            IEnumerable<Dimension> heightDimensions = valuest;
            IEnumerable<Dimension> widthDimensions = valuest;
            IEnumerable<Dimension> depthDimensions = valuest;
            List<Color> colors = new List<Color>();
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            finishes.Add(finish);

            Material material = new Material("1234", "Materail", colors, finishes);
            List<Material> listMaterial = new List<Material>();
            listMaterial.Add(material);
            IEnumerable<Material> materials = listMaterial;
            Product product1 = new Product("123", "product1", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);
            Product product2 = new Product("123456789", "product2", prodCat, materials, heightDimensions, widthDimensions, depthDimensions);

            Component component = new Component(product1);

            Assert.False(component.sameAs(product2));
        }

        //checkComponentProperties tests

        /**
        <summary>
            Test to ensure that the instance of Component isn't built if the reference is null.
        </summary>
         */
        [Fact]
        public void ensureNullReferenceIsNotValid()
        {
            Console.WriteLine("ensureNullReferenceIsNotValid");

            List<Restriction> restricions = new List<Restriction>();


            Assert.Throws<ArgumentException>(() => new Component(null, restricions));
        }

        /**
        <summary>
            Test to ensure that the instance of Material isn't built if the reference is empty.
        </summary>
       */
        [Fact]
        public void ensureEmptyReferenceIsNotValid()
        {
            Console.WriteLine("ensureEmptyReferenceIsNotValid");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Gua-ca-mole", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Feio");
            finishes.Add(finish);

            Assert.Throws<ArgumentException>(() => new Material("", "Let me see...", colors, finishes));
        }

        /**
        <summary>
            Test to ensure that the instance of Material isn't built if the designation is null.
        </summary>
       */
        [Fact]
        public void ensureNullDesignationIsNotValid()
        {
            Console.WriteLine("ensureNullDesignationIsNotValid");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("TU NUNCA ROUBARIAS UM CARRO", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Seco");
            finishes.Add(finish);

            Assert.Throws<ArgumentException>(() => new Material("Have you tried turning it off and then on again?", null, colors, finishes));
        }

        /**
        <summary>
            Test to ensure that the instance of Material isn't built if the designation is empty.
        </summary>
       */
        [Fact]
        public void ensureEmptyDesignationIsNotValid()
        {
            Console.WriteLine("ensureEmptyDesignationIsNotValid");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("NÃO ROUBARIAS UMA CARTEIRA", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Húmido");
            finishes.Add(finish);

            Assert.Throws<ArgumentException>(() => new Material("Still not working", "", colors, finishes));
        }

        /**
        <summary>
            Test to ensure that the instance of Material isn't built if the list of colors is null.
        </summary>
       */
        [Fact]
        public void ensureNullColorListIsNotValid()
        {
            Console.WriteLine("ensureNullColorListIsNotValid");

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Húmido");
            finishes.Add(finish);

            Assert.Throws<ArgumentException>(() => new Material("Hello", "It's me, Mario", null, finishes));
        }

        /**
        <summary>
            Test to ensure that the instance of Material isn't built if the list of colors is empty.
        </summary>
       */
        [Fact]
        public void ensureEmptyColorListIsNotValid()
        {
            Console.WriteLine("ensureEmptyColorListIsNotValid");

            List<Color> colors = new List<Color>();

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Seco");
            finishes.Add(finish);

            Assert.Throws<ArgumentException>(() => new Material("Goodbye", "See you later", colors, finishes));
        }

        /**
        <summary>
            Test to ensure that the instance of Material isn't built if the list of finishes is null.
        </summary>
       */
        [Fact]
        public void ensureNullFinishListIsNotValid()
        {
            Console.WriteLine("ensureNullFinishListIsNotValid");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Broken", 1, 2, 3, 0);
            colors.Add(color);

            Assert.Throws<ArgumentException>(() => new Material("Hello", "It's me, Mario", colors, null));
        }

        /**
        <summary>
            Test to ensure that the instance of Material isn't built if the list of finishes is empty.
        </summary>
       */
        [Fact]
        public void ensureEmptyFinishListIsNotValid()
        {
            Console.WriteLine("ensureEmptyFinishListIsNotValid");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Fixed", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();

            Assert.Throws<ArgumentException>(() => new Material("Goodbye", "See you later", colors, finishes));
        }

        //addColor tests

        /**
        <summary>
            Test to ensure that an already existent Color cannot be added to the Material's list of colors.
        </summary>
       */
        [Fact]
        public void ensureAlreadyExistentColorCannotBeAdded()
        {
            Console.WriteLine("ensureAlreadyExistentColorCannotBeAdded");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Freitas", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Zindeiro");
            finishes.Add(finish);

            Material material = new Material("Another", "One", colors, finishes);

            Assert.False(material.addColor(color));
        }

        /**
        <summary>
            Test to ensure that a null Color cannot be added to the Material's list of colors.
        </summary>
       */
        [Fact]
        public void ensureNullColorCannotBeAdded()
        {
            Console.WriteLine("ensureNullColorCannotBeAdded");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("4445", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Ezy");
            finishes.Add(finish);

            Material material = new Material("Still", "Here", colors, finishes);

            Assert.False(material.addColor(null));
        }

        /**
        <summary>
            Test to ensure that a valid Color can be added to the Material's list of colors.
        </summary>
       */
        [Fact]
        public void ensureValidColorCanBeAdded()
        {
            Console.WriteLine("ensureValidColorCanBeAdded");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("4445", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Ezy");
            finishes.Add(finish);

            Material material = new Material("Still", "Here", colors, finishes);

            Assert.True(material.addColor(Color.valueOf("566", 3, 2, 1, 0)));
        }

        //removeColor tests

        /**
        <summary>
            Test to ensure that a non-existent Color cannot be removed from the Material's list of colors.
        </summary>
       */
        [Fact]
        public void ensureNonExistentColorCannotBeRemoved()
        {
            Console.WriteLine("ensureNonExistentColorCannotBeRemoved");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("D. Emília", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Loja das Gomas");
            finishes.Add(finish);

            Material material = new Material("I'm", "Gone", colors, finishes);

            Assert.False(material.removeColor(Color.valueOf("Empregada", 3, 2, 1, 0)));
        }

        /**
        <summary>
            Test to ensure that a null Color cannot be removed from the Material's list of colors.
        </summary>
       */
        [Fact]
        public void ensureNullColorCannotBeRemoved()
        {
            Console.WriteLine("ensureNullColorCannotBeRemoved");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Velhinho", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("da Estação");
            finishes.Add(finish);

            Material material = new Material("Fake", "News", colors, finishes);

            Assert.False(material.removeColor(null));
        }

        /**
         <summary>
             Test to ensure that a valid Color can be removed from the Material's list of colors.
         </summary>
        */
        [Fact]
        public void ensureValidColorCanBeRemoved()
        {
            Console.WriteLine("ensureValidColorCanBeRemoved");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("4445", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Ezy");
            finishes.Add(finish);

            Material material = new Material("Still", "Here", colors, finishes);

            Assert.True(material.removeColor(color));
        }

        //addFinish tests

        /**
            <summary>
                Test to ensure that an already existent Finish cannot be added to the Material's list of finishes.
            </summary>
           */
        [Fact]
        public void ensureAlreadyExistentFinishCannotBeAdded()
        {
            Console.WriteLine("ensureAlreadyExistentFinishCannotBeAdded");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Guna", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Zindeiro");
            finishes.Add(finish);

            Material material = new Material("This", "One", colors, finishes);

            Assert.False(material.addFinish(finish));
        }

        /**
        <summary>
            Test to ensure that a null Finish cannot be added to the Material's list of finishes.
        </summary>
       */
        [Fact]
        public void ensureNullFinishCannotBeAdded()
        {
            Console.WriteLine("ensureNullFinishCannotBeAdded");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("O sangue de Jesus", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Tem poder");
            finishes.Add(finish);

            Material material = new Material("Me", "Again", colors, finishes);

            Assert.False(material.addFinish(null));
        }

        /**
        <summary>
            Test to ensure that a valid Finish can be added to the Material's list of finishes.
        </summary>
       */
        [Fact]
        public void ensureValidFinishCanBeAdded()
        {
            Console.WriteLine("ensureValidFinishCanBeAdded");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Senhor", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Acuda");
            finishes.Add(finish);

            Material material = new Material("Still", "Here", colors, finishes);

            Assert.True(material.addFinish(Finish.valueOf("Acabou")));
        }

        //removeFinish tests

        /**
        <summary>
            Test to ensure that a non-existent Finish cannot be removed from the Material's list of finishes.
        </summary>
       */
        [Fact]
        public void ensureNonExistentFinishCannotBeRemoved()
        {
            Console.WriteLine("ensureNonExistentFinishCannotBeRemoved");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Não interesso", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Existo");
            finishes.Add(finish);

            Material material = new Material("I'm", "back", colors, finishes);

            Assert.False(material.removeFinish(Finish.valueOf("Não existo")));
        }

        /**
        <summary>
            Test to ensure that a null Finish cannot be removed from the Material's list of finishes.
        </summary>
       */
        [Fact]
        public void ensureNullFinishCannotBeRemoved()
        {
            Console.WriteLine("ensureNullFinishCannotBeRemoved");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Velhinho", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("da Estação");
            finishes.Add(finish);

            Material material = new Material("Fake", "News", colors, finishes);

            Assert.False(material.removeFinish(null));
        }

        /**
         <summary>
             Test to ensure that a valid Finish can be removed from the Material's list of finishes.
         </summary>
        */
        [Fact]
        public void ensureValidFinishCanBeRemoved()
        {
            Console.WriteLine("ensureValidFinishCanBeRemoved");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("4445", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("I'm finally valid");
            finishes.Add(finish);

            Material material = new Material("Still", "Here", colors, finishes);

            Assert.True(material.removeFinish(finish));
        }

        //hasColor tests

        /**
        <summary>
            Test to ensure that an existent color is found in the Material's list of colors.
        </summary>
        */
        [Fact]
        public void ensureValidColorExists()
        {
            Console.WriteLine("ensureValidColorExists");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Look at all", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Those chickens");
            finishes.Add(finish);

            Material material = new Material("1160912", "Wii Music", colors, finishes);

            Assert.True(material.hasColor(color));
        }

        /**
        <summary>
            Test to ensure that a null color is not found in the Material's list of colors.
        </summary>
        */
        [Fact]
        public void ensureNullColorDoesNotExist()
        {
            Console.WriteLine("ensureNullColorDoesNotExist");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Lá está", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Isto é");
            finishes.Add(finish);

            Material material = new Material("Bombado70", "You have been Guru'ed", colors, finishes);

            Assert.False(material.hasColor(null));
        }

        //hasFinish tests

        /**
        <summary>
            Test to ensure that an existent finish is found in the Material's list of finishes.
        </summary>
        */
        [Fact]
        public void ensureValidFinishExists()
        {
            Console.WriteLine("ensureValidFinishExists");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Look at all", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Those chickens");
            finishes.Add(finish);

            Material material = new Material("1160912", "Wii Music", colors, finishes);

            Assert.True(material.hasFinish(finish));
        }

        /**
        <summary>
            Test to ensure that a null finish is not found in the Material's list of finishes.
        </summary>
        */
        [Fact]
        public void ensureNullFinishDoesNotExist()
        {
            Console.WriteLine("ensureNullFinishDoesNotExist");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Fechar a caneta", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Comprar viagens para Gatwick");
            finishes.Add(finish);

            Material material = new Material("Água Fresca", "Pão seco", colors, finishes);

            Assert.False(material.hasFinish(null));
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

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("DIGA NÃO À PIRATARIA", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("VAI DAR IGUAL OU NÃO");
            finishes.Add(finish);

            Material balsamic = new Material("1160912", "Cowboy Boots", colors, finishes);
            Material vinegar = new Material("1160912", "Cowboy Boots", colors, finishes);

            Assert.Equal(balsamic.GetHashCode(), vinegar.GetHashCode());
        }

        //Equals tests

        /**
        <summary>
            Test to ensure that the method Equals works, for two Materials with different references.
         </summary>
         */
        [Fact]
        public void ensureMaterialsWithDifferentReferencesAreNotEqual()
        {
            Console.WriteLine("ensureMaterialsWithDifferentReferencesAreNotEqual");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("4445 vs 4470", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Não gosto");
            finishes.Add(finish);

            Material salt = new Material("1160912", "Guru", colors, finishes);
            Material pepper = new Material("1160907", "Velhinho", colors, finishes);

            Assert.False(salt.Equals(pepper));
        }

        /**
        <summary>
            Test to ensure that the method Equals works, for two Materials with the same reference.
         </summary>
         */
        [Fact]
        public void ensureMaterialsWithSameReferencesAreEqual()
        {
            Console.WriteLine("ensureMaterialsWithSameReferencesAreEqual");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Lil", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Péssimo");
            finishes.Add(finish);

            Material ping = new Material("1160912", "Ping", colors, finishes);
            Material pong = new Material("1160912", "Pong", colors, finishes);

            Assert.True(ping.Equals(pong));
        }

        /**
        <summary>
            Test to ensure that the method Equals works, for a null Material.
         </summary>
         */
        [Fact]
        public void ensureNullObjectIsNotEqual()
        {
            Console.WriteLine("ensureNullObjectIsNotEqual");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Pump", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Horrendo");
            finishes.Add(finish);

            Material loner = new Material("1160912", "John Snow", colors, finishes);

            Assert.False(loner.Equals(null));
        }

        /**
        <summary>
            Test to ensure that the method Equals works, for a Material and an object of another type.
         </summary>
         */
        [Fact]
        public void ensureDifferentTypesAreNotEqual()
        {
            Console.WriteLine("ensureDifferentTypesAreNotEqual");

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("THAT'S MY OPINION", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Piroso");
            finishes.Add(finish);

            Material moon = new Material("1160912", "No", colors, finishes);

            List<Material> materials = new List<Material>();
            materials.Add(moon);

            Assert.False(moon.Equals("stars"));
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

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Já perdi a imaginação", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Foleiro");
            finishes.Add(finish);

            Material balsamic = new Material("1160912", "Cowboy Boots", colors, finishes);
            Material vinegar = new Material("1160912", "Cowboy Boots", colors, finishes);

            Assert.Equal(balsamic.ToString(), vinegar.ToString());
        }

        [Fact]
        public void testToDTO()
        {
            Console.WriteLine("toDTO");
            string reference = "el. psy. kongroo.";
            string designation = "I am mad scientist!";

            List<Color> colors = new List<Color>();

            Color color = Color.valueOf("Blue", 0, 0, 255, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();

            Finish finish = Finish.valueOf("Glossy");
            finishes.Add(finish);

            Material material = new Material(reference, designation, colors, finishes);


            MaterialDTO expected = new MaterialDTO();
            expected.reference = reference;
            expected.designation = designation;
            expected.colors = new List<ColorDTO>(DTOUtils.parseToDTOS(colors));
            expected.finishes = new List<FinishDTO>(DTOUtils.parseToDTOS(finishes));

            MaterialDTO actual = material.toDTO();

            Assert.Equal(expected.reference, actual.reference);
            Assert.Equal(expected.designation, actual.designation);

            int actualColorListSize = actual.colors.Count;
            int expectedColorListSize = expected.colors.Count;

            Assert.Equal(expectedColorListSize, actualColorListSize);

            for (int i = 0; i < actualColorListSize; i++)
            {
                Assert.Equal(expected.colors[i].red, actual.colors[i].red);
                Assert.Equal(expected.colors[i].green, actual.colors[i].green);
                Assert.Equal(expected.colors[i].blue, actual.colors[i].blue);
                Assert.Equal(expected.colors[i].alpha, actual.colors[i].alpha);
            }

            int actualFinishListSize = actual.finishes.Count;
            int expectedFinishListSize = expected.finishes.Count;

            Assert.Equal(expectedFinishListSize, actualFinishListSize);

            for (int i = 0; i < actualFinishListSize; i++)
            {
                Assert.Equal(expected.finishes[i].description, actual.finishes[i].description);
            }
        }

    }
}