<template>
  <div class="modal-card" style="width:auto">
    <header class="modal-card-head">
      <p class="modal-card-title">Commercial Catalogue Details</p>
    </header>
    <section class="modal-card-body">
      <b-field label="Reference">
        <b-input type="String" :disabled="!editable" v-model="reference" placeholder="No reference"></b-input>
      </b-field>
      <b-field label="Designation">
        <b-input
          type="String"
          :disabled="!editable"
          v-model="designation"
          placeholder="No designation"
        ></b-input>
      </b-field>
      <div v-if="availableCollections.length > 0">
        <b-field label="Collections"></b-field>

        <b-table
          :data="availableCollections"
          :checkable="editable"
          :checked-rows.sync="selectedCollections"
          :paginated="true"
          :pagination-simple="true"
          per-page="5"
        >
          <template slot-scope="props">
            <b-table-column label="Name">{{props.row.name}}</b-table-column>
            <b-table-column v-if="editable">
              <button class="btn-primary">
                <b-icon icon="pencil"/>
              </button>
            </b-table-column>
          </template>
        </b-table>
      </div>
      <footer v-if="editable" class="modal-card-foot">
        <button class="btn-primary" @click="updateCatalogue()">Update</button>
      </footer>
    </section>
  </div>
</template>

<script>
import CustomizedProductCollectionsRequests from "./../../../services/mycm_api/requests/customizedproductcollections.js";
import CommercialCatalogueRequests from "./../../../services/mycm_api/requests/commercialcatalogues.js";
const UPDATE_CATALOGUE_EVENT = "updateCatalogue";
export default {
  name: "CommercialCatalogueDetails",
  data() {
    return {
      reference: this.commercialCatalogue.reference,
      designation: this.commercialCatalogue.designation,
      availableCollections: [],
      selectedCollections: []
    };
  },
  props: {
    commercialCatalogue: {},
    editable: Boolean
  },
  watch: {
    /**
     * Watches for changes in the selectedCollections and performs the appropriate requests.
     */
    selectedCollections(newVal, oldVal) {
      const oldSelectedCollectionIds = oldVal.map(collection => collection.id);
      const newSelectedCollectionIds = newVal.map(collection => collection.id);

      //if oldVal has collections not included in newVal, delete them
      oldSelectedCollectionIds.forEach(oldId => {
        if (!newSelectedCollectionIds.includes(oldId)) {
          CommercialCatalogueRequests.deleteCommercialCatalogueCollection(
            this.commercialCatalogue.id,
            oldId
          );
        }
      });

      //if newVal has collections not included in oldVal, add them
      newSelectedCollectionIds.forEach(newId => {
        if (!oldSelectedCollectionIds.includes(newId)) {
          const postBody = { collectionId: newId };

          CommercialCatalogueRequests.postCommercialCatalogueCollection(
            this.commercialCatalogue.id,
            postBody
          );
        }
      });
    }
  },
  methods: {
    updateCatalogue() {
      //check if any of the basic properties changed
      if (
        this.commercialCatalogue.reference !== this.reference ||
        this.commercialCatalogue.designation !== this.designation
      ) {
        var putBody = {};

        if (this.commercialCatalogue.reference !== this.reference) {
          putBody.reference = this.reference;
        }

        if (this.commercialCatalogue.designation !== this.designation) {
          putBody.designation = this.designation;
        }

        CommercialCatalogueRequests.putCommercialCatalogue(
          this.commercialCatalogue.id,
          putBody
        )
          .then(response => {
            this.$emit(
              UPDATE_CATALOGUE_EVENT,
              response.data.id,
              response.data.reference,
              response.data.designation
            );
          })
          .catch(error => {
            this.$toast.open(error.response.data);
          });
      } else {
        //if none of the basic properties changed, emit the update event with the original data
        this.$emit(
          UPDATE_CATALOGUE_EVENT,
          this.commercialCatalogue.id,
          this.commercialCatalogue.reference,
          this.commercialCatalogue.designation
        );
      }
    },
    getAvailableCollections() {
      CustomizedProductCollectionsRequests.getCustomizedProductCollections().then(
        response => {
          var collectionIds = this.availableCollections.map(
            collection => collection.id
          );

          var additionalCollections = response.data.filter(
            collection => !collectionIds.includes(collection.id)
          );

          this.availableCollections.push(...additionalCollections);
        }
      );
    }
  },
  created() {
    if (this.commercialCatalogue.commercialCatalogueCollections !== undefined) {
      this.availableCollections.push(
        ...this.commercialCatalogue.commercialCatalogueCollections
      );

      //don't push data to the selected collections unless it's editable
      if (this.editable) {
        this.selectedCollections.push(
          ...this.commercialCatalogue.commercialCatalogueCollections
        );
      }
    }

    if (this.editable) {
      this.getAvailableCollections();
    }
  }
};
</script>
