/**
 * Represents a "Closet" object
 */
MYC.Closet=function(){
    /**
     * Dimensions and Axes values for the closet base face
     */
    this.closet_base_face_dimensions_axes=[200,0,100,0,0,0];
    /**
     * Dimensions and Axes values for the closet top face
     */
    this.closet_top_face_dimensions_axes=[200,0,100,0,100,0];
    /**
     * Dimensions and Axes values for the closet left face
     */
    this.closet_left_face_dimensions_axes=[0,100,100,100,50,0];
    /**
     * Dimensions and Axes values for the closet right face
     */
    this.closet_right_face_dimensions_axes=[0,100,100,-100,50,0];
    /**
     * Dimensions and Axes values for the closet back face
     */
    this.closet_back_face_dimensions_axes=[200,100,0,0,50,-50];

    /**
     * Array with all closet faces
     */
    this.closet_faces=[closet_base_face_dimensions_axes,closet_top_face_dimensions_axes,closet_left_face_dimensions_axes,closet_right_face_dimensions_axes,closet_back_face_dimensions_axes];

    /**
     * Array with the closet faces ids (used for dynamic scale)
     */
    this.closet_faces_ids=[];

    /**
     * Current closet slots
     */
    this.closet_slots=1;

    /**
     * Current closet slots faces
     */
    this.closet_faces=[];


    /**
     * Adds a new slot to the closet
     */
    this.addNewSlot=function(){
        this.closet_slots++;
        this.closet_faces.push([closet]);
    }
}