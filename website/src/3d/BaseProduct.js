/**
 * Class that represents the base properties of a product
 */
export default class BaseProduct{
    
    /**
     * Builds a new BaseProduct with a random identifier
     */
    constructor(){super('_' + Math.random().toString(36).substr(2,9));}
    
    /**
     * Builds a new BaseProduct with the product identifier
     * @param {Object} id Object with the product identifer
     */
    constructor(id){this.id=id;}

    /**
     * Returns the product identifier
     */
    id(){return this.id};

}