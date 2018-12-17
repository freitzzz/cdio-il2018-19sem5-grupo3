//@ts-check

/**
 * Requires Face for identifying sliding door slot holder face
 */
import Face from '../../api/domain/Face';

/**
 * Requires FaceOrientation for identifying sliding door slot holder face orientation
 */
import FaceOrientation from '../../api/domain/FaceOrientation';

/**
 * Requires ThreeSlidingDoor properties
 */
import ThreeSlidingDoor from '../domain/ThreeSlidingDoor';

/**
 * Requires Watcher for products knowledge ands events
 */
import Watcher from '../../api/domain/Watcher';

/**
 * Service class that holds all three.js sliding door animations functionalities
 */
export default class ThreeSlidingDoorAnimations{

    /**
     * Opens a SlidingDoor
     * @param {ThreeSlidingDoor} slidingDoor ThreeSlidingDoor with the sliding door to be opened
     */
    //To open a sliding door (aka slide to left) we first need to know where the sliding door is inserted (inserted slot)
    //Then we compute the distance between the sliding door X position and the sliding door inserted slot X position
    //With the computed distance we compute the sliding door inserted slot X position
    //If the sliding door inserted slot position is smaller than the distance between the sliding door and the sliding door inserted slot
    //Then we compute the open animation again
    static open(slidingDoor){
        let slidingDoorSlot=Watcher.currentWatcher().get(slidingDoor.getSlotId());
        let slidingDoorThreeFace=slidingDoor.getThreeFace();
        let slidingDoorSlotLeftThreeFace=slidingDoorSlot.getThreeFaces().get(FaceOrientation.LEFT);
        let distanceFromSlidingDoorToSlotLeftFace=Math.abs(slidingDoorThreeFace.position.x-slidingDoorSlotLeftThreeFace.position.x);
        let slidingDoorSlotPosition=(Math.abs(slidingDoorSlotLeftThreeFace.position.x-slidingDoorSlotLeftThreeFace.geometry.parameters.width)/2)-2;
        if(slidingDoorSlotPosition<distanceFromSlidingDoorToSlotLeftFace){
            slidingDoorThreeFace.translateX(-1);
            this.open(slidingDoor);
        }
        //TODO: Notify Render
        //TODO: Notify Controls
    }

    /**
     * Closes a SlidingDoor
     * @param {ThreeSlidingDoor} slidingDoor ThreeSlidingDoor with the sliding door to be closed
     */
    //The close (aka slide to right) is the inverse of the open one (slide to left)
    static close(slidingDoor){
        let slidingDoorSlot=Watcher.currentWatcher().get(slidingDoor.getSlotId());
        let slidingDoorThreeFace=slidingDoor.getThreeFace();
        let slidingDoorSlotRightThreeFace=slidingDoorSlot.getThreeFaces().get(FaceOrientation.RIGHT);
        let distanceFromSlidingDoorToSlotRightFace=Math.abs(slidingDoorThreeFace.position.x-slidingDoorSlotRightThreeFace.position.x);
        let slidingDoorSlotPosition=(Math.abs(slidingDoorSlotRightThreeFace.position.x+slidingDoorSlotRightThreeFace.geometry.parameters.width)/2)-2;
        if(slidingDoorSlotPosition<distanceFromSlidingDoorToSlotRightFace){
            slidingDoorThreeFace.translateX(1);
            this.close(slidingDoor);
        }
        //TODO: Notify Render
        //TODO: Notify Controls
    }

}