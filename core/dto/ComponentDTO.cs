using System.Collections.Generic;
using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto
{
    /// <summary>
    /// Represents a Component's Data Transfer Object.
    /// </summary>
    /// <typeparam name="Component">Type of entity</typeparam>
    /// <typeparam name="ComponentDTO">Type of DTO</typeparam>
    [DataContract]
    public class ComponentDTO : DTO, DTOParseable<Component, ComponentDTO>
    {
        /// <summary>
        /// Component's database identifier.
        /// </summary>
        /// <value>Gets/sets the value of the database identifier field.</value>
        [DataMember]
        public long id {get; set;}

        /// <summary>
        /// Component's reference.
        /// </summary>
        /// <value>Gets/sets the value of the reference field.</value>
        [DataMember]
        public string reference {get; set;}

        /// <summary>
        /// Component's designation.
        /// </summary>
        /// <value>Gets/sets the value of the designation field.</value>
        [DataMember]
        public string designation {get; set;}
        /// <summary>
        /// Component's product.
        /// </summary>
        [DataMember]
        public long productID {get; set;}
        /// <summary>
        /// Component's list restrictions.
        /// </summary>
        /// <value>Gets/sets the value of the restrictions field.</value>
        [DataMember(EmitDefaultValue = false)]  //no data field data is presented if it's null
        public List<RestrictionDTO> restrictions {get; set;}
        public Component toEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}