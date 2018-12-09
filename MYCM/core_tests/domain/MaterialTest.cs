using System;
using Xunit;
using System.Collections.Generic;
using core.domain;
using core.dto;
using support.dto;

namespace core_tests.domain
{
    /// <summary>
    /// Tests of the class Material.
    /// </summary>
    public class MaterialTest
    {
        [Fact]
        public void ensureIdMethodWorks()
        {
            string reference = "1160912";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("AND READ-ER-BIBLE", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Amém", 12);
            finishes.Add(finish);

            Material material = new Material(reference, "FR E SH A VOCA DO", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.Equal(material.id(), reference, true);
        }

        [Fact]
        public void ensureMaterialsWithEqualIdentitiesAreTheSame()
        {
            string reference = "1160912";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Road work ahead", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Sim", 14);
            finishes.Add(finish);

            Material material = new Material(reference, "FR E SH A VOCA DO", "HelloGIMPTransparentBackground.jpg", colors, finishes);
            Assert.True(material.sameAs(reference));
        }

        [Fact]
        public void ensureMaterialsWithDifferentIdentitiesAreNotTheSame()
        {
            string reference = "1160912";
            string anotherReference = "1160907";

            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("I sure hope it does", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Não", 15);
            finishes.Add(finish);

            Material material = new Material(reference, "FR E SH A VOCA DO", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(material.sameAs(anotherReference));
        }

        [Fact]
        public void ensureNullReferenceIsNotValid()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Peel the avocado", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Bonito", 16);
            finishes.Add(finish);

            Material material = new Material("Avocado", "Lady", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.Throws<ArgumentException>(() => new Material(null, "Something", "HelloGIMPTransparentBackground.jpg", colors, finishes));
            Assert.False(material.changeReference(null));
        }

        [Fact]
        public void ensureEmptyReferenceIsNotValid()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Gua-ca-mole", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Feio", 17);
            finishes.Add(finish);

            Material material = new Material("Avocado", "Lady", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.Throws<ArgumentException>(() => new Material("", "Let me see...", "HelloGIMPTransparentBackground.jpg", colors, finishes));
            Assert.False(material.changeReference(""));
        }

        [Fact]
        public void ensureNullDesignationIsNotValid()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("TU NUNCA ROUBARIAS UM CARRO", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Seco", 0);
            finishes.Add(finish);

            Material material = new Material("Avocado", "Lady", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.Throws<ArgumentException>(() => new Material("Have you tried turning it off and then on again?", null, "HelloGIMPTransparentBackground.jpg", colors, finishes));
            Assert.False(material.changeDesignation(null));
        }

        [Fact]
        public void ensureEmptyDesignationIsNotValid()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("NÃO ROUBARIAS UMA CARTEIRA", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Húmido", 100);
            finishes.Add(finish);

            Material material = new Material("Avocado", "Lady", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.Throws<ArgumentException>(() => new Material("Still not working", "", "HelloGIMPTransparentBackground.jpg", colors, finishes));
            Assert.False(material.changeDesignation(""));
        }

        [Fact]
        public void ensureNullImageFileNameIsNotValid()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("NÃO ROUBARIAS UMA CARTEIRA", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Húmido", 0);
            finishes.Add(finish);

            Material material = new Material("Avocado", "Lady", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.Throws<ArgumentException>(() => new Material("Still not working", "Have you tried turning it off and then on again?", null, colors, finishes));
            Assert.False(material.changeImage(null));
        }

        [Fact]
        public void ensureEmptyImageFileNameIsNotValid()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("NÃO ROUBARIAS UMA CARTEIRA", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Húmido", 50);
            finishes.Add(finish);

            Material material = new Material("Avocado", "Lady", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.Throws<ArgumentException>(() => new Material("Still not working", "Have you tried turning it off and then on again?", "", colors, finishes));
            Assert.False(material.changeImage(""));
        }

        [Fact]
        public void ensureImageFileNameWithInvalidExtensionIsNotValid()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("NÃO ROUBARIAS UMA CARTEIRA", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Húmido", 50);
            finishes.Add(finish);

            Material material = new Material("Avocado", "Lady", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.Throws<ArgumentException>(() => new Material("Still not working", "Have you tried turning it off and then on again?", "HelloGIMPTransparentBackground.xd", colors, finishes));
            Assert.False(material.changeImage("HelloGIMPTransparentBackground.xd"));
        }

        [Fact]
        public void ensureNullColorListIsNotValid()
        {
            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Húmido", 50);
            finishes.Add(finish);

            Assert.Throws<ArgumentException>(() => new Material("Hello", "It's me, Mario", "HelloGIMPTransparentBackground.jpg", null, finishes));
        }

        [Fact]
        public void ensureEmptyColorListIsNotValid()
        {
            List<Color> colors = new List<Color>();

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Seco", 50);
            finishes.Add(finish);

            Assert.Throws<ArgumentException>(() => new Material("Goodbye", "See you later", "HelloGIMPTransparentBackground.jpg", colors, finishes));
        }

        [Fact]
        public void ensureNullFinishListIsNotValid()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Broken", 1, 2, 3, 0);
            colors.Add(color);

            Assert.Throws<ArgumentException>(() => new Material("Hello", "It's me, Mario", "HelloGIMPTransparentBackground.jpg", colors, null));
        }

        [Fact]
        public void ensureEmptyFinishListIsNotValid()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Fixed", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();

            Assert.Throws<ArgumentException>(() => new Material("Goodbye", "See you later", "HelloGIMPTransparentBackground.jpg", colors, finishes));
        }

        [Fact]
        public void ensureMaterialWithValidDataCanBeCreated()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("NÃO ROUBARIAS UMA CARTEIRA", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Húmido", 50);
            finishes.Add(finish);

            Assert.NotNull(new Material("Avocado", "Lady", "HelloGIMPTransparentBackground.jpg", colors, finishes));
            Assert.NotNull(new Material("Avocado", "Lady", "HelloGIMPTransparentBackground.jpeg", colors, finishes));
            Assert.NotNull(new Material("Avocado", "Lady", "HelloGIMPTransparentBackground.png", colors, finishes));
            Assert.NotNull(new Material("Avocado", "Lady", "HelloGIMPTransparentBackground.gif", colors, finishes));
            Assert.NotNull(new Material("Avocado", "Lady", "HelloGIMPTransparentBackground.dds", colors, finishes));
        }

        [Fact]
        public void ensureAlreadyExistentColorCannotBeAdded()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Freitas", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Zindeiro", 5);
            finishes.Add(finish);

            Material material = new Material("Another", "One", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(material.addColor(color));
        }

        [Fact]
        public void ensureNullColorCannotBeAdded()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("4445", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Ezy", 7);
            finishes.Add(finish);

            Material material = new Material("Still", "Here", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(material.addColor(null));
        }

        [Fact]
        public void ensureValidColorCanBeAdded()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("4445", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Ezy", 7);
            finishes.Add(finish);

            Material material = new Material("Still", "Here", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.True(material.addColor(Color.valueOf("566", 3, 2, 1, 0)));
        }

        [Fact]
        public void ensureNonExistentColorCannotBeRemoved()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("D. Emília", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Loja das Gomas", 4);
            finishes.Add(finish);

            Material material = new Material("I'm", "Gone", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(material.removeColor(Color.valueOf("Empregada", 3, 2, 1, 0)));
        }

        [Fact]
        public void ensureNullColorCannotBeRemoved()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Velhinho", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("da Estação", 5);
            finishes.Add(finish);

            Material material = new Material("Fake", "News", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(material.removeColor(null));
        }

        [Fact]
        public void ensureValidColorCanBeRemoved()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("4445", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Ezy", 6);
            finishes.Add(finish);

            Material material = new Material("Still", "Here", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.True(material.removeColor(color));
        }

        [Fact]
        public void ensureAlreadyExistentFinishCannotBeAdded()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Guna", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Zindeiro", 7);
            finishes.Add(finish);

            Material material = new Material("This", "One", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(material.addFinish(finish));
        }

        [Fact]
        public void ensureNullFinishCannotBeAdded()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("O sangue de Jesus", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Tem poder", 7);
            finishes.Add(finish);

            Material material = new Material("Me", "Again", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(material.addFinish(null));
        }

        [Fact]
        public void ensureValidFinishCanBeAdded()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Senhor", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Acuda", 13);
            finishes.Add(finish);

            Material material = new Material("Still", "Here", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.True(material.addFinish(Finish.valueOf("Acabou", 13)));
        }

        [Fact]
        public void ensureNonExistentFinishCannotBeRemoved()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Não interesso", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Existo", 13);
            finishes.Add(finish);

            Material material = new Material("I'm", "back", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(material.removeFinish(Finish.valueOf("Não existo", 13)));
        }

        [Fact]
        public void ensureNullFinishCannotBeRemoved()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Velhinho", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("da Estação", 13);
            finishes.Add(finish);

            Material material = new Material("Fake", "News", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(material.removeFinish(null));
        }

        [Fact]
        public void ensureValidFinishCanBeRemoved()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("4445", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("I'm finally valid", 13);
            finishes.Add(finish);

            Material material = new Material("Still", "Here", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.True(material.removeFinish(finish));
        }

        [Fact]
        public void ensureValidColorExists()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Look at all", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Those chickens", 13);
            finishes.Add(finish);

            Material material = new Material("1160912", "Wii Music", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.True(material.hasColor(color));
        }

        [Fact]
        public void ensureValidColorNotExists()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Look at all", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Those chickens", 13);
            finishes.Add(finish);

            Material material = new Material("1160912", "Wii Music", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(material.hasColor(Color.valueOf("Outra", 2, 2, 3, 0)));
        }

        [Fact]
        public void ensureNullColorDoesNotExist()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Lá está", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Isto é", 70);
            finishes.Add(finish);

            Material material = new Material("Bombado70", "You have been Guru'ed", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(material.hasColor(null));
        }

        [Fact]
        public void ensureValidFinishExists()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Look at all", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Those chickens", 13);
            finishes.Add(finish);

            Material material = new Material("1160912", "Wii Music", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.True(material.hasFinish(finish));
        }

        [Fact]
        public void ensureValidFinishNotExists()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Look at all", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Those chickens", 13);
            Finish finish1 = Finish.valueOf("Outro", 13);
            finishes.Add(finish);

            Material material = new Material("1160912", "Wii Music", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(material.hasFinish(finish1));
        }

        [Fact]
        public void ensureNullFinishDoesNotExist()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Fechar a caneta", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Comprar viagens para Gatwick", 13);
            finishes.Add(finish);

