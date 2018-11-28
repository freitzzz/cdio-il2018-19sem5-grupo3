<template>
  <div>
    <div class="icon-div-top"><i class="material-icons md-12 md-blue btn">help</i>
      <span class="tooltiptext">In this step, you can add components to the structure.</span>
    </div>
    <div class="text-entry">
      Choose components to add:
    </div>
    <div class="scrollable-div">
    </div>
  </div>
</template>


<script>
import store from "./../store";
import { SET_CUSTOMIZED_PRODUCT_COMPONENTS } from "./../store/mutation-types.js";

import { error } from "three";

export default {
  name: "CustomizerSideBarComponentsPanel",
  created() {
    // store.dispatch(SET_CUSTOMIZED_PRODUCT_COMPONENTS, {
    //   components: this.components
    // });
  },
  data() {

  },
  methods: {}
};
</script>

<style>
.icon-div-top .tooltiptext {
  visibility: hidden;
  width: 100px;
  background-color: #797979;
  color: #fff;
  border-radius: 6px;
  font-size: 12px;
  padding: 10%;
  position: absolute;
  top: 25px;
  left: 0px;
  right: 0px;
}

.icon-div-top:hover .tooltiptext {
  visibility: visible;
}

.icon-div-top {
  top: 15px;
  left: 15px;
  margin-left: 130px;
  position: absolute;
}
</style>

