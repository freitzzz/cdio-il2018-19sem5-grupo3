<template>
  <div class="modal-card" style="width: auto">
    <header class="modal-card-head">
      <p class="modal-card-title">New Collection</p>
    </header>
    <section class="modal-card-body">
      <b-field label="Name">
        <b-input
          type="String"
          v-model="nameItem.value"
          :placeholder="placeholders.name"
          icon="pound"
          required
        ></b-input>
      </b-field>
      <b-checkbox type="is-info" @input="enableCustomizedProducts()">Customized Products</b-checkbox>
      <div v-if="customizedProducts">
        <customized-selected-items
          :available-items="availableCustomizedProducts"
          :customized-label="customizedProductsItems.customizedLabel"
          :icon="customizedProductsItems.icon"
          :place-holder="customizedProductsItems.placeHolder"
          @emitItems="changeCurrentCustomizedProducts"
        />
      </div>
    </section>
    <footer class="modal-card-foot">
      <div class="has-text-centered">
        <button class="btn-primary" @click="emitCustomizedProductCollection($parent)">Create</button>
      </div>
    </footer>
  </div>
</template>

<script>
import CustomizedSelectedItems from "../../UIComponents/CustomizedSelectedItems";

export default {
  components: {
    CustomizedSelectedItems
  },
  props: {
    availableCustomizedProducts: {
      type: Array,
      required: true
    },
    active: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      nameItem: {
        value: null
      },
      customizedProductsItem: {
        value: null
      },
      customizedProductsItems: {
        availableItems: [],
        customizedLabel: "Customized Products",
        icon: "buffer",
        placeHolder: "Select a Customized Product"
      },
      placeholders: {
        name: "Collection Name"
      },
      customizedProducts: false
    };
  },
  methods: {
    changeCurrentCustomizedProducts(customizedProducts) {
      this.customizedProductsItem.value = customizedProducts;
    },
    emitCustomizedProductCollection(modal) {
      let collectionDetails = {
        name: this.nameItem.value,
        customizedProducts: this.customizedProductsItem.value
      };
      this.$emit("emitCustomizedProductCollection", collectionDetails);
    },
    enableCustomizedProducts() {
      this.customizedProducts = !this.customizedProducts;
    }
  }
};
</script>
