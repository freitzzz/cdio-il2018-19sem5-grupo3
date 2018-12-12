<template>
    <vuetable :api-mode="false" :data="data" :fields="columns">
        <template slot = "actions" slot-scope = "props">
            <div class="custom-actions">
                <button class = "button is-danger" @click="openCollectionDetails(props.rowData.id)">
                    <b-icon icon = "magnify"/>
                </button>
                <button class = "button is-danger" @click="editCollectionDetails(props.rowData.id)">
                    <b-icon icon = "pencil"/>
                </button>
                <button class = "button is-danger" @click="deleteProduct(props.rowData.id)">
                    <b-icon icon = "minus"/>
                </button>
            </div>
            <div v-if="showCollectionDetails">
                <b-modal :active.sync="showCollectionDetails" has-modal-card scroll="keep">
                    <customized-product-collection-details
                        :customized-product-collection="currentSelectedCollection"
                    />
                </b-modal>
            </div>
            <div v-if="showEditCollectionDetails">
                <b-modal :active.sync="showEditCollectionDetails" has-modal-card scroll="keep">
                    <edit-customized-product-collection
                        @emitCustomizedProductCollection="updateCustomizedProductCollection"
                        :available-collections="availableCollections"
                        :available-customized-products="availableCustomizedProducts"
                        :current-selected-collection="currentSelectedCollection"
                    />
                </b-modal>
            </div>
        </template>
    </vuetable>
</template>

<script>

import Axios from 'axios';

import CustomizedProductCollectionDetails from './CustomizedProductCollectionDetails';

import EditCustomizedProductCollection from './EditCustomizedProductCollection';

import Config,{MYCM_API_URL} from '../../../config';

export default{

    components:{
        EditCustomizedProductCollection,
        CustomizedProductCollectionDetails
    },
    data(){
        return {
            availableCollections:[],
            availableCustomizedProducts:[],
            currentSelectedCollection: null,
            columns:[
                {
                    name: "id",
                    title: "ID"
                },
                {
                    name: "name",
                    title: "Name"
                },
                {
                    name: "CustomizedProducts",
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
        }
    },
    methods:{
        booleansAsIcons(value){
            return value 
                ? '<span class="ui teal label"><i class="material-icons">check</i></span>'
                : '<span class="ui teal label"><i class="material-icons">close</i></span>'
            ;     
        },
        deleteCustomizedProductCollection(collectionId){
            Axios
                .delete(MYCM_API_URL+'/collections/'+collectionId)
                .then(()=>{
                    this.$toast.open({message:"Collection was disabled successfully!"});
                })
                .catch((error_message)=>{
                    this.$toast.open({message:error_message.data.message});
                });
        },
    },
    props:{
        data:[]
    }
}

</script>