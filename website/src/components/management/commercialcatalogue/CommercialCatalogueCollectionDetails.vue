<template>
  <div class="modal-card" style="width:auto">
    <header class="modal-card-head">
      <p class="modal-card-title">{{collectionName}}</p>
    </header>
    <section class="modal-card-body">
      <b-field label="Customized Products"/>

      <b-table
        :data="availableCustomizedProducts"
        :checkable="editable"
        :checked-rows.sync="selectedCustomizedProducts"
        :paginated="true"
        :pagination-simple="true"
        per-page="5"
      >
        <template slot-scope="props">
          <b-table-column label="ID">{{props.row.id}}</b-table-column>
          <b-table-column
            label="Serial Number/Reference"
          >{{getCustomizedProductBusinessIdentifier(props.row)}}</b-table-column>
        </template>
      </b-table>
    </section>
    <footer v-if="editable" class="modal-card-foot">
      <button class="btn-primary" @click="$parent.close">OK</button>
    </footer>
  </div>
</template>

<script>
import CommercialCatalogueRequests from "./../../../services/mycm_api/requests/commercialcatalogues.js";
import CustomizedProductCollectionRequests from "./../../../services/mycm_api/requests/customizedproductcollections.js";
export default {
  name: "CommercialCatalogueCollectionDetails",
  data() {
    return {
      availableCustomizedProducts: [],
      selectedCustomizedProducts: []
    };
  },
  props: {
    editable: Boolean,
    commercialCatalogueId: Number,
    collectionId: Number,
    collectionName: String
  },
  watch: {
    /**
     * Watches for changes in selectedCustomizedProducts and performs the appropriate requests.
     * @param {Array<object>} newVal
     * @param {Array<object>} oldVal
     */
    selectedCustomizedProducts(newVal, oldVal) {
      const oldSelectedCustomizedProductIds = oldVal.map(
        customizedProduct => customizedProduct.id
      );
      const newSelectedCustomizedProductIds = newVal.map(
        customizedProduct => customizedProduct.id
      );

      oldSelectedCustomizedProductIds.forEach(oldId => {
        if (!newSelectedCustomizedProductIds.includes(oldId)) {
          CommercialCatalogueRequests.deleteCommercialCatalogueCollectionCustomizedProduct(
            this.commercialCatalogueId,
            this.collectionId,
            oldId
          ).catch(error => {
            this.$toast.open(error.response.data);
          });
        }
      });

      newSelectedCustomizedProductIds.forEach(newId => {
        if (!oldSelectedCustomizedProductIds.includes(newId)) {
          const postBody = { id: newId };

          CommercialCatalogueRequests.postCommercialCatalogueCollectionCustomizedProduct(
            this.commercialCatalogueId,
            this.collectionId,
            postBody
          ).catch(error => {
            this.$toast.open(error.response.data);
          });
        }
      });
    }
  },
  methods: {
    /**
     * Retrieves the CustomizedProduct's business identifier.
     * @param {object} customizedProduct
     */
    getCustomizedProductBusinessIdentifier(customizedProduct) {
      if (customizedProduct.reference !== undefined) {
        return customizedProduct.reference;
      }

      if (customizedProduct.serialNumber !== undefined) {
        return customizedProduct.serialNumber;
      }
    },
    /**
     * Retrieves the CustomizedProductCollection.
     */
    getCustomizedProductCollection() {
      return CustomizedProductCollectionRequests.getCustomizedProductCollection(
        this.collectionId
      );
    },
    /**
     * Retrieves the CommercialCatalogue's collection.
     */
    getCommercialCatalogueCollection() {
      return CommercialCatalogueRequests.getCommercialCatalogueCollection(
        this.commercialCatalogueId,
        this.collectionId
      );
    },
    /**
     *
     */
    fillTable() {
      this.getCommercialCatalogueCollection()
        .then(catalogueCollectionResponse => {
          const { data: catalogueCollection } = catalogueCollectionResponse;

          var catalogueCollectionProducts =
            catalogueCollection.customizedProducts !== undefined
              ? catalogueCollection.customizedProducts
              : [];

          if (this.editable) {
            this.getCustomizedProductCollection().then(collectionResponse => {
              const { data: collection } = collectionResponse;

              var collectionProducts =
                collection.customizedProducts !== undefined
                  ? collection.customizedProducts
                  : [];

              const numCatalogueCollectionProducts =
                catalogueCollectionProducts.length;
              const numCollectionProducts = collectionProducts.length;

              for (let i = 0; i < numCollectionProducts; i++) {
                for (let j = 0; j < numCatalogueCollectionProducts; j++) {
                  if (
                    collectionProducts[i].id ===
                    catalogueCollectionProducts[j].id
                  ) {
                    this.selectedCustomizedProducts.push(collectionProducts[i]);
                  }
                }
                this.availableCustomizedProducts.push(collectionProducts[i]);
              }
            });
          } else {
            this.availableCustomizedProducts.push(
              ...catalogueCollectionProducts
            );
          }
        })
        .catch(error => {
          //this should only occur if the catalogue collection was deleted immediately after selecting it
          this.$toast.open(error.response.data);
          this.$parent.close();
        });
    }
  },
  created() {
    this.fillTable();
  }
};
</script>