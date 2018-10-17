using System;
using System.Runtime.Serialization;
using core.domain;
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
        [DataMember]
        public double height { get; set; }

        /// <summary>
        /// CustomizedDimensions width value
        /// </summary>
        /// <returns>Gets/Sets for the width value</returns>
        [DataMember]
        public double width { get; set; }

        /// <summary>
        /// CustomizedDimensions depth value
        /// </summary>
        /// <returns>Gets/Sets for the depth value</returns>
        [DataMember]
        public double depth { get; set; }

        public CustomizedDimensions toEntity()
        {
            return CustomizedDimensions.valueOf(height, width, depth);
        }
    }
}