//@ts-check

/**
 * Requires Face properties
 */
import Face from '../../api/domain/Face';

/**
 * Requires Three.js Group for representing Three.js hinged doors
 */
import {Group} from 'three';

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
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(face,productId=null,slotId=null){
        super(face,productId,slotId);
        this.faceMesh=null;
        this.faceGroup=null;
    }

    /**
     * Returns the current hinged door Three.js face
     */
    getThreeFace(){return this.faceMesh;}

    /**
     * Draws the current Three.js hinged door
     * @returns {Group} Group with the created hinged door
     */
    draw(){
        let hingedDoorGroup=new Group();
        let hingeedDoorMesh=this.face.draw();
        hingedDoorGroup.add(hingeedDoorMesh);
        this.faceMesh=hingeedDoorMesh;
        this.faceGroup=hingedDoorGroup;
        this.baseId=hingedDoorGroup.id;
        return hingedDoorGroup;
    }
}