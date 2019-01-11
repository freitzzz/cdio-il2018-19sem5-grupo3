<template>
  <div class="modal-card" style="width: auto">
    <header class="modal-card-head">
      <p class="modal-card-title">Edit Collection</p>
    </header>
    <section class="modal-card-body">
      <b-field label="Name">
        <b-input type="String" v-model="collection.name" icon="pound" required/>
      </b-field>
      <b-checkbox type="is-info" @input="enableCustomizedProducts()">Customized Products</b-checkbox>
      <div v-if="customizedProducts">
        <customized-selected-items
          :added-items="toCustomizedSelectedCustomizedProducts(collection.customizedProducts ? collection.customizedProducts : [])"
          :available-items="toCustomizedSelectedCustomizedProducts(availableCustomizedProducts)"
          :customized-label="customizedProductsItems.customizedLabel"
          :icon="customizedProductsItems.icon"
          :place-holder="customizedProductsItems.placeholder"
          @emitItems="changeCurrentCustomizedProducts"
        />
      </div>
    </section>
    <footer class="modal-card-foot">
      <div class="has-text-centered">
        <button class="btn-primary" @click="emitCollection($parent)">Save</button>
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
    collection: {
      type: Object,
      required: true
    }
  },
  data() {
    return {
      customizedProducts: false,
      customizedProductsItem: {
        value: null
      },
      customizedProductsItems: {
        availableCustomizedProducts: [],
        customizedLabel: "Customized Products",
        icon: "buffer",
        placeHolder: "Select a Customized Product"
      }
    };
  },
  methods: {
    changeCurrentCustomizedProducts(customizedProducts) {
      this.customizedProductsItem.value = customizedProducts;
    },
    emitCollection(modal) {
      let collectionDetails = {
        id: this.collection.id,
        name: this.collection.name,
        customizedProducts: this.customizedProductsItem.value
      };
      this.$emit("emitCollection", collectionDetails);
    },
    enableCustomizedProducts() {
      this.customizedProducts = !this.customizedProducts;
    },
    toCustomizedSelectedCustomizedProducts(customizedProducts) {
      let customizedSelectedCustomizedProducts = [];
      for (let i = 0; i < customizedProducts.length; i++) {
        customizedSelectedCustomizedProducts.push({
          id: customizedProducts[i].id,
          value: customizedProducts[i].id
        });
      }
      return customizedSelectedCustomizedProducts;
    }
  }
};
</script>

