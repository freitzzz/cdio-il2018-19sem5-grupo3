using System;
using System.Collections.Generic;
using System.Linq;
using core.domain;
using core.modelview.customizeddimensions;
using core.modelview.customizedproduct;
using core.services;

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
        /// <exception cref="System.ArgumentException">Thrown when the provided instance of Slot is null.</exception>
        public static GetSlotModelView fromEntity(Slot slot)
        {
            return fromEntity(slot, MeasurementUnitService.getMinimumUnit());
        }

        /// <summary>
        /// Converts an instance of Slot into an instance of GetSlotModelView.
        /// </summary>
        /// <param name="slot">Instance of Slot being converted.</param>
        /// <param name="unit">String representing the unit to which the Slot's dimensions will be converted to.</param>
        /// <returns>Instance of GetSlotModelView.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the provided instance of Slot is null.</exception>
        public static GetSlotModelView fromEntity(Slot slot, string unit)
        {
            if (slot == null)
            {
                throw new ArgumentException(ERROR_NULL_SLOT);
            }

            GetSlotModelView slotModelView = new GetSlotModelView();
            slotModelView.slotId = slot.Id;
            slotModelView.slotDimensions = CustomizedDimensionsModelViewService.fromEntity(slot.slotDimensions, unit);

            if (slot.customizedProducts.Any())
            {
                slotModelView.customizedProducts = CustomizedProductModelViewService.fromCollection(slot.customizedProducts);
            }

            return slotModelView;
        }

        /// <summary>
        /// Converts an IEnumerable of Slot into an instance of GetAllSlotsModelView.
        /// </summary>
        /// <param name="slots">IEnumerable of Slot being converted.</param>
        /// <returns>An instance of GetAllSlotsModelView representing all of the Slots in the IEnumerable.</returns>
        public static GetAllSlotsModelView fromCollection(IEnumerable<Slot> slots)
        {
            return fromCollection(slots, MeasurementUnitService.getMinimumUnit());
        }

        /// <summary>
        /// Converts an IEnumerable of Slot into an instance of GetAllSlotsModelView.
        /// </summary>
        /// <param name="slots">IEnumerable of Slot being converted.</param>
        /// <returns>An instance of GetAllSlotsModelView representing all of the Slots in the IEnumerable.</returns>
        ///<exception cref="System.ArgumentException">Thrown when the provided IEnumerable of Slot is null.</exception>
        public static GetAllSlotsModelView fromCollection(IEnumerable<Slot> slots, string unit)
        {
            if (slots == null)
            {
                throw new ArgumentException();
            }

            GetAllSlotsModelView allSlotsModelView = new GetAllSlotsModelView();
            foreach (Slot slot in slots)
            {
                allSlotsModelView.Add(fromEntity(slot, unit));
            }

            return allSlotsModelView;
        }
    }
}