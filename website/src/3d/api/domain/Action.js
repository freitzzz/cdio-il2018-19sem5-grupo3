//@ts-check

/**
 * Class that represents an action that can be executed
 */
export default class Action{
    /**
     * Builds a new action
     * @param {Function} action Function with the function that holds the action
     */
    constructor(action){
        this.action=action;
    }

    /**
     * Executes the action
     */
    execute(){
        this.action();
    }
}