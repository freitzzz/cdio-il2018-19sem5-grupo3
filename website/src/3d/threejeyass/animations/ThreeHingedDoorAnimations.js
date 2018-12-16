//@ts-check

/**
 * Requires ThreeHingedDoor properties
 */
import ThreeHingedDoor from '../domain/ThreeHingedDoor';

/**
 * Service class that holds all three.js hinged door animations functionalities
 */
export default class ThreeHingedDoorAnimations{

    /**
     * Opens a HingedDoor
     * @param {ThreeHingedDoor} hingedDoor ThreeHingedDoor with the hinged door to be opened
     */
    //To open an hinged door we have followed the following logic
    //If the hingeed door rotation in Y axis is bigger then the half of a full rotation (-PI/2)
    //Then we meed to translate the hingeed door in X axis using hinged door rotation X axis
    //We continue by decrementing the hingeed door Y axis rotation by a little more than a quarter of a full rotation (PI/100)
    //To finalize we translate the hinged door in X axis again using hinged door X axis rotation value
    //While the hinged door Y axis rotation is greater then half of a full rotation we loop the computation
    static open(hingedDoor){
        let hingedDoorThreeFace=hingedDoor.getThreeFace();
        if(hingedDoorThreeFace.rotation.y>(-Math.PI/2)){
            let hingedDoorRotationXAxis=hingedDoorThreeFace.geometry.parameters.width/2;
            hingedDoorThreeFace.translateX(-hingedDoorRotationXAxis);
            hingedDoorThreeFace.rotation.y-=Math.PI/100;
            hingedDoorThreeFace.translateX(hingedDoorRotationXAxis);
            this.open(hingedDoor);
        }
        //TODO: Notify Render
        //TODO: Notify Controls
    }

    /**
     * Closes a HingedDoor
     * @param {ThreeHingedDoor} hingedDoor ThreeHingedDoor with the hinged door to be closed
     */
    //The logic for close animation is the inverse of the open one
    static close(hingedDoor){
        let hingedDoorThreeFace=hingedDoor.getThreeFace();
        if(hingedDoorThreeFace.rotation.y<0){
            let hingedDoorRotationXAxis=hingedDoorThreeFace.geometry.parameters.width/2;
            hingedDoorThreeFace.translateX(-hingedDoorRotationXAxis);
            hingedDoorThreeFace.rotation.y+=Math.PI/100;
            hingedDoorThreeFace.translateX(hingedDoorRotationXAxis);
            this.close(hingedDoor);
        }
        //TODO: Notify Render
        //TODO: Notify Controls
    }

}