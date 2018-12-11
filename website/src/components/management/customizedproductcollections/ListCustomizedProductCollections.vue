<template>
    <div>
        <div>
            <button class = "button is-danger" @click="createNewCollection()">
                <b-icon icon="plus"/>
            </button>
             <div v-if="createNewCollectionModal">
                <b-modal :active.sync="createNewCollectionModal" has-modal-card scroll="keep">
                    <create-new-customized-product-collection
                        :active="createNewCollectionModal" 
                        :available-customized-products="availableCustomizedProducts"
                        @emitCustomizedProductCollecion="postCustomizedProductCollection"
                    />
                </b-modal>
            </div>
            <button class="button is-danger" @click="fetchRequests()">
                <b-icon 
                    icon="refresh"
                    custom-class="fa-spin"/>
            </button>
        </div>
        <customized-product-collections-table
            :data="data"
        />
    </div>
</template>

<script>

import CreateNewCustomizedProductCollection from './CreateNewCustomizedProductCollection.vue';
import CustomizedProductCollectionsTable from './CustomizedProductCollectionsTable.vue';
import Axios from 'axios';
import Config,{ MYCM_API_URL } from '../../../config.js';

let customizedProducts = [];

export default {
    components:{
        CreateNewCustomizedProductCollection,
        CustomizedProductCollectionsTable
    },
    created(){
        this.fetchRequests();
    },
    data(){
        return{
            createNewCollectionModal:false,
            customizedProductCollection:null,
            customizedProductCollectionClone:null,
            currentSelectedCustomizedProductCollection:0,
            availableCollections:Array,
            availableCustomizedProducts:customizedProducts,
            columns: [],
            data:Array,
            total:Number,
            failedToFetchCollectionsNotification:false
        }
    },
    methods:{
        fetchRequests(){
            this.refreshCollections();
            //this.fetchAvailableCustomizedProducts();
        },
        refreshCollections(){
            Axios.get(MYCM_API_URL+'/collections')
            .then((response)=>{
                alert(this.data);
                alert(response);
                this.data = this.generateCollectionsTableData(response.data);
                this.columns = this.generateCollectionsTableColumns();
                this.total = this.data.length;
            })
            .catch((error_message)=>{
                this.$toast.open({message:error_message.response.data.message});
            });
        },
        fetchAvailableCustomizedProducts(){
            Axios
                .get(MYCM_API_URL+'/customizedproducts')
                .then((response)=>{
                    let availableCustomizedProducts = response.data;
                    availableCustomizedProducts.forEach((customizedProduct)=>{
                        customizedProducts.push({
                            id:customizedProduct.id,
                            productId:customizedProduct.productId,
                            reference:customizedProduct.reference,
                            designation:customizedProduct.designation
                        });
                    });
                })
                .catch((error_message)=>{
                    this.$toast.open({message:error_message.response.data.message});
                });
        },
        createNewCollection(){

        },
        generateCollectionsTableColumns(){
            return [
                {
                    field:"ud",
                    label:"ID",
                    centered:true
                },
                {
                    field:"name",
                    label:"Name",
                    centered:true
                },
                {
                    field:"CustomizedProducts",
                    label:"Customized Products",
                    centered:true
                },
                {
                    field:"actions",
                    label:"Actions",
                    centered:true
                }
            ];
        },
        generateCollectionsTableData(collections){
            alert("aaaaaaaa");
            let collectionsTableData = [];
            collections.forEach((collection)=>{
                collectionsTableData.push({
                    id:collection.id,
                    name:collection.name,
                });
            });
            return collectionsTableData;
        }
    }
}
</script>
