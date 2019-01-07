using System;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.modelview.cataloguecollection;

namespace core.modelview.commercialcatalogue
{
    /// <summary>
    /// Service class used for converting instances of Commercial Catalogues to ModelViews.
    /// </summary>
    public class CommercialCatalogueModelViewService
    {

        /// <summary>
        /// Constant that represents the message presented when the provided instance of CommercialCatalogue is null.
        /// </summary>
        private const string ERROR_NULL_CATALOGUE = "Unable to convert the catalogue into a view.";

        /// <summary>
        /// Constant that represents the message presented when the provided IEnumerable of CommercialCatalogue is null.
        /// </summary>
        private const string ERROR_NULL_CATALOGUE_ENUMERABLE = "Unable to convert catalogues into views.";

        /// <summary>
        /// Builds an instance of GetBasicCommercialCatalogueModelView from an instance of CommercialCatalogue.
        /// </summary>
        /// <param name="commercialCatalogue">Instance of CommercialCatalogue from which the ModelView will be built.</param>
        /// <returns>An instance of GetBasicCommercialCatalogueModelView.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of CommercialCatalogue is null.</exception>
        public static GetBasicCommercialCatalogueModelView fromEntityAsBasic(CommercialCatalogue commercialCatalogue)
        {
            if (commercialCatalogue == null)
            {
                throw new ArgumentNullException(ERROR_NULL_CATALOGUE);
            }

            GetBasicCommercialCatalogueModelView basicCommercialCatalogueModelView = new GetBasicCommercialCatalogueModelView();
            basicCommercialCatalogueModelView.commercialCatalogueId = commercialCatalogue.Id;
            basicCommercialCatalogueModelView.reference = commercialCatalogue.reference;
            basicCommercialCatalogueModelView.designation = commercialCatalogue.designation;
            basicCommercialCatalogueModelView.hasCollections = commercialCatalogue.catalogueCollectionList.Any();

            return basicCommercialCatalogueModelView;
        }


        /// <summary>
        /// Builds an instance of GetCommercialCatalogueModelView from an instance of CommercialCatalogue.
        /// </summary>
        /// <param name="commercialCatalogue">Instance of CommercialCatalogue from which the ModelView will be built.</param>
        /// <returns>An instance of GetCommercialCatalogueModelView.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of CommercialCatalogue is null.</exception>
        public static GetCommercialCatalogueModelView fromEntity(CommercialCatalogue commercialCatalogue)
        {
            if (commercialCatalogue == null)
            {
                throw new ArgumentNullException(ERROR_NULL_CATALOGUE);
            }

            GetCommercialCatalogueModelView commercialCatalogueView = new GetCommercialCatalogueModelView();

            commercialCatalogueView.commercialCatalogueId = commercialCatalogue.Id;
            commercialCatalogueView.reference = commercialCatalogue.reference;
            commercialCatalogueView.designation = commercialCatalogue.designation;
            if (commercialCatalogue.catalogueCollectionList.Any())
            {
                commercialCatalogueView.commercialCatalogueCollections = CatalogueCollectionModelViewService.fromCollection(commercialCatalogue.catalogueCollectionList);

            }
            return commercialCatalogueView;
        }


        /// <summary>
        /// Builds an instance of GetAllCommercialCataloguesModelView from an IEnumerable of CommercialCatalogue.
        /// </summary>
        /// <param name="commercialCatalogues">IEnumerable of CommercialCatalogue being converted into model views.</param>
        /// <returns>An instance of GetAllComemrcialCataloguesModelView.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided IEnumerable of CommercialCatalogue is null.</exception>
        public static GetAllCommercialCataloguesModelView fromCollection(IEnumerable<CommercialCatalogue> commercialCatalogues)
        {
            if (commercialCatalogues == null)
            {
                throw new ArgumentNullException(ERROR_NULL_CATALOGUE_ENUMERABLE);
            }

            GetAllCommercialCataloguesModelView allCommercialCataloguesModelView = new GetAllCommercialCataloguesModelView();

            foreach (CommercialCatalogue commercialCatalogue in commercialCatalogues)
            {
                allCommercialCataloguesModelView.Add(fromEntityAsBasic(commercialCatalogue));
            }

            return allCommercialCataloguesModelView;
        }

    }
}