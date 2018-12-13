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