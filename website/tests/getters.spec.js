/* eslint-disable no-unused-vars */
/* eslint-disable no-undef */
import * as VuexStore from '../src/store/index'

//TODO Refactor these tests after Store structure changes are made

describe('product getters', () => {
    describe('product-id', () => {
        test('productId-returns-correct-value',
            ensureGetProductIdReturnsCorrectValue
        );
    });
    describe('product-dimensions', () => {
        test('productDimensions-returns-correct-dimensions',
            ensureGetProductDimensionsReturnsCorrectValues
        );
    });
    describe('product-slots', () => {
        test('productSlotWidths-returns-correct-slot-widths',
            ensureGetProductSlotWidthsReturnsCorrectValues
        );
        test('productRecommendedSlotWidth-returns-correct-value',
            ensureGetProductRecommendedSlotWidthReturnsCorrectValue
        );
        test('productMaximumSlotWidth-returns-correct-value',
            ensureGetProductMaximumSlotWidthReturnsCorrectValue
        );
        test('productMinimumSlotWidth-returns-correct-value',
            ensureGetProductMinimumSlotWidthReturnsCorrectValue
        );
    });
    describe('product-materials', () => {
        test('productMaterials-returns-correct-materials',
            ensureGetProductMaterialsReturnsCorrectValues
        );
    });
    describe('product-components', () => {
        test('productComponents-returns-correct-components',
            ensureGetProductComponentsReturnsCorrectValues
        );
    });
})

describe('customized product getters', () => {
    describe('customized-product-dimensions', () => {
        test('customizedProductWidth-returns-correct-value',
            ensureGetCustomizedProductWidthReturnsCorrectValue
        );
        //TODO Fix failing test
        /* test('customizedProductHeight-returns-correct-value',
            ensureGetCustomizedProductHeightReturnsCorrectValue
        ); */
        test('customizedProductDepth-returns-correct-value',
            ensureGetCustomizedProductDepthReturnsCorrectValue
        );
        test('customizedProductUnit-returns-correct-value',
            ensureGetCustomizedProductUnitReturnsCorrectValue
        );
    });
    describe('customized-product-slots', () => {
        //!Implement tests related to slot widths once getters are corrected
    });
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

function ensureGetCustomizedProductWidthReturnsCorrectValue() {
    const expectedWidth = 200;
    VuexStore.default.replaceState({
        customizedProduct: {
            //TODO Why is this named dimensions?
            dimensions: {
                "width": expectedWidth
            }
        }
    });
    const actualWidth = VuexStore.getters.width(VuexStore.default.state);
    expect(actualWidth).toBe(expectedWidth);
}

function ensureGetCustomizedProductHeightReturnsCorrectValue() {
    const expectedHeight = 200;
    VuexStore.default.replaceState({
        customizedProduct: {
            //TODO Why is this named dimensions
            dimensions: {
                "height": expectedHeight
            }
        }
    });
    const actualHeight = VuexStore.getters.height(VuexStore.default.state);
    expect(actualHeight).toBe(expectedHeight);
}

function ensureGetCustomizedProductDepthReturnsCorrectValue() {
    const expectedDepth = 200;
    VuexStore.default.replaceState({
        customizedProduct: {
            //TODO Why is this named dimensions
            dimensions: {
                "depth": expectedDepth
            }
        }
    });
    const actualDepth = VuexStore.getters.depth(VuexStore.default.state);
    expect(actualDepth).toBe(expectedDepth);
}

function ensureGetCustomizedProductUnitReturnsCorrectValue() {
    const expectedUnit = "cm";
    VuexStore.default.replaceState({
        customizedProduct: {
            //TODO Why is this named dimensions
            dimensions: {
                "unit": expectedUnit
            }
        }
    });
    const actualUnit = VuexStore.getters.unit(VuexStore.default.state);
    expect(actualUnit).toBe(expectedUnit);
}

