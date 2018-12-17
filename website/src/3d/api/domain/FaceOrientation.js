
/**
 * Represents a "base" face of a product
 */
const BASE="base";

/**
 * Represents a "top" face of a product
 */
const TOP="top";

/**
 * Represents a "front" face of a product
 */
const FRONT="front";

/**
 * Represents a "back" face of a product
 */
const BACK="back";

/**
 * Represents a "left" face of a product
 */
const LEFT="left";

/**
 * Represents a "right" face of a product
 */
const RIGHT="right";

/**
 * Represents an enum of the various faces orientation
 */
const FaceOrientationEnum={
    BASE:BASE,
    TOP:TOP,
    FRONT:FRONT,
    BACK:BACK,
    LEFT:LEFT,
    RIGHT:RIGHT,
    values:[BASE,TOP,FRONT,BACK,LEFT,RIGHT]
};

/**
 * Exports FaceOrientation enum values
 */
export default FaceOrientationEnum;