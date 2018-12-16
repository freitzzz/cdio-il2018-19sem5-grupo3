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

    /**
     * Watches a new product
     * @param {BaseProduct} product BaseProduct with the product being watched
     * @param {Map<String,Action>} actionsMap Map with the product actions
     */
    watch(product,actionsMap){
        this.products.set(product.id(),product);
        this.productsActions.set(product.id(),actionsMap);
    }

    /**
     * Triggers a product action
     * @param {BaseProduct} product BaseProduct with the product which actions will be executed
     * @param {String} action String with the product action identifier
     */
    trigger(product,action){
        let productActions=this.productsActions.get(product);
        let productAction=productActions.get(action);
        productAction.execute();
    }
}