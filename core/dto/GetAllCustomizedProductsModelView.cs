using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using core.domain;

namespace core.dto
{

    /// <summary>
    /// Model View that represents the body of a GET All Request of the Customized Products
    /// </summary>
    [DataContract]
    public class GetAllCustomizedProductsModelView
    {
        /// <summary>
        /// List of Basic Model Views for the GET All Request 
        /// </summary>
        [DataMember]
        public List<GetAllBasicCustomizedProductsModelView> basicModelViewList;

        /// <summary>
        /// Model View that represents the basic info of a Customized Product
        /// </summary>
        [DataContract]
        public class GetAllBasicCustomizedProductsModelView
        {
            /// <summary>
            /// CustomizedProducts Identifier
            /// </summary>
            /// <value>Gets/Sets the identifier</value>
            [DataMember]
            public long id { get; set; }

            /// <summary>
            /// Identifier of the Product that the Customized Product is built off of
            /// </summary>
            /// <value>Gets/Sets the identifier</value>
            [DataMember]
            public long productId { get; set; }

            /// <summary>
            /// CustomizedProducts designation
            /// </summary>
            /// <value>Gets/Sets the designation</value>
            [DataMember]
            public string designation { get; set; }

            /// <summary>
            /// CustomizedProducts reference
            /// </summary>
            /// <value>Gets/Sets the reference</value>
            [DataMember]
            public string reference { get; set; }
        }

        /// <summary>
        /// Builds a GetAllCustomizedProductsModelView from a list of customized products
        /// </summary>
        /// <param name="customizedProducts">IEnumerable of all available customized products</param>
        /// <returns>ModelView representing the body of a GET All Request to the Customized Products Collection</returns>
        public static GetAllCustomizedProductsModelView fromEntities(IEnumerable<CustomizedProduct> customizedProducts)
        {
            GetAllCustomizedProductsModelView result = new GetAllCustomizedProductsModelView();
            result.basicModelViewList = new List<GetAllBasicCustomizedProductsModelView>();

            foreach (CustomizedProduct customizedProduct in customizedProducts)
            {
                GetAllBasicCustomizedProductsModelView basicModelView = new GetAllBasicCustomizedProductsModelView();
                basicModelView.id = customizedProduct.Id;
                basicModelView.productId = customizedProduct.product.Id;
                basicModelView.reference = customizedProduct.reference;
                basicModelView.designation = customizedProduct.designation;

                result.basicModelViewList.Add(basicModelView);
            }

            return result;
        }
    }
}