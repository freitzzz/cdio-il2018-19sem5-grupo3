import { mutations } from '../src/store/mutations'

//TODO Refactor these tests after Store structure changes are made

describe('product mutations', () => {
    describe('product-id', () => {
        test('INIT_PRODUCT-updates-state-with-correct-product-id',
            ensureInitProductUpdatesStateCorrectly);
    })
});

describe('customized product mutations', () => {
    describe('customized-product-dimensions', () => {
        test('SET_CUSTOMIZED_PRODUCT_WIDTH-updates-state-with-correct-width',
            ensureSetCustomizedProductWidthUpdatesStateCorrectly);
        test('SET_CUSTOMIZED_PRODUCT_HEIGHT-updates-state-with-correct-height',
            ensureSetCustomizedProductHeightUpdatesStateCorrectly);
        test('SET_CUSTOMIZED_PRODUCT_DEPTH-updates-state-with-correct-depth',
            ensureSetCustomizedProductDepthUpdatesStateCorrectly);
        test('SET_CUSTOMIZED_PRODUCT_UNIT-updates-state-with-correct-unit',
            ensureSetCustomizedProductUnitUpdatesStateCorrectly)
    });
    describe('customized-product-slots', () => {
        describe('slot-dimensions', () => {
            test('SET_SLOT_WIDTH-updates-state-with-correct-width',
                ensureSetSlotWidthUpdatesStateCorrectly);
            test('SET_SLOT_HEIGHT-updates-state-with-correct-height',
                ensureSetSlotHeightUpdatesStateCorrectly);
            test('SET_SLOT_DEPTH-updates-state-with-correct-depth',
                ensureSetSlotDepthUpdatesStateCorrectly);
            test('SET_SLOT_UNIT-updates-state-with-correct-uni',
                ensureSetSlotUnitUpdatesStateCorrectly);
        })
    })
})

function ensureInitProductUpdatesStateCorrectly() {
    const state = {
        product: {
            "id": ""
        }
    };
    const payload = {
        product: {
            "id": "1"
        }
    };
    mutations.init_product(state, payload);
    expect(state.product.id).toBe(payload.product.id);
}

function ensureSetCustomizedProductWidthUpdatesStateCorrectly() {
    const state = {
        customizedProduct: {
            customizedDimensions: {
                width: "",
                height: "",
                depth: "",
                unit: ""
            }
        }
    }
    const payload = {
        width: 100
    }
    mutations.set_customized_product_width(state, payload);
    expect(state.customizedProduct.customizedDimensions.width).toBe(payload.width);
}

function ensureSetCustomizedProductHeightUpdatesStateCorrectly() {
    const state = {
        customizedProduct: {
            customizedDimensions: {
                width: "",
                height: "",
                depth: "",
                unit: ""
            }
        }
    }
    const payload = {
        height: 100
    }
    mutations.set_customized_product_height(state, payload);
    expect(state.customizedProduct.customizedDimensions.height).toBe(payload.height);
}

function ensureSetCustomizedProductDepthUpdatesStateCorrectly() {
    const state = {
        customizedProduct: {
            customizedDimensions: {
                width: "",
                height: "",
                depth: "",
                unit: ""
            }
        }
    }
    const payload = {
        depth: 100
    }
    mutations.set_customized_product_depth(state, payload);
    expect(state.customizedProduct.customizedDimensions.depth).toBe(payload.depth);
}

function ensureSetCustomizedProductUnitUpdatesStateCorrectly() {
    const state = {
        customizedProduct: {
            customizedDimensions: {
                width: "",
                height: "",
                depth: "",
                unit: ""
            }
        }
    }
    const payload = {
        unit: "cm"
    }
    mutations.set_customized_product_unit(state, payload);
    expect(state.customizedProduct.customizedDimensions.unit).toBe(payload.unit);
}

function ensureSetSlotWidthUpdatesStateCorrectly() {
    //!Implement test after mutation is fixed
}

function ensureSetSlotHeightUpdatesStateCorrectly() {
    //!Implement test after mutation is fixed
}

function ensureSetSlotDepthUpdatesStateCorrectly() {
    //!Implement test after mutation is fixed
}

function ensureSetSlotUnitUpdatesStateCorrectly() {
    //!Implement test after mutation is fixed
}
