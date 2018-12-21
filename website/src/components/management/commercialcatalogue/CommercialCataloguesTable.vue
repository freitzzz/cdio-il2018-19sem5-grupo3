<template>
  <div>
    <vuetable :api-mode="false" :data="data" :fields="columns">
      <template slot="actions" slot-scope="props">
        <div class="custom-actions">
          <button class="btn-primary" @click="displayCommercialCatalogue(props.rowData.id)">
            <b-icon icon="magnify"/>
          </button>
          <button class="btn-primary" @click="editCommercialCatalogue(props.rowData.id)">
            <b-icon icon="pencil"/>
          </button>
          <button class="btn-primary" @click="deleteCommercialCatalogue(props.rowData.id)">
            <b-icon icon="minus"/>
          </button>
        </div>
      </template>
    </vuetable>
  </div>
</template>

<script>
import CommercialCatalogueRequests from "./../../../services/mycm_api/requests/commercialcatalogues.js";
export default {
  name: "CommercialCataloguesTable",
  data() {
    return {
      columns: [
        {
          name: "id",
          title: "ID"
        },
        {
          name: "reference",
          title: "Reference"
        },
        {
          name: "designation",
          title: "Designation"
        },
        {
          name: "__slot:actions",
          title: "Actions"
        }
      ]
    };
  },
  props: {
    data: []
  },
  watch: {
    /**
     * Watches for changes in the data prop and updates the table.
     */
    data(newVal) {
      this.data = newVal;
    }
  },
  methods: {
    /**
     *
     */
    displayCommercialCatalogueDetails(commercialCatalogueId) {
      CommercialCatalogueRequests.getCommercialCatalogue(commercialCatalogueId)
        .then()
        .catch();
    },

    /**
     * Deletes a CommercialCatalogue.
     */
    deleteCommercialCatalogue(commercialCatalogueId) {
      CommercialCatalogueRequests.deleteCommercialCatalogue(
        commercialCatalogueId
      )
        .then(() => {
          this.deleteTableEntry(commercialCatalogueId);
          this.$toast.open({ message: "Catalogue was deleted successfully!" });
        })
        .catch(error => {
          //delete the entry in the table even if the api call fails
          this.deleteTableEntry(commercialCatalogueId);
          this.$toast.open(error.response.data.message);
        });
    },

    /**
     * Deletes an entry from the CommercialCatalogue table with a matching identifier.
     */
    deleteTableEntry(commercialCatalogueId) {
      //remove the element in the array with the matching id
      for (let i = 0; i < this.data.length; i++) {
        if (this.data[i].id == commercialCatalogueId) {
          this.data.splice(i, 1);
        }
      }
    }
  },
  created() {
    this.getCatalogues();
  }
};
</script>
