using System;
using core.domain;
using core.modelview.customizeddimensions;
using core.modelview.customizedproduct;

namespace core.modelview.slot
{
    public static class SlotModelViewService
    {
        /// <summary>
        /// Constant representing the error message presented when the provided instance of Slot is null.
        /// </summary>
        private const string ERROR_NULL_SLOT = "The provided slot is invalid.";

        /// <summary>
        /// Converts an instance of Slot into an instance of GetSlotModelView.
        /// </summary>
        /// <param name="slot">Instance of Slot being converted.</param>
        /// <returns>Instance of GetSlotModelView.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the provided instance of Slot is null.</exception>
        public static GetSlotModelView fromEntity(Slot slot)
        {
            if (slot == null)
            {
                throw new ArgumentNullException(ERROR_NULL_SLOT);
            }

            GetSlotModelView slotModelView = new GetSlotModelView();
            slotModelView.slotId = slot.Id;
            slotModelView.slotDimensions = CustomizedDimensionsModelViewService.fromEntity(slot.slotDimensions);
            slotModelView.customizedProducts = CustomizedProductModelViewService.fromCollection(slot.customizedProducts);

            return slotModelView;
        }
    }
}