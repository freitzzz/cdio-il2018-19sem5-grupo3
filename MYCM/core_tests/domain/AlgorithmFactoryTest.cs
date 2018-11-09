using core.domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace core_tests.domain {
    public class AlgorithmFactoryTest {
        [Fact]
        public void ensureWidthPercentageAlgorithmIsCreated() {
            Console.WriteLine("ensureWidthPercentageAlgorithmIsCreated");
            AlgorithmFactory factory = new AlgorithmFactory();
            Algorithm algorithm = factory.createAlgorithm(RestrictionAlgorithm.WIDTH_PERCENTAGE_ALGORITHM);
            Assert.True(algorithm.GetType().ToString() == "core.domain.WidthPercentageAlgorithm");
        }
        [Fact]
        public void ensureCreateAlgorithmFailsIfAlgorithmDoesNotExistd() {
            Console.WriteLine("ensureCreateAlgorithmFailsIfAlgorithmDoesNotExistd");
            AlgorithmFactory factory = new AlgorithmFactory();
            Action create = () => factory.createAlgorithm(0);
            Assert.Throws<ArgumentOutOfRangeException>(create);
        }
    }
}
