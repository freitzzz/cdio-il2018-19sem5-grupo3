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
                    @click="openMaterialDetails(props.rowData)"
                >
                    <b-icon icon="chart-line"/>
                </button>
                <button
                    class="btn-primary"
                    @click="editMaterialDetails(props.rowData)"
                >
                    <b-icon icon="pencil"/>
                </button>
            </div>
            <div v-if="showMaterialDetails">
                <b-modal :active.sync="showMaterialDetails" has-modal-card scroll="keep">
                    <price-material-details
                        :material="currentSelectedMaterial"
                    />
                </b-modal>
            </div>                    
            <div v-if="showEditMaterialDetails">
                <b-modal :active.sync="showEditMaterialDetails" has-modal-card scroll="keep">
                    <edit-price-material
                        @emitMaterial="updateMaterial"
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
import PriceMaterialDetails from './PriceMaterialDetails';

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
import PriceTable from './../../../services/mycm_api/requests/pricetables.js';


export default {
   
    /**
     * Components exported components
     */
    components:{
        EditPriceMaterial,
        PriceMaterialDetails,
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
            showEditMaterialDetails:false,
            showMaterialDetails:false,
            showListFinishes:false
        }
    },
    /**
     * Component methods
     */
    methods:{
        /**
         * Opens a modal with the material details
         */
        openMaterialDetails(material){
             this
                .getMaterialDetails(material)
                .then((material)=>{ 
                    this.showMaterialDetails=true;
                 }); 
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
                                    PriceTable.getCurrentMaterialFinishPrice(response.data.id, response.data.finishes[i].id, "", "")
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
        editMaterialDetails(materialId, price){
            this.getMaterialDetails(materialId, price)
                .then((matrial)=>{this.showEditMaterialDetails=true;});
        },
        /**
         * Updates a given material
         */
        updateMaterial(materialDetails){
            this
                .updateMaterialProperties(materialDetails)
                .then(()=>{
                    this.$emit('refreshData');
                    this.$toast.open({message:"Material was updated with success!"});      
                })
                .catch((error_message)=>{
                    this.$toast.open({message:error_message});
                });
        },
        /**
         * Updates a given material properties (PUT) in a promise way
         */
        updateMaterialProperties(materialDetails){
            
        },
    },
      props:{
        data:Array
    } 
}
</script>
