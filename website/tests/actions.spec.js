/* eslint-disable no-unused-vars */
/* eslint-disable no-undef */
import * as mutationsTypes from '../src/store/mutation-types'
import * as VuexStore from '../src/store/index'

describe('product actions', () => {
    describe('init product', () => {
        test('initialize product with correct info',
            ensureInitProductActionUpdatesStateCorrectly);
    });
})

describe('customized product actions', () => {
    describe('customized product id', () => {
        test('set id',
            ensureSetCustomizedProductIdUpdatesStateCorrectly
        );
    });
    describe('customized product dimensions', () => {
        test('set customized product dimensions',
            ensureSetCustomizedProductDimensionsUpdatesStateCorrectly
        );
    });
    describe('customized product slots', () => {
        describe('slot id', () => {
            test('set slot id',
                ensureSetSlotIdUpdatesStateCorrectly
            );
        });
        describe('slot dimensions', () => {
            test('add slot dimensions',
                ensureAddSlotDimensionsUpdatesStateCorrectly
            );
            test('initialize slots array as empty if payload is null',
                ensureAddSlotDimensionsInitializesArrayAsEmptyIfPayloadIsNull
            );
        });
    });
    describe('customized product material', () => {
        test('set customized product material',
            ensureSetCustomizedProductMaterialUpdatesStateCorrectly
        );
        test('set material finish',
            ensureSetCustomizedProductFinishUpdatesStateCorrectly
        );
        test('set material color',
            ensureSetCustomizedProductColorUpdatesStateCorrectly
        );
    });
    describe('customized product components', () => {
        test('set customized product component',
            ensureSetCustomizedProductComponentUpdatesStateCorrectly
        );
        test('don\'t add a component to a customized product if it has no slots',
            ensureSetCustomizedProductComponentDoesntAddComponentIfCustomizedProductHasNoSlots
        );
        test('initialize components array as empty if payload is null',
            ensureSetCustomizedProductComponentInitializesComponentsArrayAsEmptyIfPayloadIsNull
        );
        test('remove a component from a customized product',
            ensureRemoveCustomizedProductComponentUpdatesStateCorrectly
        );
    });
})

describe('canvas controls actions', () => {
    describe('closet movement controls', () => {
        test('activate closet movement',
            ensureActivatingClosetMovementFlagUpdatesStateCorrectly
        );
        test('deactivate closet movement',
            ensureDeactivatingClosetMovementFlagUpdatesStateCorrectly
        );
    });
    describe('slots movement controls', () => {
        test('activate slots movement',
            ensureActivatingSlotsMovementFlagUpdatesStateCorrectly
        );
        test('deactivate slots movement',
            ensureDeactivatingSlotsMovementFlagUpdatesStateCorrectly
        );
    });
    describe('components controls', () => {
        test('activate components movement',
            ensureActivatingComponentsMovementFlagUpdatesStateCorrectly
        );
        test('deactivate components movement',
            ensureDeactivatingComponentsMovementFlagUpdatesStateCorrectly
        );
    });
})

describe('resize factor dimensions actions', () => {
    test('set resize factor dimensions values', () => {
        ensureSetResizeFactorDimensionsUpdatesStateCorrectly
    });
})

describe('resize vector global actions', () => {
    test('set resize vector global values', () => {
        ensureSetResizeVectorGlobalUpdatesStateCorrectly
    });
})

function ensureInitProductActionUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        product: {}
    });
    const payload = {
        product: {
            id: 3,
            reference: "123",
            designation: "Closet",
            model: "closet.glb",
            category: {
                id: 1,
                name: "All Products"
            },
            materials: [
                {
                    id: 1,
                    reference: "#666",
                    designation: "Cherry Wood",
                    image: "cherry-wood.png"
                }
            ],
            components: [
                {
                    id: 1,
                    reference: "1",
                    designation: "component1",
                    model: "component1.glb",
                    mandatory: true
                }
            ],
            dimensions: [
                {
                    id: 5,
                    height: {
                        id: 14,
                        unit: "mm",
                        values: [
                            100,
                            140,
                            180,
                            400
                        ]
                    },
                    width: {
                        id: 15,
                        unit: "mm",
                        value: 500
                    },
                    depth: {
                        id: 13,
                        unit: "mm",
                        minValue: 15000,
                        maxValue: 20000,
                        increment: 250
                    }
                },
                {
                    id: 6,
                    height: {
                        id: 17,
                        unit: "mm",
                        minValue: 1,
                        maxValue: 5,
                        increment: 0.1
                    },
                    width: {
                        id: 18,
                        unit: "mm",
                        value: 500
                    },
                    depth: {
                        id: 16,
                        unit: "mm",
                        values: [
                            100000,
                            125000
                        ]
                    }
                }
            ],
            slotWidths: {
                minWidth: 200,
                maxWidth: 500,
                recommendedWidth: 250,
                unit: "mm"
            }
        }
    };
    VuexStore.default.dispatch(mutationsTypes.INIT_PRODUCT, payload);
    expect(VuexStore.default.state.product).toEqual(payload.product);
}

function ensureSetCustomizedProductDimensionsUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        customizedProduct: {
            customizedDimensions: {
                width: 0,
                height: 0,
                depth: 0,
                unit: ""
            }
        }
    });
    const payload = {
        width: 100,
        height: 100,
        depth: 100,
        unit: "cm"
    };
    VuexStore.default.dispatch(mutationsTypes.SET_CUSTOMIZED_PRODUCT_DIMENSIONS, payload);
    expect(VuexStore.default.state.customizedProduct.customizedDimensions).toEqual(payload);
}

function ensureSetSlotIdUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        customizedProduct: {
            slots: [
                {
                    idSlot: 1,
                    width: 100,
                    height: 100,
                    depth: 100,
                    unit: "dm"
                }
            ]
        }
    });
    const expectedSlotId = 2;
    const payload = {
        position: 0,
        idSlot: expectedSlotId
    };
    VuexStore.default.dispatch(mutationsTypes.SET_ID_SLOT, payload);
    expect(VuexStore.default.state.customizedProduct.slots[0].idSlot).toEqual(expectedSlotId);
}

function ensureAddSlotDimensionsUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        customizedProduct: {
            slots: []
        }
    });
    const payload = {
        idSlot: 1,
        width: 100,
        height: 100,
        depth: 100,
        unit: "dm"
    };
    VuexStore.default.dispatch(mutationsTypes.ADD_SLOT_DIMENSIONS, payload);
    expect(VuexStore.default.state.customizedProduct.slots[0]).toEqual(payload);
}

function ensureAddSlotDimensionsInitializesArrayAsEmptyIfPayloadIsNull() {
    VuexStore.default.replaceState({
        customizedProduct: {}
    });
    const payload = null;
    VuexStore.default.dispatch(mutationsTypes.ADD_SLOT_DIMENSIONS, payload);
    expect(VuexStore.default.state.customizedProduct.slots).toHaveLength(0);
}

function ensureSetCustomizedProductMaterialUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        customizedProduct: {
            customizedMaterial: {
                id: "",
                reference: "",
                designation: "",
                image: ""
            }
        }
    });
    const payload = {
        id: 1,
        reference: "hello i'm a reference",
        designation: "and i'm a designation",
        image: "image.jpg"
    };
    VuexStore.default.dispatch(mutationsTypes.SET_CUSTOMIZED_PRODUCT_MATERIAL, payload);
    expect(VuexStore.default.state.customizedProduct.customizedMaterial).toEqual(payload);
}

function ensureSetCustomizedProductFinishUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        customizedProduct: {
            customizedMaterial: {
                finish: {}
            }
        }
    });
    const payload = {
        description: "varnish",
        shininess: 100
    };
    VuexStore.default.dispatch(mutationsTypes.SET_CUSTOMIZED_PRODUCT_FINISH, payload);
    expect(VuexStore.default.state.customizedProduct.customizedMaterial.finish).toEqual(payload);
}

function ensureSetCustomizedProductColorUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        customizedProduct: {
            customizedMaterial: {
                color: {}
            }
        }
    });
    const payload = {
        name: "blue",
        red: 100,
        green: 100,
        blue: 100,
        alpha: 1
    };
    VuexStore.default.dispatch(mutationsTypes.SET_CUSTOMIZED_PRODUCT_COLOR, payload);
    expect(VuexStore.default.state.customizedProduct.customizedMaterial.color).toEqual(payload);
}

function ensureSetCustomizedProductComponentUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        customizedProduct: {
            components: [],
            slots: [{
                components: [],
                idSlot: 1,
                width: 100,
                height: 100,
                depth: 100,
                unit: "dm"
            }]
        }
    });
    const payload = {
        component: {
            designation: "Drawer",
            hasComponents: false,
            id: 1,
            mandatory: true,
            model: "drawer.fbx",
            reference: "#666",
            slot: "1",
            supportsSlots: true
        }
    }
    VuexStore.default.dispatch(mutationsTypes.SET_CUSTOMIZED_PRODUCT_COMPONENTS, payload);
    expect(VuexStore.default.state.customizedProduct.components[0]).toEqual(payload.component);
}

function ensureSetCustomizedProductComponentDoesntAddComponentIfCustomizedProductHasNoSlots() {
    VuexStore.default.replaceState({
        customizedProduct: {
            components: [],
            slots: []
        }
    });
    const payload = {
        component: {
            designation: "Drawer",
            hasComponents: false,
            id: 1,
            mandatory: true,
            model: "drawer.fbx",
            reference: "#666",
            slot: "1",
            supportsSlots: true
        }
    };
    VuexStore.default.dispatch(mutationsTypes.SET_CUSTOMIZED_PRODUCT_COMPONENTS, payload);
    expect(VuexStore.default.state.customizedProduct.components).toHaveLength(0);
}

