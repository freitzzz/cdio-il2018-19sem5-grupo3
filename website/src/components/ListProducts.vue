<template>
    <div>
        <!-- CUD BUTTONS -->
        <div>
            <button class="button is-danger" @click="createNewProduct()">
                <b-icon icon="plus"/>
            </button>
            <create-new-product :active="create" />
            <button class="button is-danger" @click="removeSelectedProduct()">
                <b-icon icon="minus"/>
            </button>
            <button class="button is-danger" @click="updateSelectedProduct()">
                <b-icon 
                    icon="refresh"
                    custom-class="fa-spin"/>
            </button>
            <CreateNewProduct :active="createNewProductModal"/>
        </div>
        <PaginatedTable 
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
import PaginatedTable from './UIComponents/PaginatedTable.vue';
import NotificationSnackbar from './UIComponents/NotificationSnackbar.vue';
import Axios from 'axios';
import NotificationSnackbarVue from './UIComponents/NotificationSnackbar.vue';

export default {
    components:{
        PaginatedTable,
        CreateNewProduct,
    },
    /**
     * Function that is called when the component is created
     */
    created(){
        this.updateFetchedProducts();
    },
    data(){
        return{
            createNewProductModal:false,
            currentSelectedProduct:0,
            availableProducts:Array,
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
         * Triggers the deletion of the selected product
         */
        removeSelectedProduct(){
            Axios
            .delete('http://localhost:5000/mycm/api/products/'+this.currentSelectedProduct)
            .then((response)=>{
                console.log(response.data)
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
            .then((_error)=>{
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
