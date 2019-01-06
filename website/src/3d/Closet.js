//@ts-check

/**
 * Requires SlotableProduct for representing products that can have slots
 */
import SlotableProduct from "./BaseProduct";

/**
 * Requires HingedDoor
 */
import HingedDoor from './HingedDoor';

/**
 * Requires SlidingDoor
 */
import SlidingDoor from './SlidingDoor';

const WIDTH = 0;
const HEIGHT = 1;
const DEPTH = 2;

/**
 * Represents the internal core of a Closet
 */
export default class Closet extends SlotableProduct{
    /**
 * Resize factor to convert slider values to three.js closet values for dimensions
 * The resize factor shoulb be as follows: closetWidth/sliderResizeDimensionsWidth...
 */
closetResizeValues=[4.2/404.5,300/300,100/245];

    /**
     * Builds a new Closet with the dimensions and axes values for all faces
     * @param {Array} closet_base_face_dimensions_axes Array with the base face dimensions and axes values
     * @param {Array} closet_top_face_dimensions_axes Array with the top face dimensions and axes values
     * @param {Array} closet_left_face_dimensions_axes Array with the left face dimensions and axes values
     * @param {Array} closet_right_face_dimensions_axes Array with the right face dimensions and axes values
     * @param {Array} closet_back_face_dimensions_axes Array with the back face dimensions and axes values
     */
    constructor(closet_base_face_dimensions_axes
        , closet_top_face_dimensions_axes
        , closet_left_face_dimensions_axes
        , closet_right_face_dimensions_axes
        , closet_back_face_dimensions_axes
        , productId) {
            super(productId);
            this.closet_base_face_dimensions_axes = closet_base_face_dimensions_axes.slice();
            this.closet_top_face_dimensions_axes = closet_top_face_dimensions_axes.slice();
            this.closet_left_face_dimensions_axes = closet_left_face_dimensions_axes.slice();
            this.closet_right_face_dimensions_axes = closet_right_face_dimensions_axes.slice();
            this.closet_back_face_dimensions_axes = closet_back_face_dimensions_axes.slice();
            this._prepare_closet_init();
            this.poles = [];
            this.shelves = [];
            this.slidingDoors = [];
            this.hingedDoors = [];
            this.drawers = [];
            this.modules = [];
        }

    //Closet Logic

    /**
     * Changes the closet width
     */
    changeClosetWidth(width) {
        if (width > 0) {
            var axesWidth = width / 2;
            this.closet_base_face_dimensions_axes[0] = width;
            this.closet_top_face_dimensions_axes[0] = width;
            this.closet_back_face_dimensions_axes[0] = width;
            this.closet_left_face_dimensions_axes[3] = -axesWidth;
            this.closet_right_face_dimensions_axes[3] = axesWidth;
        }
    }

    /**
     * Changes the closet height
     */
    changeClosetHeight(height) {
        if (height > 0) {
            var axesHeight = height / 2;
            var heighpos = this.closet_top_face_dimensions_axes[4];
            this.closet_top_face_dimensions_axes[4] = (this.closet_top_face_dimensions_axes[4] - this.closet_left_face_dimensions_axes[1] / 2) + axesHeight;
            this.closet_base_face_dimensions_axes[4] = (this.closet_base_face_dimensions_axes[4] + this.closet_left_face_dimensions_axes[1] / 2) - axesHeight;
            this.closet_left_face_dimensions_axes[1] = height;
            this.closet_right_face_dimensions_axes[1] = height;
            this.closet_back_face_dimensions_axes[1] = height;
            for (var i = 0; i < this.closet_slots_faces.length; i++) {
                this.closet_slots_faces[i][1] = height;
            }
        }
    }

    /**
     * Changes the closet depth
     */
    changeClosetDepth(depth) {
        if (depth > 0) {
            var axesDepth = depth / 2;
     
            this.closet_base_face_dimensions_axes[2] = depth*this.closetResizeValues[DEPTH];
            this.closet_top_face_dimensions_axes[2] = depth*this.closetResizeValues[DEPTH]; 
            this.closet_back_face_dimensions_axes[5] = -depth; 
            this.closet_left_face_dimensions_axes[2] = depth*this.closetResizeValues[DEPTH]; 
            this.closet_right_face_dimensions_axes[2] = depth*this.closetResizeValues[DEPTH];
            for (var i = 0; i < this.closet_slots_faces.length; i++) {
                this.closet_slots_faces[i][2] = depth*this.closetResizeValues[DEPTH];
            }
        }
    }

    //Slots Logic

