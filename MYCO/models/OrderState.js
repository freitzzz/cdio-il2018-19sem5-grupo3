/**
 * Constant that represents the In Validation Order State
 */
const IN_VALIDATION='In Validation';

/**
 * Constant that represents the In Validation Order State
 */
const VALIDATED='Validated';

/**
 * Constant that represents the In Production Order State
 */
const IN_PRODUCTION='In Production';

/**
 * Constant that represents the Producted Order State
 */
const PRODUCTED='Producted';

/**
 * Constant that represents the Ready to Ship Order State
 */
const READY_TO_SHIP='Ready to Ship';

/**
 * Constant that represents the Shipped Order Start
 */
const SHIPPED='Shipped';

/**
 * Constant that represents the Delivered Order State
 */
const DELIVERED='Delivered';

/**
 * Constant that represents the enum representation of the orders state
 */
const OrderStateEnum={
    IN_VALIDATION:IN_VALIDATION,
    VALIDATED:VALIDATED,
    IN_PRODUCTION:IN_PRODUCTION,
    PRODUCTED:PRODUCTED,
    READY_TO_SHIP:READY_TO_SHIP,
    SHIPPED:SHIPPED,
    DELIVERED:DELIVERED,
    values:[IN_VALIDATION,VALIDATED,IN_PRODUCTION,PRODUCTED,READY_TO_SHIP,SHIPPED,DELIVERED]
};

/**
 * Exports OrderState as an enum
 */
module.exports=OrderStateEnum;