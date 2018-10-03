using System;
using Xunit;
using core.dto;
using core.domain;

namespace core_tests.dto
{
    public class ContinuousDimensionIntervalDTOTest
    {
        [Fact]
        public void testToEntity()
        {
            long id = 5;
            double minValue = 3.0;
            double maxValue = 60.0;
            double increment = 1.0;
            ContinuousDimensionIntervalDTO instance = new ContinuousDimensionIntervalDTO();
            ContinuousDimensionIntervalDTO other = new ContinuousDimensionIntervalDTO();
            instance.id = id;
            instance.minValue = minValue;
            instance.maxValue = maxValue;
            instance.increment = increment;
            other.id = id;
            other.minValue = minValue;
            other.maxValue = maxValue;
            other.increment = increment;

            Assert.Equal(instance.toEntity(), other.toEntity());
        }
    }
}