//@ts-check

/**
 * Requires ThreeDrawer properties
 */
import ThreeDrawer from '../domain/ThreeDrawer';

/**
 * Service class that holds all three.js drawer animations functions
 */
export default class ThreeDrawerAnimations{

    /**
     * Opens a drawer
     * @param {ThreeDrawer} drawer ThreeDrawer with the drawer to be opened
     */
    //The animation for this one is actually not that hard
    //We need to translate our drawer faces in the Z axis while 
    //Our drawer front face doesn't hit a certain a certain desired length
    static open(drawer){
        let drawerThreeFaces=drawer.getThreeFaces();
        let drawerFrontThreeFace=drawerThreeFaces.get(ThreeDrawer.FRONT);
        if(drawerFrontThreeFace.Z()<=-50){
            let drawerThreeFacesValues=drawerThreeFaces.values();
            for(let i=0;i<drawerThreeFacesValues.length;i++)
                drawerThreeFacesValues[i].translateZ(1);
            this.open(drawer);
        }
    }

    /**
     * Closes a drawer
     * @param {ThreeDrawer} drawer ThreeDrawer with the drawer to be closed
     */
    static close(drawer){}

    /**
     * Draws a drawer
     * @param {ThreeDrawer} drawer ThreeDrawer with the drawer to be drawed
     */
    static draw(drawer){}

}