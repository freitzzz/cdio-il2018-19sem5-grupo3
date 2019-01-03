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
                    @click="openListPriceFinishes(props.rowData.id)"
                >List of Finishes
                    
                </button>
               
            </div>
            <div v-if="showListFinishes">
                <b-modal :active.sync="showListFinishes" has-modal-card scroll="keep">
                    <list-price-finishes
                        :material="currentSelectedMaterial2"
                    />
                </b-modal>
            </div>                    
            
        </template>
        <template slot="actions" slot-scope="props">
            <!-- Table Actions -->
            <div class="custom-actions">
                <button
                    class="btn-primary"
                    @click="openMaterialDetails(props.rowData.id)"
                >
                    <b-icon icon="chart-line"/>
                </button>
                <button
                    class="btn-primary"
                    @click="editMaterialDetails(props.rowData.id)"
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
                        :material="currentSelectedMaterial2"
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
            availableColors:[],
            availableFinishes:[],
            /**
             * Current Table Selected Material
             */
            currentSelectedMaterial:null,
            currentSelectedMaterial2:null,
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
        openMaterialDetails(materialId){
            /* this
                .getMaterialDetails(materialId)
                .then((material)=>{ */
                    this.showMaterialDetails=true;
                /* }); */
        },
        /**
         * Opens a modal with the material details
         */
        openListPriceFinishes(materialId){
            this
                .getMaterialDetails(materialId)
                .then((material)=>{
                    this.showListFinishes=true;
                });
        },
        /**
         * Fetches the details of a certain material in a promise way
         */
        getMaterialDetails(materialId){
            
        },
        /**
         * Edits the details of a material
         */
        editMaterialDetails(materialId){
            this.getMaterialDetails(materialId)
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
