/* eslint-disable no-unused-vars */
/* eslint-disable no-undef */
import * as VuexStore from '../src/store/index'

describe('product getters', () => {
    describe('product id', () => {
        test('product id returns correct value',
            ensureGetProductIdReturnsCorrectValue
        );
    });
    describe('product dimensions', () => {
        test('product dimensions returns correct dimensions',
            ensureGetProductDimensionsReturnsCorrectValues
        );
    });
    describe('product slots', () => {
        test('product slot widths returns correct slot widths',
            ensureGetProductSlotWidthsReturnsCorrectValues
        );
        test('product recommended slot width returns correct value',
            ensureGetProductRecommendedSlotWidthReturnsCorrectValue
        );
        test('product maximum slot width returns correct value',
            ensureGetProductMaximumSlotWidthReturnsCorrectValue
        );
        test('product minimum slot width returns correct value',
            ensureGetProductMinimumSlotWidthReturnsCorrectValue
        );
    });
    describe('product materials', () => {
        test('product materials returns correct materials',
            ensureGetProductMaterialsReturnsCorrectValues
        );
    });
    describe('product components', () => {
        test('product components returns correct components',
            ensureGetProductComponentsReturnsCorrectValues
        );
    });
})

describe('customized product getters', () => {
    describe('customized product id', () => {
        test('customized product id returns correct value',
            ensureGetCustomizedProductIdReturnsCorrectValue
        );
    });
    describe('customized product dimensions', () => {
        test('customized product dimensions returns correct values',
            ensureGetCustomizedProductDimensionsReturnsCorrectValues
        );
    });
    describe('customized product slots', () => {
        test('customized products slot width returns correct values',
            ensureGetCustomizedProductSlotWidthReturnsCorrectValues
        );
    });
    describe('customized product components', () => {
        test('customized products components returns correct values',
            ensureGetCustomizedProductComponentsReturnsCorrectValues
        );
    })
    describe('customized products customized material', () => {
        test('customized material image returns correct value',
            ensureGetCustomizedProductCustomizedMaterialImageReturnsCorrectValue
        );
        test('customized material color rgba returns correct values',
            ensureGetCustomizedProductCustomizedMaterialColorReturnsCorrectValues
        );
        test('customized material color name returns correct value',
            ensureGetCustomizedProductCustomizedMaterialColorNameReturnsCorrectValue
        );
        test('customized material finish shininess returns correct value',
            ensureGetCustomizedProductCustomizedMaterialFinishReturnsCorrectValue
        );
        test('customized material finish name returns correct value',
            ensureGetCustomizedProductCustomizedMaterialFinishDescriptionReturnsCorrectValue
        );
    })
})

describe('canvas controls getters', () => {
    describe('movement controls', () => {
        describe('closet movement controls', () => {
            test('can move closet returns correct value',
                ensureGetCanMoveClosetReturnsCorrectValue
            );
        });
        describe('slots movement controls', () => {
            test('can move slots returns correct value',
                ensureGetCanMoveSlotsReturnsCorrectValue
            );
        });
        describe('components movement controls', () => {
            test('can move components returns correct value',
                ensureGetCanMoveComponentsReturnsCorrectValue
            );
        });
    });
    describe('removal controls', () => {
        describe('component removal controls', () => {
            test('component to remove returns correct value',
                ensureGetComponentToRemoveReturnsCorrectValue
            );
        });
    });
})

describe('resize factor dimensions getters', () => {
    test('get resize factor dimensions returns correct values',
        ensureGetResizeFactorDimensionsReturnsCorrectValues
    );
})

describe('resize vector global getters', () => {
    test('get resize vector global returns correct values',
        ensureGetResizeVectorGlobalReturnsCorrectValues
    );
})

function ensureGetProductIdReturnsCorrectValue() {
    const expectedId = 1;
    VuexStore.default.replaceState({
        product: {
            "id": expectedId
        }
    });
    const actualId = VuexStore.getters.productId(VuexStore.default.state);
    expect(actualId).toBe(expectedId);
}

function ensureGetProductDimensionsReturnsCorrectValues() {
    const expectedDimensions = [
        {
            "id": 5,
            "height": {
                "id": 14,
                "unit": "mm",
                "values": [
                    100,
                    140,
                ]
            },
            "width": {
                "id": 15,
                "unit": "mm",
                "value": 500
            },
            "depth": {
                "id": 13,
                "unit": "mm",
                "minValue": 15000,
                "maxValue": 20000,
                "increment": 250
            }
        }
    ]
    VuexStore.default.replaceState({
        product: {
            "dimensions": expectedDimensions
        }
    });
    const actualDimensions = VuexStore.getters.productDimensions(VuexStore.default.state);
    expect(actualDimensions).toBe(expectedDimensions);
}

