//@ts-check

/**
 * Requires BaseProduct for product base properties
 */
import BaseProduct from "./BaseProduct";

/**
 * Requires Face for representing the shelf face
 */
import Face from './Face';

/**
 * Requires ProductType for identifying the shelf product type
 */
import ProductType from "./ProductType";

/**
 * Represents the internal core of a shelf
 */
export default class Shelf extends BaseProduct{

    /**
     * Builds a new Shelf
     * @param {Face} face Face with the shelf face
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(face,productId,slotId=null) {
        super(ProductType.SHELF,productId,slotId);
        this.face=face;
        this.initial_face=Object.assign({},face);
    }

    /**
     * Returns the current width of the shelf
     */
    getShelfWidth() { return this.face.width(); }

    /**
     * Returns the current height of the shelf
     */
    getShelfHeight() { return this.face.height(); }

    /**
     * Returns the current depth of the shelf
     */
    getShelfDepth() { return this.face.depth(); }

    /**
     * Returns all current shelf initial faces
     */
    getInitialShelfFaces() { return [this.initial_face]; }

    /**
     * Returns all current shelf faces
     */
    getShelfFaces() { return [this.face]; }

    /**
    * Changes the width of the Shelf
    * @param {Number} width Number with the new shelf width
    */
    changeShelfWidth(width) { this.face.changeWidth(width); }
}