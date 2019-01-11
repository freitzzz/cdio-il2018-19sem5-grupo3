//@ts-check

/**
 * Requires Action for executing products actions
 */
import Action from './Action';

/**
 * Requires BaseProduct for tracking products base properties
 */
import BaseProduct from './BaseProduct';

/**
 * Watcher with the current global watcher
 */
let GLOBAL_WATCHER=null;

/**
 * Class that watches products actions
 */
export default class Watcher{
    
    /**
     * Builds a new Watcher
     */
    constructor(){
        this.products=new Map();
        this.productsActions=new Map();
        this.actionsWatchers=new Map();
        this.watchersActions=new Map();
    }

    /**
     * Returns the current global watcher
     * @returns {Watcher} Watcher with the current global watcher
     */
    static currentWatcher(){
        if(!GLOBAL_WATCHER)GLOBAL_WATCHER=new Watcher();
        return GLOBAL_WATCHER;
    }
    
    /**
     * Returns a product being watched by a certain identifier
     * @param {Number} productId Number with the product id
     */
    get(productId){return this.products.get(productId);}

    ///**
    // * Watches a new product
    // * @param {BaseProduct} product BaseProduct with the product being watched
    // * @param {Map<String,Action>} actionsMap Map with the product actions
    // */
    //watch(product,actionsMap){
    //    this.products.set(product.id(),product);
    //    this.productsActions.set(product.id(),actionsMap);
    //}

    /**
     * Watches an action
     * @param {String} actionType String with the action type being watched
     * @param {Action} action Action with the action being executed after watched
     */
    watch(actionType,action){
        this.actionsWatchers.set(actionType,action);
        this.watchersActions.set(actionType,[]);
    }

    /**
     * Queues an action in a certain action being watched
     * @param {String} actionType String with the action type being watched
     * @param {Action} action Action with the action being queued in the watched action
     */
    queueAction(actionType,action){
        this.watchersActions.get(actionType).unshift(action);//Unshift => Pushs an element on the first position of the array (Necessary for Queue (FIFO) )
    }
    
    ///**
    // * Triggers a product action
    // * @param {BaseProduct} product BaseProduct with the product which actions will be executed
    // * @param {String} action String with the product action identifier
    // */
    //trigger(product,action){
    //    let productActions=this.productsActions.get(product);
    //    let productAction=productActions.get(action);
    //    productAction.execute();
    //}

    /**
     * Triggers an action
     * @param {String} action String with the action type of the action being triggered
     */
    trigger(action){
        let watcherAction=this.actionsWatchers.get(action);
        let actionsWatched=this.watchersActions.get(action);
        watcherAction
            .execute()
            .then(()=>{
                for(let i=0;i<actionsWatched.length;i++)
                    actionsWatched.pop().execute();
            });
    }
}