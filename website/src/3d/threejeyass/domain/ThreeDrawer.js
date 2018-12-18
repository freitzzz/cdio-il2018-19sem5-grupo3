//@ts-check

/**
 * Requires Drawer base properties
 */
import Drawer from '../../api/domain/Drawer';

/**
 * Requires Face properties
 */
import Face from '../../api/domain/Face';

/**
 * Requires Three.js Object3D for representing Three.js drawer faces
 */
import {Object3D} from 'three';

/**
 * Represents a Drawer built with Three.js properties
 */
export default class ThreeDrawer extends Drawer{

    /**
     * Builds a new ThreeDrawer with its defined faces
     * @param {Map<String,Face>} drawer_faces Map with the drawer faces
     * @param {Map<String,Object3D>} three_drawer_faces Map with the Three.js drawer faces
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(drawer_faces,three_drawer_faces,productId,slotId=null){
        super(drawer_faces,productId,slotId);
        this.three_drawer_faces=three_drawer_faces;
    }

    /**
     * Returns the current drawer Three.js faces
     */
    getThreeFaces(){return this.three_drawer_faces};
}