<template>
  <b-modal :active="true" :onCancel="closeModal" has-modal-card scroll="keep">
    <div class="modal-card" style="width:auto">
      <header class="modal-card-head">
        <p class="modal-card-title">Commercial Catalogue Details</p>
      </header>
      <section class="modal-card-body">
        <b-field label="Reference">
          <b-input
            type="String"
            :disabled="!editable"
            required
            v-model="reference"
            placeholder="No reference"
          ></b-input>
        </b-field>
        <b-field label="Designation">
          <b-input
            type="String"
            :disabled="!editable"
            required
            v-model="designation"
            placeholder="No designation"
          ></b-input>
        </b-field>
        <div v-if="availableCollections.length > 0">
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
          <div v-if="isInputtingData" class="expandable-div"/>

          <b-table
            :data="availableCollections"
            :checkable="editable"
            :checked-rows.sync="selectedCollections"
            :paginated="true"
            :pagination-simple="true"
            per-page="5"
          >
            <template slot-scope="props">
              <b-table-column label="ID">{{props.row.id}}</b-table-column>
              <b-table-column label="Name">{{props.row.name}}</b-table-column>
              <!--Only display the edit button if the editable props is set and the collection has products-->
              <b-table-column v-if="editable && props.row.hasCustomizedProducts">
                <!--Conditionally bind class to the button-->
                <button
                  :class="[selectedCollections.map(collection => collection.id).includes(props.row.id) ? 'btn-primary' : 'btn-primary-disabled']"
                  :disabled="!selectedCollections.map(collection => collection.id).includes(props.row.id)"
                  @click="enableCollectionDetails(props.row.id, props.row.name)"
                >
                  <b-icon icon="pencil"/>
                </button>
              </b-table-column>
              <!--If the editable prop is not set, but the collection has customized products display the details button-->
              <b-table-column v-else-if="!editable && props.row.hasCustomizedProducts">
                <button
                  class="btn-primary"
                  @click="enableCollectionDetails(props.row.id, props.row.name)"
                >
                  <b-icon icon="magnify"/>
                </button>
              </b-table-column>
            </template>
          </b-table>
        </div>
        <footer v-if="editable" class="modal-card-foot">
          <button class="btn-primary" @click="updateCatalogue()">Update</button>
        </footer>
      </section>

      <b-modal :active.sync="displayCollectionDetails" has-modal-card scroll="keep">
        <commercial-catalogue-collection-details
          :editable="editable"
          :commercialCatalogueId="commercialCatalogueId"
          :collectionId="selectedCollectionId"
          :collectionName="selectedCollectionName"
        />
      </b-modal>
    </div>
  </b-modal>
</template>

<script>
import CommercialCatalogueCollectionDetails from "./CommercialCatalogueCollectionDetails";
import CustomizedProductCollectionsRequests from "./../../../services/mycm_api/requests/customizedproductcollections.js";
import CommercialCatalogueRequests from "./../../../services/mycm_api/requests/commercialcatalogues.js";

const UPDATE_TABLE_ENTRY_EVENT = "updateTableEntry";
const CLOSE_MODAL_EVENT = "closeModal";