function ensureGetProductSlotWidthsReturnsCorrectValues() {
    const expectedSlotWidths = {
        "minWidth": 200,
        "maxWidth": 500,
        "recommendedWidth": 250,
        "unit": "mm"
    };
    VuexStore.default.replaceState({
        product: {
            "slotWidths": expectedSlotWidths
        }
    });
    const actualSlotWidths = VuexStore.getters.productSlotWidths(VuexStore.default.state);
    expect(actualSlotWidths).toBe(expectedSlotWidths);
}

function ensureGetProductRecommendedSlotWidthReturnsCorrectValue() {
    const expectedRecommendedSlotWidth = 250;
    VuexStore.default.replaceState({
        product: {
            "slotWidths": {
                "recommendedWidth": 250
            }
        }
    });
    const actualRecommendedSlotWidth = VuexStore.getters.recommendedSlotWidth(VuexStore.default.state);
    expect(actualRecommendedSlotWidth).toBe(expectedRecommendedSlotWidth);
}

function ensureGetProductMaximumSlotWidthReturnsCorrectValue() {
    const expectedMaximumSlotWidth = 500;
    VuexStore.default.replaceState({
        product: {
            "slotWidths": {
                "maxWidth": 500
            }
        }
    });
    const actualMaximumSlotWidth = VuexStore.getters.maxSlotWidth(VuexStore.default.state);
    expect(actualMaximumSlotWidth).toBe(expectedMaximumSlotWidth);
}

function ensureGetProductMinimumSlotWidthReturnsCorrectValue() {
    const expectedMinimumSlotWidth = 200;
    VuexStore.default.replaceState({
        product: {
            "slotWidths": {
                "minWidth": 200
            }
        }
    });
    const actualMinimumSlotWidth = VuexStore.getters.minSlotWidth(VuexStore.default.state);
    expect(actualMinimumSlotWidth).toBe(expectedMinimumSlotWidth);
}

function ensureGetProductMaterialsReturnsCorrectValues() {
    const expectedMaterials = [
        {
            "id": 1,
            "reference": "#666",
            "designation": "Cherry Wood",
            "image": "cherry-wood.png"
        }
    ];
    VuexStore.default.replaceState({
        product: {
            "materials": expectedMaterials
        }
    });
    const actualMaterials = VuexStore.getters.productMaterials(VuexStore.default.state);
    expect(actualMaterials).toBe(expectedMaterials);
}

function ensureGetProductComponentsReturnsCorrectValues() {
    const expectedComponents = [
        {
            "id": 1,
            "reference": "1",
            "designation": "component1",
            "model": "component1.glb",
            "mandatory": true
        },
        {
            "id": 2,
            "reference": "2",
            "designation": "component2",
            "model": "component2.glb",
            "mandatory": false
        }
    ];
    VuexStore.default.replaceState({
        product: {
            "components": expectedComponents
        }
    });
    const actualComponents = VuexStore.getters.productComponents(VuexStore.default.state);
    expect(actualComponents).toBe(expectedComponents);
}

function ensureGetCustomizedProductDimensionsReturnsCorrectValues() {
    const expectedDimensions = {
        dimensions: {
            height: 200,
            width: 200,
            depth: 200,
            unit: "cm"
        }
    };
    VuexStore.default.replaceState({
        customizedProduct: {
            customizedDimensions: expectedDimensions
        }
    });
    const actualDimensions = VuexStore.getters.customizedProductDimensions(VuexStore.default.state);
    expect(actualDimensions).toBe(expectedDimensions);
}

function ensureGetCustomizedProductSlotWidthReturnsCorrectValues() {
    const expectedFirstSlotWidth = 100;
    const expectedSlots =
        [
            {
                width: expectedFirstSlotWidth
            }
        ];
    VuexStore.default.replaceState({
        customizedProduct: {
            slots: expectedSlots
        }
    });
    const actualFirstSlotWidth = VuexStore.getters.customizedProductSlotWidth(VuexStore.default.state)(0).width;
    expect(actualFirstSlotWidth).toBe(expectedFirstSlotWidth);
}

