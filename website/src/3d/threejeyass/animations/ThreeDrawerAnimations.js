//@ts-check

/**
 * Requires ThreeDrawer properties
 */
import ThreeDrawer from '../domain/ThreeDrawer';

/**
 * Requires FaceOrientation for identifying drawer faces orientation
 */
import FaceOrientation from '../../api/domain/FaceOrientation';

/**
 * Service class that holds all three.js drawer animations functionalities
 */
export default class ThreeDrawerAnimations{

    /**
     * Opens a drawer
     * @param {ThreeDrawer} drawer ThreeDrawer with the drawer to be opened
     */
    //The animation for this one is actually not that hard
    //We need to translate our drawer faces in the Z axis while 
    //Our drawer front face doesn't hit a certain a certain desired length
    //50 is the drawer depth/2
    static open(drawer){
        let drawerThreeFaces=drawer.getThreeFaces();
        let drawerFrontThreeFace=drawerThreeFaces.get(FaceOrientation.FRONT);
        if(drawerFrontThreeFace.position.z<=-50){
            let drawerThreeFacesValues=drawerThreeFaces.values();
            for(let threeDrawerFace of drawerThreeFacesValues){
                threeDrawerFace.translateZ(1);
            }
            this.open(drawer);
        }
        //TODO: Notify Render
        //TODO: Notify Controls
    }

    /**
     * Closes a drawer
     * @param {ThreeDrawer} drawer ThreeDrawer with the drawer to be closed
     */
    //The animation for this one is similar to the open one
    //First we need to compare both back and front drawer faces positions relatively to Z axis
    //Then if the back face Z axis position is still bigger than the front one
    //We loop through all drawer faces and translate through the Z axis, counterclockwise
    //If the back face Z axis position is already bigger than the initial front one + face thickness
    //Then the animation was successful
    //3 is the value for drawer thickness
    static close(drawer){
        let drawerThreeFaces=drawer.getThreeFaces();
        let drawerFrontThreeFace=drawerThreeFaces.get(FaceOrientation.FRONT);
        let drawerBackThreeFace=drawerThreeFaces.get(FaceOrientation.BACK);
        let drawerFrontThreeFaceZAxis=drawerFrontThreeFace.position.z;
        if(drawerBackThreeFace.position.z>drawerFrontThreeFaceZAxis+3){
            for(let drawerThreeFace of drawerThreeFaces.values()){
                drawerThreeFace.translateZ(-1);
            }
            this.close(drawer);
        }
        //TODO: Notify Render
        //TODO: Notify Controls
    }
}