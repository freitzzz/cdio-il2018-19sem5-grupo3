/**
 * Represents the internal core of a Drawer
 */
class Drawer{
    //This is the representation of a Drawer as an ES6 class
    //In ES6 no properties are defined out of constructors context
    //So properties should only be defined in constructor context
    //The properties defined below are only for allowing a better comprehensive state of the object
    
    /**
     * Dimensions and Axes values for the drawer base face
     */
    //drawer_base_face_dimensions_axes=[];
 
    /**
     * Dimensions and Axes values for the drawer frent face
     */
    //drawer_frent_face_dimensions_axes=[];
    
    /**
     * Dimensions and Axes values for the drawer left face
     */
    //drawer_left_face_dimensions_axes=[];
    
    /**
     * Dimensions and Axes values for the drawer right face
     */
    //drawer_right_face_dimensions_axes=[];
    
    /**
     * Dimensions and Axes values for the drawer back face
     */
    //drawer_back_face_dimensions_axes=[];

    /**
     * Array with all drawer faces
     */
    //drawer_faces=[];

    /**
     * Array with the initial drawer faces values
     */
    //initial_drawer_faces=[];
    
    /**
     * Builds a new Drawer with the dimensions and axes values for all faces
     * @param {Array} drawer_base_face_dimensions_axes Array with the base face dimensions and axes values
     * @param {Array} drawer_frent_face_dimensions_axes Array with the frent face dimensions and axes values
     * @param {Array} drawer_left_face_dimensions_axes Array with the left face dimensions and axes values
     * @param {Array} drawer_right_face_dimensions_axes Array with the right face dimensions and axes values
     * @param {Array} drawer_back_face_dimensions_axes Array with the back face dimensions and axes values
     */
    constructor(drawer_base_face_dimensions_axes
               ,drawer_frent_face_dimensions_axes
               ,drawer_left_face_dimensions_axes
               ,drawer_right_face_dimensions_axes
               ,drawer_back_face_dimensions_axes){
        this._checkDrawerFace(drawer_base_face_dimensions_axes);
        this._checkDrawerFace(drawer_frent_face_dimensions_axes);
        this._checkDrawerFace(drawer_left_face_dimensions_axes);
        this._checkDrawerFace(drawer_right_face_dimensions_axes);
        this._checkDrawerFace(drawer_back_face_dimensions_axes);
        this.drawer_base_face_dimensions_axes=drawer_base_face_dimensions_axes.slice();
        this.drawer_frent_face_dimensions_axes=drawer_frent_face_dimensions_axes.slice();
        this.drawer_left_face_dimensions_axes=drawer_left_face_dimensions_axes.slice();
        this.drawer_right_face_dimensions_axes=drawer_right_face_dimensions_axes.slice();
        this.drawer_back_face_dimensions_axes=drawer_back_face_dimensions_axes.slice();
        
        this._prepare_drawer_init();
    }

    // //Drawer Logic

    // /**
    //  * Changes the drawer width
    //  */
    // changeDrawerWidth(width){
    //     if(width>0){
    //         var axesWidth=width/2;
    //         this.drawer_base_face_dimensions_axes[0]=width;
    //         this.drawer_frent_face_dimensions_axes[0]=width;
    //         this.drawer_back_face_dimensions_axes[0]=width;
    //         this.drawer_left_face_dimensions_axes[3]=-axesWidth;
    //         this.drawer_right_face_dimensions_axes[3]=axesWidth;
    //     }else{
    //         //THROW ERROR
    //     }
    // }

    // /**
    //  * Changes the drawer height
    //  */
    // changeDrawertHeight(height){
    //     if(height>0){
    //         var axesHeight=height/2;
    //         this.drawer_base_face_dimensions_axes[4]=(this.drawer_base_face_dimensions_axes[4]+this.drawer_left_face_dimensions_axes[1]/2)-axesHeight;
    //         this.drawer_left_face_dimensions_axes[1]=height;
    //         this.drawer_right_face_dimensions_axes[1]=height;
    //         this.drawer_back_face_dimensions_axes[1]=height;
    //         this.drawer_frent_face_dimensions_axes[1]=height;
            
    //     }else{
    //         //THROW ERROR
    //     }
    // }   

    // /**
    //  * Changes the drawer depth
    //  */
    // changeDrawerDepth(depth){
    //     if(depth>0){
    //         var axesDepth=depth/2;
    //         this.drawer_base_face_dimensions_axes[2]=depth;
    //         this.drawer_frent_face_dimensions_axes[2]=axesDepth;
    //         this.drawer_back_face_dimensions_axes[5]=-axesDepth;
    //         this.drawer_left_face_dimensions_axes[2]=depth;
    //         this.drawer_right_face_dimensions_axes[2]=depth;
    //     }else{
    //         //THROW ERROR
    //     }
    // }
    
    //Accessors

    /**
     * Returns the current width of the drawer
     */
    getDrawerWidth(){return this.drawer_base_face_dimensions_axes[0];}

    /**
     * Returns the current height of the drawer
     */
    getDrawerHeight(){return this.drawer_left_face_dimensions_axes[1];}

    /**
     * Returns the current depth of the drawer
     */
    getDrawerDepth(){return this.drawer_base_face_dimensions_axes[2];}

    /**
     * Returns all current drawer initial drawer
     */
    getInitialDrawerFaces(){return this.initial_drawer_faces;}
    
    /**
     * Returns all current drawer drawer
     */
    getDrawerFaces(){return this.drawer_faces;}

    //Private Methods

    /**
     * Prepare the drawer initialization
     */
    _prepare_drawer_init(){
        this.drawer_faces=[this.drawer_base_face_dimensions_axes,
            this.drawer_frent_face_dimensions_axes,
            this.drawer_left_face_dimensions_axes,
            this.drawer_right_face_dimensions_axes,
            this.drawer_back_face_dimensions_axes];
        this.initial_drawer_faces=[this.drawer_base_face_dimensions_axes.slice(),
            this.drawer_frent_face_dimensions_axes.slice(),
            this.drawer_left_face_dimensions_axes.slice(),
            this.drawer_right_face_dimensions_axes.slice(),
            this.drawer_back_face_dimensions_axes.slice()];
    }

    /**
     * Checks if a drawer face is valid
     */
    _checkDrawerFace(){
        //TODO: Implement method
    }
}