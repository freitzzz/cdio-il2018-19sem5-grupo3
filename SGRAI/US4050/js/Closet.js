/**
 * Represents the internal core of a Closet
 */
class Closet{
    //This is the representation of a Closet as an ES6 class
    //In ES6 no properties are defined out of constructors context
    //So properties should only be defined in constructor context
    //The properties defined below are only for allowing a better comprehensive state of the object
    
    /**
     * Dimensions and Axes values for the closet base face
     */
    //closet_base_face_dimensions_axes=[];

    /**
     * Dimensions and Axes values for the closet top face
     */
    //closet_top_face_dimensions_axes=[];
    
    /**
     * Dimensions and Axes values for the closet left face
     */
    //closet_left_face_dimensions_axes=[];
    
    /**
     * Dimensions and Axes values for the closet right face
     */
    //closet_right_face_dimensions_axes=[];
    
    /**
     * Dimensions and Axes values for the closet back face
     */
    //closet_back_face_dimensions_axes=[];

    /**
     * Array with all closet faces
     */
    //closet_faces=[];

    /**
     * Array with the initial closet faces values
     */
    //initial_closet_faces=[];

    /**
     * Current closet slots
     */
    //closet_slots=1;

    /**
     * Current closet slots faces
     */
    //closet_slots_faces=[];

    /**
     * Array with the initial closet faces values
     */
    //initial_closet_slots_faces=[];
    
    /**
     * Builds a new Closet with the dimensions and axes values for all faces
     * @param {Array} closet_base_face_dimensions_axes Array with the base face dimensions and axes values
     * @param {Array} closet_top_face_dimensions_axes Array with the top face dimensions and axes values
     * @param {Array} closet_left_face_dimensions_axes Array with the left face dimensions and axes values
     * @param {Array} closet_right_face_dimensions_axes Array with the right face dimensions and axes values
     * @param {Array} closet_back_face_dimensions_axes Array with the back face dimensions and axes values
     */
    constructor(closet_base_face_dimensions_axes
               ,closet_top_face_dimensions_axes
               ,closet_left_face_dimensions_axes
               ,closet_right_face_dimensions_axes
               ,closet_back_face_dimensions_axes){
        this._checkClosetFace(closet_base_face_dimensions_axes);
        this._checkClosetFace(closet_top_face_dimensions_axes);
        this._checkClosetFace(closet_left_face_dimensions_axes);
        this._checkClosetFace(closet_right_face_dimensions_axes);
        this._checkClosetFace(closet_back_face_dimensions_axes);
        this.closet_base_face_dimensions_axes=closet_base_face_dimensions_axes.slice();
        this.closet_top_face_dimensions_axes=closet_top_face_dimensions_axes.slice();
        this.closet_left_face_dimensions_axes=closet_left_face_dimensions_axes.slice();
        this.closet_right_face_dimensions_axes=closet_right_face_dimensions_axes.slice();
        this.closet_back_face_dimensions_axes=closet_back_face_dimensions_axes.slice();
        console.log(this.closet_back_face_dimensions_axes);
        this._prepare_closet_init();
    }

    /**
     * Changes the closet width
     */
    changeClosetWidth(width){
        if(width>0){
            var axesWidth=width/2;
            this.closet_base_face_dimensions_axes[0]=width;
            this.closet_top_face_dimensions_axes[0]=width;
            this.closet_back_face_dimensions_axes[0]=width;
            this.closet_left_face_dimensions_axes[3]=-axesWidth;
            this.closet_right_face_dimensions_axes[3]=axesWidth;
        }else{
            //THROW ERROR
        }
    }

