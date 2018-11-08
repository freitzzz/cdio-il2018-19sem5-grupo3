using System.Net.Http;
using System.Threading.Tasks;
using core.modelview.pricetableentries;
using core.services;

namespace core.application
{
    /// <summary>
    /// Application controller for Price Table Entries
    /// </summary>
    public class PriceTablesController
    {
        /// <summary>
        /// Adds a new price table entry for a material
        /// </summary>
        /// <param name="modelView">model view with the price table entry's information</param>
        public async Task<AddPriceTableEntryModelView> addMaterialPriceTableEntry(AddPriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            return await AddMaterialPriceTableEntryModelViewService.transform(modelView, clientFactory);
        }

        /// <summary>
        /// Adds new price table entry for a material's finish
        /// </summary>
        /// <param name="modelView">model view with the price table entry's information</param>
        public async Task<AddFinishPriceTableEntryModelView> addFinishPriceTableEntry(AddFinishPriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            return await AddFinishPriceTableEntryModelViewService.transform(modelView, clientFactory);
        }

        /// <summary>
        /// Updates a material's price table entry
        /// </summary>
        /// <param name="modelView">model view with the necessary update information</param>
        public async Task<bool> updateMaterialPriceTableEntry(UpdatePriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            return await UpdateMaterialPriceTableEntryModelViewService.update(modelView, clientFactory);
        }

        /// <summary>
        /// Updates a finish's price table entry
        /// </summary>
        /// <param name="modelView">model view with the necessary update information</param>
        public async Task<bool> updateFinishPriceTableEntry(UpdateFinishPriceTableEntryModelView modelView, IHttpClientFactory clientFactory)
        {
            return await UpdateFinishPriceTableEntryModelViewService.update(modelView, clientFactory);
        }
    }
}