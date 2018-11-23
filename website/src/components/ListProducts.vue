<template>
    <div>
        <!-- CUD BUTTONS -->
        <div>
            <button class="button is-danger" @click="createNewProduct()">
                <b-icon icon="plus"/>
            </button>
            <create-new-product 
                :active="createNewProductModal" 
                :available-materials="availableMaterials"
                :available-categories="availableCategories"
                :available-components="availableComponents"
                @emitProduct="postProduct"
            />
            <button class="button is-danger" @click="removeSelectedProduct()">
                <b-icon icon="minus"/>
            </button>
            <button class="button is-danger" @click="updateSelectedProduct()">
                <b-icon 
                    icon="refresh"
                    custom-class="fa-spin"/>
            </button>
            <edit-product
                :active="updateProductModal"
                :product="product"
                :available-materials="availableMaterials"
                :available-categories="availableCategories"
                :available-components="availableComponents"
                @emitProduct="updateProduct"
                />
        </div>
        <paginated-table 
        :total.sync="total" 
        :columns.sync="columns"
        :data.sync="data"
        :title.sync="title"
        :showTotalInput=false
        :showItemsPerPageInput=false
        @clicked="changeSelectedProduct"
        />
    </div>
</template>

<script>

import CreateNewProduct from './CreateNewProduct.vue';
import EditProduct from './EditProduct.vue';
import PaginatedTable from './UIComponents/PaginatedTable.vue';
import NotificationSnackbar from './UIComponents/NotificationSnackbar.vue';
import Axios from 'axios';
import NotificationSnackbarVue from './UIComponents/NotificationSnackbar.vue';

let categories=[];
let components=[];
let materials=[];

export default {
    components:{
        PaginatedTable,
        CreateNewProduct,
        EditProduct
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
         * Triggers the update of a selected product
         */
        updateSelectedProduct(){
            this.fetchSelectedProduct();
            this.updateProductModal=true;
        },
        /**
         * Posts a new product
         */
        postProduct(productDetails){
            Axios
                .post('http://localhost:5000/mycm/api/products',productDetails)
                .then((response)=>{
                    this.$toast.open({message:"The product was created with success!"});
                    this.createNewProductModal=false;    
                    this.fetchRequests();
                })
                .catch((_error)=>{
                    this.$toast.open({message:_error.response.data.error});
                });
        },
        updateProduct(productDetails){
            this.updateProductProperties(productDetails);
        },
        updateProductProperties(productDetails){
            let productProperties={};
            if(this.productClone.reference!=productDetails.reference)productProperties.reference=productDetails.reference;
            if(this.productClone.designation!=productDetails.designation)productProperties.designation=productDetails.designation
            if(this.productClone.category!=productDetails.category)productProperties.categoryId=productDetails.category;
            Axios
                .put('http://localhost:5000/mycm/api/products/'+this.currentSelectedProduct
                    ,productProperties)
                .then((_response)=>{
                    this.$toast.open({message:"The product properties were updated with success"});
                    this.updateProductFromData(_response.data);
                    this.fetchRequests();
                    this.updateProductModal=false;
                })
                .catch((_error)=>{
                    this.$toast.open({message:"An error ocurrd while updating the product properties"})
                });
        },
        /**
         * Triggers the deletion of the selected product
         */
        removeSelectedProduct(){
            Axios
            .delete('http://localhost:5000/mycm/api/products/'+this.currentSelectedProduct)
            .then((response)=>{
                this.$toast.open({message:"The product was deleted with success!"});
                this.fetchRequests();
            })
            .catch(()=>{
                this.$toast.open({message:"An error occurred while deleting the product"});
            });
        },
        updateProductFromData(data){
            let productDetails=data;
            this.product={
                id:productDetails.id,
                reference:productDetails.reference,
                designation:productDetails.designation,
                category:productDetails.category.id,
                materials:materials
            };
            this.productClone={
                id:productDetails.id,
                reference:productDetails.reference,
                designation:productDetails.designation,
                category:productDetails.category.id,
                materials:materials
            };
        },
        fetchSelectedProduct(){
            Axios
                .get('http://localhost:5000/mycm/api/products/'+this.currentSelectedProduct)
                .then((response)=>{
                    let productDetails=response.data;
                    let materials=[];
                    productDetails.material.forEach((material)=>{
                        materials.push({id:material.id,value:material.designation});
                    });
                    this.product={
                        id:productDetails.id,
                        reference:productDetails.reference,
                        designation:productDetails.designation,
                        category:productDetails.category.id,
                        materials:materials
                    };
                    this.productClone={
                        id:productDetails.id,
                        reference:productDetails.reference,
                        designation:productDetails.designation,
                        category:productDetails.category.id,
                        materials:materials
                    };
                    console.log(productDetails)
                })
                .catch(()=>{
                    this.$toast.open({message:"An error occurred while fetching the product"});
                });
        },
        fetchRequests(){
            this.updateFetchedProducts();
            this.fetchAvailableCategories();
            this.fetchAvailableComponents();
            this.fetchAvailableMaterials();
        },
        fetchAvailableCategories(){
        Axios
          .get('http://localhost:5000/mycm/api/categories/leaves')
          .then((response)=>{
            let availableCategories=response.data;
            availableCategories.forEach((category)=>{
              categories.push(category);
            });
          })
          .catch(()=>{

          });
    },
    fetchAvailableComponents(){
        Axios
          .get('http://localhost:5000/mycm/api/products')
          .then((response)=>{
            let availableComponents=response.data;
            availableComponents.forEach((component)=>{
              components.push({
                id:component.id,
                value:component.designation
              });
            });
          })
          .catch(()=>{
            
          });
    },
    fetchAvailableMaterials(){
        Axios
          .get('http://localhost:5000/mycm/api/materials')
          .then((response)=>{
            let availableMaterials=response.data;
            availableMaterials.forEach((material)=>{
              materials.push({
                id:material.id,
                value:material.designation
              });
            });
          })
          .catch(()=>{
            
          });
        },
        /**
         * Fetches all available products
         */
        updateFetchedProducts(){
            Axios.get('http://localhost:5000/mycm/api/products')
            .then((_response)=>{
                this.data=this.generateProductsTableData(_response.data);
                this.columns=this.generateProductsTableColumns();
                this.total=this.data.length;
            })
            .catch((_error)=>{
                console.log(_error);
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
                }
            ];
        },
        generateProductsTableData(products){
            let productsTableData=[];
            products.forEach((product)=>{
                productsTableData.push({
                    id:product.id,
                    reference:product.reference,
                    designation:product.designation
                });
            });
            return productsTableData;
        }
    }
}
</script>
