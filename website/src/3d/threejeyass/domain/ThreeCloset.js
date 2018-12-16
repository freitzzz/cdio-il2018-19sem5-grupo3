//@ts-check

/**
 * Requires Closet base properties
 */
import Closet from '../../api/domain/Closet';

/**
 * Requires ThreeFace properties
 */
import ThreeFace from '../../threejeyass/domain/ThreeFace';

/**
 * Requires Three.js Group and Mesh
 */
import {Group,Mesh} from 'three';

/**
 * Represents a Closet built with Three.js properties
 */
export default class ThreeCloset extends Closet{

    /**
     * Builds a new ThreeCloset with its defined faces
     * @param {Map<String,ThreeFace>} closet_faces Map with the closet faces
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(closet_faces,productId,slotId=null){
        super(closet_faces,productId,slotId);
        this.faceGroup=null;
    }

    /**
     * Adds a new slot to the closet
     */
    addClosetSlot(){
        let slot=super.addClosetSlot();
        let drawnSlot=slot.draw();
        this.faceGroup.add(drawnSlot);
        return slot;
    }

    /**
     * Draws the current Three.js closet
     * @returns {Group} Group with the created closet
     */
    draw(){
        let closetGroup=new Group();
        let closetFaces=this.getClosetFaces().entries();
        for(let closetFace of closetFaces)
            closetGroup.add(closetFace["1"].draw());
        this.faceGroup=closetGroup;
        this.baseId=closetGroup.id;
        return closetGroup;
    }
}