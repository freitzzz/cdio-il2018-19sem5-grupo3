<template>
  <div>
    <div>
      <button class="btn-primary" @click="enableCatalogueModal">
        <b-icon icon="plus"/>
      </button>
      <button class="btn-primary">
        <b-icon icon="refresh"/>
      </button>
    </div>
    <!--The v-if is used for disabling the modal after clicking on the create button-->
    <div v-if="modalEnabled">
      <b-modal :active="modalEnabled" has-modal-card scroll="keep" :onCancel="confirmClose">
        <create-new-commercial-catalogue @createCatalogue="createCatalogue"/>
      </b-modal>
    </div>
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
     * Retrieves all of the available CommercialCatalogues.
     */
    getCatalogues() {
      CommercialCatalogueRequests.getCommercialCatalogues()
        .then(response => {
          this.data.push(...response.data);
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

          var addedCatalogue = {
            id: response.data.id,
            reference: response.data.reference,
            designation: response.data.designation
          };
          //push added catalogue to data so that tables can be updated
          this.data.push(addedCatalogue);
        })
        .catch(error => {
          this.$toast.open(error.response.data);
        });
    },

    /**
     * Displays a dialog in order to confirm closing the Commercial Catalogue creation modal.
     */
    confirmClose() {
      this.$dialog.confirm({
        title: "Confirm Close",
        message: `Are you sure you want exit?`,
        cancelText: "Cancel",
        confirmText: "OK",
        type: "is-info",
        onConfirm: () => (this.modalEnabled = false),
        onCancel: () => (this.modalEnabled = true)
      });
    }
  },
  created() {
    this.getCatalogues();
  }
};
</script>
