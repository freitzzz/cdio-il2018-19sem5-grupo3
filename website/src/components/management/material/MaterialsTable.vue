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
                    class="btn-primary"
                    @click="openMaterialDetails(props.rowData.id)"
                >
                    <b-icon icon="magnify"/>
                   
                </button>
                <button
                    class="btn-primary"
                    @click="editMaterialDetails(props.rowData.id)"
                >
                    <b-icon icon="pencil"/>
                </button>
                <button
                    class="btn-primary"
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
                        :available-colors="availableColors"
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
        MaterialDetails,
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
                        for(let i =0; i<this.currentSelectedMaterial.colors.length; i++){
                            this.currentSelectedMaterial.colors[i].colors = {
                                red: this.currentSelectedMaterial.colors[i].red,
                                green: this.currentSelectedMaterial.colors[i].green,
                                blue: this.currentSelectedMaterial.colors[i].blue,
                            }
                        }
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
                    this.$emit('refreshData');
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
        /**
         * Updates a given material
         */
        updateMaterial(materialDetails){
            this
                .updateMaterialProperties(materialDetails)
                .then(()=>{
                    this
                        .updateMaterialColors(materialDetails)
                        .then(()=>{
                            this
                                .updateMaterialFinishes(materialDetails)
                                
                                        .then(()=>{
                                            this.showEditMaterialDetails=false;
                                            this.$emit('refreshData');
                                            this.$toast.open({message:"Material was updated with success!"});
                                        }).catch((error_message)=>{
                                            this.$toast.open({message:error_message});
                                        });
                               
                        })
                        .catch((error_message)=>{
                            this.$toast.open({message:error_message});
                        });
                })
                .catch((error_message)=>{
                    this.$toast.open({message:error_message});
                });
        },
        /**
         * Updates a given material properties (PUT) in a promise way
         */
        updateMaterialProperties(materialDetails){
            let materialPropertiesToUpdate={};
            let atLeastOneUpdate=false;
            if(materialDetails.reference!=null && materialDetails.reference!=this.currentSelectedMaterial.reference){
                materialPropertiesToUpdate.reference=materialDetails.reference;
                atLeastOneUpdate=true;
            }
            if(materialDetails.designation!=null && materialDetails.designation!=this.currentSelectedMaterial.designation){
                materialPropertiesToUpdate.designation=materialDetails.designation;
                atLeastOneUpdate=true;
            }            
            return new Promise((accept,reject)=>{
                if(atLeastOneUpdate){
                    Axios
                    .put(MYCM_API_URL+'/materials/'+materialDetails.id,materialPropertiesToUpdate)
                    .then((material)=>{
                        accept(material);
                    })
                    .catch((error_message)=>{
                        reject(error_message.data.message);
                    });
                }else{
                    accept();
                }
            });
        },
        /**
         * Updates a given material colors (POST + DELETE) in a promise way
         */
        updateMaterialColors(materialDetails){
            let oldMaterialColors=[];
            let addColors=[];
            let deleteColors=[];

            if(this.currentSelectedMaterial.colors!=null){
                for(let i=0;i<this.currentSelectedMaterial.colors.length;i++)
                    oldMaterialColors.push(this.currentSelectedMaterial.colors[i].id);
            }

            let newMaterialColors=materialDetails.colors!=null ? materialDetails.colors : [];

            //Color to add

            for(let i=0;i<newMaterialColors.length;i++){
                if(!oldMaterialColors.includes(newMaterialColors[i]))
                    addColors.push(newMaterialColors[i]);
            }

            //Color to delete

            for(let i=0;i<oldMaterialColors.length;i++){
                if(!newMaterialColors.includes(oldMaterialColors[i]))
                    addColors.push(oldMaterialColors[i]);
            }

            return new Promise((accept,reject)=>{
                if(newMaterialColors.length==0)accept();
                if(addColors.length>0){
                    for(let i=0;i<addColors.length;i++){
                        Axios
                            .post(MYCM_API_URL+'/materials/'+materialDetails.id+'/colors/',{
                                id:addColors[i]
                            })
                            .catch((error_message)=>{
                                reject(error_message.data.message);
                            });
                    }
                }

                if(deleteColors.length>0){
                    for(let i=0;i<deleteColors.length;i++){
                        Axios
                            .delete(MYCM_API_URL+'/materials/'+materialDetails.id+'/colors/'+deleteColors[i])
                            .catch((error_message)=>{
                                reject(error_message.data.message);
                            });
                    }
                }
                accept();
            });
        },
        /**
         * Updates a given material finishes (POST + DELETE) in a promise way
         */
        updateMaterialFinishes(materialDetails){
            let oldMaterialFinishes=[];
            let addFinishes=[];
            let deleteFinishes=[];

            if(this.currentSelectedMaterial.finishes!=null){
                for(let i=0;i<this.currentSelectedMaterial.finishes.length;i++)
                    oldMaterialFinishes.push(this.currentSelectedMaterial.finishes[i].id);
            }

            let newMaterialFinishes=materialDetails.finishes!=null ? materialDetails.finishes : [];

            //Finish to add

            for(let i=0;i<newMaterialFinishes.length;i++){
                if(!oldMaterialFinishes.includes(newMaterialFinishes[i]))
                    addFinishes.push(newMaterialFinishes[i]);
            }

            //Finish to delete

            for(let i=0;i<oldMaterialFinishes.length;i++){
                if(!newMaterialFinishes.includes(oldMaterialFinishes[i]))
                    deleteFinishes.push(oldMaterialFinishes[i]);
            }

            return new Promise((accept,reject)=>{
                if(newMaterialFinishes.length==0)accept();
                if(addFinishes.length>0){
                    for(let i=0;i<addFinishes.length;i++){
                        Axios
                            .post(MYCM_API_URL+'/materials/'+materialDetails.id+'/finishes/',{
                                id:addFinishes[i]
                            })
                            .catch((error_message)=>{
                                reject(error_message.data.message);
                            });
                    }
                }

                if(deleteFinishes.length>0){
                    for(let i=0;i<deleteFinishes.length;i++){
                        Axios
                            .delete(MYCM_API_URL+'/materials/'+materialDetails.id+'/finishes/'+deleteFinishes[i])
                            .catch((error_message)=>{
                                reject(error_message.data.message);
                            });
                    }
                }
                accept();
            });
        },
       
    },
      props:{
        data:[]
    } 
}
</script>