    /**
     * Changes the closet height
     */
    changeClosetHeight(height){
        if(height>0){
            var axesHeight=height/2;
            var heighpos =this.closet_top_face_dimensions_axes[4];
            this.closet_top_face_dimensions_axes[4]=(this.closet_top_face_dimensions_axes[4]-this.closet_left_face_dimensions_axes[1]/2)+axesHeight;
            this.closet_base_face_dimensions_axes[4]=(this.closet_base_face_dimensions_axes[4]+this.closet_left_face_dimensions_axes[1]/2)-axesHeight;
            this.closet_left_face_dimensions_axes[1]=height;
            this.closet_right_face_dimensions_axes[1]=height;
            this.closet_back_face_dimensions_axes[1]=height;
            for(var i=0;i<this.closet_slots_faces.length;i++){
                this.closet_slots_faces[i][1]=height;
            }
        }else{
            //THROW ERROR
        }
    }   

    /**
     * Changes the closet depth
     */
    changeClosetDepth(depth){
        if(depth>0){
            var axesDepth=depth/2;
            this.closet_base_face_dimensions_axes[2]=depth;
            this.closet_top_face_dimensions_axes[2]=depth;
            this.closet_back_face_dimensions_axes[5]=-axesDepth;
            this.closet_left_face_dimensions_axes[2]=depth;
            this.closet_right_face_dimensions_axes[2]=depth;
            for(var i=0;i<this.closet_slots_faces.length;i++){
                this.closet_slots_faces[i][2]=depth;
            }
        }else{
            //THROW ERROR
        }
    }

    /**
     * Changes the current closet slots
     */
    computeNewClosetSlots(slots){
        if(slots>=1){
            return (slots)-this.closet_slots;
        }else{
            //THROW ERROR
        }
    }

    /**
     * Adds a slot to the closet
     */
    addSlot(){
        this.closet_slots++;
        this.closet_slots_faces.push(this.closet_right_face_dimensions_axes.slice());
        this.initial_closet_slots_faces.push(this.closet_right_face_dimensions_axes.slice());
        _updateClosetSlots(this);
        return this.closet_slots_faces[this.closet_slots_faces.length-1];
    }

    /**
     * Removes a slot from the closet
     */
    removeSlot(){
        if(this.closet_slots>1){
            this.closet_slots--;
            this.closet_slots_faces.pop();
            this.initial_closet_slots_faces.pop();
            _updateClosetSlots(this);
        }
    }
    
    /**
     * Returns the current width of the closet
     */
    getClosetWidth(){return this.closet_base_face_dimensions_axes[0];}

    /**
     * Returns the current height of the closet
     */
    getClosetHeight(){return this.closet_left_face_dimensions_axes[1];}

    /**
     * Returns the current depth of the closet
     */
    getClosetDepth(){return this.closet_base_face_dimensions_axes[2];}

    /**
     * Updates the closet slots size
     */
    _updateClosetSlots(){
        var distance_per_slot=(this.closet_right_face_dimensions_axes[3]-this.closet_left_face_dimensions_axes[3])/closet.closet_slots;
        var left_closet_face_x_value=this.closet_left_face_dimensions_axes[3];
        for(var i=0;i<this.closet_slots_faces.length;i++){
            if(i==0){
                this.closet_slots_faces[i][3]=left_closet_face_x_value+(distance_per_slot);
            }else{
                this.closet_slots_faces[i][3]=this.closet_slots_faces[i-1][3]+(distance_per_slot);
            }
        }
    }

    /**
     * Prepare the closet initialization
     */
    _prepare_closet_init(){
        this.closet_faces=[this.closet_base_face_dimensions_axes,this.closet_top_face_dimensions_axes,this.closet_left_face_dimensions_axes,this.closet_right_face_dimensions_axes,this.closet_back_face_dimensions_axes];
        this.initial_closet_faces=[this.closet_base_face_dimensions_axes.slice(),this.closet_top_face_dimensions_axes.slice(),this.closet_left_face_dimensions_axes.slice(),this.closet_right_face_dimensions_axes.slice(),this.closet_back_face_dimensions_axes.slice()];
        this.closet_slots=1;
        this.closet_slots_faces=[];
        this.initial_closet_faces=[];
    }

    /**
     * Checks if a closet face is valid
     */
    _checkClosetFace(){
        //TODO: Implement method
    }
}