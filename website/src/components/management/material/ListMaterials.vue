<template>
    <div>
        <!-- CUD BUTTONS -->
        <div>
            <button class="button is-danger" @click="createMaterial()">
                <b-icon icon="plus"/>
            </button>
            <create-material 
                :active="createMaterialModal" 
                :available-colors="availableColors"
                :available-finishes="availableFinishes"
                @emitMaterial="postMaterial"
            />
            <button class="button is-danger" @click="removeSelectedMaterial()">
                <b-icon icon="minus"/>
            </button>
            <button class="button is-danger" @click="updateSelectedMaterial()">
                <b-icon 
                    icon="refresh"
                    custom-class="fa-spin"/>
            </button>
            <edit-material
                :active="updateMaterialModal"
                :material="material"
                :available-colors="availableColors"
                :available-finishes="availableFinishes"
                @emitMaterial="updateMaterial"
                />
        </div>
        <paginated-table 
        :total.sync="total" 
        :columns.sync="columns"
        :data.sync="data"
        :title.sync="title"
        :showTotalInput=false
        :showItemsPerPageInput=false
        @clicked="changeSelectedMaterial"
        />
    </div>
</template>

<script>

import CreateMaterial from './CreateMaterial.vue';
import EditMaterial from './EditMaterial.vue';
import PaginatedTable from './../../UIComponents/PaginatedTable.vue';
import Axios from 'axios';

let colors=[];
let finishes=[];

export default {
    components:{
        PaginatedTable,
        CreateMaterial,
        EditMaterial
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
         * Triggers the update of a selected material
         */
        updateSelectedMaterial(){
            this.fetchSelectedMaterial();
            this.updateMaterialModal=true;
        },
        /**
         * Posts a new material
         */
        postMaterial(materialDetails){
            Axios
                .post('http://localhost:5000/mycm/api/materials',materialDetails)
                .then((response)=>{
                    this.$toast.open({message:"The material was created with success!"});
                    this.createMaterialModal=false;    
                    this.fetchRequests();
                })
                .catch((_error)=>{
                    this.$toast.open({message:_error.response.data.error});
                });
        },
        updateMaterial(materialDetails){
            this.updateMaterialProperties(materialDetails);
        },
        updateMaterialProperties(materialDetails){
            let materialProperties={};
            if(this.materialClone.reference!=materialDetails.reference)materialProperties.reference=materialDetails.reference;
            if(this.materialClone.designation!=materialDetails.designation)materialProperties.designation=materialDetails.designation
            Axios
                .put('http://localhost:5000/mycm/api/materials/'+this.currentSelectedMaterial
                    ,materialProperties)
                .then((_response)=>{
                    this.$toast.open({message:"The material properties were updated with success"});
                    this.updateMaterialFromData(_response.data);
                    this.fetchRequests();
                    this.updateMaterialModal=false;
                })
                .catch((_error)=>{
                    this.$toast.open({message:"An error ocurrd while updating the material properties"})
                });
        },
        /**
         * Triggers the deletion of the selected material
         */
        removeSelectedMaterial(){
            Axios
            .delete('http://localhost:5000/mycm/api/material/'+this.currentSelectedMaterial)
            .then((response)=>{
                this.$toast.open({message:"The material was deleted with success!"});
                this.fetchRequests();
            })
            .catch(()=>{
                this.$toast.open({message:"An error occurred while deleting the material"});
            });
        },
        updateMaterialFromData(data){
            let materialDetails=data;
            this.material={
                id:materialDetails.id,
                reference:materialDetails.reference,
                designation:materialDetails.designation,
                colors:colors
            };
            this.materialClone={
                id:materialDetails.id,
                reference:materialDetails.reference,
                designation:materialDetails.designation,
                colors:colors
            };
        },
        fetchSelectedMaterial(){
            Axios
                .get('http://localhost:5000/mycm/api/materials/'+this.currentSelectedMaterial)
                .then((response)=>{
                    let materialDetails=response.data;
                    let materials=[];
                    materialDetails.material.forEach((material)=>{
                        materials.push({id:material.id,value:material.designation});
                    });
                    this.material={
                        id:materialDetails.id,
                        reference:materialDetails.reference,
                        designation:materialDetails.designation,
                        colors:colors
                    };
                    this.materialClone={
                        id:materialDetails.id,
                        reference:materialDetails.reference,
                        designation:materialDetails.designation,
                        colors:colors
                    };
                    console.log(materialDetails)
                })
                .catch(()=>{
                    this.$toast.open({message:"An error occurred while fetching the material"});
                });
        },
        fetchRequests(){
            this.updateFetchedMaterials();
            this.fetchAvailableFinishes();
            this.fetchAvailableColors();
        },
    },
    fetchAvailableFinishes(){
        Axios
          .get('http://localhost:5000/mycm/api/finishes')
          .then((response)=>{
            let availableFinishes=response.data;
            availableFinishes.forEach((finish)=>{
              finishes.push({
                id:finish.id,
                value:finish.designation
              });
            });
          })
          .catch(()=>{
            
          });
    },
    fetchAvailableColors(){
        Axios
          .get('http://localhost:5000/mycm/api/colors')
          .then((response)=>{
            let availableColors=response.data;
            availableColors.forEach((color)=>{
              colors.push({
                id:color.id,
                value:color.designation
              });
            });
          })
          .catch(()=>{
            
          });
        },
        /**
         * Fetches all available materials
         */
        updateFetchedMaterials(){
            Axios.get('http://localhost:5000/mycm/api/materials')
            .then((_response)=>{
                this.data=this.generateMaterialsTableData(_response.data);
                this.columns=this.generateMaterialsTableColumns();
                this.total=this.data.length;
            })
            .catch((_error)=>{
                console.log(_error);
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
                }
            ];
        },
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
</script>
