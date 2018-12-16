//@ts-check

/**
 * Requires BaseProduct for representing products that can have slots
 */
import BaseProduct from './BaseProduct';

/**
 * Class that represents a product that can have slots
 */
export default class SlotableProduct extends BaseProduct{
    
    /**
     * Builds a new SlotableProduct
     * @param {Number} productId Number with the product identifier
     * @param {Number} slotId Number with the slot identifier which the product is placed on
     */
    constructor(productId,slotId=null){
        super(productId,slotId);
        this.slots=new Map();
    }

    /**
     * Adds a new slot to the product
     * @param {Number} slotId Number with the slot identifier
     */
    addSlot(slotId){
        this.slots.set(slotId,[]);
    }

    /**
     * Adds a new product into a certain slot
     * @param {Number} slotId Number with the slot identifier which product will be placed on
     * @param {BaseProduct} product BaseProduct with the product which will be placed on the desired slot
     */
    addProduct(slotId,product){
        this.slots.get(slotId).push(product);
    }

    /**
     * Removes a product from a certain slot
     * @param {Number} slotId Number with the slot identifier which product product will be removed from
     * @param {BaseProduct} product BaseProduct with the product which will be removed from the desired slot
     */
    removeProduct(slotId,product){
        this.slots.get(slotId).pop(product);
    }
}