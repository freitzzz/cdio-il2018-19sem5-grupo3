//@ts-check
/**
 * Represents a hinged door using box geometry
 */
export default class HingedDoor {

    /**
     * Builds a new HingedDoor with the dimensions and axes values for the door
     * @param {Array} hinged_door_axes Array with the dimensions and axes values of the door
     * @param {Number} slotId Id of the slot that the hinged door belongs to
     * @param {Number} meshId Id of the hinged door mesh
     */
    constructor(hinged_door_axes, slotId, meshId) {
        this.hinged_door_axes = hinged_door_axes.slice();
        this.slotId = slotId;
        this.meshId = meshId;
    }

    /**
     * Changes the height of the hingedDoor
     * 
     * @param {Number} height New height of the door 
     */
    changeHeight(height) { if (height > 0) this.hinged_door_axes[1] = height; }

    /**
     * Changes the width of the hingedDoor
     * 
     * @param {Number} width New width of the door 
     */
    changeWidth(width) { if (width > 0) this.hinged_door_axes[0] = width; }

    /**
     * Returns the height of the hingedDoor
     */
    getHeight() { return this.hinged_door_axes[1]; }

    /**
     * Returns the width of the hingedDoor
     */
    getWidth() { return this.hinged_door_axes[0]; }

    /**
     * Returns the ID of the slot
     */
    getSlotId() { return this.slotId; }
}