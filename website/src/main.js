import Vue from 'vue'
import App from './App.vue'
import store from './store'
import Buefy from 'buefy';
import 'buefy/dist/buefy.css'; 

Vue.use(Buefy); 

import Vuetable from "vuetable-2";
//import VueTablePagination from 'vuetable-2/src/components/VuetablePagination'

function install(Vue){
    Vue.component("vuetable", Vuetable);
    /* Vue.component("vuetable-pagination", VueTablePagination);
    Vue.component("vuetable-pagination-dropdown", VueTablePaginationDropDown);
    Vue.component("vuetable-pagination-info", VueTablePaginationInfo); */
  }

install(Vue);

new Vue({
    el: '#app',
    store,
    render: h =>h(App)
})