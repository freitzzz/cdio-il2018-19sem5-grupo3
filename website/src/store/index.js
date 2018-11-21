import 'es6-promise/auto'
import Vue from 'vue'
import Vuex from 'vuex'
import { mutations } from './mutations'
import { actions } from './actions'

Vue.use(Vuex)

const state = {
    product: {},
    customizedProduct: {}
}

export default new Vuex.Store({
    state,
    actions,
    mutations
})