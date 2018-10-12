using System;
using Xunit;
using System.Collections.Generic;
using System.Text;
using core.services;

namespace core_tests.services {
    /// <summary>
    /// Test class of MeasurementUnitService
    /// </summary>
    public class MeasurementUnitServiceTest {
        /// <summary>
        /// Ensures convertToUnit returns the correct values
        /// </summary>
        [Fact]
        public void ensureConvertToUnitSucceeds() {
            Console.WriteLine("ensureConvertToUnitSucceeds");
            double value = 10;
            string unit = "cm";
            Assert.Equal(1, MeasurementUnitService.convertToUnit(value, unit), 1);
        }
        /// <summary>
        /// Ensures convertToUnit uses the minimum unit in the file (*1 multiplier) if the argument is null
        /// </summary>
        [Fact]
        public void ensureConvertToUnitUsesMinimumUnitIfArgumentIsNull() {
            Console.WriteLine("ensureConvertToUnitUsesMinimumUnitIfArgumentIsNull");
            double value = 10;
            Assert.Equal(value, MeasurementUnitService.convertToUnit(value, null), 1);
        }
        /// <summary>
        /// Ensures convertToUnit throws KeyNotFoundException if key is not in the file
        /// </summary>
        [Fact]
        public void ensureConvertToUnitThrowsKeyNotFoundException() {
            Console.WriteLine("ensureConvertToUnitThrowsKeyNotFoundException");
            double value = 1;
            Action action = () => MeasurementUnitService.convertToUnit(value, "der alte würfelt nicht");
            Assert.Throws<KeyNotFoundException>(action);
        }
        /// <summary>
        /// Ensures convertFromUnit returns the correct values
        /// </summary>
        [Fact]
        public void ensureConvertFromUnitSucceeds() {
            Console.WriteLine("ensureConvertFromUnitSucceds");
            double value = 100;
            string unit = "dm";
            Assert.Equal(10000, MeasurementUnitService.convertFromUnit(value, unit), 1);
        }
        /// <summary>
        /// Ensures convertFromUnit uses the minimum unit in the file (*1 multiplier) if the argument is null
        /// </summary>
        [Fact]
        public void ensureConvertFromUnitUsesMinimumUnitIfArgumentIsNull() {
            Console.WriteLine("ensureConvertFromUnitUsesMinimumUnitIfArgumentIsNull");
            double value = 100;
            Assert.Equal(100,MeasurementUnitService.convertFromUnit(value, null),1);
        }
        /// <summary>
        /// Ensures convertFromUnit throws KeyNotFoundException if key is not in the file
        /// </summary>
        [Fact]
        public void ensureConvertFromUnitThrowsKeyNotFoundException() {
            Console.WriteLine("ensureConvertToUnitThrowsKeyNotFoundException");
            double value = 1;
            Action action = () => MeasurementUnitService.convertFromUnit(value, "der alte würfelt nicht");
            Assert.Throws<KeyNotFoundException>(action);
        }
        /// <summary>
        /// Ensures getMinimumUnit succeeds
        /// </summary>
        [Fact]
        public void ensureGetMinimumUnitSucceeds() {
            Console.WriteLine("ensureGetMinimumUnitSucceds");
            Assert.Equal("mm", MeasurementUnitService.getMinimumUnit());
        }
    }
}
