import 'es6-promise/auto'
import Vue from 'vue'
import Vuex from 'vuex'
import { mutations } from './mutations'

Vue.use(Vuex)

const state = {
    product: {},

    customizedProduct: {
        dimensions: {
            height: "",
            width: "",
            depth: ""
        },
        material: {
            id: "",
            finish: "",
            color: ""
        },
        slotNumber: "",
        customizedProducts: []
    }
}

export default new Vuex.Store({
    state,
    mutations
})