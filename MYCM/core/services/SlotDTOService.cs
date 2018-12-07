using System;
using System.Collections.Generic;
using core.domain;
using core.dto;
using support.dto;
using System.Linq;
using core.persistence;
using core.services.ensurance;

namespace core.services
{
    public sealed class SlotDTOService
    {
        /// <summary>
        /// Transform a SlotDTO into a Slot via a service
        /// </summary>
        /// <param name="slotDTO">slotDTO object to be transformed</param>
        /// <returns></returns>
        /* public Slot transform(SlotDTO slotDTO)
        {
            CustomizedDimensions customizedDimensions = DTOUtils.
            reverseDTO(slotDTO.customizedDimensions);

            IEnumerable<CustomizedProduct> customizedProducts = new List<CustomizedProduct>();
            if (slotDTO.customizedProducts != null)
            {
                customizedProducts = PersistenceContext.
                repositories().
                    createCustomizedProductRepository().
                        findCustomizedProductsByTheirPIDS(slotDTO.customizedProducts);
                FetchEnsurance.ensureSlotsCustomizedProductsFetchWasSuccessful(slotDTO.customizedProducts, customizedProducts);
            }

            Slot slot = new Slot(customizedDimensions);

            foreach (CustomizedProduct customizedProduct in customizedProducts)
            {
                slot.addCustomizedProduct(customizedProduct);
            }

            return slot;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotDTOs"></param>
        /// <returns></returns>
        public IEnumerable<Slot> transform(IEnumerable<SlotDTO> slotDTOs)
        {

            List<Slot> slots = new List<Slot>();
            foreach (SlotDTO slotDTO in slotDTOs)
            {
                slots.Add(transform(slotDTO));
            }
            return slots;
        } */
    }
}