
/**
 * Requires BaseProduct properties and functionalities
 */
import BaseProduct from "./BaseProduct";

//@ts-check

/**
 * Represents a product that can be open
 */
export default class OpenableProduct extends BaseProduct{

    /**
     * Builds a new OpenableProduct
     * @param {String} productType String with the product type
     * @param {Number} productId Number with the product identifier
     * @param {Number} slotId Number with the slot identifier
     */
    constructor(productType,productId=null,slotId=null){
        super(productType,productId,slotId);
        this.isOpen=false;
    }

    /**
     * Opens the product
     */
    open(){this.isOpen=true;}
    
    /**
     * Closes the product
     */
    close(){this.isClosed=true;}

    /**
     * Checks if the product is currently open
     */
    isOpen(){return this.isOpen;}

    /**
     * Checks if the product is currently closed
     */
    isClosed(){return !this.isOpen();}

}