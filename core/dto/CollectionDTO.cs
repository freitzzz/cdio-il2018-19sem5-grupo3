using System.Collections.Generic;
using System.Runtime.Serialization;
using core.domain;
using support.dto;

namespace core.dto
{
    /// <summary>
    /// DTO that represents a instance of a Collection
    /// </summary>
    [DataContract]
    public class CollectionDTO : DTO, DTOParseable<Collection, CollectionDTO>
    {
        /**
        <summary>
            long with the id of the Collection in the DB.
        </summary>
        */
        [DataMember]
        public long id { get; set; }


        /**
        <summary>
            String with the Collection's reference.
        </summary>
        */
        public string reference { get; set; }

        /** 
        <summary>
            String with the Collection's designation.
        </summary>
        */
        public string designation { get; set; }

        /**
        <summary>
          Constant that represents a list of Customized Products of a collection.
        </summary>
        */
        public List<CustomizedProduct> list;



        public Collection toEntity()
        {
            Collection instanceFromDTO = Collection.valueOf(reference, designation, list);
            instanceFromDTO.Id = this.id;
            return instanceFromDTO;
        }
    }
}