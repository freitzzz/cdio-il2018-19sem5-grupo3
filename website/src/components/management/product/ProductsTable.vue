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
                    @click="onAction('delete-item', props.rowData, props.rowIndex)"
                >
                    <b-icon icon="minus"/>
                </button>
            </div>
            <div v-if="showProductDetails">
                <product-details
                    :product="currentSelectedProduct"
                />
            </div>
            <div v-if="showEditProductDetails">
                <edit-product
                    @emitProduct="updateProduct"
                    :available-categories="availableCategories"
                    :available-components="availableComponents"
                    :available-materials="availableMaterials"
                    :product="currentSelectedProduct2"
                />
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
            /**d
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
                    .get('http://localhost:5000/mycm/api/categories/')
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
                    .get('http://localhost:5000/mycm/api/products/')
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
                    .get('http://localhost:5000/mycm/api/materials/')
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
                    .get('http://localhost:5000/mycm/api/products/'+productId)
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

            console.log(productDetails)
            alert("!!!")
            return new Promise((accept,reject)=>{
                if(!atLeastOneUpdate)accept();
                Axios
                .put('http://localhost:5000/mycm/api/products/'+productDetails.id,productPropertiesToUpdate)
                .then((product)=>{
                    accept(product);
                })
                .catch((error_message)=>{
                    reject(error_message);
                });
            });
        },
        /**
         * Updates a given product components (POST + DELETE) in a promise way
         */
        updateProductComponents(productDetails){
            let oldProductComponents=[];
            let addComponents=[];
            let deleteComponents=[];
            for(let i=0;i<this.currentSelectedProduct.components.length;i++)
                oldProductComponents.push(this.currentSelectedProduct.components.id);
            let componentsToUpdate=productDetails.components!=null ? productDetails.components : [];
            
            for(let i=0;i<componentsToUpdate.length;i++){
                if(!oldProductComponents.includes(componentsToUpdate[i]))
                    addComponents.push(componentsToUpdate[i]);
            }
            
            for(let i=0;i<oldProductComponents.length;i++){
                if(!componentsToUpdate.includes(oldProductComponents[i]))
                    deleteComponents.push(oldProductComponents[i]);
            }
            
            return new Promise((accept,reject)=>{
                if(addComponents.length>0){
                    for(let i=0;i<addComponents.length;i++){
                        Axios
                            .post('http://localhost:5000/mycm/api/products/'+productDetails.id+'/components/',{
                                id:addComponents[i]
                            })
                            .catch((error_message)=>{
                                reject(error_message);
                            });
                    }
                }

                if(deleteComponents.length>0){
                    for(let i=0;i<deleteComponents.length;i++){
                        Axios
                            .delete('http://localhost:5000/mycm/api/products/'+productDetails.id+'/components/'+deleteComponents[i])
                            .catch((error_message)=>{
                                reject(error_message);
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
                oldProductDimensions.push(this.currentSelectedProduct.dimensions.id);
            let dimensionsToUpdate=productDetails.dimensions!=null ? productDetails.dimensions : [];
            
            for(let i=0;i<dimensionsToUpdate.length;i++){
                if(!oldProductDimensions.includes(dimensionsToUpdate[i]))
                    addDimensions.push(dimensionsToUpdate[i]);
            }
            
            for(let i=0;i<oldProductDimensions.length;i++){
                if(!dimensionsToUpdate.includes(oldProductDimensions[i]))
                    deleteDimensions.push(oldProductDimensions[i]);
            }
            return new Promise((accept,reject)=>{
                if(addDimensions.length>0){
                    for(let i=0;i<addDimensions.length;i++){
                        Axios
                            .post('http://localhost:5000/mycm/api/products/'+productDetails.id+'/dimensions/',{
                                id:addDimensions[i]
                            })
                            .catch((error_message)=>{
                                reject(error_message);
                            });
                    }
                }

                if(deleteDimensions.length>0){
                    for(let i=0;i<deleteDimensions.length;i++){
                        Axios
                            .delete('http://localhost:5000/mycm/api/products/'+productDetails.id+'/dimensions/'+deleteDimensions[i])
                            .catch((error_message)=>{
                                reject(error_message);
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
                oldProductMaterials.push(this.currentSelectedProduct.materials.id);
            let materialsToUpdate=productDetails.materials!=null ? productDetails.materials : [];
            
            for(let i=0;i<materialsToUpdate.length;i++){
                if(!oldProductMaterials.includes(materialsToUpdate[i]))
                    addMaterials.push(materialsToUpdate[i]);
            }
            
            for(let i=0;i<oldProductMaterials.length;i++){
                if(!materialsToUpdate.includes(oldProductMaterials[i]))
                    deleteMaterials.push(oldProductMaterials[i]);
            }
            return new Promise((accept,reject)=>{
                if(addMaterials.length>0){
                    for(let i=0;i<addMaterials.length;i++){
                        Axios
                            .post('http://localhost:5000/mycm/api/products/'+productDetails.id+'/materials/',{
                                id:addMaterials[i]
                            })
                            .catch((error_message)=>{
                                reject(error_message);
                            });
                    }
                }

                if(deleteMaterials.length>0){
                    for(let i=0;i<deleteMaterials.length;i++){
                        Axios
                            .delete('http://localhost:5000/mycm/api/products/'+productDetails.id+'/materials/'+deleteMaterials[i])
                            .catch((error_message)=>{
                                reject(error_message);
                            });
                    }
                }
                accept();
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
