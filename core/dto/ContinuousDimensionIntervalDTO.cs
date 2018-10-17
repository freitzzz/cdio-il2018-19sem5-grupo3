using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using core.domain;
using support.dto;
using core.services;

namespace core.dto
{
    /// <summary>
    /// DTO that represents a ContinuousDimensionInterval instance
    /// </summary>
    [DataContract]
    public class ContinuousDimensionIntervalDTO : DimensionDTO
    {

        /// <summary>
        /// Minimum value of the interval
        /// </summary>
        /// <value>Get/Set of the value</value>
        [DataMember(Order = 2)]
        public double minValue { get; set; }

        /// <summary>
        /// Maximum value of the interval
        /// </summary>
        /// <value>Get/Set of the value</value>
        [DataMember(Order = 3)]
        public double maxValue { get; set; }

        /// <summary>
        /// Increment value of the interval
        /// </summary>
        /// <value>Get/Set of the value</value>
        [DataMember(Order = 4)]
        public double increment { get; set; }

        /// <summary>
        /// Builds a ContinuousDimensionInterval instance from a ContinuousDimensionIntervalDTO
        /// </summary>
        /// <returns>ContinuousDimensionInterval instance</returns>
        public override Dimension toEntity()
        {
            double minValue = MeasurementUnitService.convertFromUnit(this.minValue, unit);
            double maxValue = MeasurementUnitService.convertFromUnit(this.maxValue, unit);
            double increment = MeasurementUnitService.convertFromUnit(this.increment, unit);

            ContinuousDimensionInterval instanceFromDTO = new ContinuousDimensionInterval(minValue, maxValue, increment);
            instanceFromDTO.Id = id;
            
            return instanceFromDTO;
        }
    }
}