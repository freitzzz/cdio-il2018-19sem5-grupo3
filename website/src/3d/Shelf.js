//@ts-check

/**
 * Requires BaseProduct for product base properties
 */
import BaseProduct from "./BaseProduct";

/**
 * Represents the internal core of a shelf
 */
export default class Shelf extends BaseProduct{
    /**
     * Builds a new shelf with the dimensions and axes values for all faces
     * @param {Array} shelf_base_face_dimensions_axes Array with the base face dimensions and axes values
     */
    constructor(shelf_base_face_dimensions_axes, slotId) {
        this.shelf_base_face_dimensions_axes = shelf_base_face_dimensions_axes.slice();
        this.slotId = slotId;
        this._prepare_shelf_init();
    }

    /**
     * Returns the current width of the shelf
     */
    getShelfWidth() { return this.shelf_base_face_dimensions_axes[0]; }

    /**
     * Returns the current height of the shelf
     */
    getShelfHeight() { return this.shelf_base_face_dimensions_axes[1]; }

    /**
     * Returns the current depth of the shelf
     */
    getShelfDepth() { return this.shelf_base_face_dimensions_axes[2]; }

    /**
     * Returns all current shelf initial faces
     */
    getInitialShelfFaces() { return this.initial_shelf_faces; }

    /**
     * Returns all current shelf faces
     */
    getShelfFaces() { return this.shelf_faces; }

    /**
    * Changes the width of the Shelf
    * @param {Number} width 
    */
    changeShelfWidth(width) { this.width = width; }

    /**
     * Prepare the shelf initialization
     */
    _prepare_shelf_init() {
        this.shelf_faces = [this.shelf_base_face_dimensions_axes];
        this.initial_shelf_faces = [this.shelf_base_face_dimensions_axes.slice()];
    }
}