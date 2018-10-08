using System.Collections.Generic;
using core.dto;
using support.domain.ddd;
using support.dto;

namespace core.domain
{
    /**
    <summary>
        Class that represents a Collection-
        <br> Collection is a value object;
    </summary>
    */
    public class Collection : ValueObject, DTOAble<CollectionDTO>
    {
         /**
        <summary>
            Constant that represents the id of a certain collection.
        </summary>
         */
        public long Id { get; set; }

         /**
        <summary>
            Constant that represents a lust of Customized Products of a collection.
        </summary>
         */
        private List<CustomizedProduct> list;

        
        /**
        <summary>
            String with the Collection's reference.
        </summary>
        */
        public string reference { get; protected set; }

        /** 
        <summary>
            String with the Collection's designation.
        </summary>
        */
        public string designation { get; set; }


       /*  public static Collection valueOf(string reference,string designation,List<CustomizedProduct> list)
        {
           
        } */

        public CollectionDTO toDTO()
        {
            throw new System.NotImplementedException();
        }
    }
}