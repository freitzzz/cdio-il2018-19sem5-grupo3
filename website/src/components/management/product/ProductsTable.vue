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
                    @click="openProductDetails(props.rowData.id)"
                >
                    <b-icon icon="magnify"/>
                </button>
                <button
                    class="button is-danger"
                    @click="editProductDetails(props.rowData.id)"
                >
                    <b-icon icon="pencil"/>
                </button>
                <button
                    class="button is-danger"
                    @click="deleteProduct(props.rowData.id)"
                >
                    <b-icon icon="minus"/>
                </button>
            </div>
            <div v-if="showProductDetails">
                <b-modal :active.sync="showProductDetails" has-modal-card scroll="keep">
                    <product-details
                        :product="currentSelectedProduct"
                    />
                </b-modal>
            </div>
            <div v-if="showEditProductDetails">
                <b-modal :active.sync="showEditProductDetails" has-modal-card scroll="keep">
                    <edit-product
                        @emitProduct="updateProduct"
                        :available-categories="availableCategories"
                        :available-components="availableComponents"
                        :available-materials="availableMaterials"
                        :product="currentSelectedProduct2"
                    />
                </b-modal>
            </div>
        </template>
    </vuetable>
</template>

<script>

/**
 * Requies Axios for MYCM Products API requests
 */
import Axios from 'axios';

/**
 * Requires ProductDetails modal for product details
 */
import ProductDetails from './ProductDetails';

/**
 * Requires EditProduct modal for product edition
 */
import EditProduct from './EditProduct';

/**
 * Requires App Configuration for accessing MYCM API URL
 */
import Config,{MYCM_API_URL} from '../../../config';

