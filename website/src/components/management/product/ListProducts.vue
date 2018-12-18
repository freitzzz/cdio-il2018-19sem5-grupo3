<template>
    <div>
        <!-- CUD BUTTONS -->
        <div>
            <button class="btn-primary" @click="createNewProduct()">
                <b-icon icon="plus"/>
            </button>
            <div v-if="createNewProductModal">
                <b-modal :active.sync="createNewProductModal" has-modal-card scroll="keep">
                    <create-new-product 
                        :active="createNewProductModal" 
                        :available-materials="availableMaterials"
                        :available-categories="availableCategories"
                        :available-components="availableComponents"
                        :available-units="availableUnits"
                        @emitProduct="postProduct"
                    />
                </b-modal>
            </div>
            <button class="btn-primary" @click="fetchRequests()">
                <b-icon 
                    icon="refresh"
                    custom-class="fa-spin"/>
            </button>
        </div>
        <products-table
            :data.sync="data"
            @refreshData="refreshProducts"
        />
    </div>
</template>

<script>

import CreateNewProduct from './CreateNewProduct.vue';
import ProductsTable from './ProductsTable';
import Axios from 'axios';
import Config,{ MYCM_API_URL } from '../../../config.js';


let categories=[];
let categoriesIds=[];
let components=[];
let componentsIds=[];
let materials=[];
let materialsIds=[];
let units=[];

export default {
    components:{
        CreateNewProduct,
        ProductsTable
    },
    /**
     * Function that is called when the component is created
     */
    created(){
        this.fetchRequests();
    },
    data(){
        return{
            createNewProductModal:false,
            updateProductModal:false,
            product:null,
            productClone:null,
            currentSelectedProduct:0,
            availableProducts:Array,
            availableCategories:categories,
            availableComponents:components,
            availableMaterials:materials,
            availableUnits:units,
            columns:[],
            data:Array,
            total:Number,
            failedToFetchProductsNotification:false
        }
    },
    methods:{
        /**
         * Changes the current selected product
         */
        changeSelectedProduct(tableRow){
            this.currentSelectedProduct=tableRow.id;
        },
        /**
         * Triggers the creation of a new product
         */
        createNewProduct(){
            this.createNewProductModal=true;
        },
        /**
         * Posts a new product
         */
        postProduct(productDetails){
            let newProduct={};
            newProduct.reference=productDetails.reference;
            newProduct.designation=productDetails.designation;
            newProduct.categoryId=productDetails.category;
            
            if(productDetails.materials!=null){
                let newProductMaterials=[];
                for(let i=0;i<productDetails.materials.length;i++){
                    newProductMaterials.push({id:productDetails.materials[i]});
                }
                newProduct.materials=newProductMaterials.slice();
            }

            if(productDetails.components!=null){
                let newProductComponents=[];
                for(let i=0;i<productDetails.components.length;i++){
                    newProductComponents.push({id:productDetails.components[i].id,mandatory:productDetails.components[i].required});
                }
                newProduct.components=newProductComponents.slice();
            }
            
            newProduct.dimensions=productDetails.dimensions;
            if(productDetails.slots!=null){
                console.log(Object.assign({},productDetails.slots));
                if(!(productDetails.slots.minSize==null 
                    && productDetails.slots.recommendedSize==null 
                    && productDetails.slots.maxSize==null)){
                        newProduct.slotWidths={
                            minWidth:productDetails.slots.minSize,
                            recommendedWidth:productDetails.slots.recommendedSize,
                            maxWidth:productDetails.slots.maxSize
                        };
                        if(productDetails.slots.unit)newProduct.slotWidths.unit=productDetails.slots.unit;
                    }
            }

            newProduct.model=productDetails.model

            Axios
                .post(MYCM_API_URL+'/products',newProduct)
                .then((response)=>{
                    this.$toast.open({message:"The product was created with success!"});
                    this.createNewProductModal=false;    
                    this.fetchRequests();
                })
                .catch((error_message)=>{
                    this.$toast.open({message:error_message.response.data.message});
                });
        },
        fetchRequests(){
            this.refreshProducts();
            this.fetchAvailableCategories();
            this.fetchAvailableComponents();
            this.fetchAvailableMaterials();
            this.fetchAvailableUnits();
        },
        /**
         * Fetches the available categories
         */
        fetchAvailableCategories(){
        Axios
            .get(MYCM_API_URL+'/categories/leaves')
            .then((response)=>{
                let availableCategories=response.data;
                availableCategories.forEach((category)=>{
                    if(!categoriesIds.includes(category.id)){
                        categories.push(category);
                        categoriesIds.push(category.id);
                    }
                });
            })
            .catch((error_message)=>{
                this.$toast.open({message:error_message.response.data.message});
            });
    },
    /**
     * Fetches the available components
     */
    fetchAvailableComponents(){
        Axios
            .get(MYCM_API_URL+'/products')
            .then((response)=>{
                let availableComponents=response.data;
                availableComponents.forEach((component)=>{
                    if(!componentsIds.includes(component.id)){
                        components.push({
                            id:component.id,
                            value:component.designation
                        });
                        componentsIds.push(component.id);
                    }
                });
            })
            .catch((error_message)=>{
                this.$toast.open({message:error_message.response.data.message});
            });
    },
    /**
     * Fetches the available materials
     */
    fetchAvailableMaterials(){
        Axios
            .get(MYCM_API_URL+'/materials')
            .then((response)=>{
                let availableMaterials=response.data;
                availableMaterials.forEach((material)=>{
                    if(!materialsIds.includes(material.id)){
                        materials.push({
                            id:material.id,
                            value:material.designation
                        });
                        materialsIds.push(material.id);
                    }
                });
            })
            .catch((error_message)=>{
                this.$toast.open({message:error_message.response.data.message});
            });
        },
        /**
     * Fetches the available units
     */
    fetchAvailableUnits(){
        Axios
            .get(MYCM_API_URL+'/units')
            .then((response)=>{
                let availableUnits=response.data;
                if(units.length!=availableUnits.length)
                    availableUnits.forEach((unit)=>{
                        units.push(unit);
                    });
            })
            .catch((error_message)=>{
                this.$toast.open({message:error_message.response.data.message});
            });
        },
        /**
         * Fetches all available products
         */
        refreshProducts(){
            Axios.get(MYCM_API_URL+'/products')
            .then((_response)=>{
                this.data=this.generateProductsTableData(_response.data);
                this.columns=this.generateProductsTableColumns();
                this.total=this.data.length;
            })
            .catch((error_message)=>{
                this.$toast.open({message:error_message.response.data.message});
            });
        },
        /**
         * Generates the needed columns for the products table
         */
        generateProductsTableColumns(){
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
         * Generates the data of a products table by a given list of products
         */
        generateProductsTableData(products){
            let productsTableData=[];
            products.forEach((product)=>{
                productsTableData.push({
                    id:product.id,
                    reference:product.reference,
                    designation:product.designation,
                    hasComponents:product.hasComponents,
                    supportsSlots:product.supportsSlots
                });
            });
            return productsTableData;
        }
    }
}
</script>
