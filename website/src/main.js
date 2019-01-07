import Vue from 'vue'
import VueRouter from 'vue-router'
import App from './App.vue'
import store from './store'
import Buefy from 'buefy';
import 'buefy/dist/buefy.css';
import Vuetable from "vuetable-2";
//import VueTablePagination from 'vuetable-2/src/components/VuetablePagination'
import { router } from "./router"

import VueClipBoard2 from 'vue-clipboard2';

Vue.use(VueClipBoard2);
Vue.use(VueRouter);
Vue.use(Buefy);

function install(Vue) {
  Vue.component("vuetable", Vuetable);
  /* Vue.component("vuetable-pagination", VueTablePagination);
  Vue.component("vuetable-pagination-dropdown", VueTablePaginationDropDown);
  Vue.component("vuetable-pagination-info", VueTablePaginationInfo); */
}

install(Vue);

new Vue({
  el: '#app',
  store,
  router,
  render: h => h(App)
})