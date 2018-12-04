//@ts-check
/**
 * Represents a sliding door using box geometry
 */
class SlidingDoor {

    /**
     * Builds a new SlidingDoor with the dimensions and axes values for the door
     * @param {Array} sliding_door_axes Array with the dimensions and axes values of the door
     */
    constructor(sliding_door_axes) {
        this.sliding_door_axes = sliding_door_axes.slice();
    }

    /**
     * Changes the height of the SlidingDoor
     * 
     * @param {Number} height New height of the door 
     */
    changeHeight(height) {
        if (height > 0) this.sliding_door_axes[1] = height;
    }

    /**
     * Changes the width of the SlidingDoor
     * 
     * @param {Number} width New width of the door 
     */
    changeWidth(width) {
        if (width > 0) this.sliding_door_axes[0] = width;
    }

    /**
     * Returns the height of the SlidingDoor
     */
    getHeight() {
        return this.sliding_door_axes[1];
    }

    /**
     * Returns the width of the SlidingDoor
     */
    getWidth() {
        return this.sliding_door_axes[0];
    }
}