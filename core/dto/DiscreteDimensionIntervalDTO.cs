using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using core.domain;
using support.dto;
using core.services;

namespace core.dto
{
    /// <summary>
    /// DTO that represents a DiscreteDimensionInterval instance
    /// </summary>
    [DataContract]
    public class DiscreteDimensionIntervalDTO : DimensionDTO
    {

        [DataMember(Order = 2)]
        /// <summary>
        /// List of values that the dimension can have
        /// </summary>
        /// <value>Get/Set of the list of values</value>
        public List<double> values { get; set; }

        /// <summary>
        /// Builds a DiscreteDimensionInterval instance from a DiscreteDimensionIntervalDTO
        /// </summary>
        /// <returns>DiscreteDimensionInterval instance</returns>
        public override Dimension toEntity()
        {
            List<double> valuesInMilimetres = new List<double>();

            foreach (double value in values)
            {
                valuesInMilimetres.Add(MeasurementUnitService.convertFromUnit(value, unit));
            }

            DiscreteDimensionInterval instanceFromDTO = new DiscreteDimensionInterval(valuesInMilimetres);
            instanceFromDTO.Id = id;

            if (this.restrictions != null)
            {
                IEnumerable<Restriction> restrictions = DTOUtils.reverseDTOS(this.restrictions);

                foreach (Restriction restriction in restrictions)
                {
                    instanceFromDTO.addRestriction(restriction);
                }
            }

            return instanceFromDTO;
        }
    }
}