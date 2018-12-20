//@ts-check

/**
 * Requires OpenableProduct for openable product base properties
 */
import OpenableProduct from "./OpenableProduct";

/**
 * Requires Face for identifying the sliding door face
 */
import Face from './Face';

/**
 * Requires ProductType for identifying the sliding door product type
 */
import ProductType from "./ProductType";

/**
 * Represents a sliding door using box geometry
 */
export default class SlidingDoor extends OpenableProduct{

    /**
     * Builds a new SlidingDoor
     * @param {Face} face Face with the sliding door face
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(face,productId,slotId) {
        super(ProductType.SLIDING_DOOR,productId,slotId);
        this.face=face;
    }

    /**
     * Changes the height of the SlidingDoor
     * 
     * @param {Number} height New height of the door 
     */
    changeHeight(height) {
        if (height > 0) this.face.changeHeight(height);
    }

    /**
     * Changes the width of the SlidingDoor
     * 
     * @param {Number} width New width of the door 
     */
    changeWidth(width) {
        if (width > 0) this.face.changeWidth(width);
    }

    /**
     * Returns the height of the SlidingDoor
     */
    getHeight() {
        return this.face.height();
    }

    /**
     * Returns the width of the SlidingDoor
     */
    getWidth() {
        return this.face.width();
    }
}