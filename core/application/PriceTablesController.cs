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
        /// <param name="modelView"></param>
        public AddPriceTableEntryModelView addMaterialPriceTableEntry(AddPriceTableEntryModelView modelView)
        {
            return AddMaterialPriceTableEntryModelViewService.transform(modelView);
        }
    }
}