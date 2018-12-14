//@ts-check

/**
 * Requires BaseProduct for product base properties
 */
import BaseProduct from "./BaseProduct";

/**
 * Requires Face for drawer faces
 */
import Face from './Face';

/**
 * Requires FaceOrientation for identifying drawer faces orientation
 */
import FaceOrientation from './FaceOrientation';

/**
 * Requires ProductType for identifying the drawer product type
 */
import ProductType from "./ProductType";

/**
 * Represents the internal core of a Drawer
 */
export default class Drawer extends BaseProduct{
    
    /**
     * Builds a new Drawer with the dimensions and axes values for all faces
     * @param {Map<String,Face>} drawer_faces Map with the drawer faces
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(drawer_faces,productId,slotId=null){
        super(ProductType.DRAWER,productId,slotId);
        this.drawer_faces=Object.assign({},drawer_faces);
        this.initial_drawer_faces=Object.assign({},drawer_faces);
    }

    //Drawer Logic

    /**
     * Changes the drawer width
     */
    changeDrawerWidth(width){
        if(width>0){
            var axesWidth=width/2;
            this.drawer_faces.get(FaceOrientation.BASE).changeWidth(width);
            this.drawer_faces.get(FaceOrientation.FRONT).changeWidth(width);
            this.drawer_faces.get(FaceOrientation.BACK).changeWidth(width);
            this.drawer_faces.get(FaceOrientation.LEFT).changeXAxis(-axesWidth);
            this.drawer_faces.get(FaceOrientation.RIGHT).changeXAxis(-axesWidth);
        }
    }

    /**
     * Changes the drawer height
     */
    changeDrawerHeight(height){
        if(height>0){
            var axesHeight=height/2;
            this.drawer_faces.get(FaceOrientation.BASE).changeYAxis((this.drawer_faces.get(FaceOrientation.BASE).Y()+this.drawer_faces.get(FaceOrientation.LEFT).height()/2)-axesHeight);
            this.drawer_faces.get(FaceOrientation.LEFT).changeHeight(height);
            this.drawer_faces.get(FaceOrientation.RIGHT).changeHeight(height);
            this.drawer_faces.get(FaceOrientation.BACK).changeHeight(height);
            this.drawer_faces.get(FaceOrientation.FRONT).changeHeight(height);
        }
    }   

    /**
     * Changes the drawer depth
     */
    changeDrawerDepth(depth){
        if(depth>0){
            var axesDepth=depth/2;
            this.drawer_faces.get(FaceOrientation.BASE).changeDepth(depth);
            this.drawer_faces.get(FaceOrientation.FRONT).changeDepth(axesDepth);
            this.drawer_faces.get(FaceOrientation.BACK).changeZAxis(-axesDepth);
            this.drawer_faces.get(FaceOrientation.LEFT).changeDepth(depth);
            this.drawer_faces.get(FaceOrientation.RIGHT).changeDepth(depth);
        }else{
            //THROW ERROR
        }
    }
    
    //Accessors

    /**
     * Returns the current width of the drawer
     */
    getDrawerWidth(){return this.drawer_faces.get(FaceOrientation.BASE).width();}

    /**
     * Returns the current height of the drawer
     */
    getDrawerHeight(){return this.drawer_faces.get(FaceOrientation.LEFT).height();}

    /**
     * Returns the current depth of the drawer
     */
    getDrawerDepth(){return this.drawer_faces.get(FaceOrientation.BASE).depth();}

    /**
     * Returns all current drawer initial drawer
     */
    getInitialDrawerFaces(){return this.initial_drawer_faces;}
    
    /**
     * Returns all current drawer drawer
     */
    getDrawerFaces(){return this.drawer_faces;}
}