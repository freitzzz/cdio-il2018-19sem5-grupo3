import * as types from './mutation-types'

export const mutations = {
    [types.INIT_PRODUCT] (state, payload){
        state.product = payload
    }
}