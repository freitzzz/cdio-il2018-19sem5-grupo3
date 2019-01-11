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
                    <create-material 
                        :active="createMaterialModal" 
                        :available-colors="availableColors"
                        :available-finishes="availableFinishes"
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
        <materials-table
            :data="data"
        />
    </div>
</template>

<script>
import CreateMaterial from './CreateMaterial.vue';
import MaterialsTable from './MaterialsTable.vue';
import Axios from 'axios';
import Config,{ MYCM_API_URL } from '../../../config.js';

let colors=[];
let finishes=[];

export default {
    components:{
        MaterialsTable,
        CreateMaterial,
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
            availableFinishes:finishes,
            availableColors:colors,
            columns:[],
            data:Array,
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
            Axios.get(MYCM_API_URL+'/materials')
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
                materialsTableData.push({
                    id:material.id,
                    reference:material.reference,
                    designation:material.designation
                });
            });
            return materialsTableData;
        }
    }
    }
</script>
