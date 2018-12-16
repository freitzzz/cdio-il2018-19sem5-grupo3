//@ts-check

/**
 * Requires Face properties
 */
import Face from './../../api/domain/Face';

/**
 * Requires Three.js CubeGeometry, Material and Mesh
 */
import {CubeGeometry,Material,Mesh} from 'three';

/**
 * Represents a Product Face build with Three.js
 */
export default class ThreeFace extends Face{
    
    /**
     * Builds a new Three.js Product Face
     * @param {Number} faceId Number with the face id
     * @param {Material} faceMaterial Material with the face material
     * @param {String} faceOrientation FaceOrientation with the face orientation
     * @param {Number} faceWidth Number with the face width dimension value
     * @param {Number} faceHeight Number with the face height dimension value
     * @param {Number} faceDepth Number with the face depth dimension value
     * @param {Number} faceXAxis Number with the face X axis value
     * @param {Number} faceYAxis Number with the face Y axis value
     * @param {Number} faceZAxis Number with the face Z axis value
     */
    constructor(faceId=null,faceMaterial=null,faceOrientation,faceWidth,faceHeight,faceDepth,faceXAxis,faceYAxis,faceZAxis){
        super(faceId,faceOrientation,faceWidth,faceHeight,faceDepth,faceXAxis,faceYAxis,faceZAxis);
        this.faceMaterial=faceMaterial;
        this.faceMesh=null;
    }

    /**
     * Returns the current face Three.js material
     */
    material(){return this.faceMaterial;}

    /**
     * Returns the current face Three.js mesh
     */
    mesh(){return this.faceMesh};

    /**
     * Changes the current face Three.js material
     * @param {Material} material Material with the new face material
     */
    changeMaterial(material){this.faceMaterial=material;}

    /**
     * Draws the current Three.js face
     * @returns {Mesh} Mesh with the created face mesh
     */
    draw(){
        let faceCubeGeometry=new CubeGeometry(this.faceWidth,this.faceHeight,this.faceDepth);
        let faceMesh=new Mesh(faceCubeGeometry,this.faceMaterial);
        faceMesh.position.x=this.faceXAxis;
        faceMesh.position.y=this.faceYAxis;
        faceMesh.position.z=this.faceZAxis;
        this.faceMesh=faceMesh;
        this.changeId(faceMesh.id);
        return this.faceMesh;
    }

    /**
     * Clones the current face and returns a new unique one
     * @return {Face} Face with the cloned face
     */
    clone(){return new ThreeFace(null,this.faceMaterial,this.faceOrientation,this.faceWidth,this.faceHeight,this.faceDepth,this.faceXAxis,this.faceYAxis,this.faceZAxis);}

}