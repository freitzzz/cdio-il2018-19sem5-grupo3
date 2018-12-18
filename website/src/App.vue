<template>
  <div id="app">
    <!--<home></home>-->
    <management-top-bar></management-top-bar>
  </div>
</template>

<script>
import Home from "./components/Home.vue";
import ManagementTopBar from "./components/ManagementTopBar.vue"
export default {
  name: "app",
  data() {
    return {
   
    };
  },
  components: {
    Home,
    ManagementTopBar
  }
};
</script> 