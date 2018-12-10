<template>
    <vuetable
        :api-mode="false"
        :data="data"
        :fields="columns"
    >
        <template slot="actions" slot-scope="props">
            <!-- Table Actions -->
            <div class="custom-actions">
                <button
                    class="button is-danger"
                    @click="openMaterialDetails(props.rowData.id)"
                >
                    <b-icon icon="magnify"/>
                </button>
                <button
                    class="button is-danger"
                    @click="editMaterialDetails(props.rowData.id)"
                >
                    <b-icon icon="pencil"/>
                </button>
                <button
                    class="button is-danger"
                    @click="deleteMaterial(props.rowData.id)"
                >
                    <b-icon icon="minus"/>
                </button>
            </div>
            <div v-if="showMaterialDetails">
                <b-modal :active.sync="showMaterialDetails" has-modal-card scroll="keep">
                    <material-details
                        :material="currentSelectedMaterial"
                    />
                </b-modal>
            </div>
            <div v-if="showEditMaterialDetails">
                <b-modal :active.sync="showEditMaterialDetails" has-modal-card scroll="keep">
                    <edit-material
                        @emitMaterial="updateMaterial"
                        :available-colors="availableMaterials"
                        :available-finishes="availableFinishes"
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
import MaterialDetails from './MaterialDetails';

/**
 * Requires EditMaterial modal for material edition
 */
import EditMaterial from './EditMaterial';

/**
 * Requires App Configuration for accessing MYCM API URL
 */
import Config,{MYCM_API_URL} from '../../../config';

export default {
    /**
     * Components exported components
     */
    components:{
        EditMaterial,
        MaterialDetails
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
                    name: "__slot:actions", // <----
                    title: "Actions",
                    titleClass: "center aligned",
                    dataClass: "center aligned"
                }
            ],
            showEditMaterialDetails:false,
            showMaterialDetails:false
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
            this
                .getMaterialDetails(materialId)
                .then((material)=>{
                    this.showMaterialDetails=true;
                });
        },
        /**
         * Fetches the details of a certain material in a promise way
         */
        getMaterialDetails(materialId){
            return new Promise((accept,reject)=>{
                Axios
                    .get(MYCM_API_URL+'/materials/'+materialId)
                    .then((material)=>{
                        this.currentSelectedMaterial=material.data;
                        this.currentSelectedMaterial2=Object.assign({},this.currentSelectedMaterial);
                        accept(material);
                    })
                    .catch((error_message)=>{
                        this.$toast.open({message:error_message});
                        reject();
                    });
            });
        },
         /**
         * Deletes a given material
         */
        deleteMaterial(materialId){
            Axios
                .delete(MYCM_API_URL+'/materials/'+materialId)
                .then(()=>{
                    this.$toast.open({message:"Material was deleted with success!"});
                })
                .catch((error_message)=>{
                    this.$toast.open({message:error_message.data.message});
                });
        },
        /**
         * Edits the details of a material
         */
        editMaterialDetails(materialId){
            this
                .getMaterialDetails(materialId)
                .then((matrial)=>{
                    
                                   
                                    this.showEditMaterialDetails=true;
                              
                });
        },
         /**
         * Fetches all colors available in a promise way
         */
        getAllColors(){
            return new Promise((accept,reject)=>{
                Axios
                    .get(MYCM_API_URL+'/colors/')
                    .then((colors)=>{
                        this.availableColors=colors.data;
                        accept();
                    })
                    .catch((error_message)=>{
                        this.$toast.open({message:error_message});
                        reject();
                    });
            });
        },
         /**
         * Fetches all finishes available in a promise way
         */
        getAllFinishes(){
            return new Promise((accept,reject)=>{
                Axios
                    .get(MYCM_API_URL+'/finishes/')
                    .then((finishes)=>{
                        this.availableFinishes=finishes.data;
                        accept();
                    })
                    .catch((error_message)=>{
                        this.$toast.open({message:error_message});
                        reject();
                    });
            });
        },
       
    },
    props:{
        data:[]
    }
}
</script>
