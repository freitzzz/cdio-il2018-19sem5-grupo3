//@ts-check

/**
 * Requires BaseProduct for representing products that can have slots
 */
import BaseProduct from './BaseProduct';

/**
 * Requires Face for representing slots faces
 */
import Face from './Face';

/**
 * Requires ProductType for identifying the various products that can be inserted on the slots
 */
import ProductType from './ProductType';

/**
 * Class that represents a product that can have slots
 */
export default class SlotableProduct extends BaseProduct{
    
    /**
     * Builds a new SlotableProduct
     * @param {Face} face Face with the initial product slot
     * @param {Number} productId Number with the product identifier
     * @param {Number} slotId Number with the slot identifier which the product is placed on
     */
    constructor(productType,face=null,productId,slotId=null){
        super(productType,productId,slotId);
        this.slots=new Map();
        this.slot_faces=[];
        this.initial_slot_faces=[];
        this.currentSlots=0;
        /* if(face!=null)this.addSlot(face); */ //TODO: ????????
    }

    /**
     * Adds a new slot to the product
     * @param {Face} slot Face with the slot being added
     */
    addSlot(slot){
        this.slots.set(slot,[]);
        this.slot_faces.push(slot);
        this.initial_slot_faces.push(slot);
        this.currentSlots++;
    }

    /**
     * Removes a slot from the product
     * @param {Face} slot Face with the slot being removed
     */
    removeSlot(slot){
        this.slots.delete(slot);
        let newSlotFaces=[];
        for(let slotFace of this.slot_faces)
            if(slotFace!=slot)newSlotFaces.push(slotFace);
        this.slot_faces=newSlotFaces.slice();
        this.initial_slot_faces=newSlotFaces.slice();
        this.currentSlots--;
    }

    /**
     * Adds a new product into a certain slot
     * @param {Number} slotId Number with the slot identifier which product will be placed on
     * @param {BaseProduct} product BaseProduct with the product which will be placed on the desired slot
     */
    addProduct(slotId,product){
        //for(let asd of this.slots.keys())console.log(asd)
        if(!this.slots.get(slotId)){
            this.slots.set(slotId,[]);
        }else{
            this.slots.get(slotId).push(product);
        }
    }

    /**
     * Returns the product which is contained on a certain slot by its identifier
     * @param {Number} productId Number with the product identifier
     */
    getProduct(productId){
        for(let slot of this.slots)
            for(let product of this.slots.get(slot))
                if(product.id()==productId)
                    return product;
        return null;
    }

    /**
     * Returns all product inserted products
     * @param {String} productType String with the product type being queried
     */
    getProducts(productType=null){
        let products=[];
        let insertedProductsInSlots=this.slots.values();
        for(let insertedProductsInSlot of insertedProductsInSlots){
            let insertedProducts=insertedProductsInSlot;
            for(let insertedProduct of insertedProducts){
                if(productType==null){
                    products.push(insertedProduct);
                }else if(insertedProduct.getProductType()==productType){
                    products.push(insertedProduct);
                }
            }
        }
        return products;
    }

    /**
     * Removes a product from a certain slot
     * @param {Number} slotId Number with the slot identifier which product product will be removed from
     * @param {BaseProduct} product BaseProduct with the product which will be removed from the desired slot
     */
    removeProduct(slotId,product){
        this.slots.get(slotId).pop(product);
    }

    /**
     * Returns the current product slot faces
     * @returns {Face[]} Array with the product slot faces
     */
    getSlotFaces(){return this.slot_faces;}

    /**
     * Returns the current product initial slot faces
     * @returns {Face[]} Array with the product initial slot faces
     */
    getInitialSlotFaces(){return this.initial_slot_faces;}

    /**
     * Returns the current product slot number
     */
    getSlotNumber(){return this.currentSlots;}
}