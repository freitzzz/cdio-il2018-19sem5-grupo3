//@ts-check
/**
 * Represents the internal core of a Closet
 */
export default class Closet {

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
        }
    }

    /**
     * Adds a slot to the closet
     */
    addSlot(valuesSlot) {
        this.closet_slots++;
        var slotWidth = valuesSlot.width;
        var slotHeight = valuesSlot.width;
        var slotDepth = valuesSlot.width;
        var face_slot_dimensions = {slotWidth,slotHeight, slotDepth}
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
    _updateClosetSlots(widthSlot) {
        ///var distance_per_slot = (this.closet_right_face_dimensions_axes[3] - this.closet_left_face_dimensions_axes[3]) / this.closet_slots;
        var left_closet_face_x_value = this.closet_left_face_dimensions_axes[3];
        for (var i = 0; i < this.closet_slots_faces.length; i++) {
            if (i == 0) {
                this.closet_slots_faces[i][3] = left_closet_face_x_value + (widthSlot);
            } else {
                this.closet_slots_faces[i][3] = this.closet_slots_faces[i - 1][3] + (widthSlot);
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