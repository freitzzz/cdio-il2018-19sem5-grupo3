/* eslint-disable no-unused-vars */
/* eslint-disable no-undef */
import { mutations } from '../src/store/mutations'

describe('product mutations', () => {
    describe('product information', () => {
        test('init product',
            ensureInitProductUpdatesStateCorrectly
        );
    });
});

describe('customized product mutations', () => {
    describe('customized product id', () => {
        test('set customized product id',
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
            test('initialize slots to empty array if mutation payload is null',
                ensureAddSlotDimensionsInitializesEmptyArrayIfMutationPayloadIsNull
            );
        });
    });
    describe('customized product material', () => {
        test('set customized material',
            ensureSetCustomizedProductMaterialUpdatesStateCorrectly
        );
        test('set customized material color',
            ensureSetCstomizedProductMaterialColorUpdatesStateCorrectly
        );
        test('set customized material finish',
            ensureSetCustomizedProductMaterialFinishUpdatesStateCorrectly
        );
    });
    describe('customized product components', () => {
        test('add a component to a customized product',
            ensureSetCustomizedProductComponentsUpdatesStateCorrectly
        );
        test('don\'t add a component to a customized product if it has no slots',
            ensureSetCustomizedProductComponentDoesntAddComponentIfCustomizedProductDoesntHaveSlots
        );
        test('initialize empty array if payload is null',
            ensureSetCustomizedProductComponentInitializesEmptyArrayIfPayloadIsNull
        );
        test('remove a component from a customized product',
            ensureRemoveCustomizedProductComponentUpdatesStateCorrectly
        );
    });
})

describe('canvas controls mutations', () => {
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
    describe('components movement controls', () => {
        test('activate components movement',
            ensureActivatingComponentsMovementFlagUpdatesStateCorrectly
        );
        test('deactivate components movement',
            ensureDeactivatingComponentsMovementFlagUpdatesStateCorrectly
        );
    });
})

describe('resize factor dimensions mutations', () => {
    test('set resize factor dimensions values', () => {
        ensureSetResizeFactorDimensionsUpdatesStateCorrectly
    });
})

function ensureInitProductUpdatesStateCorrectly() {
    const state = {
        product: {

        }
    };
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
    mutations.init_product(state, payload);
    expect(state.product).toEqual(payload.product);
}

function ensureSetCustomizedProductDimensionsUpdatesStateCorrectly() {
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
        width: 100,
        height: 100,
        depth: 100,
        unit: "cm"
    }
    mutations.set_customized_product_dimensions(state, payload);
    expect(state.customizedProduct.customizedDimensions).toEqual(payload);
}

function ensureSetCustomizedProductIdUpdatesStateCorrectly() {
    const state = {
        customizedProduct: {
            id: ""
        }
    };
    const payload = {
        id: 1
    };
    mutations.set_id_customized_product(state, payload);
    expect(state.customizedProduct.id).toBe(payload);
}

function ensureAddSlotDimensionsUpdatesStateCorrectly() {
    const state = {
        customizedProduct: {
            slots: []
        }
    };
    const payload = {
        idSlot: 1,
        width: 100,
        height: 100,
        depth: 100,
        unit: "dm"
    };
    mutations.add_slot_dimensions(state, payload);
    expect(state.customizedProduct.slots[0]).toEqual(payload);
}

function ensureAddSlotDimensionsInitializesEmptyArrayIfMutationPayloadIsNull() {
    const state = {
        customizedProduct: {
            slots: []
        }
    };
    const payload = null;
    mutations.add_slot_dimensions(state, payload);
    expect(state.customizedProduct.slots).toHaveLength(0);
}

function ensureSetSlotIdUpdatesStateCorrectly() {
    const state = {
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
    };
    const expectedSlotId = 2;
    const payload = {
        position: 0,
        idSlot: expectedSlotId
    };
    mutations.set_id_slot(state, payload);
    expect(state.customizedProduct.slots[0].idSlot).toEqual(expectedSlotId);
}

function ensureSetCustomizedProductMaterialUpdatesStateCorrectly() {
    const state = {
        customizedProduct: {
            customizedMaterial: {
                id: "",
                reference: "",
                designation: "",
                image: ""
            }
        }
    };
    const payload = {
        id: 1,
        reference: "hello i'm a reference",
        designation: "and i'm a designation",
        image: "image.jpg"
    };
    mutations.set_customized_product_material(state, payload);
    expect(state.customizedProduct.customizedMaterial).toEqual(payload);
}

