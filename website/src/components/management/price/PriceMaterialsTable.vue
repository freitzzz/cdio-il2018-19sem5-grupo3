<template>
    <vuetable
        :api-mode="false"
        :data="data"
        :fields="columns"
    >
     <template slot="finishes" slot-scope="props">
            <!-- Table Actions -->
            <div class="custom-actions">
                <button
                    class="btn-primary"
                    @click="openListPriceFinishes(props.rowData)"
                >      
                    <b-icon icon="format-list-bulleted"></b-icon>              
                </button>
               
            </div>
            <div v-if="showListFinishes">
                <b-modal :active.sync="showListFinishes" has-modal-card scroll="keep">
                    <list-price-finishes
                        :material="currentSelectedMaterial"
                    />
                </b-modal>
            </div>                    
            
        </template>
        <template slot="actions" slot-scope="props">
            <!-- Table Actions -->
            <div class="custom-actions">
                <button
                    class="btn-primary"
                    @click="showMaterialPriceHistory(props.rowData)"
                >
                    <b-icon icon="chart-line"/>
                </button>
                <button
                    class="btn-primary"
                    @click="editMaterialPriceTableEntry(props.rowData)"
                >
                    <b-icon icon="pencil"/>
                </button>
            </div>
            <div v-if="showMaterialPriceHistoryModal">
                <b-modal :active.sync="showMaterialPriceHistoryModal" has-modal-card scroll="keep">
                    <material-price-history
                        :materialId="currentSelectedMaterial.id"
                    />
                </b-modal>
            </div>                    
            <div v-if="showEditMaterialPriceTableEntry">
                <b-modal :active.sync="showEditMaterialPriceTableEntry" has-modal-card scroll="keep">
                    <edit-price-material
                        @updateMaterialPriceTableEntry="updateMaterialPriceTableEntry"
                        :material="currentSelectedMaterial"
                    />
                </b-modal>
            </div>
        </template>
    </vuetable>
</template>

<script>

/**
 * Requies Axios for MYCM Materials API requests
 */
import Axios from 'axios';

/**
 * Requires MaterialDetails modal for material details
 */
import MaterialPriceHistory from './MaterialPriceHistory';

/**
 * Requires MaterialDetails modal for material details
 */
import ListPriceFinishes from './ListPriceFinishes';

/**
 * Requires EditMaterial modal for material edition
 */
import EditPriceMaterial from './EditPriceMaterial';

/**
 * Requires App Configuration for accessing MYCM API URL
 */
import Config,{MYCM_API_URL} from '../../../config';

import MaterialRequest from './../../../services/mycm_api/requests/materials';
import PriceTablesRequests from './../../../services/mycm_api/requests/pricetables.js';


export default {
   
    name:"PriceMaterialsTable",

    /**
     * Components exported components
     */
    components:{
        EditPriceMaterial,
        MaterialPriceHistory,
        ListPriceFinishes
    },
    /**
     * Component data
     */
    data(){
        return{
            finishes: [],
            /**
             * Current Table Selected Material
             */
            currentSelectedMaterial:null,
            /**
             * Materials table columns
             */
            columns:[
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
                    name: "price",
                    title: "Current Price"
                },
                {
                    name: "__slot:finishes",
                    title: "Finishes",
                    titleClass: "center aligned",
                    dataClass: "center aligned"
                },
                {
                    name: "__slot:actions",
                    title: "Actions",
                    titleClass: "center aligned",
                    dataClass: "center aligned"
                }
            ],
            showEditMaterialPriceTableEntry:false,
            showListFinishes:false,
            showMaterialPriceHistoryModal:false
        }
    },
    /**
     * Component methods
     */
    methods:{
        /**
         * Opens a modal with the material's price history
         */
        showMaterialPriceHistory(material){
            this.currentSelectedMaterial = material;
            this.showMaterialPriceHistoryModal=true;
        },
        /**
         * Opens a modal with the material details
         */
        openListPriceFinishes(material){
            this
                .getMaterialDetails(material)
                .then((material)=>{
                    this.showListFinishes=true;
                });
        },
        /**
         * Fetches the details of a certain material in a promise way
         */
        getMaterialDetails(material){
            let value = material.price.split(" ")[0];
            let currency = material.price.split(" ")[1].split("/")[0];
            let area = material.price.split(" ")[1].split("/")[1];

            let initialDate = material.startingDate.split("T")[0];
            let initalTime = material.startingDate.split("T")[1];
            let endDate = material.endingDate.split("T")[0];
            let endTime = material.endingDate.split("T")[1];

            return new Promise((accept,reject)=>{
                MaterialRequest.getMaterial(material.id)
                    .then((response)=>{

                                for(let i=0; i<response.data.finishes.length; i++){
                                    PriceTablesRequests.getCurrentMaterialFinishPrice(response.data.id, response.data.finishes[i].id, "", "")
                                    .then((finishData) => {
                                        this.finishes.push({
                                            id: response.data.finishes[i].id,
                                            description: response.data.finishes[i].description,
                                            shininess: response.data.finishes[i].shininess,
                                            value: finishData.data.currentPrice.value + " " + finishData.data.currentPrice.currency + "/" + finishData.data.currentPrice.area,
                                        })
                                    })
                                    .catch(); 
                                };
                                this.currentSelectedMaterial= {
                                id: response.data.id,
                                tableEntryId: material.tableEntryId,
                                reference: response.data.reference,
                                designation: response.data.designation,
                                //finishes: this.finishes,
                                finishes: response.data.finishes,
                                value: value,
                                currency: currency,
                                area: area,
                                startingDate: initialDate,
                                endingDate: endDate,
                                startingTime: initalTime,
                                endingTime: endTime
                                };
                                accept(response);
                             
                    })
                    .catch((error_message)=>{
                        this.$toast.open({message:error_message});
                        reject();
                    });
            });
            
        },
        /**
         * Edits the details of a material
         */
        editMaterialPriceTableEntry(materialId, price){
            this.getMaterialDetails(materialId, price)
                .then((material)=>{this.showEditMaterialPriceTableEntry=true;});
        },
        /**
         * Updates a price table entry of a material
         */
        updateMaterialPriceTableEntry(materialId, tableEntryId, updatedEntry){
            PriceTablesRequests.putMaterialPriceTableEntry(materialId, tableEntryId, updatedEntry)
                .then(response =>{
                    this.$toast.open({
                        message: "Update was successful!"
                    })
                    this.$emit("refreshData");
                    this.showEditMaterialPriceTableEntry = false;
                })
                .catch(error =>{
                    this.$toast.open(error.response.data.message);
                });
        }
    },
      props:{
        data:Array
    } 
}
</script>
