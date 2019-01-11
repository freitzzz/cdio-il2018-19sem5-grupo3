//@ts-check

/**
 * Requires Face properties
 */
import Face from '../../api/domain/Face';

/**
 * Requires Three.js Group for representing Three.js sliding doors 
 */
import {Group} from 'three';

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
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(face,productId=null,slotId=null){
        super(face,productId,slotId);
        this.faceMesh=null;
        this.faceGroup=null;
    }

    /**
     * Returns the current sliding door Three.js face
     */
    getThreeFace(){return this.faceMesh;}

    /**
     * Draws the current Three.js sliding door
     * @returns {Group} Group with the created sliding door
     */
    draw(){
        let slidingDoorGroup=new Group();
        let slidingDoorMesh=this.face.draw();
        slidingDoorGroup.add(slidingDoorMesh);
        this.faceMesh=slidingDoorMesh;
        this.faceGroup=slidingDoorGroup;
        this.baseId=slidingDoorGroup.id;
        return slidingDoorGroup;
    }
}