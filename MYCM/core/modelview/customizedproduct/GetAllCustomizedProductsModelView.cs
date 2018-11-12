using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using core.domain;

namespace core.modelview.customizedproduct
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
        public List<BasicCustomizedProductModelView> basicModelViewList;

        /// <summary>
        /// Builds a GetAllCustomizedProductsModelView from a list of customized products
        /// </summary>
        /// <param name="customizedProducts">IEnumerable of all available customized products</param>
        /// <returns>ModelView representing the body of a GET All Request to the Customized Products Collection</returns>
        public static GetAllCustomizedProductsModelView fromEntities(IEnumerable<CustomizedProduct> customizedProducts)
        {
            GetAllCustomizedProductsModelView result = new GetAllCustomizedProductsModelView();
            result.basicModelViewList = new List<BasicCustomizedProductModelView>();

            foreach (CustomizedProduct customizedProduct in customizedProducts)
            {
                BasicCustomizedProductModelView basicModelView = new BasicCustomizedProductModelView();
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