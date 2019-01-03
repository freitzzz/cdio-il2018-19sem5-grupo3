<template>
    <div>
        <!-- CUD BUTTONS -->
        <div>
            <small-padding-div>
                <button class="btn-primary" @click="createMaterial()">
                <b-icon icon="plus"/>
                </button>
            </small-padding-div>
            <div v-if="createMaterialModal">
                <b-modal :active.sync="createMaterialModal" has-modal-card scroll="keep">
                    <create-price-material 
                        :active="createMaterialModal" 
                        @emitMaterial="postMaterial"
                    />
                </b-modal>
            </div> 
            <button class="btn-primary" @click="fetchRequests()">
                <b-icon 
                    icon="refresh"
                    custom-class="fa-spin"/>
            </button>
        </div>
        <price-materials-table
            :data="data"
        />
    </div>
</template>

<script>
import CreatePriceMaterial from './CreatePriceMaterial.vue';
import PriceMaterialsTable from './PriceMaterialsTable.vue';
import Axios from 'axios';
import Config,{ MYCM_API_URL } from '../../../config.js';

let colors=[];
let finishes=[];

export default {
    components:{
        PriceMaterialsTable,
        CreatePriceMaterial,
    },
    /**
     * Function that is called when the component is created
     */
    created(){
        this.fetchRequests();
    },
    data(){
        return{
            createMaterialModal:false,
            updateMaterialModal:false,
            material:null,
            materialClone:null,
            currentSelectedMaterial:0,
            availableMaterials:Array,
            columns:[],
            data:Array,
            dataMaterial:null,
            total:Number,
            failedToFetchMaterialsNotification:false
        }
    },
    methods:{
        /**
         * Changes the current selected material
         */
        changeSelectedMaterial(tableRow){
            this.currentSelectedMaterial=tableRow.id;
        },
        /**
         * Triggers the creation of a new material
         */
        createMaterial(){
            this.createMaterialModal=true;
        },
        /**
         * Posts a new material
         */
        postMaterial(materialDetails){
            let newMaterial={};
            newMaterial.reference=materialDetails.reference;
            newMaterial.designation=materialDetails.designation;
            
            if(materialDetails.colors!=null){
                let newMaterialColors=[];
                for(let i=0;i<materialDetails.colors.length;i++){
                    newMaterialColors.push({id:materialDetails.colors[i]});
                }
                newMaterial.colors=newMaterialColors.slice();
            }

            if(materialDetails.finishes!=null){
                let newMaterialFinishes=[];
                for(let i=0;i<materialDetails.finishes.length;i++){
                    newMaterialFinishes.push({id:materialDetails.finishes[i]});
                }
                newMaterial.finishes=newMaterialFinishes.slice();
            }

            newMaterial.model="closet.glb";

            Axios
                .post(MYCM_API_URL+'/material',newMaterial)
                .then((response)=>{
                    this.$toast.open({message:"The material was created with success!"});
                    this.createMaterialModal=false;    
                    this.fetchRequests();
                })
                .catch((error_message)=>{
                    this.$toast.open({message:error_message.response.data.message});
                });
        },
        fetchRequests(){
            this.refreshMaterials();
            /* this.fetchAvailableColors();
            this.fetchAvailableFinishes(); */
        },
        /**
         * Fetches all available materials
         */
        refreshMaterials(){
            Axios.get(MYCM_API_URL+'/prices/materials/?currency=EUR&area=m2')
            .then((_response)=>{
                this.data=this.generateMaterialsTableData(_response.data);
                this.columns=this.generateMaterialsTableColumns();
                this.total=this.data.length;
            })
            .catch((error_message)=>{
                this.$toast.open({message:error_message.response.data.message});
            });
        },   
        /**
         * Generates the needed columns for the materials table
         */
        generateMaterialsTableColumns(){
            return [
                {
                    field:"id",
                    label:"ID",
                    centered:true   
                },
                {
                    field:"reference",
                    label:"Reference",
                    centered:true   
                },
                {
                    field:"designation",
                    label:"Designation",
                    centered:true   
                },
                {
                    field:"price",
                    label:"Current Price",
                    centered:true   
                },
                {
                    field:"finishes",
                    label:"Finishes",
                    centered:true   
                },
                {
                    field:"actions",
                    label:"Actions",
                    centered:true
                }
            ];
        },
        /**
         * Generates the data of a materials table by a given list of materials
         */
        generateMaterialsTableData(materials){
            let materialsTableData=[];
            materials.forEach((material)=>{


                 Axios.get(MYCM_API_URL+`/materials/${material.id}`)
                .then(response => {
                    this.dataMaterial = response.data;

                    materialsTableData.push({
                        id:material.id,
                        reference: this.dataMaterial.reference,
                        designation: this.dataMaterial.designation,
                        price: material.value + " " + material.currency + "/" + material.area
                    });
                })
                .catch((error_message)=>{
                    this.$toast.open({message:error_message.response.data.message});
                }); 
            });
            return materialsTableData;
        }
    }
    }
</script>
