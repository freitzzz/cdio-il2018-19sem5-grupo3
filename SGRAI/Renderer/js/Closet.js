/**
 * Represents the internal core of a Closet
 */
class Closet {
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
        , closet_back_face_dimensions_axes) {
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
            this.closet_base_face_dimensions_axes[2] = depth;
            this.closet_top_face_dimensions_axes[2] = depth;
            this.closet_back_face_dimensions_axes[5] = -axesDepth;
            this.closet_left_face_dimensions_axes[2] = depth;
            this.closet_right_face_dimensions_axes[2] = depth;
            for (var i = 0; i < this.closet_slots_faces.length; i++) {
                this.closet_slots_faces[i][2] = depth;
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
    addSlot() {
        this.closet_slots++;
        this.closet_slots_faces.push(this.closet_right_face_dimensions_axes.slice());
        this.initial_closet_slots_faces.push(this.closet_right_face_dimensions_axes.slice());
        this._updateClosetSlots(this);
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
            this._updateClosetSlots(this);
        }
    }

    /**
     * Adds a pole to the closet
     * @param {Pole} pole to add 
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
     * @param {Shelf} shelf to add 
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
     * @param {SlidingDoor} door to add 
     */
    addSlidingDoor(door) {
        this.slidingDoors.push(door);
    }

    /**
     * Removes a door from the closet
     *  @param {SlidingDoor} door to remove
     */
    removeSlidingDoor() {
        this.slidingDoors.pop();
    }

     /**
     * Adds a door to the closet
     * @param {HingedDoor} door to add 
     */
    addHingedDoor(door) {
        this.hingedDoors.push(door);
    }

    /**
     * Removes a door from the closet
     *  @param {HingedDoor} door to remove
     */
    removeHingedDoor() {
        this.hingedDoors.pop();
    }
    /**
     * Adds a drawer to the closet
     * @param {Drawer} drawer to add 
     */
    addDrawer(drawer) {
        this.drawers.push(drawer);
    }

    /**
     * Removes a drawer from the closet
     *  @param {Drawer} drawer to remove
     */
    removeDrawer() {
        this.drawers.pop();
    }

    /**
     * Adds a module to the closet
     * @param {Module} module to add 
     */
    addModule(module) {
        this.modules.push(module);
    }

    /**
     * Removes a module from the closet
     *  @param {Module} module to remove
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
    _updateClosetSlots() {
        var distance_per_slot = (this.closet_right_face_dimensions_axes[3] - this.closet_left_face_dimensions_axes[3]) / closet.closet_slots;
        var left_closet_face_x_value = this.closet_left_face_dimensions_axes[3];
        for (var i = 0; i < this.closet_slots_faces.length; i++) {
            if (i == 0) {
                this.closet_slots_faces[i][3] = left_closet_face_x_value + (distance_per_slot);
            } else {
                this.closet_slots_faces[i][3] = this.closet_slots_faces[i - 1][3] + (distance_per_slot);
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