            Material material = new Material("Água Fresca", "Pão seco", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(material.hasFinish(null));
        }

        [Fact]
        public void ensureGetHashCodeWorks()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("DIGA NÃO À PIRATARIA", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("VAI DAR IGUAL OU NÃO", 13);
            finishes.Add(finish);

            Material balsamic = new Material("1160912", "Cowboy Boots", "HelloGIMPTransparentBackground.jpg", colors, finishes);
            Material vinegar = new Material("1160912", "Cowboy Boots", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.Equal(balsamic.GetHashCode(), vinegar.GetHashCode());
        }

        [Fact]
        public void ensureMaterialsWithDifferentReferencesAreNotEqual()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("4445 vs 4470", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Não gosto", 13);
            finishes.Add(finish);

            Material salt = new Material("1160912", "Guru", "HelloGIMPTransparentBackground.jpg", colors, finishes);
            Material pepper = new Material("1160907", "Velhinho", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(salt.Equals(pepper));
        }

        [Fact]
        public void ensureMaterialsWithSameReferencesAreEqual()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Lil", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Péssimo", 13);
            finishes.Add(finish);

            Material ping = new Material("1160912", "Ping", "HelloGIMPTransparentBackground.jpg", colors, finishes);
            Material pong = new Material("1160912", "Pong", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.True(ping.Equals(pong));
        }

        [Fact]
        public void ensureNullObjectIsNotEqual()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Pump", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Horrendo", 13);
            finishes.Add(finish);

