//@ts-check

/**
 * Requires Action base properties and functionalities
 */
import Action from "../../api/domain/Action";



/**
 * Represents a Promise like action using request animation frame
 */
export default class ThreeAction extends Action{

    /**
     * Builds a new ThreeAction
     * @param {Function} action Function with the action to be executed 
     */
    constructor(action){
        super(action);
    }

    /**
     * Executes the action
     */
    execute(){
        return new Promise((accept,_)=>{
            requestAnimationFrame(()=>{
                this.action();
                accept();
            });
        });
    }
}