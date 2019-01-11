//@ts-check

/**
 * Requires Shelf base properties
 */
import Shelf from '../../api/domain/Shelf';

/**
 * Requires Face properties
 */
import Face from '../../api/domain/Face';

/**
 * Requires Three.js Object3D for representing Three.js shelf face
 */
import {Object3D} from 'three';

/**
 * Represents a Shelf built with Three.js properties
 */
export default class ThreeShelf extends Shelf{

    /**
     * Builds a new ThreeShelf with its defined face
     * @param {Face} shelf_face Face with the shelf face
     * @param {Object3D} three_shelf_face Object3D with the Three.js dface
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(shelf_face,three_shelf_face,productId,slotId=null){
        super(shelf_face,productId,slotId);
        this.three_shelf_face=three_shelf_face;
    }

    /**
     * Returns the current shelf Three.js face
     */
    getThreeFace(){return this.three_shelf_face};
}