export default {
    /**
     * Components exported components
     */
    components:{
        EditProduct,
        ProductDetails
    },
    /**
     * Component data
     */
    data(){
        return{
            availableCategories:[],
            availableComponents:[],
            availableMaterials:[],
            /**
             * Current Table Selected Product
             */
            currentSelectedProduct:null,
            currentSelectedProduct2:null,
            /**
             * Products table columns
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
                    name: "supportsSlots",
                    title: "Supports Slots",
                    dataClass: "centered aligned",
                    callback: this.booleansAsIcons
                },
                {
                    name: "hasComponents",
                    title: "Has Components",
                    callback: this.booleansAsIcons
                },
                {
                    name: "__slot:actions", // <----
                    title: "Actions",
                    titleClass: "center aligned",
                    dataClass: "center aligned"
                }
            ],
            showEditProductDetails:false,
            showProductDetails:false
        }
    },
    /**
     * Component methods
     */
    methods:{
        /**
         * Transforms a boolean value into a icon
         */
        booleansAsIcons(value){
            return value 
                ? '<span class="ui teal label"><i class="material-icons">check</i></span>'
                : '<span class="ui teal label"><i class="material-icons">close</i></span>'
            ;
        },
        /**
         * Deletes a given product
         */
        deleteProduct(productId){
            Axios
                .delete(MYCM_API_URL+'/products/'+productId)
                .then(()=>{
                    this.$emit('refreshData');
                    this.$toast.open({message:"Product was deleted with success!"});
                })
                .catch((error_message)=>{
                    this.$toast.open({message:error_message.data.message});
                });
        },
        /**
         * Edits the details of a product
         */
        editProductDetails(productId){
            this
                .getProductDetails(productId)
                .then((product)=>{
                    this
                        .getAllCategories()
                        .then(()=>{
                            this
                                .getAllComponents()
                                .then(()=>{
                                    this
                                        .getAllMaterials()
                                        .then(()=>{
                                            this.showEditProductDetails=true;
                                        });
                                });
                        });
                });
        },
        /**
         * Fetches all categories available in a promise way
         */
        getAllCategories(){
            return new Promise((accept,reject)=>{
                Axios
                    .get(MYCM_API_URL+'/categories/')
                    .then((categories)=>{
                        this.availableCategories=categories.data;
                        accept();
                    })
                    .catch((error_message)=>{
                        this.$toast.open({message:error_message});
                        reject();
                    });
            });
        },
        /**
         * Fetches all components available in a promise way
         */
        getAllComponents(){
            return new Promise((accept,reject)=>{
                Axios
                    .get(MYCM_API_URL+'/products/')
                    .then((components)=>{
                        this.availableComponents=components.data;
                        accept();
                    })
                    .catch((error_message)=>{
                        this.$toast.open({message:error_message});
                        reject();
                    });
            });
        },
        /**
         * Fetches all materials available in a promise way
         */
        getAllMaterials(){
            return new Promise((accept,reject)=>{
                Axios
                    .get(MYCM_API_URL+'/materials/')
                    .then((materials)=>{
                        this.availableMaterials=materials.data;
                        accept();
                    })
                    .catch((error_message)=>{
                        this.$toast.open({message:error_message});
                        reject();
                    });
            });
        },
        /**
         * Fetches the details of a certain product in a promise way
         */
        getProductDetails(productId){
            return new Promise((accept,reject)=>{
                Axios
                    .get(MYCM_API_URL+'/products/'+productId)
                    .then((product)=>{
                        this.currentSelectedProduct=product.data;
                        this.currentSelectedProduct2=Object.assign({},this.currentSelectedProduct);
                        accept(product);
                    })
                    .catch((error_message)=>{
                        this.$toast.open({message:error_message});
                        reject();
                    });
            });
        },
        /**
         * Opens a modal with the product details
         */
        openProductDetails(productId){
            this
                .getProductDetails(productId)
                .then((product)=>{
                    this.showProductDetails=true;
                });
        },
        /**
         * Updates a given product
         */
        updateProduct(productDetails){
            this
                .updateProductProperties(productDetails)
                .then(()=>{
                    this
                        .updateProductMaterials(productDetails)
                        .then(()=>{
                            this
                                .updateProductComponents(productDetails)
                                .then(()=>{
                                    this
                                        .updateProductDimensions(productDetails)
                                        .then(()=>{
                                            this.showEditProductDetails=false;
                                            this.$emit('refreshData');
                                            this.$toast.open({message:"Product was updated with success!"});
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
                })
                .catch((error_message)=>{
                    this.$toast.open({message:error_message});
                });
        },
        /**
         * Updates a given product properties (PUT) in a promise way
         */
        updateProductProperties(productDetails){
            let productPropertiesToUpdate={};
            let atLeastOneUpdate=false;
            if(productDetails.reference!=null && productDetails.reference!=this.currentSelectedProduct.reference){
                productPropertiesToUpdate.reference=productDetails.reference;
                atLeastOneUpdate=true;
            }
            if(productDetails.designation!=null && productDetails.designation!=this.currentSelectedProduct.designation){
                productPropertiesToUpdate.designation=productDetails.designation;
                atLeastOneUpdate=true;
            }
            if(productDetails.category!=null && productDetails.category!=this.currentSelectedProduct.category.id){
                productPropertiesToUpdate.categoryId=productDetails.category;
                atLeastOneUpdate=true;
            }
            
            return new Promise((accept,reject)=>{
                if(atLeastOneUpdate){
                    Axios
                    .put(MYCM_API_URL+'/products/'+productDetails.id,productPropertiesToUpdate)
                    .then((product)=>{
                        accept(product);
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
         * Updates a given product components (POST + DELETE) in a promise way
         */
        updateProductComponents(productDetails){
            let oldProductComponents=[];
            let addComponents=[];
            let deleteComponents=[];

            if(this.currentSelectedProduct.components!=null){
                for(let i=0;i<this.currentSelectedProduct.components.length;i++)
                    oldProductComponents.push(this.currentSelectedProduct.components[i].id);
            }

            let newProductComponents=productDetails.components!=null ? productDetails.components : [];

            //Components to add

            for(let i=0;i<newProductComponents.length;i++){
                if(!oldProductComponents.includes(newProductComponents[i]))
                    addComponents.push(newProductComponents[i]);
            }

            //Components to delete

            for(let i=0;i<oldProductComponents.length;i++){
                if(!newProductComponents.includes(oldProductComponents[i]))
                    deleteComponents.push(oldProductComponents[i]);
            }

            return new Promise((accept,reject)=>{
                if(newProductComponents.length==0)accept();
                if(addComponents.length>0){
                    for(let i=0;i<addComponents.length;i++){
                        Axios
                            .post(MYCM_API_URL+'/products/'+productDetails.id+'/components/',{
                                id:addComponents[i]
                            })
                            .catch((error_message)=>{
                                reject(error_message.data.message);
                            });
                    }
                }

                if(deleteComponents.length>0){
                    for(let i=0;i<deleteComponents.length;i++){
                        Axios
                            .delete(MYCM_API_URL+'/products/'+productDetails.id+'/components/'+deleteComponents[i])
                            .catch((error_message)=>{
                                reject(error_message.data.message);
                            });
                    }
                }
                accept();
            });
        },
        /**
         * Updates a given product dimensions (POST + DELETE) in a promise way
         */
        updateProductDimensions(productDetails){
            let oldProductDimensions=[];
            let addDimensions=[];
            let deleteDimensions=[];
            for(let i=0;i<this.currentSelectedProduct.dimensions.length;i++)
                oldProductDimensions.push(this.currentSelectedProduct.dimensions[i].id);
            let newProductDimensions=productDetails.dimensions!=null ? productDetails.dimensions : [];
            
            for(let i=0;i<newProductDimensions.length;i++){
                if(!oldProductDimensions.includes(newProductDimensions[i]))
                    addDimensions.push(newProductDimensions[i]);
            }
            
            for(let i=0;i<oldProductDimensions.length;i++){
                if(!newProductDimensions.includes(oldProductDimensions[i]))
                    deleteDimensions.push(oldProductDimensions[i]);
            }
            return new Promise((accept,reject)=>{
                if(newProductDimensions.length==0)accept();
                if(addDimensions.length>0){
                    for(let i=0;i<addDimensions.length;i++){
                        Axios
                            .post(MYCM_API_URL+'/products/'+productDetails.id+'/dimensions/',{
                                id:addDimensions[i]
                            })
                            .catch((error_message)=>{
                                reject(error_message.data.message);
                            });
                    }
                }

                if(deleteDimensions.length>0){
                    for(let i=0;i<deleteDimensions.length;i++){
                        Axios
                            .delete(MYCM_API_URL+'/products/'+productDetails.id+'/dimensions/'+deleteDimensions[i])
                            .catch((error_message)=>{
                                reject(error_message.data.message);
                            });
                    }
                }
                accept();
            });
        },
        /**
         * Updates a given product materials (POST + DELETE) in a promise way
         */
        updateProductMaterials(productDetails){
            let productMaterialsToAdd=[];
            let oldProductMaterials=[];
            let addMaterials=[];
            let deleteMaterials=[];
            for(let i=0;i<this.currentSelectedProduct.materials.length;i++)
                oldProductMaterials.push(this.currentSelectedProduct.materials[i].id);
            let newProductMaterials=[];
            if(productDetails.materials!=null){
                for(let i=0;i<productDetails.materials.length;i++){
                    newProductMaterials.push(productDetails.materials[i].id);
                }
            }
            
            for(let i=0;i<newProductMaterials.length;i++){
                if(!oldProductMaterials.includes(newProductMaterials[i]))
                    addMaterials.push(newProductMaterials[i]);
            }
            
            for(let i=0;i<oldProductMaterials.length;i++){
                if(!newProductMaterials.includes(oldProductMaterials[i]))
                    deleteMaterials.push(oldProductMaterials[i]);
            }
            
            return new Promise((accept,reject)=>{
                if(newProductMaterials.length>0){
                    if(addMaterials.length>0){
                        for(let i=0;i<addMaterials.length;i++){
                            Axios
                                .post(MYCM_API_URL+'/products/'+productDetails.id+'/materials/',{
                                    id:addMaterials[i]
                                })
                                .catch((error_message)=>{
                                    reject(error_message.data.message);
                                });
                        }
                    }

                    if(deleteMaterials.length>0){
                        for(let i=0;i<deleteMaterials.length;i++){
                            Axios
                                .delete(MYCM_API_URL+'/products/'+productDetails.id+'/materials/'+deleteMaterials[i])
                                .catch((error_message)=>{
                                    reject(error_message.data.message);
                                });
                        }
                    }
                    accept();
                }else{
                    accept();
                }
            });
        },
    },
    /**
     * Component properties
     */
    props:{
        data:[]
    }
}
</script>
