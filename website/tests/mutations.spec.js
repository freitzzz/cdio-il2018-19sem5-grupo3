import { mutations } from '../src/store/mutations'

test('INIT_PRODUCT updates state with correct product id', () => {
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
})

test('SET_CUSTOMIZED_PRODUCT_WIDTH updates state with correct width', () => {
    const state = {
        
    }
})

test('SET_CUSTOMIZED_PRODUCT_HEIGHT updates state with correct height', () => {
    const state = {

    }
})

test('SET_CUSTOMIZED_PRODUCT_DEPTH updates state with correct depth', () => {
    const state = {

    }
})

test('SET_CUSTOMIZED_PRODUCT_UNIT updates state with correct unit', () => {
    const state = {

    }
})