function ensureGetCustomizedProductComponentsReturnsCorrectValues() {
    const expectedComponent = {
        designation: "Drawer",
        hasComponents: false,
        id: 1,
        mandatory: true,
        model: "drawer.fbx",
        reference: "#666",
        slot: "1",
        supportsSlots: true
    };
    VuexStore.default.replaceState({
        customizedProduct: {
            components: [expectedComponent],
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
    const actualComponent = VuexStore.getters.customizedProductComponents(VuexStore.default.state)[0];
    expect(actualComponent).toEqual(expectedComponent);
}

function ensureGetCustomizedProductCustomizedMaterialImageReturnsCorrectValue() {
    const expectedImage = "material.jpg";
    const expectedMaterial = {
        image: expectedImage
    };
    VuexStore.default.replaceState({
        customizedProduct: {
            customizedMaterial: expectedMaterial
        }
    });
    const actualMaterialImage = VuexStore.getters.customizedMaterial(VuexStore.default.state);
    expect(actualMaterialImage).toBe(expectedImage);
}

function ensureGetCustomizedProductCustomizedMaterialColorReturnsCorrectValues() {
    const expectedColor = {
        red: 100,
        green: 100,
        blue: 100,
        alpha: 1
    };
    const expectedMaterial = {
        color: expectedColor
    };
    const expectedResult = expectedColor.red + '-' + expectedColor.green +
        '-' + expectedColor.blue + "-" + expectedColor.alpha;
    VuexStore.default.replaceState({
        customizedProduct: {
            customizedMaterial: expectedMaterial
        }
    });
    const actualResult = VuexStore.getters.customizedMaterialColor(VuexStore.default.state);
    expect(actualResult).toEqual(expectedResult);
}

function ensureGetCustomizedProductCustomizedMaterialColorNameReturnsCorrectValue() {
    const expectedColor = {
        name: "blue"
    };
    const expectedMaterial = {
        color: expectedColor
    };
    VuexStore.default.replaceState({
        customizedProduct: {
            customizedMaterial: expectedMaterial
        }
    });
    const actualColorName = VuexStore.getters.customizedMaterialColorName(VuexStore.default.state);
    expect(actualColorName).toEqual(expectedColor.name);
}

function ensureGetCustomizedProductCustomizedMaterialFinishReturnsCorrectValue() {
    const expectedFinish = {
        shininess: 4
    };
    const expectedMaterial = {
        finish: expectedFinish
    };
    VuexStore.default.replaceState({
        customizedProduct: {
            customizedMaterial: expectedMaterial
        }
    });
    const actualFinishShininess = VuexStore.getters.customizedMaterialFinish(VuexStore.default.state);
    expect(actualFinishShininess).toEqual(expectedFinish.shininess);
}

function ensureGetCustomizedProductCustomizedMaterialFinishDescriptionReturnsCorrectValue() {
    const expectedFinish = {
        description: "varnish"
    };
    const expectedMaterial = {
        finish: expectedFinish
    };
    VuexStore.default.replaceState({
        customizedProduct: {
            customizedMaterial: expectedMaterial
        }
    });
    const actualFinishDescription = VuexStore.getters.customizedMaterialFinishDescription(VuexStore.default.state);
    expect(actualFinishDescription).toEqual(expectedFinish.description);
}

function ensureGetCanMoveClosetReturnsCorrectValue() {
    const expectedFlag = true;
    VuexStore.default.replaceState({
        canvasControls: {
            canMoveCloset: expectedFlag
        }
    });
    const actualFlag = VuexStore.getters.canMoveCloset(VuexStore.default.state);
    expect(actualFlag).toBe(expectedFlag);
}

function ensureGetCanMoveSlotsReturnsCorrectValue() {
    const expectedFlag = true;
    VuexStore.default.replaceState({
        canvasControls: {
            canMoveSlots: expectedFlag
        }
    });
    const actualFlag = VuexStore.getters.canMoveSlots(VuexStore.default.state);
    expect(actualFlag).toBe(expectedFlag);
}

function ensureGetCanMoveComponentsReturnsCorrectValue() {
    const expectedFlag = true;
    VuexStore.default.replaceState({
        canvasControls: {
            canMoveComponents: expectedFlag
        }
    });
    const actualFlag = VuexStore.getters.canMoveComponents(VuexStore.default.state);
    expect(actualFlag).toBe(expectedFlag);
}

function ensureGetComponentToRemoveReturnsCorrectValue() {
    const expectedComponent = {
        designation: "Drawer",
        hasComponents: false,
        id: 1,
        mandatory: true,
        model: "drawer.fbx",
        reference: "#666",
        slot: "1",
        supportsSlots: true
    };
    VuexStore.default.replaceState({
        canvasControls: {
            componentToRemove: expectedComponent
        },
        customizedProduct: {
            components: [expectedComponent],
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
    const actualComponent = VuexStore.getters.componentToRemove(VuexStore.default.state);
    expect(actualComponent).toEqual(expectedComponent);
}

function ensureGetCustomizedProductIdReturnsCorrectValue() {
    const expectedId = 1;
    VuexStore.default.replaceState({
        customizedProduct: {
            id: expectedId
        }
    });
    const actualId = VuexStore.getters.customizedProductId(VuexStore.default.state);
    expect(actualId).toBe(expectedId);
}

function ensureGetResizeFactorDimensionsReturnsCorrectValues() {
    const expectedValues = {
        width: 100,
        height: 100,
        depth: 100
    };
    VuexStore.default.replaceState({
        resizeFactorDimensions: expectedValues
    });
    const actualValues = VuexStore.getters.resizeFactorDimensions(VuexStore.default.state);
    expect(actualValues).toEqual(expectedValues);
}

function ensureGetResizeVectorGlobalReturnsCorrectValues() {
    const expectedValues = {
        width: 100,
        height: 100,
        depth: 100
    };
    VuexStore.default.replaceState({
        resizeVectorGlobal: expectedValues
    });
    const actualValues = VuexStore.getters.resizeVectorGlobal(VuexStore.default.state);
    expect(actualValues).toEqual(expectedValues);
}