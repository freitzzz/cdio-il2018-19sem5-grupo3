//@ts-check

/**
 * Requires Closet base properties
 */
import Closet from '../../api/domain/Closet';

/**
 * Requires Face properties
 */
import Face from '../../api/domain/Face';

/**
 * Requires Three.js Object3D for representing Three.js closet faces
 */
import {Object3D} from 'three';

/**
 * Represents a Closet built with Three.js properties
 */
export default class ThreeCloset extends Closet{

    /**
     * Builds a new ThreeCloset with its defined faces
     * @param {Map<String,Face>} closet_faces Map with the closet faces
     * @param {Map<String,Object3D>} three_closet_faces Map with the Three.js closet faces
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(closet_faces,three_closet_faces,productId,slotId=null){
        super(closet_faces,productId,slotId);
        this.three_closet_faces=three_closet_faces;
    }

    /**
     * Returns the current closet Three.js faces
     */
    getThreeFaces(){return this.three_closet_faces};
}