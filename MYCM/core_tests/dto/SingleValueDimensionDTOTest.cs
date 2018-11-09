using System;
using Xunit;
using core.dto;
using core.domain;

namespace core_tests.dto
{
    /// <summary>
    /// Unit testing class for SingleValueDimension
    /// </summary>
    public class SingleValueDimensionDTOTest
    {
        [Fact]
        public void testToEntity()
        {
            long id = 5;
            double value = 1.34;
            SingleValueDimensionDTO instance = new SingleValueDimensionDTO();
            SingleValueDimensionDTO other = new SingleValueDimensionDTO();
            instance.id = id;
            instance.value = value;
            other.id = id;
            other.value = value;

            Assert.Equal(instance.toEntity(), other.toEntity());
        }
    }
}