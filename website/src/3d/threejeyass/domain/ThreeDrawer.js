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
 * Requires Three.js Group and Object3D for representing Three.js drawer and drawer faces
 */
import {Group,Object3D} from 'three';

/**
 * Represents a Drawer built with Three.js properties
 */
export default class ThreeDrawer extends Drawer{

    /**
     * Builds a new ThreeDrawer with its defined faces
     * @param {Map<String,Face>} drawer_faces Map with the drawer faces
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(drawer_faces,productId=null,slotId=null){
        super(drawer_faces,productId,slotId);
        this.three_drawer_faces=null;
        this.drawerGroup=null;
    }

    /**
     * Returns the current drawer Three.js faces
     */
    getThreeFaces(){return this.three_drawer_faces};

    /**
     * Returns the current drawer Three.js group
     */
    getThreeGroup(){return this.drawerGroup;}

    /**
     * Draws the current Three.js drawer
     * @returns {Group} Group with the created drawer
     */
    draw(){
        let drawerGroup=new Group();
        let drawerFaces=this.getDrawerFaces().entries();
        this.three_drawer_faces=new Map();
        for(let drawerFace of drawerFaces){
            let drawnFace=drawerFace["1"].draw();
            drawerGroup.add(drawnFace);
            this.three_drawer_faces.set(drawerFace["0"],drawnFace);
        }
        this.drawerGroup=drawerGroup;
        this.baseId=drawerGroup.id;
        return drawerGroup;
    }
}