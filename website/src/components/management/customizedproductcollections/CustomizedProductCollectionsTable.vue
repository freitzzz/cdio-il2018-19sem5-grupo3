<template>
  <vuetable :api-mode="false" :data="data" :fields="columns">
    <template slot="actions" slot-scope="props">
      <div class="custom-actions">
        <button class="btn-primary" @click="openCollectionDetails(props.rowData.id)">
          <b-icon icon="magnify"/>
        </button>
        <button class="btn-primary" @click="editCollectionDetails(props.rowData.id)">
          <b-icon icon="pencil"/>
        </button>
        <button class="btn-primary" @click="deleteCustomizedProductCollection(props.rowData.id)">
          <b-icon icon="minus"/>
        </button>
      </div>
      <div v-if="showCollectionDetails">
        <b-modal :active.sync="showCollectionDetails" has-modal-card scroll="keep">
          <customized-product-collection-details :collection="currentSelectedCollection"/>
        </b-modal>
      </div>
      <div v-if="showEditCollectionDetails">
        <b-modal :active.sync="showEditCollectionDetails" has-modal-card scroll="keep">
          <edit-customized-product-collection
            @emitCollection="updateCustomizedProductCollection"
            :available-customized-products="availableCustomizedProducts"
            :collection="currentSelectedCollectionClone"
          />
        </b-modal>
      </div>
    </template>
  </vuetable>
</template>

<script>
import Axios from "axios";

import CustomizedProductCollectionDetails from "./CustomizedProductCollectionDetails";

import EditCustomizedProductCollection from "./EditCustomizedProductCollection";

import Config, { MYCM_API_URL } from "../../../config";

