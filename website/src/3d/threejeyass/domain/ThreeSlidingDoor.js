//@ts-check

/**
 * Requires Face properties
 */
import Face from '../../api/domain/Face';

/**
 * Requires Three.js Object3D for representing Three.js hinged door face
 */
import {Object3D} from 'three';

/**
 * Requires SlidingDoor properties
 */
import SlidingDoor from '../../api/domain/SlidingDoor';

/**
 * Represents a SlidingDoor built with Three.js properties
 */
export default class ThreeSlidingDoor extends SlidingDoor{

    /**
     * Builds a new ThreeSlidingDoor
     * @param {Face} face Face with the sliding door face
     * @param {Object3D} three_face Object3D with the Three.js sliding door face
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(face,three_face,productId,slotId){
        super(face,productId,slotId);
        this.three_face=three_face;
    }

    /**
     * Returns the current sliding door Three.js face
     */
    getThreeFace(){return this.three_face;}
}