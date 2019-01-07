<template>
  <div>
    <div>
      <button class="btn-primary" @click="enableCatalogueModal">
        <b-icon icon="plus"/>
      </button>
      <button class="btn-primary" @click="getCatalogues()">
        <b-icon icon="refresh"/>
      </button>
    </div>

    <create-new-commercial-catalogue
      v-if="modalEnabled"
      @createCatalogue="createCatalogue"
      @closeModal="closeCatalogueModal"
    />

    <commercial-catalogues-table :data="data"/>
  </div>
</template>


<script>
import CommercialCataloguesTable from "./CommercialCataloguesTable.vue";
import CreateNewCommercialCatalogue from "./CreateNewCommercialCatalogue.vue";
import CommercialCatalogueRequests from "./../../../services/mycm_api/requests/commercialcatalogues.js";

export default {
  name: "ListCommercialCatalogues",
  data() {
    return {
      modalEnabled: false,
      data: []
    };
  },
  components: {
    CommercialCataloguesTable,
    CreateNewCommercialCatalogue
  },
  methods: {
    /**
     * Enables the catalogue creation modal.
     */
    enableCatalogueModal() {
      this.modalEnabled = true;
    },

    /**
     * Disables the catalogue creation modal.
     */
    closeCatalogueModal() {
      this.modalEnabled = false;
    },

    /**
     * Retrieves all of the available CommercialCatalogues.
     */
    getCatalogues() {
      CommercialCatalogueRequests.getCommercialCatalogues()
        .then(response => {
          this.data = response.data;
        })
        .catch(error => {
          error.response.data.message;
        });
    },

    /**
     * Creates a new CommercialCatalogue.
     */
    createCatalogue(commercialCatalogue) {
      CommercialCatalogueRequests.postCommercialCatalogue(commercialCatalogue)
        .then(response => {
          this.modalEnabled = false;
          this.$toast.open({
            message: "Commercial Catalogue created succesfully!"
          });

          var hasCollections = false;

          if (
            response.data.commercialCatalogueCollections !== undefined &&
            response.data.commercialCatalogueCollections.length > 0
          ) {
            hasCollections = true;
          }

          var addedCatalogue = {
            id: response.data.id,
            reference: response.data.reference,
            designation: response.data.designation,
            hasCollections
          };
          //push added catalogue to data so that tables can be updated
          this.data.push(addedCatalogue);
        })
        .catch(error => {
          this.$toast.open(error.response.data);
        });
    }
  },
  created() {
    this.getCatalogues();
  }
};
</script>