export default {
  name: "CommercialCatalogueDetails",
  components: {
    CommercialCatalogueCollectionDetails
  },
  data() {
    return {
      reference: "",
      designation: "",
      searchedCollection: "",
      availableCollections: [],
      selectedCollections: [],
      displayCollectionDetails: false,
      selectedCollectionId: 0,
      selectedCollectionName: ""
    };
  },
  props: {
    commercialCatalogueId: {
      type: Number,
      required: true,
      default: 0
    },
    editable: {
      type: Boolean,
      required: true,
      default: false
    }
  },
  computed: {
    suggestedCollections() {
      var suggestedCollections = [];

      //the "i" flag makes the pattern case insensitive
      var patt = new RegExp(`^.*(${this.searchedCollection}).*$`, "i");

      const numAvailableCollections = this.availableCollections.length;
      for (let i = 0; i < numAvailableCollections; i++) {
        var match = patt.test(this.availableCollections[i].name);
        if (match) {
          suggestedCollections.push(this.availableCollections[i]);
        }
      }

      return suggestedCollections;
    },
    isInputtingData() {
      return this.searchedCollection.length > 0;
    }
  },
  watch: {
    /**
     * Watches for changes in the selectedCollections and performs the appropriate requests.
     * @param {Array} newVal
     * @param {Array} oldVal
     */
    selectedCollections(newVal, oldVal) {
      const oldSelectedCollectionIds = oldVal.map(collection => collection.id);
      const newSelectedCollectionIds = newVal.map(collection => collection.id);

      //if oldVal has collections not included in newVal, delete them
      oldSelectedCollectionIds.forEach(oldId => {
        if (!newSelectedCollectionIds.includes(oldId)) {
          CommercialCatalogueRequests.deleteCommercialCatalogueCollection(
            this.commercialCatalogueId,
            oldId
          )
            .then(() => {
              //only emit the update table event if the request resolved
              if (newVal.length == 0) {
                this.$emit(
                  UPDATE_TABLE_ENTRY_EVENT,
                  this.commercialCatalogueId,
                  this.reference,
                  this.designation,
                  false,
                  true
                );
              }
            })
            .catch(error => {
              this.$toast.open(error.response.data);
            });
        }
      });

      //if newVal has collections not included in oldVal, add them
      newSelectedCollectionIds.forEach(newId => {
        if (!oldSelectedCollectionIds.includes(newId)) {
          const postBody = { collectionId: newId };

          CommercialCatalogueRequests.postCommercialCatalogueCollection(
            this.commercialCatalogueId,
            postBody
          )
            .then(() => {
              //only emit the update table event if the request resolved
              if (oldVal.length == 0 && newVal.length > 0) {
                this.$emit(
                  UPDATE_TABLE_ENTRY_EVENT,
                  this.commercialCatalogueId,
                  this.reference,
                  this.designation,
                  true,
                  true
                );
              }
            })
            .catch(error => {
              this.$toast.open(error.response.data);
            });
        }
      });
    }
  },
  methods: {
    //? should there be a watcher for basic properties?
    /**
     * Updates the catalogue's basic properties and emits the changes back to the table.
     */
    async updateCatalogue() {
      var putBody = {
        reference: this.reference,
        designation: this.designation
      };

      CommercialCatalogueRequests.putCommercialCatalogue(
        this.commercialCatalogueId,
        putBody
      )
        .then(response => {
          var hasCollections = false;

          if (
            response.data.commercialCatalogueCollections !== undefined &&
            response.data.commercialCatalogueCollections.length > 0
          ) {
            hasCollections = true;
          }

          this.$emit(
            UPDATE_TABLE_ENTRY_EVENT,
            response.data.id,
            response.data.reference,
            response.data.designation,
            hasCollections,
            false
          );
        })
        .catch(error => {
          this.$toast.open(error.response.data);
        });
    },

    closeModal() {
      this.$emit(CLOSE_MODAL_EVENT);
    },
    //TODO: fix selection
    selectCollection(collection) {
      if (this.editable) {
        const { id: collectionId } = collection;
        var collectionFound = false;
        var alreadyAdded = false;
        const numAvailableCollections = this.availableCollections.length;
        var selectedIndex = 0;

        for (let i = 0; i < numAvailableCollections; i++) {
          if (this.availableCollections[i].id === collection.id) {
            if (
              this.selectedCollections
                .map(collection => collection.id)
                .includes(collectionId)
            ) {
              alreadyAdded = true;
            } else {
              collectionFound = true;
              selectedIndex = i;
            }
            break;
          }
        }

        if (!alreadyAdded && collectionFound) {
          this.selectedCollections.push(
            this.availableCollections[selectedIndex]
          );
        }
      }
    },
    /**
     * Enables the collection details modal.
     * @param {number} collectionId
     * @param {string} collectionName
     */
    enableCollectionDetails(collectionId, collectionName) {
      this.selectedCollectionId = collectionId;
      this.selectedCollectionName = collectionName;
      this.displayCollectionDetails = true;
    },

    getCommercialCatalogue() {
      return CommercialCatalogueRequests.getCommercialCatalogue(
        this.commercialCatalogueId
      );
    },

    /**
     * Fills the collections table.
     * If the editable prop is set to true, then the table is filled with all available collections and the those currently added to the catalogue are checked.
     * If the editable prop is set to false, the the table is filled with all of the catalogue's collections.
     *
     */
    fillTable() {
      this.getCommercialCatalogue()
        .then(catalogueResponse => {
          const { data: commercialCatalogue } = catalogueResponse;
          const { reference, designation } = commercialCatalogue;

          this.reference = reference;
          this.designation = designation;

          var commercialCatalogueCollections =
            commercialCatalogue.commercialCatalogueCollections !== undefined
              ? commercialCatalogue.commercialCatalogueCollections
              : [];

          if (this.editable) {
            CustomizedProductCollectionsRequests.getCustomizedProductCollections().then(
              collectionsResponse => {
                const {
                  data: customizedProductCollections
                } = collectionsResponse;
                const numCustomizedProductCollections =
                  customizedProductCollections.length;
                const numCommercialCatalogueCollections =
                  commercialCatalogueCollections.length;

                for (let i = 0; i < numCustomizedProductCollections; i++) {
                  for (let j = 0; j < numCommercialCatalogueCollections; j++) {
                    if (
                      customizedProductCollections[i].id ===
                      commercialCatalogueCollections[j].id
                    ) {
                      this.selectedCollections.push(
                        customizedProductCollections[i]
                      );
                    }
                  }
                  this.availableCollections.push(
                    customizedProductCollections[i]
                  );
                }
              }
            );
          } else {
            this.availableCollections.push(...commercialCatalogueCollections);
          }
        })
        .catch(error => {
          //this should only occur if the catalogue was deleted immediately after selecting it
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


<style>
.expandable-div {
  padding-bottom: 25%;
}
</style>
