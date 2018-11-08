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
        public AddPriceTableEntryModelView addMaterialPriceTableEntry(AddPriceTableEntryModelView modelView)
        {
            return AddMaterialPriceTableEntryModelViewService.transform(modelView);
        }

        /// <summary>
        /// Adds new price table entry for a material's finish
        /// </summary>
        /// <param name="modelView">model view with the price table entry's information</param>
        public AddFinishPriceTableEntryModelView addFinishPriceTableEntry(AddFinishPriceTableEntryModelView modelView)
        {
            return AddFinishPriceTableEntryModelViewService.transform(modelView);
        }

        /// <summary>
        /// Updates a material's price table entry
        /// </summary>
        /// <param name="modelView">model view with the necessary update information</param>
        public bool updateMaterialPriceTableEntry(UpdatePriceTableEntryModelView modelView)
        {
            return UpdateMaterialPriceTableEntryModelViewService.update(modelView);
        }

        /// <summary>
        /// Updates a finish's price table entry
        /// </summary>
        /// <param name="modelView">model view with the necessary update information</param>
        public bool updateFinishPriceTableEntry(UpdateFinishPriceTableEntryModelView modelView)
        {
            return UpdateFinishPriceTableEntryModelViewService.update(modelView);
        }
    }
}