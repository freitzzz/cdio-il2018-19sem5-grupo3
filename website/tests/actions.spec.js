/* eslint-disable no-unused-vars */
/* eslint-disable no-undef */
import * as mutationsTypes from '../src/store/mutation-types'
import * as VuexStore from '../src/store/index'

describe('product actions', () => {
    describe('product-id', () => {
        test('INIT_PRODUCT-updates-state-with-correct-product-id',
            ensureInitProductActionUpdatesStateWithCorrectProductId);
    });
})

describe('customized product actions', () => {
    describe('customized-product-dimensions', () => {
        //TODO Fix these failing tests
        /* test('SET_CUSTOMIZED_PRODUCT_WIDTH-updates-state-with-correct-width',
            ensureSetCustomizedProductWidthActionUpdatesStateCorrectly);
        test('SET_CUSTOMIZED_PRODUCT_HEIGHT-updates-state-with-correct-height',
            ensureSetCustomizedProductHeightActionUpdatesStateCorrectly);
        test('SET_CUSTOMIZED_PRODUCT_DEPTH-updates-state-with-correct-depth',
            ensureSetCustomizedProductDepthActionUpdatesStateCorrectly);
        test('SET_CUSTOMIZED_PRODUCT_UNIT-updates-state-with-correct-unit',
            ensureSetCustomizedProductUnitActionUpdatesStateCorrectly) */
    });
    //TODO Comment out these tests after customized product slots structure is updated
    /* describe('customized-product-slots', () => {
        describe('slot-dimensions', () => {
            test('SET_SLOT_WIDTH-updates-state-with-correct-width',
                ensureSetSlotWidthActionUpdatesStateCorrectly);
            test('SET_SLOT_HEIGHT-updates-state-with-correct-height',
                ensureSetSlotHeightActionUpdatesStateCorrectly);
            test('SET_SLOT_DEPTH-updates-state-with-correct-depth',
                ensureSetSlotDepthActionUpdatesStateCorrectly);
            test('SET_SLOT_UNIT-updates-state-with-correct-uni',
                ensureSetSlotUnitActionUpdatesStateCorrectly);
        })
    }) */
})

function ensureInitProductActionUpdatesStateWithCorrectProductId() {
    const expectedId = 1;
    VuexStore.default.replaceState({
        product: {
            id: ""
        }
    });
    const payload = {
        product: {
            id: expectedId
        }
    };
    VuexStore.default.dispatch(mutationsTypes.INIT_PRODUCT, payload);
    expect(VuexStore.default.state.product.id).toBe(expectedId);
}

function ensureSetCustomizedProductWidthActionUpdatesStateCorrectly() {
    const expectedWidth = 100;
    VuexStore.default.replaceState({
        customizedProduct: {
            customizedDimensions: {
                "width": 0
            }
        }
    });
    const payload = {
        width: expectedWidth
    };
    VuexStore.default.dispatch(mutationsTypes.SET_CUSTOMIZED_PRODUCT_WIDTH, payload);
    expect(VuexStore.default.state.customizedProduct.customizedDimensions.width).toBe(expectedWidth);
}

function ensureSetCustomizedProductHeightActionUpdatesStateCorrectly() {
    const expectedHeight = 100;
    VuexStore.default.replaceState({
        customizedProduct: {
            customizedDimensions: {
                "height": 0
            }
        }
    });
    const payload = {
        height: expectedHeight
    };
    VuexStore.default.dispatch(mutationsTypes.SET_CUSTOMIZED_PRODUCT_HEIGHT, payload);
    expect(VuexStore.default.state.customizedProduct.customizedDimensions.height).toBe(expectedHeight);
}

function ensureSetCustomizedProductDepthActionUpdatesStateCorrectly() {
    const expectedDepth = 100;
    VuexStore.default.replaceState({
        customizedProduct: {
            customizedDimensions: {
                "depth": 0
            }
        }
    });
    const payload = {
        depth: expectedDepth
    };
    VuexStore.default.dispatch(mutationsTypes.SET_CUSTOMIZED_PRODUCT_DEPTH, payload);
    expect(VuexStore.default.state.customizedProduct.customizedDimensions.depth).toBe(expectedDepth);
}

function ensureSetCustomizedProductUnitActionUpdatesStateCorrectly() {
    const expectedUnit = "cm";
    VuexStore.default.replaceState({
        customizedProduct: {
            customizedDimensions: {
                "unit": ""
            }
        }
    });
    const payload = {
        unit: expectedUnit
    };
    VuexStore.default.dispatch(mutationsTypes.SET_CUSTOMIZED_PRODUCT_UNIT, payload);
    expect(VuexStore.default.state.customizedProduct.customizedDimensions.unit).toBe(expectedUnit);
}

function ensureSetSlotWidthActionUpdatesStateCorrectly() {
    //!Implement test after action is fixed
}

function ensureSetSlotHeightActionUpdatesStateCorrectly() {
    //!Implement test after action is fixed
}

function ensureSetSlotDepthActionUpdatesStateCorrectly() {
    //!Implement test after action is fixed
}

function ensureSetSlotUnitActionUpdatesStateCorrectly() {
    //!Implement test after action is fixed
}