function ensureSetCustomizedProductComponentInitializesComponentsArrayAsEmptyIfPayloadIsNull() {
    VuexStore.default.replaceState({
        customizedProduct: {
            slots: [{
                components: [],
                idSlot: 1,
                width: 100,
                height: 100,
                depth: 100,
                unit: "dm"
            }]
        }
    });
    const payload = null;
    VuexStore.default.dispatch(mutationsTypes.SET_CUSTOMIZED_PRODUCT_COMPONENTS, payload);
    expect(VuexStore.default.state.customizedProduct.components).toHaveLength(0);
}

function ensureRemoveCustomizedProductComponentUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        canvasControls: {
            componentToRemove: {}
        },
        customizedProduct: {
            components: [],
            slots: [{
                components: [],
                idSlot: 1,
                width: 100,
                height: 100,
                depth: 100,
                unit: "dm"
            }]
        }
    });
    const payload = {
        component: {
            designation: "Drawer",
            hasComponents: false,
            id: 1,
            mandatory: true,
            model: "drawer.fbx",
            reference: "#666",
            slot: "1",
            supportsSlots: true
        }
    }
    VuexStore.default.dispatch(mutationsTypes.SET_CUSTOMIZED_PRODUCT_COMPONENTS, payload);
    VuexStore.default.dispatch(mutationsTypes.REMOVE_CUSTOMIZED_PRODUCT_COMPONENT, payload);
    expect(VuexStore.default.state.customizedProduct.components).toHaveLength(0);
}

function ensureActivatingClosetMovementFlagUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        canvasControls: {
            canMoveCloset: false
        }
    });
    VuexStore.default.dispatch(mutationsTypes.ACTIVATE_CAN_MOVE_CLOSET);
    expect(VuexStore.default.state.canvasControls.canMoveCloset).toBeTruthy();
}

function ensureDeactivatingClosetMovementFlagUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        canvasControls: {
            canMoveCloset: true
        }
    });
    VuexStore.default.dispatch(mutationsTypes.DEACTIVATE_CAN_MOVE_CLOSET);
    expect(VuexStore.default.state.canvasControls.canMoveCloset).toBeFalsy();
}

function ensureActivatingSlotsMovementFlagUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        canvasControls: {
            canMoveSlots: false
        }
    });
    VuexStore.default.dispatch(mutationsTypes.ACTIVATE_CAN_MOVE_SLOTS);
    expect(VuexStore.default.state.canvasControls.canMoveSlots).toBeTruthy();
}

function ensureDeactivatingSlotsMovementFlagUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        canvasControls: {
            canMoveSlots: true
        }
    });
    VuexStore.default.dispatch(mutationsTypes.DEACTIVATE_CAN_MOVE_SLOTS);
    expect(VuexStore.default.state.canvasControls.canMoveSlots).toBeFalsy();
}

function ensureActivatingComponentsMovementFlagUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        canvasControls: {
            canMoveComponents: false
        }
    });
    VuexStore.default.dispatch(mutationsTypes.ACTIVATE_CAN_MOVE_COMPONENTS);
    expect(VuexStore.default.state.canvasControls.canMoveComponents).toBeTruthy();
}

function ensureDeactivatingComponentsMovementFlagUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        canvasControls: {
            canMoveComponents: true
        }
    });
    VuexStore.default.dispatch(mutationsTypes.DEACTIVATE_CAN_MOVE_COMPONENTS);
    expect(VuexStore.default.state.canvasControls.canMoveComponents).toBeFalsy();
}

function ensureSetCustomizedProductIdUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        customizedProduct: {
            id: ""
        }
    });
    const payload = {
        id: 1
    };
    VuexStore.default.dispatch(mutationsTypes.SET_ID_CUSTOMIZED_PRODUCT, payload);
    expect(VuexStore.default.state.customizedProduct.id).toEqual(payload);
}

function ensureSetResizeFactorDimensionsUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        resizeFactorDimensions: {
            width: "",
            height: "",
            depth: ""
        }
    });
    const payload = {
        width: "2",
        height: "2",
        depth: "2"
    };
    VuexStore.default.dispatch(mutationsTypes.SET_RESIZE_FACTOR_DIMENSIONS, payload);
    expect(VuexStore.default.state.resizeFactorDimensions).toEqual(payload);
}

function ensureSetResizeVectorGlobalUpdatesStateCorrectly() {
    VuexStore.default.replaceState({
        resizeVectorGlobal: {
            width: "",
            height: "",
            depth: ""
        }
    });
    const payload = {
        width: "2",
        height: "2",
        depth: "2"
    };
    VuexStore.default.dispatch(mutationsTypes.SET_RESIZE_VECTOR_GLOBAL, payload);
    expect(VuexStore.default.state.resizeVectorGlobal).toEqual(payload);
}