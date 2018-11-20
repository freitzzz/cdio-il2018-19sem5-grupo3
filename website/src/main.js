import Vue from 'vue'
import App from './App.vue'
import store from './store'
import Buefy from 'buefy';
import 'buefy/dist/buefy.css'; 

Vue.use(Buefy); 

new Vue({
    el: '#app',
    store,
    render: h =>h(App)
})