            Material loner = new Material("1160912", "John Snow", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.False(loner.Equals(null));
        }

        [Fact]
        public void ensureDifferentTypesAreNotEqual()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("THAT'S MY OPINION", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Piroso", 13);
            finishes.Add(finish);

            Material moon = new Material("1160912", "No", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            List<Material> materials = new List<Material>();
            materials.Add(moon);

            Assert.False(moon.Equals("stars"));
        }

        [Fact]
        public void ensureToStringWorks()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Já perdi a imaginação", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Foleiro", 13);
            finishes.Add(finish);

            Material balsamic = new Material("1160912", "Cowboy Boots", "HelloGIMPTransparentBackground.jpg", colors, finishes);
            Material vinegar = new Material("1160912", "Cowboy Boots", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.Equal(balsamic.ToString(), vinegar.ToString());
        }

        [Fact]
        public void ensureToStringNotWorks()
        {
            List<Color> colors = new List<Color>();
            Color color = Color.valueOf("Já perdi a imaginação", 1, 2, 3, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();
            Finish finish = Finish.valueOf("Foleiro", 13);
            finishes.Add(finish);

            Material balsamic = new Material("1160912", "Cowboy Boots", "HelloGIMPTransparentBackground.jpg", colors, finishes);
            Material vinegar = new Material("1160", "Cowboy Boots", "HelloGIMPTransparentBackground.jpg", colors, finishes);

            Assert.NotEqual(balsamic.ToString(), vinegar.ToString());
        }

        [Fact]
        public void testToDTO()
        {
            string reference = "el. psy. kongroo.";
            string designation = "I am mad scientist!";

            List<Color> colors = new List<Color>();

            Color color = Color.valueOf("Blue", 0, 0, 255, 0);
            colors.Add(color);

            List<Finish> finishes = new List<Finish>();

            Finish finish = Finish.valueOf("Glossy", 13);
            finishes.Add(finish);

            Material material = new Material(reference, designation, "HelloGIMPTransparentBackground.jpg", colors, finishes);


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
