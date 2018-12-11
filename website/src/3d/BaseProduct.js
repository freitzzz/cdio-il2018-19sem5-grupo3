//@ts-check

/**
 * Class that represents the base properties of a product
 */
export default class BaseProduct{
    
    /**
     * Builds a new BaseProduct
     * @param {Object} id Object with the product identifer
     * @param {Object} slotId Object with the slot identifier which the product is based on 
     */
    constructor(id,slotId=null){
        this.id=id;
        this.slotId=slotId;
    }

    /**
     * Changes the slot which the product is currently based on
     * @param {Number} slotId 
     */
    changeSlot(slotId){
        this.slotId=slotId;
    }

    /**
     * Returns the product identifier
     */
    id(){return this.id;}

    /**
     * Returns the identifier of the slot which product is based on
     */
    getSlotId(){return this.slotId;}

}