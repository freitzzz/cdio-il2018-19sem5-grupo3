<template>
    <div id="app">
      <!--  <customizer v-if="customizerEnabled"></customizer>
      <button v-else @click="enableCustomizer">Get Started</button>  -->
      <!--create-new-category :active=true></create-new-category-->
      <side-bar></side-bar>
      <!-- <edit-category :active="true"></edit-category> -->
     <!--  <remove-category :active="true"></remove-category> -->
    </div>
</template>

<script>
import Customizer from "./components/Customizer.vue";
import CreateNewCategory from "./components/management/category/CreateNewCategory.vue";
import EditCategory from "./components/management/category/EditCategory.vue";
import RemoveCategory from "./components/management/category/RemoveCategory.vue";
import SideBar from "./components/SideBar.vue";
export default {
  name: "app",
  data() {
    return {
      customizerEnabled: false
    };
  },
  methods: {
    enableCustomizer() {
      this.customizerEnabled = true;
    }
  },
  components: {
    Customizer,
    CreateNewCategory,
    SideBar ,
    EditCategory,
    RemoveCategory
  }
};
</script> 