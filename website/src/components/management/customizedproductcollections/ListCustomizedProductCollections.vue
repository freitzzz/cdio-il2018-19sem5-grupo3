<template>
  <div>
    <div>
      <button class="btn-primary" @click="createNewCollection()">
        <b-icon icon="plus"/>
      </button>
      <div v-if="createNewCollectionModal">
        <b-modal :active.sync="createNewCollectionModal" has-modal-card scroll="keep">
          <create-new-customized-product-collection
            :active="createNewCollectionModal"
            :available-customized-products="toCustomizedSelectedCustomizedProducts(availableCustomizedProducts)"
            @emitCustomizedProductCollection="postCustomizedProductCollection"
          />
        </b-modal>
      </div>
      <button class="btn-primary" @click="fetchRequests()">
        <b-icon icon="refresh" custom-class="fa-spin"/>
      </button>
    </div>
    <customized-product-collections-table :data="data"
        @refreshData="refreshCollections()"/>
  </div>
</template>

<script>
import CreateNewCustomizedProductCollection from "./CreateNewCustomizedProductCollection.vue";
import CustomizedProductCollectionsTable from "./CustomizedProductCollectionsTable.vue";
import Axios from "axios";
import Config, { MYCM_API_URL } from "../../../config.js";

let customizedProducts = [];

export default {
  components: {
    CreateNewCustomizedProductCollection,
    CustomizedProductCollectionsTable
  },
  created() {
    this.fetchRequests();
  },
  data() {
    return {
      createNewCollectionModal: false,
      customizedProductCollection: null,
      customizedProductCollectionClone: null,
      currentSelectedCustomizedProductCollection: 0,
      availableCollections: Array,
      availableCustomizedProducts: customizedProducts,
      columns: [],
      data: Array,
      total: Number,
      failedToFetchCollectionsNotification: false
    };
  },
  methods: {
    fetchRequests() {
      this.refreshCollections();
      this.fetchAvailableCustomizedProducts();
    },
    refreshCollections() {
      Axios.get(MYCM_API_URL + "/collections")
        .then(response => {
          this.data = this.generateCollectionsTableData(response.data);
          this.columns = this.generateCollectionsTableColumns();
          this.total = this.data.length;
        })
        .catch(error_message => {
          this.$toast.open({ message: error_message.response.data.message });
        });
    },
    fetchAvailableCustomizedProducts() {
      Axios.get(MYCM_API_URL + "/customizedproducts")
        .then(response => {
          let availableCustomizedProducts = response.data;
          availableCustomizedProducts.forEach(customizedProduct => {
            customizedProducts.push({
              id: customizedProduct.id,
              productId: customizedProduct.productId,
              serialNumber: customizedProduct.serialNumber
            });
          });
        })
        .catch(error_message => {
          this.$toast.open({ message: error_message.response.data.message });
        });
    },
    createNewCollection() {
      this.createNewCollectionModal = true;
    },
    postCustomizedProductCollection(collectionDetails) {
      let newCollection = {};
      newCollection.name = collectionDetails.name;
      newCollection.customizedProducts = collectionDetails.customizedProducts;

      if (collectionDetails.customizedProducts != null) {
        let newCollectionCustomizedProducts = [];
        for (let i = 0; i < collectionDetails.customizedProducts.length; i++) {
          newCollectionCustomizedProducts.push({
            id: collectionDetails.customizedProducts[i]
          });
        }
        newCollection.customizedProducts = newCollectionCustomizedProducts;
      }

      Axios.post(MYCM_API_URL + "/collections", newCollection)
        .then(response => {
          this.$toast.open({
            message: "The collection was created successfully!"
          });
          this.createNewCollectionModal = false;
          this.fetchRequests();
        })
        .catch(error_message => {
          this.$toast.open({ message: error_message.response.data.message });
        });
    },
    generateCollectionsTableColumns() {
      return [
        {
          field: "id",
          label: "ID",
          centered: true
        },
        {
          field: "name",
          label: "Name",
          centered: true
        },
        {
          field: "hasCustomizedProducts",
          label: "Customized Products",
          centered: true
        },
        {
          field: "actions",
          label: "Actions",
          centered: true
        }
      ];
    },
    generateCollectionsTableData(collections) {
      let collectionsTableData = [];
      collections.forEach(collection => {
        collectionsTableData.push({
          id: collection.id,
          name: collection.name,
          hasCustomizedProducts: collection.hasCustomizedProducts
        });
      });
      return collectionsTableData;
    },
    changeSelectedCollection(tableRow) {
      tihs.currentSelectedCustomizedProductCollection = tableRow.id;
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
