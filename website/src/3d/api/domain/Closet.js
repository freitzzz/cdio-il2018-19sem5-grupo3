//@ts-check

/**
 * Requires Face for representing closet faces
 */
import Face from './Face';

/**
 * Requires FaceOrientation for identifying closet faces orientations
 */
import FaceOrientation from './FaceOrientation';

/**
 * Requires SlotableProduct for representing products that can have slots
 */
import SlotableProduct from "./SlotableProduct";

/**
 * Requires ProductType from identifying the closet product type
 */
import ProductType from './ProductType';

/**
 * Represents the internal core of a Closet
 */
export default class Closet extends SlotableProduct{

    /**
     * Builds new Closet
     * @param {Map<String,Face>} faces Map with the closet faces
     * @param {Number} productId Number with the product id
     * @param {Number} slotId Number with the slot id
     */
    constructor(faces,productId,slotId=null) {
        super(ProductType.CLOSET,Object.assign({},faces.get(FaceOrientation.RIGHT)),productId,slotId);
        this.faces=faces;
        this.initial_faces=Object.assign({},faces);
    }

    //Closet Logic

    /**
     * Changes the closet width
     */
    changeClosetWidth(width) {
        if (width > 0) {
            var axesWidth = width / 2;
            this.faces.get(FaceOrientation.BASE).changeWidth(width);
            this.faces.get(FaceOrientation.TOP).changeWidth(width);
            this.faces.get(FaceOrientation.BACK).changeWidth(width);
            this.faces.get(FaceOrientation.LEFT).changeXAxis(-axesWidth);
            this.faces.get(FaceOrientation.RIGHT).changeXAxis(axesWidth);
        }
    }

    /**
     * Changes the closet height
     */
    changeClosetHeight(height) {
        if (height > 0) {
            var axesHeight = height / 2;
            var heighpos = this.faces.get(FaceOrientation.TOP).Y()[4]; //TODO: Remove heighpos ?
            this.faces.get(FaceOrientation.TOP).changeYAxis((this.faces.get(FaceOrientation.TOP).Y()-this.faces.get(FaceOrientation.LEFT).height()/2)+axesHeight);
            this.faces.get(FaceOrientation.TOP).changeYAxis((this.faces.get(FaceOrientation.BASE).Y()+this.faces.get(FaceOrientation.LEFT).height()/2)-axesHeight);
            this.faces.get(FaceOrientation.LEFT).changeHeight(height);
            this.faces.get(FaceOrientation.RIGHT).changeHeight(height);
            this.faces.get(FaceOrientation.BACK).changeHeight(height);
            for(let closetSlot of this.getSlotFaces()){
                closetSlot.changeHeight(height);
            }
        }
    }

    /**
     * Changes the closet depth
     */
    changeClosetDepth(depth) {
        if (depth > 0) {
            var axesDepth = depth / 2;
            this.faces.get(FaceOrientation.BASE).changeDepth(depth);
            this.faces.get(FaceOrientation.TOP).changeDepth(depth);
            this.faces.get(FaceOrientation.BACK).changeZAxis(-axesDepth);
            this.faces.get(FaceOrientation.LEFT).changeDepth(depth);
            this.faces.get(FaceOrientation.RIGHT).changeDepth(depth);
            for(let closetSlotFace of this.getSlotFaces()){
                closetSlotFace.changeDepth(depth);
            }
        }
    }

    //Slots Logic

    /**
     * Changes the current closet slots
     */
    computeNewClosetSlots(slots) {
        if (slots >= 1) {
            return (slots) - this.currentSlots;
        } else {
            //THROW ERROR
        }
    }

    /**
     * Adds a slot to the closet
     * @param {Face} slotFace Face with the slot face being added
     */
    addClosetSlot(slotFace) {
        let slotWidth = slotFace.width();
        this.addSlot(slotFace);
        this._updateClosetSlots(slotWidth);
        return this.getSlotFaces()[this.currentSlots-1];
    }

    /**
     * Removes a slot from the closet
     */
    removeClosetSlot(slotFace) {
        if(this.currentSlots > 1){
            this.removeSlot(slotFace);
            this._updateClosetSlots();
        }
    }
    

    //Accessors

    /**
     * Returns the current width of the closet
     */
    getClosetWidth() { return this.faces.get(FaceOrientation.BASE); }

    /**
     * Returns the current height of the closet
     */
    getClosetHeight() { return this.faces.get(FaceOrientation.LEFT); }

    /**
     * Returns the current depth of the closet
     */
    getClosetDepth() { return this.faces.get(FaceOrientation.BASE).depth(); }

    /**
     * Returns all current closet initial faces
     */
    getInitialClosetFaces() { return this.initial_faces; }

    /**
     * Returns all current closet faces
     */
    getClosetFaces() { return this.faces; }

    /**
     * Returns all current closet initial faces
     */
    getInitialClosetSlotFaces() { return this.getInitialClosetSlotFaces(); }

    /**
     * Returns all current closet faces
     */
    getClosetSlotFaces() { return this.getSlotFaces(); }

    //Private Methods

    /**
     * Updates the closet slots size
     */
    _updateClosetSlots(width) {
        let left_closet_face_x_value = this.faces.get(FaceOrientation.LEFT).X();
        let closetSlotFaces=this.getClosetSlotFaces();
        for(let i=0;i<closetSlotFaces.length;i++){
            if(i==0){
                closetSlotFaces[i].changeXAxis(left_closet_face_x_value+width);
            }else{
                closetSlotFaces[i].changeXAxis(closetSlotFaces[i-1].X()+width);
            }
        }
    }

}