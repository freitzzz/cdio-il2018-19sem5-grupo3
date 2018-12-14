//@ts-check

/**
 * Represents the type of the base product
 */
import ProductType from './ProductType';

/**
 * Class that represents the base properties of a product
 */
export default class BaseProduct{
    
    /**
     * Builds a new BaseProduct
     * @param {String} productType String with the base product type
     * @param {Number} id Object with the product identifer
     * @param {Number} slotId Object with the slot identifier which the product is based on 
     */
    constructor(productType,id,slotId=null){
        this.productType=productType;
        this.baseId=id;
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
    id(){return this.baseId;}

    /**
     * Returns the identifier of the slot which product is based on
     */
    getSlotId(){return this.slotId;}

    /**
     * Returns the current product type
     */
    getProductType(){return this.productType;}

}