export default {
  components: {
    EditCustomizedProductCollection,
    CustomizedProductCollectionDetails
  },
  data() {
    return {
      availableCustomizedProducts: [],
      currentSelectedCollection: null,
      currentSelectedCollectionClone: null,
      columns: [
        {
          name: "id",
          title: "ID"
        },
        {
          name: "name",
          title: "Name"
        },
        {
          name: "hasCustomizedProducts",
          title: "Customized Products",
          dataClass: "centered aligned",
          callback: this.booleansAsIcons
        },
        {
          name: "__slot:actions",
          title: "Actions",
          titleClass: "center aligned",
          dataClass: "center aligned"
        }
      ],
      showEditCollectionDetails: false,
      showCollectionDetails: false
    };
  },
  methods: {
    booleansAsIcons(value) {
      return value
        ? '<span class="ui teal label"><i class="material-icons">check</i></span>'
        : '<span class="ui teal label"><i class="material-icons">close</i></span>';
    },
    openCollectionDetails(collectionId) {
      this.getCollectionDetails(collectionId).then(collection => {
        this.showCollectionDetails = true;
      });
    },
    getCollectionDetails(collectionId) {
      return new Promise((accept, reject) => {
        Axios.get(MYCM_API_URL + "/collections/" + collectionId)
          .then(collection => {
            this.currentSelectedCollection = collection.data;
            this.currentSelectedCollectionClone = Object.assign(
              {},
              this.currentSelectedCollection
            );
            accept(collection);
          })
          .catch(error_message => {
            this.$toast.open({ message: error_message });
            reject();
          });
      });
    },
    deleteCustomizedProductCollection(collectionId) {
      Axios.delete(MYCM_API_URL + "/collections/" + collectionId)
        .then(() => {
          this.$toast.open({
            message: "Collection was disabled successfully!"
          });
          this.$emit("refreshData");
        })
        .catch(error_message => {
          this.$toast.open({ message: error_message.data.message });
        });
    },
    editCollectionDetails(collectionId) {
      this.getCollectionDetails(collectionId).then(collection => {
        this.getAllCustomizedProducts().then(() => {
          this.showEditCollectionDetails = true;
        });
      });
    },
    getAllCustomizedProducts() {
      return new Promise((accept, reject) => {
        Axios.get(MYCM_API_URL + "/customizedproducts")
          .then(customizedProducts => {
            this.availableCustomizedProducts = customizedProducts.data;
            accept();
          })
          .catch(error_message => {
            this.$toast.open({ message: error_message });
            reject();
          });
      });
    },
    updateCustomizedProductCollection(collectionDetails) {
      this.updateCollectionProperties(collectionDetails)
        .then(() => {
          this.updateCollectionCustomizedProducts(collectionDetails)
            .then(() => {
              this.showEditCollectionDetails = false;
              this.$toast.open({ message: "Collection updated successfully!" });
              this.$emit("refreshData");
            })
            .catch(error_message => {
              this.$toast.open({ message: error_message });
            });
        })
        .catch(error_message => {
          this.$toast.open({ message: error_message });
        });
    },
    updateCollectionProperties(collectionDetails) {
      let collectionPropertiesToUpdate = {};
      let atLeastOneUpdate = false;
      if (
        collectionDetails.name != null &&
        collectionDetails.name != this.currentSelectedCollection.name
      ) {
        collectionPropertiesToUpdate.name = collectionDetails.name;
        atLeastOneUpdate = true;
      }

      return new Promise((accept, reject) => {
        if (atLeastOneUpdate) {
          Axios.put(
            MYCM_API_URL + "/collections/" + collectionDetails.id,
            collectionPropertiesToUpdate
          )
            .then(collection => {
              accept(collection);
            })
            .catch(error_message => {
              reject(error_message.data.message);
            });
        } else {
          accept();
        }
      });
    },
    updateCollectionCustomizedProducts(collectionDetails) {
      let oldCollectionCustomizedProducts = [];
      let addCustomizedProducts = [];
      let deleteCustomizedProducts = [];

      if(this.currentSelectedCollection.customizedProducts!=null){
          for(let i=0;i<this.currentSelectedCollection.customizedProducts.length;i++){
              oldCollectionCustomizedProducts.push(this.currentSelectedCollection.customizedProducts[i].id);
          }
      }

      let newCollectionCustomizedProducts =
        collectionDetails.customizedProducts!=null ? 
        collectionDetails.customizedProducts
        : [];

        for(let i=0;i<newCollectionCustomizedProducts.length;i++){
            if(!oldCollectionCustomizedProducts.includes(newCollectionCustomizedProducts[i])){
                addCustomizedProducts.push(newCollectionCustomizedProducts[i]);
            }
        }

        for(let i=0;i<oldCollectionCustomizedProducts.length;i++){
            if(!newCollectionCustomizedProducts.includes(oldCollectionCustomizedProducts[i])){
                deleteCustomizedProducts.push(oldCollectionCustomizedProducts[i]);
            }
        }

        return new Promise((accept, reject)=>{
            if(newCollectionCustomizedProducts.length==0){
                accept();
            }

            let addedCustomizedProducts = [];
            let deletedCustomizedProducts = [];

            let postRequests = new Promise((acceptAddition,rejectAddition)=>{
                if(addCustomizedProducts.length>0){
                    for(let i=0; i < addCustomizedProducts.length; i++){
                        Axios
                            .post(MYCM_API_URL+'/collections/'
                                +collectionDetails.id
                                +'/customizedproducts',
                                {
                                    customizedProductId:addCustomizedProducts[i]
                                })
                                .then(()=>{
                                    addedCustomizedProducts.push(true);
                                    if(addedCustomizedProducts.length==addCustomizedProducts.length){
                                        acceptAddition();
                                    }
                                })
                                .catch((error_message)=>{
                                    reject(`Customized Product with id: ` + 
                                          addCustomizedProducts[i] + ` could not be added`);
                                });
                    }
                }else{
                  acceptAddition();
                }
            });
            postRequests
                    .then(()=>{
                        let deleteRequests = new Promise((acceptRemoval,rejectRemoval)=>{
                            if(deleteCustomizedProducts.length>0){
                                for(let i=0;i<deleteCustomizedProducts.length;i++){
                                    Axios
                                        .delete(MYCM_API_URL+'/collections/'
                                                +collectionDetails.id
                                                +'/customizedproducts/'
                                                +deleteCustomizedProducts[i])
                                        .then(()=>{
                                            deletedCustomizedProducts.push(true);
                                            if(deletedCustomizedProducts.length==deleteCustomizedProducts.length){
                                                acceptRemoval();
                                            }
                                        })
                                        .catch((error_message)=>{
                                            reject(`Customized Product with id: ` + 
                                          deleteCustomizedProducts[i] + ` could not be deleted`);
                                        });
                                }
                            }else{
                              acceptRemoval();
                            }
                        });
                        deleteRequests
                                .then(()=>{
                                    accept();
                                })
                                .catch((error_message)=>{
                                    reject(error_message);
                                })
                    })
                    .catch((error_message)=>{
                        reject(error_message);
                    });
        });
    }
  },
  props: {
    data: []
  }
};
</script>