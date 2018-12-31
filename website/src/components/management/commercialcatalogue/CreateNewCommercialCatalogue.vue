<template>
  <div class="modal-card" style="width:auto">
    <header class="modal-card-head">
      <p class="modal-card-title">New Commercial Catalogue</p>
    </header>
    <section class="modal-card-body">
      <b-field label="Reference">
        <b-input type="String" placeholder="Catalogue's Reference" required v-model="reference"/>
      </b-field>
      <b-field label="Designation">
        <b-input
          type="String"
          placeholder="Catalogue's Designation"
          required
          v-model="designation"
        />
      </b-field>
      <div v-if="collectionsAvailable">
        <b-field label="Collections">
          <b-autocomplete
            rounded
            v-model="searchedCollection"
            :keep-first="true"
            :data="suggestedCollections"
            field="name"
            :clear-on-select="true"
            @select="option => selectCollection(option)"
            placeholder="e.g. Winter 2018"
            icon="magnify"
          >
            <template slot="empty">No collections found</template>
          </b-autocomplete>
        </b-field>

        <!--Prevents the auto complete prompt from overlapping the checkboxes-->
        <div v-if="isInputtingData" class="expandable-div"></div>

        <b-table
          :data="collections"
          :checked-rows.sync="selectedCollections"
          :paginated="true"
          :pagination-simple="true"
          checkable
          per-page="5"
        >
          <template slot-scope="props">
            <b-table-column label="Name">{{props.row.name}}</b-table-column>
            <b-table-column>
              <button class="btn-primary">
                <b-icon icon="pencil"></b-icon>
              </button>
            </b-table-column>
          </template>
        </b-table>
      </div>
      <footer class="modal-card-foot">
        <button class="btn-primary" @click="createCatalogue">Create</button>
      </footer>
    </section>
  </div>
</template>

<script>
import CustomizedProductCollectionsRequests from "./../../../services/mycm_api/requests/customizedproductcollections.js";
const CREATE_CATALOGUE_EVENT = "createCatalogue";
export default {
  name: "CreateNewCommercialCatalogue",
  data() {
    return {
      reference: null,
      designation: null,
      searchedCollection: "",
      collectionsAvailable: false, //boolean used as flag for rendering the autocomplete box and checkboxes
      selectedCollections: [],
      collections: []
    };
  },
  methods: {
    /**
     * Emits an event so that the parent can create the Catalogue.
     */
    createCatalogue() {
      var catalogue = {
        reference: this.reference,
        designation: this.designation,
        collections: this.buildCollectionArray()
      };

      this.$emit(CREATE_CATALOGUE_EVENT, catalogue);
    },

    /**
     * Builds an array of collectionId from the selectedCollections Object.
     */
    buildCollectionArray() {
      var collectionArray = [];

      for (let i = 0; i < this.selectedCollections.length; i++) {
        var collection = { collectionId: this.selectedCollections[i].id };
        collectionArray.push(collection);
      }

      return collectionArray;
    },

    /**
     * Selects an option.
     */
    selectCollection(option) {
      var alreadyAdded = false;

      //check it the collection was already added
      for (let i = 0; i < this.selectedCollections.length; i++) {
        if (this.selectedCollections[i].id == option.id) {
          alreadyAdded = true;
          break;
        }
      }

      //don't push more than once
      if (!alreadyAdded) {
        this.selectedCollections.push(option);
      }
    },

    /**
     * Retrieves all of the available collections.
     */
    getAvailableCollections() {
      CustomizedProductCollectionsRequests.getCustomizedProductCollections()
        .then(response => {
          this.collectionsAvailable = true;
          this.collections.push(...response.data);
        })
        .catch(error => {
          this.$toast.open(error.response.data);
        });
    }
  },
  computed: {
    /**
     * Computes the Collections with references matching the pattern inserted in the search box.
     */
    suggestedCollections() {
      var suggestedCollections = [];

      //the "i" flag makes the pattern case insensitive
      var patt = new RegExp(`^.*(${this.searchedCollection}).*$`, "i");

      for (let i = 0; i < this.collections.length; i++) {
        var match = patt.test(this.collections[i].name);
        if (match) {
          suggestedCollections.push(this.collections[i]);
        }
      }

      return suggestedCollections;
    },

    isInputtingData() {
      return this.searchedCollection.length > 0;
    }
  },
  created() {
    this.getAvailableCollections();
  }
};
</script>

<style>
.scrollable-checkboxes {
  max-height: 200px;
  overflow-y: auto;
}

.expandable-div {
  padding-bottom: 25%;
}
</style>

