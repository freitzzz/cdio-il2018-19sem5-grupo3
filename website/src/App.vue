<template>
  <div id="app">
<!--     <side-bar></side-bar> -->
    <home></home>
  </div>
</template>

<script>
import Home from "./components/Home.vue";
import SideBar from "./components/SideBar.vue";
export default {
  name: "app",
  data() {
    return {
   
    };
  },
  components: {
    Home,
    SideBar

  }
};
</script> 