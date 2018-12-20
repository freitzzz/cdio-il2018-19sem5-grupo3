//@ts-check

/**
 * Requires OpenableProduct for openable product base properties
 */
import OpenableProduct from "./OpenableProduct";

/**
 * Requires Face for hinged door face properties
 */
import Face from './Face';

/**
 * Requires ProductType for identifying the hinged door product type
 */
import ProductType from "./ProductType";

/**
 * Represents a hinged door using box geometry
 */
export default class HingedDoor extends OpenableProduct{

    /**
     * Builds a new HingedDoor
     * @param {Face} face Face with the hinged door product face
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(face,productId,slotId=null) {
        super(ProductType.HINGED_DOOR,productId,slotId);
        this.face=face;
    }

    /**
     * Changes the height of the hingedDoor
     * 
     * @param {Number} height New height of the door 
     */
    changeHeight(height) { if (height > 0) this.face.changeHeight(height); }

    /**
     * Changes the width of the hingedDoor
     * 
     * @param {Number} width New width of the door 
     */
    changeWidth(width) { if (width > 0) this.face.changeWidth(width); }

    /**
     * Returns the height of the hingedDoor
     */
    getHeight() { return this.face.height(); }

    /**
     * Returns the width of the hingedDoor
     */
    getWidth() { return this.face.width(); }

    /**
     * Returns the face of the hinged door
     */
    getFace(){return this.face;}

    /**
     * Draws the current hinged door
     * @returns {Object} Object with the drawn hinged door
     */
    draw(){}
}