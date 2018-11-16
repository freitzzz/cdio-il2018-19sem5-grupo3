/**
 * Represents the internal core of a shelf
 */
class Shelf{
    //This is the representation of a shelf as an ES6 class
    //In ES6 no properties are defined out of constructors context
    //So properties should only be defined in constructor context
    //The properties defined below are only for allowing a better comprehensive state of the object
    
    /**
     * Dimensions and Axes values for the shelf base face
     */
    //shelf_base_face_dimensions_axes=[];

    /**
     * Array with all shelf faces
     */
    //shelf_faces=[];

    /**
     * Array with the initial shelf faces values
     */
    //initial_shelf_faces=[];
    
    /**
     * Builds a new shelf with the dimensions and axes values for all faces
     * @param {Array} shelf_base_face_dimensions_axes Array with the base face dimensions and axes values
     */
    constructor(shelf_base_face_dimensions_axes){
        this._checkShelfFace(shelf_base_face_dimensions_axes);
        this.shelf_base_face_dimensions_axes=shelf_base_face_dimensions_axes.slice();
        this._prepare_shelf_init();
    }

    //Shelf Logic

    // /**
    //  * Changes the shelf width
    //  */
    // changeModuleWidth(width){
    //     if(width>0){
    //         var axesWidth=width/2;
    //         this.module_base_face_dimensions_axes[0]=width;
    //         this.module_top_face_dimensions_axes[0]=width;
    //         this.module_back_face_dimensions_axes[0]=width;
    //         this.module_left_face_dimensions_axes[3]=-axesWidth;
    //         this.module_right_face_dimensions_axes[3]=axesWidth;
    //     }else{
    //         //THROW ERROR
    //     }
    // }

    // /**
    //  * Changes the module height
    //  */
    // changeModuleHeight(height){
    //     if(height>0){
    //         var axesHeight=height/2;
    //         var heighpos =this.module_top_face_dimensions_axes[4];
    //         this.module_top_face_dimensions_axes[4]=(this.module_top_face_dimensions_axes[4]-this.module_left_face_dimensions_axes[1]/2)+axesHeight;
    //         this.module_base_face_dimensions_axes[4]=(this.module_base_face_dimensions_axes[4]+this.module_left_face_dimensions_axes[1]/2)-axesHeight;
    //         this.module_left_face_dimensions_axes[1]=height;
    //         this.module_right_face_dimensions_axes[1]=height;
    //         this.module_back_face_dimensions_axes[1]=height;
    //         for(var i=0;i<this.module_slots_faces.length;i++){
    //             this.module_slots_faces[i][1]=height;
    //         }
    //     }else{
    //         //THROW ERROR
    //     }
    // }   

    // /**
    //  * Changes the module depth
    //  */
    // changeModuleDepth(depth){
    //     if(depth>0){
    //         var axesDepth=depth/2;
    //         this.module_base_face_dimensions_axes[2]=depth;
    //         this.module_top_face_dimensions_axes[2]=depth;
    //         this.module_back_face_dimensions_axes[5]=-axesDepth;
    //         this.module_left_face_dimensions_axes[2]=depth;
    //         this.module_right_face_dimensions_axes[2]=depth;
    //         for(var i=0;i<this.module_slots_faces.length;i++){
    //             this.module_slots_faces[i][2]=depth;
    //         }
    //     }else{
    //         //THROW ERROR
    //     }
    // }

    //Accessors

    /**
     * Returns the current width of the shelf
     */
    getShelfWidth(){return this.shelf_base_face_dimensions_axes[0];}

    /**
     * Returns the current height of the shelf
     */
    getShelfHeight(){return this.shelf_left_face_dimensions_axes[1];}

    /**
     * Returns the current depth of the shelf
     */
    getShelfDepth(){return this.shelf_base_face_dimensions_axes[2];}

    /**
     * Returns all current shelf initial faces
     */
    getInitialShelfFaces(){return this.initial_shelf_faces;}
    
    /**
     * Returns all current shelf faces
     */
    getShelfFaces(){return this.shelf_faces;}

    //Private Methods

    /**
     * Prepare the shelf initialization
     */
    _prepare_shelf_init(){
        this.shelf_faces=[this.shelf_base_face_dimensions_axes];
        this.initial_shelf_faces=[this.shelf_base_face_dimensions_axes.slice()];
    }

    /**
     * Checks if a shelf face is valid
     */
    _checkShelfFace(){
        //TODO: Implement method
    }
}