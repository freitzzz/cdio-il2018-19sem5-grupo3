/**
 * Represents a "Closet" object
 */
Closet=function(){
    /**
     * Dimensions and Axes values for the closet base face
     */
    this.closet_base_face_dimensions_axes=[215,4.20,100,0,0,0];

    /**
     * Dimensions and Axes values for the closet top face
     */
    this.closet_top_face_dimensions_axes=[204.5,4.20,100,0,100,0];
    /**
     * Dimensions and Axes values for the closet left face
     */
    this.closet_left_face_dimensions_axes=[4.20,100,100,-100,50,0];
    /**
     * Dimensions and Axes values for the closet right face
     */
    this.closet_right_face_dimensions_axes=[4.20,100,100,100,50,0];
    /**
     * Dimensions and Axes values for the closet back face
     */
    this.closet_back_face_dimensions_axes=[200,100,0,0,50,-50];

    /**
     * Array with all closet faces
     */
    this.closet_faces=[this.closet_base_face_dimensions_axes,this.closet_top_face_dimensions_axes,this.closet_left_face_dimensions_axes,this.closet_right_face_dimensions_axes,this.closet_back_face_dimensions_axes];

    /**
     * Array with the initial closet faces values
     */
    this.initial_closet_faces=[this.closet_base_face_dimensions_axes.slice(),this.closet_top_face_dimensions_axes.slice(),this.closet_left_face_dimensions_axes.slice(),this.closet_right_face_dimensions_axes.slice(),this.closet_back_face_dimensions_axes.slice()];

    /**
     * Current closet slots
     */
    this.closet_slots=1;

    /**
     * Current closet slots faces
     */
    this.closet_slots_faces=[];

    /**
     * Array with the initial closet faces values
     */
    this.initial_closet_slots_faces=[];

    /**
     * Changes the closet width
     */
    this.changeClosetWidth=function(width){
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
    this.changeClosetHeight=function(height){
        if(height>0){
            var axesHeight=height/2;
            this.closet_top_face_dimensions_axes[4]=axesHeight+(this.closet_left_face_dimensions_axes[1]/2);
            this.closet_base_face_dimensions_axes[4]=-axesHeight+(this.closet_left_face_dimensions_axes[1]/2);
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
    this.changeClosetDepth=function(depth){
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
     * Adds a slot to the closet
     */
    this.addSlot=function(){
        this.closet_slots++;
        this.closet_slots_faces.push(this.closet_right_face_dimensions_axes.slice());
        this.initial_closet_slots_faces.push(this.closet_right_face_dimensions_axes.slice());
        updateClosetSlots(this);
        return this.closet_slots_faces[this.closet_slots_faces.length-1];
    }

    /**
     * Removes a slot from the closet
     */
    this.removeSlot=function(){
        if(this.closet_slots>1){
            this.closet_slots--;
            this.closet_slots_faces.pop();
            this.initial_closet_slots_faces.pop();
            updateClosetSlots(this);
        }
    }
    
    /**
     * Updates the closet slots size
     * @param {MYC.Closet} closet MYC.Closet with the closet which slots will be updated
     */
    function updateClosetSlots(closet){
        var distance_per_slot=(closet.closet_right_face_dimensions_axes[3]-closet.closet_left_face_dimensions_axes[3])/closet.closet_slots;
        var left_closet_face_x_value=closet.closet_left_face_dimensions_axes[3];
        for(var i=0;i<closet.closet_slots_faces.length;i++){
            if(i==0){
                closet.closet_slots_faces[i][3]=left_closet_face_x_value+(distance_per_slot);
            }else{
                closet.closet_slots_faces[i][3]=closet.closet_slots_faces[i-1][3]+(distance_per_slot);
            }
        }
    }
}