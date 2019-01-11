
/**
 * Requires Face for representing module faces
 */
import Face from '../../api/domain/Face';

/**
 * Requires Module base properties
 */
import Module from '../../api/domain/Module';

/**
 * Requires Three.js Group
 */
import {Group} from 'three';

//@ts-check

export default class ThreeModule extends Module{

    /**
     * Builds a new ThreeModule with its defined faces
     * @param {Map<String,Face>} faces Map with the module faces
     * @param {Number} productId Number with the product identifier
     * @param {Number} slotId Number with the slot identifier which the module is inserted on
     */
    constructor(faces,productId=null,slotId=null){
        super(faces,productId,slotId);
        this.moduleGroup=null;
    }

    /**
     * Returns the current module Three.js group
     */
    getThreeGroup(){return this.moduleGroup;}

    /**
     * Draws the current Three.js module
     * @returns {Group} Group with the created module
     */
    draw(){
        let moduleGroup=new Group();
        let moduleFaces=this.getModuleFaces().entries();
        for(let moduleFace of moduleFaces)
            moduleGroup.add(moduleFace["1"].draw());
        this.moduleGroup=moduleGroup;
        this.baseId=moduleGroup.id;
        return moduleGroup;
    }
}