    /**
     * Changes the current closet slots
     */
    computeNewClosetSlots(slots) {
        if (slots >= 1) {
            return (slots) - this.closet_slots;
        } else {
            //THROW ERROR
        }
    }

    /**
     * Adds a slot to the closet
     */
    addSlot(valueSlot) {
        this.closet_slots++;
        var slotWidth = valueSlot.width;
        this.closet_slots_faces.push(this.closet_right_face_dimensions_axes.slice());
        this.initial_closet_slots_faces.push(this.closet_right_face_dimensions_axes.slice());
        this._updateClosetSlots(slotWidth);
        return this.closet_slots_faces[this.closet_slots_faces.length - 1];
    }

    /**
     * Removes a slot from the closet
     */
    removeSlot() {
        if (this.closet_slots > 1) {
            this.closet_slots--;
            this.closet_slots_faces.pop();
            this.initial_closet_slots_faces.pop();
            this._updateClosetSlots();
        }
    }

    /**
     * Adds a pole to the closet
     */
    addPole(pole) {
        this.poles.push(pole);
    }

    /**
     * Removes a pole from the closet
     */
    removePole() {
        this.poles.pop();
    }

    /**
     * Adds a shelf to the closet
     */
    addShelf(shelf) {
        this.shelves.push(shelf);
    }

    /**
     * Removes a shelf from the closet
     */
    removeShelf() {
        this.shelves.pop();
    }

    /**
     * Adds a door to the closet
     */
    addSlidingDoor(door) {
        this.slidingDoors.push(door);
    }

    /**
     * Removes a door from the closet
     */
    removeSlidingDoor() {
        this.slidingDoors.pop();
    }

    /**
    * Adds a door to the closet
    * @param {SlidingDoor|HingedDoor} door to add 
    */
    addHingedDoor(door) {
        this.hingedDoors.push(door);
    }

    /**
     * Removes a door from the closet
     */
    removeHingedDoor() {
        this.hingedDoors.pop();
    }
    /**
     * Adds a drawer to the closet
     */
    addDrawer(drawer) {
        this.drawers.push(drawer);
    }

    /**
     * Removes a drawer from the closet
     */
    removeDrawer() {
        this.drawers.pop();
    }

    /**
     * Adds a module to the closet
     */
    addModule(module) {
        this.modules.push(module);
    }

    /**
     * Removes a module from the closet
     */
    removeModule() {
        this.modules.pop();
    }

    //Accessors
    /**
     * Returns the current width of the closet
     */
    getClosetWidth() { return this.closet_base_face_dimensions_axes[0]; }

    /**
     * Returns the current height of the closet
     */
    getClosetHeight() { return this.closet_left_face_dimensions_axes[1]; }

    /**
     * Returns the current depth of the closet
     */
    getClosetDepth() { return this.closet_base_face_dimensions_axes[2]; }

    /**
     * Returns all current closet initial faces
     */
    getInitialClosetFaces() { return this.initial_closet_faces; }

    /**
     * Returns all current closet faces
     */
    getClosetFaces() { return this.closet_faces; }

    /**
     * Returns all current closet initial faces
     */
    getInitialClosetSlotFaces() { return this.initial_closet_slots_faces; }

    /**
     * Returns all current closet faces
     */
    getClosetSlotFaces() { return this.closet_slots_faces; }

    //Private Methods

    /**
     * Updates the closet slots size
     */
    _updateClosetSlots(width) {
        var left_closet_face_x_value = this.closet_left_face_dimensions_axes[3];
        for (var i = 0; i < this.closet_slots_faces.length; i++) {
            if (i == 0) {
                this.closet_slots_faces[i][3] = left_closet_face_x_value + (width);
            } else {
                this.closet_slots_faces[i][3] = this.closet_slots_faces[i - 1][3] + (width);
            }
        }
    }

    /**
     * Prepare the closet initialization
     */
    _prepare_closet_init() {
        this.closet_faces = [this.closet_base_face_dimensions_axes, this.closet_top_face_dimensions_axes, this.closet_left_face_dimensions_axes, this.closet_right_face_dimensions_axes, this.closet_back_face_dimensions_axes];
        this.initial_closet_faces = [this.closet_base_face_dimensions_axes.slice(), this.closet_top_face_dimensions_axes.slice(), this.closet_left_face_dimensions_axes.slice(), this.closet_right_face_dimensions_axes.slice(), this.closet_back_face_dimensions_axes.slice()];
        this.closet_slots = 1;
        this.closet_slots_faces = [];
        this.initial_closet_slots_faces = [];
    }
}