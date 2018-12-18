//@ts-check

/**
 * Requires BaseProduct for product base properties
 */
import BaseProduct from "./BaseProduct";

/**
 * Requires Face for representing module faces
 */
import Face from './Face';

/**
 * Requires FaceOrientation for identifying module faces orientations
 */
import FaceOrientation from './FaceOrientation';

/**
 * Requires ProductType for identifying the module product type
 */
import ProductType from "./ProductType";

/**
 * Represents the internal core of a Module
 */
export default class Module extends BaseProduct{    

    /**
     * Builds a new Module
     * @param {Map<String,Face>} faces Map with the module faces
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(faces,productId,slotId){
        super(ProductType.MODULE,productId,slotId);
        this.faces=faces;
        this.initial_faces=Object.assign({},faces);
    }

    //Module Logic

    /**
     * Changes the module width
     */
    changeModuleWidth(width){
        if(width>0){
            var axesWidth=width/2;
            this.faces.get(FaceOrientation.BASE).changeWidth(width);
            this.faces.get(FaceOrientation.TOP).changeWidth(width);
            this.faces.get(FaceOrientation.LEFT).changeXAxis(-axesWidth);
            this.faces.get(FaceOrientation.RIGHT).changeXAxis(axesWidth);
        }
    }

    /**
     * Changes the module height
     */
    changeModuleHeight(height){
        if(height>0){
            var axesHeight=height/2;
            this.faces.get(FaceOrientation.TOP).changeYAxis((this.faces.get(FaceOrientation.TOP).Y()-this.faces.get(FaceOrientation.LEFT).height()/2)+axesHeight);
            this.faces.get(FaceOrientation.BASE).changeYAxis((this.faces.get(FaceOrientation.BASE).Y()+this.faces.get(FaceOrientation.LEFT).height()/2)-axesHeight);
            this.faces.get(FaceOrientation.LEFT).changeHeight(height);
            this.faces.get(FaceOrientation.RIGHT).changeHeight(height);
        }
    }   

    /**
     * Changes the module depth
     */
    changeModuleDepth(depth){
        if(depth>0){
            var axesDepth=depth/2;//TODO: Remove axesDepth ?
            this.faces.get(FaceOrientation.BASE).changeDepth(depth);
            this.faces.get(FaceOrientation.TOP).changeDepth(depth);
            this.faces.get(FaceOrientation.LEFT).changeDepth(depth);
            this.faces.get(FaceOrientation.RIGHT).changeDepth(depth);
        }
    }
    //Accessors
    /**
     * Returns the current width of the module
     */
    getModuleWidth(){return this.faces.get(FaceOrientation.BASE).width();}

    /**
     * Returns the current height of the module
     */
    getModuleHeight(){return this.faces.get(FaceOrientation.LEFT).height();}

    /**
     * Returns the current depth of the module
     */
    getModuleDepth(){return this.faces.get(FaceOrientation.BASE).depth();}

    /**
     * Returns all current module initial faces
     */
    getInitialModuleFaces(){return this.initial_faces;}
    
    /**
     * Returns all current module faces
     */
    getModuleFaces(){return this.faces;}
}