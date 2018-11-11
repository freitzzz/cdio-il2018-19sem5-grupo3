using System;
using System.Runtime.Serialization;
using core.domain;
using core.services;
using support.dto;

namespace core.dto
{
    /// <summary>
    /// DTO representation of CustomizedDimensions
    /// </summary>
    /// <typeparam name="CustomizedDimensions">Domain entity type</typeparam>
    /// <typeparam name="CustomizedDimensionsDTO">DTO type</typeparam>
    [DataContract]
    public class CustomizedDimensionsDTO : DTO, DTOParseable<CustomizedDimensions, CustomizedDimensionsDTO>
    {

        /// <summary>
        /// CustomizedDimensions database identifier
        /// </summary>
        /// <value>Gets/Sets for the identifier</value>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// CustomizedDimensions height value
        /// </summary>
        /// <value>Gets/Sets for the height value</value>
        [DataMember(Name="height")]
        public double height { get; set; }

        /// <summary>
        /// CustomizedDimensions width value
        /// </summary>
        /// <returns>Gets/Sets for the width value</returns>
        [DataMember(Name="width")]
        public double width { get; set; }

        /// <summary>
        /// CustomizedDimensions depth value
        /// </summary>
        /// <returns>Gets/Sets for the depth value</returns>
        [DataMember(Name="depth")]
        public double depth { get; set; }

        /// <summary>
        /// String with the customized dimensions unit
        /// </summary>
        [DataMember(Name="unit")]
        public string unit{get;set;}

        public CustomizedDimensions toEntity()
        {
            if(unit==null){
                CustomizedDimensions custDimensions = CustomizedDimensions.valueOf(height, width, depth);
                custDimensions.Id=this.Id;
                return custDimensions;
            }else{
                return CustomizedDimensions.valueOf(MeasurementUnitService.convertFromUnit(this.height, unit)
                                                    ,MeasurementUnitService.convertFromUnit(this.width, unit)
                                                    ,MeasurementUnitService.convertFromUnit(this.depth, unit));
            }
        }
    }
}