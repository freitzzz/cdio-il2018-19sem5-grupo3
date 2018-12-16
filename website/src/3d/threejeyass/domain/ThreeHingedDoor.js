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
 * Requires HingedDoor base properties
 */
import HingedDoor from '../../api/domain/HingedDoor';

/**
 * Represents a HingedDoor built with Three.js properties
 */
export default class ThreeHingeedDoor extends HingedDoor{

    /**
     * Builds a new ThreeHingedDoor
     * @param {Face} face Face with the hinged door face
     * @param {Object3D} three_face Object3D with Three.js hinged door face
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(face,three_face,productId,slotId=null){
        super(face,productId,slotId);
        this.three_face=three_face;
    }

    /**
     * Returns the current hinged door Three.js face
     */
    getThreeFace(){return this.three_face;}
}