function ensureSetCstomizedProductMaterialColorUpdatesStateCorrectly() {
    const state = {
        customizedProduct: {
            customizedMaterial: {
                color: {}
            }
        }
    };
    const payload = {
        name: "blue",
        red: 100,
        green: 100,
        blue: 100,
        alpha: 1
    };
    mutations.set_customized_product_color(state, payload);
    expect(state.customizedProduct.customizedMaterial.color).toEqual(payload);
}

function ensureSetCustomizedProductMaterialFinishUpdatesStateCorrectly() {
    const state = {
        customizedProduct: {
            customizedMaterial: {
                finish: {}
            }
        }
    };
    const payload = {
        description: "varnish",
        shininess: 100
    };
    mutations.set_customized_product_finish(state, payload);
    expect(state.customizedProduct.customizedMaterial.finish).toEqual(payload);
}

function ensureSetCustomizedProductComponentsUpdatesStateCorrectly() {
    const state = {
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
    }
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
    mutations.set_slot_components(state, payload);
    expect(state.customizedProduct.components[0]).toEqual(payload.component);
}

function ensureSetCustomizedProductComponentDoesntAddComponentIfCustomizedProductDoesntHaveSlots() {
    const state = {
        customizedProduct: {
            components: [],
            slots: []
        }
    }
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
    mutations.set_slot_components(state, payload);
    expect(state.customizedProduct.components).toHaveLength(0);
}

function ensureSetCustomizedProductComponentInitializesEmptyArrayIfPayloadIsNull() {
    const state = {
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
    }
    const payload = null;
    mutations.set_slot_components(state, payload);
    expect(state.customizedProduct.components).toHaveLength(0);
}

function ensureRemoveCustomizedProductComponentUpdatesStateCorrectly() {
    const state = {
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
    }
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
    mutations.set_slot_components(state, payload);
    mutations.remove_slot_component(state, payload);
    expect(state.customizedProduct.components).toHaveLength(0);
}

function ensureActivatingClosetMovementFlagUpdatesStateCorrectly() {
    const state = {
        canvasControls: {
            canMoveCloset: false
        }
    };
    mutations.activate_can_move_closet(state);
    expect(state.canvasControls.canMoveCloset).toBeTruthy();
}

function ensureDeactivatingClosetMovementFlagUpdatesStateCorrectly() {
    const state = {
        canvasControls: {
            canMoveCloset: true
        }
    };
    mutations.deactivate_can_move_closet(state);
    expect(state.canvasControls.canMoveCloset).toBeFalsy();
}

function ensureActivatingSlotsMovementFlagUpdatesStateCorrectly() {
    const state = {
        canvasControls: {
            canMoveSlots: false
        }
    };
    mutations.activate_can_move_slots(state);
    expect(state.canvasControls.canMoveSlots).toBeTruthy();
}

function ensureDeactivatingSlotsMovementFlagUpdatesStateCorrectly() {
    const state = {
        canvasControls: {
            canMoveSlots: true
        }
    };
    mutations.deactivate_can_move_slots(state);
    expect(state.canvasControls.canMoveSlots).toBeFalsy();
}

function ensureActivatingComponentsMovementFlagUpdatesStateCorrectly() {
    const state = {
        canvasControls: {
            canMoveComponents: false
        }
    };
    mutations.activate_can_move_components(state);
    expect(state.canvasControls.canMoveComponents).toBeTruthy();
}

function ensureDeactivatingComponentsMovementFlagUpdatesStateCorrectly() {
    const state = {
        canvasControls: {
            canMoveComponents: true
        }
    };
    mutations.deactivate_can_move_components(state);
    expect(state.canvasControls.canMoveComponents).toBeFalsy();
}

function ensureSetResizeFactorDimensionsUpdatesStateCorrectly() {
    const state = {
        resizeFactorDimensions: {
            width: "",
            height: "",
            depth: ""
        }
    };
    const payload = {
        width: "2",
        height: "2",
        depth: "2"
    };
    mutations.set_resize_factor_dimensions(state, payload);
    expect(state.resizeFactorDimensions).toEqual(payload);
}