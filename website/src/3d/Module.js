//@ts-check

/**
 * Requires BaseProduct for product base properties
 */
import BaseProduct from "./BaseProduct";

/**
 * Represents the internal core of a Module
 */
export default class Module extends BaseProduct{    
    /**
     * Builds a new module with the dimensions and axes values for all faces
     * @param {Array} module_base_face_dimensions_axes Array with the base face dimensions and axes values
     * @param {Array} module_top_face_dimensions_axes Array with the top face dimensions and axes values
     * @param {Array} module_left_face_dimensions_axes Array with the left face dimensions and axes values
     * @param {Array} module_right_face_dimensions_axes Array with the right face dimensions and axes values
     */
    constructor(module_base_face_dimensions_axes
               ,module_top_face_dimensions_axes
               ,module_left_face_dimensions_axes
               ,module_right_face_dimensions_axes){
        this.module_base_face_dimensions_axes=module_base_face_dimensions_axes.slice();
        this.module_top_face_dimensions_axes=module_top_face_dimensions_axes.slice();
        this.module_left_face_dimensions_axes=module_left_face_dimensions_axes.slice();
        this.module_right_face_dimensions_axes=module_right_face_dimensions_axes.slice();
        this._prepare_module_init();
    }

    //Module Logic

    /**
     * Changes the module width
     */
    changeModuleWidth(width){
        if(width>0){
            var axesWidth=width/2;
            this.module_base_face_dimensions_axes[0]=width;
            this.module_top_face_dimensions_axes[0]=width;
            this.module_left_face_dimensions_axes[3]=-axesWidth;
            this.module_right_face_dimensions_axes[3]=axesWidth;
        }
    }

    /**
     * Changes the module height
     */
    changeModuleHeight(height){
        if(height>0){
            var axesHeight=height/2;
            this.module_top_face_dimensions_axes[4]=(this.module_top_face_dimensions_axes[4]-this.module_left_face_dimensions_axes[1]/2)+axesHeight;
            this.module_base_face_dimensions_axes[4]=(this.module_base_face_dimensions_axes[4]+this.module_left_face_dimensions_axes[1]/2)-axesHeight;
            this.module_left_face_dimensions_axes[1]=height;
            this.module_right_face_dimensions_axes[1]=height;
            
        }
    }   

    /**
     * Changes the module depth
     */
    changeModuleDepth(depth){
        if(depth>0){
            var axesDepth=depth/2;
            this.module_base_face_dimensions_axes[2]=depth;
            this.module_top_face_dimensions_axes[2]=depth;
            this.module_left_face_dimensions_axes[2]=depth;
            this.module_right_face_dimensions_axes[2]=depth;
        }
    }
    //Accessors
    /**
     * Returns the current width of the module
     */
    getModuleWidth(){return this.module_base_face_dimensions_axes[0];}

    /**
     * Returns the current height of the module
     */
    getModuleHeight(){return this.module_left_face_dimensions_axes[1];}

    /**
     * Returns the current depth of the module
     */
    getModuleDepth(){return this.module_base_face_dimensions_axes[2];}

    /**
     * Returns all current module initial faces
     */
    getInitialModuleFaces(){return this.initial_module_faces;}
    
    /**
     * Returns all current module faces
     */
    getModuleFaces(){return this.module_faces;}

    //Private Methods

    /**
     * Prepare the module initialization
     */
    _prepare_module_init(){
        this.module_faces=[this.module_base_face_dimensions_axes,
            this.module_top_face_dimensions_axes,
            this.module_left_face_dimensions_axes,
            this.module_right_face_dimensions_axes];
        this.initial_module_faces=[this.module_base_face_dimensions_axes.slice(),
            this.module_top_face_dimensions_axes.slice(),
            this.module_left_face_dimensions_axes.slice(),
            this.module_right_face_dimensions_axes.slice()];
    }
}