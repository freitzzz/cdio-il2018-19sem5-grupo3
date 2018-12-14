
/**
 * Represents a "closet" product type
 */
const CLOSET="closet";

/**
 * Represents a "drawer" product type
 */
const DRAWER="drawer";

/**
 * Represents a "hinged door" product type
 */
const HINGED_DOOR="hinged door";

/**
 * Represents a "module" product type
 */
const MODULE="module";

/**
 * Represents a "pole" product type
 */
const POLE="pole";

/**
 * Represents a "shelf" product type
 */
const SELF="shelf";

/**
 * Represents a "sliding door" product type
 */
const SLIDING_DOOR="sliding door";

/**
 * Represents an enum of the various products type
 */
const ProductTypeEnum={
    CLOSET:CLOSET,
    DRAWER:DRAWER,
    HINGED_DOOR:HINGED_DOOR,
    MODULE:MODULE,
    POLE:POLE,
    SELF:SELF,
    SLIDING_DOOR:SLIDING_DOOR,
    values:[CLOSET,DRAWER,HINGED_DOOR,MODULE,POLE,SELF,SLIDING_DOOR]
};

/**
 * Exports ProductType enum values
 */
export default ProductTypeEnum;