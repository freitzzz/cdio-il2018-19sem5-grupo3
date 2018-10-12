using System;
using System.Collections.Generic;
using Xunit;
using core.dto;
using core.domain;

namespace core_tests.dto
{
    /// <summary>
    /// Unit testing class for DimensionIntervalDTO
    /// </summary>
    public class DiscreteDimensionIntervalDTOTest
    {
        [Fact]
        public void testToEntity()
        {
            long id = 5;
            var list = new List<double>();
            list.Add(30.5);
            DiscreteDimensionIntervalDTO instance = new DiscreteDimensionIntervalDTO();
            DiscreteDimensionIntervalDTO other = new DiscreteDimensionIntervalDTO();
            instance.id = id;
            instance.values = list;
            other.id = id;
            other.values = list;

            Assert.Equal(instance.toEntity(), other.toEntity());
        }
    }
}