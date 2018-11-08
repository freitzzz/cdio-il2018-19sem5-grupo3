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
        /// <returns></returns>
        public AddFinishPriceTableEntryModelView addFinishPriceTableEntry(AddFinishPriceTableEntryModelView modelView)
        {
            return AddFinishPriceTableEntryModelViewService.transform(modelView);
        }
    }
}