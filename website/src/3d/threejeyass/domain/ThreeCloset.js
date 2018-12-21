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
import BaseProduct from '../../api/domain/BaseProduct';

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
     * @param {Object} slotValues Object with the slot values (spacing) #TODO: REVIEW THIS!!!
     */
    addClosetSlot(slotValues){
        let slot=super.addClosetSlot(slotValues);
        let drawnSlot=slot.draw();
        this.faceGroup.add(drawnSlot);
        return slot;
    }
    
    /**
     * Adds a new product into a certain slot of the closet
     * @param {Number} slotId Number with the slot identifier where the product will be inserted
     * @param {BaseProduct} product BaseProduct with the product to be added on the current closet slot
     */
    addProduct(slotId,product){
        if(!product.isDrawn())
            this.faceGroup.add(product.draw());
        super.addProduct(slotId,product);
    }

    /**
     * Removes a slot from the closet
     */
    removeClosetSlot(slotFace) {
        super.removeClosetSlot(slotFace);
        this.faceGroup.remove(slotFace.id());
    }

    /**
     * Returns the current closet Three.js group
     */
    getThreeGroup(){return this.faceGroup;}

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