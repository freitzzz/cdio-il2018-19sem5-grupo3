//@ts-check

/**
 * Requires FaceOrientation for identifying the face orientation
 */
import FaceOrientation from './FaceOrientation';

/**
 * Class that represents a product face
 */
export default class Face{
    
    /**
     * Builds a new Product Face
     * @param {Number} faceId Number with the face id
     * @param {String} faceOrientation FaceOrientation with the face orientation
     * @param {Number} faceWidth Number with the face width dimension value
     * @param {Number} faceHeight Number with the face height dimension value
     * @param {Number} faceDepth Number with the face depth dimension value
     * @param {Number} faceXAxis Number with the face X axis value
     * @param {Number} faceYAxis Number with the face Y axis value
     * @param {Number} faceZAxis Number with the face Z axis value
     */
    constructor(faceId=null,faceOrientation,faceWidth,faceHeight,faceDepth,faceXAxis,faceYAxis,faceZAxis){
        this.faceId=faceId;
        this.faceOrientation=faceOrientation;
        this.faceWidth=faceWidth;
        this.faceHeight=faceHeight;
        this.faceDepth=faceDepth;
        this.faceXAxis=faceXAxis;
        this.faceYAxis=faceYAxis;
        this.faceZAxis=faceZAxis;
    }

    /**
     * Returns the face identifier
     */
    id(){return this.faceId;}

    /**
     * Returns the current face axis values
     */
    axis(){return {x:this.faceXAxis,y:this.faceYAxis,z:this.faceZAxis};}

    /**
     * Returns the current face dimensions values (Width,Height,Depth) 
     */
    dimensions(){return {faceWidth:this.faceWidth,faceHeight:this.faceHeight,faceDepth:this.faceDepth};}

    /**
     * Returns the current face width value
     */
    width(){return this.faceWidth;}
    
    /**
     * Returns the current face height value
     */
    height(){return this.faceHeight;}

    /**
     * Returns the current face depth value
     */
    depth(){return this.faceDepth;}

    /**
     * Returns the current face orientation
     */
    orientation(){return this.faceOrientation;}

    /**
     * Returns the current face X axis value
     */
    X(){return this.faceXAxis;}

    /**
     * Returns the current face Y axis value
     */
    Y(){return this.faceYAxis;}

    /**
     * Returns the current face Z axis value
     */
    Z(){return this.faceZAxis;}

    /**
     * Changes the id of the current face
     * @param {Number} faceId Number with the new face id
     */
    changeId(faceId){this.faceId=faceId;}

    /**
     * Changes the current face width
     * @param {Number} width Number with the face width
     */
    changeWidth(width){this.faceWidth=width};

    /**
     * Changes the current face height
     * @param {Number} height Number with the face height
     */
    changeHeight(height){this.faceHeight=height};

    /**
     * Changes the current face depth
     * @param {Number} depth Number with the face depth
     */
    changeDepth(depth){this.faceDepth=depth};

    /**
     * Changes the current face X axis
     * @param {Number} XAxis Number with the face X axis
     */
    changeXAxis(XAxis){this.faceXAxis=XAxis};

    /**
     * Changes the current face Y axis
     * @param {Number} YAxis Number with the face Y axis
     */
    changeYAxis(YAxis){this.faceYAxis=YAxis};

    /**
     * Changes the current face Z axis
     * @param {Number} ZAxis Number with the face Z axis
     */
    changeZAxis(ZAxis){this.faceZAxis=ZAxis};

    /**
     * Draws the current face
     * @returns {Object} Drawn Face
     */
    draw(){}
}